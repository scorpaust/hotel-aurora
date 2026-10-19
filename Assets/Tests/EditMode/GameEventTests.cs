using System.Collections.Generic;
using Aurora.Core;
using NUnit.Framework;
using UnityEngine;

namespace Aurora.Tests.EditMode
{
    public class GameEventTests
    {
        private GameEvent _event = null!;

        [SetUp]
        public void SetUp()
        {
            _event = ScriptableObject.CreateInstance<GameEvent>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_event);
        }

        [Test]
        public void Raise_NotifiesListenersInRegistrationOrder()
        {
            var order = new List<int>();
            _event.Register(() => order.Add(1));
            _event.Register(() => order.Add(2));
            _event.Register(() => order.Add(3));

            _event.Raise();

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, order);
        }

        [Test]
        public void Unregister_StopsFutureNotifications()
        {
            var callCount = 0;
            void Listener() => callCount++;

            _event.Register(Listener);
            _event.Unregister(Listener);
            _event.Raise();

            Assert.AreEqual(0, callCount);
        }

        [Test]
        public void Register_SameListenerTwice_OnlyInvokedOnce()
        {
            var callCount = 0;
            void Listener() => callCount++;

            _event.Register(Listener);
            _event.Register(Listener);
            _event.Raise();

            Assert.AreEqual(1, callCount);
        }
    }
}
