using System;
using System.Drawing;
using System.Windows.Forms;

namespace LMCIS.Utilities
{
    public static class OwnedImage
    {
        public static Bitmap LoadDetached(string path)
        {
            using (var source = Image.FromFile(path))
            {
                return new Bitmap(source);
            }
        }

        public static void Replace(PictureBox pictureBox, ref Bitmap ownedImage, Bitmap replacement)
        {
            var previous = ownedImage;
            pictureBox.Image = replacement;
            ownedImage = replacement;
            if (previous != null && !ReferenceEquals(previous, replacement))
            {
                previous.Dispose();
            }
        }

        public static void TrackDisposal(PictureBox pictureBox, Func<Bitmap> getOwnedImage)
        {
            pictureBox.Disposed += (sender, args) =>
            {
                var image = getOwnedImage();
                if (image != null)
                {
                    image.Dispose();
                }
            };
        }
    }
}
