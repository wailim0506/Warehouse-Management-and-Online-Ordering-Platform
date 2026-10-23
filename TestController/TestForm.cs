using System;
using System.Threading;
using LMCIS.System_page;
using Xunit;

namespace TestController
{
    public class TestForm
    {
        [Fact]
        public void LoginFormCanBeConstructedAndDisposedOnStaThreadWithoutLoadingServices()
        {
            Exception failure = null;
            var thread = new Thread(() =>
            {
                try
                {
                    using (var loginForm = new Login())
                    {
                        Assert.NotNull(loginForm);
                        Assert.False(loginForm.Visible);
                    }
                }
                catch (Exception exception)
                {
                    failure = exception;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (failure != null) throw new Exception("STA form construction failed.", failure);
        }
    }
}
