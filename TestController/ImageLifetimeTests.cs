using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LMCIS.Utilities;
using Xunit;

namespace TestController
{
    public class ImageLifetimeTests
    {
        [Fact]
        public void LoadingDetachedImageReleasesSourceFile()
        {
            WithSta(() =>
            {
                var path = CreateImageFile();
                try
                {
                    using (var image = OwnedImage.LoadDetached(path))
                    using (var source = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        Assert.Equal(2, image.Width);
                    }
                }
                finally
                {
                    File.Delete(path);
                }
            });
        }

        [Fact]
        public void ReplacingOrClearingDisposesOnlyThePreviouslyOwnedImage()
        {
            WithSta(() =>
            {
                using (var pictureBox = new PictureBox())
                using (var sharedImage = new Bitmap(2, 2))
                {
                    pictureBox.Image = sharedImage;
                    Bitmap owned = null;

                    OwnedImage.Replace(pictureBox, ref owned, new Bitmap(3, 3));
                    var firstOwned = owned;
                    OwnedImage.Replace(pictureBox, ref owned, new Bitmap(4, 4));

                    AssertDisposed(firstOwned);
                    Assert.Equal(2, sharedImage.Width);
                    var secondOwned = owned;
                    OwnedImage.Replace(pictureBox, ref owned, null);

                    Assert.Null(owned);
                    Assert.Null(pictureBox.Image);
                    AssertDisposed(secondOwned);
                    Assert.Equal(2, sharedImage.Width);
                }
            });
        }

        [Fact]
        public void DisposingPictureBoxReleasesItsCurrentOwnedImage()
        {
            WithSta(() =>
            {
                var pictureBox = new PictureBox();
                Bitmap owned = null;
                OwnedImage.TrackDisposal(pictureBox, () => owned);
                OwnedImage.Replace(pictureBox, ref owned, new Bitmap(2, 2));

                var image = owned;
                pictureBox.Dispose();

                AssertDisposed(image);
            });
        }

        private static string CreateImageFile()
        {
            var path = Path.Combine(Path.GetTempPath(), "warehouse-image-" + Guid.NewGuid().ToString("N") + ".png");
            using (var image = new Bitmap(2, 2))
                image.Save(path, ImageFormat.Png);
            return path;
        }

        private static void AssertDisposed(Bitmap image)
        {
            Assert.ThrowsAny<Exception>(() => image.GetPixel(0, 0));
        }

        private static void WithSta(Action action)
        {
            Exception failure = null;
            var thread = new Thread(() =>
            {
                try { action(); }
                catch (Exception exception) { failure = exception; }
            }) { IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Assert.True(thread.Join(TimeSpan.FromSeconds(15)), "The image lifetime operation did not complete.");
            if (failure != null) throw new Exception("Image lifetime check failed on an STA thread.", failure);
        }
    }
}
