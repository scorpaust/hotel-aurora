using Aurora.Core;
using NUnit.Framework;

namespace Aurora.Tests.EditMode
{
    public class ServicesTests
    {
        private interface IDummy
        {
        }

        private sealed class Dummy : IDummy
        {
        }

        [TearDown]
        public void TearDown()
        {
            Services.Reset();
        }

        [Test]
        public void Register_ThenGet_ReturnsSameInstance()
        {
            var dummy = new Dummy();
            Services.Register<IDummy>(dummy);

            Assert.AreSame(dummy, Services.Get<IDummy>());
        }

        [Test]
        public void TryGet_WhenNotRegistered_ReturnsFalse()
        {
            Assert.IsFalse(Services.TryGet<IDummy>(out var instance));
            Assert.IsNull(instance);
        }

        [Test]
        public void Reset_ClearsAllRegistrations()
        {
            Services.Register<IDummy>(new Dummy());
            Services.Reset();

            Assert.IsFalse(Services.TryGet<IDummy>(out _));
        }

        [Test]
        public void Get_WhenNotRegistered_Throws()
        {
            Assert.Throws<System.InvalidOperationException>(() => Services.Get<IDummy>());
        }
    }
}
