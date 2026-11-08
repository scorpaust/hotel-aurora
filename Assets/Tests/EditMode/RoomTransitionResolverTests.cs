using Aurora.Core;
using NUnit.Framework;

namespace Aurora.Tests.EditMode
{
    public class RoomTransitionResolverTests
    {
        [Test]
        public void Resolve_NoCondition_ReturnsPrimaryDestination()
        {
            var primary = new RoomDestination("RoomB", "FromA");
            var fallback = new RoomDestination("RoomC", "FromA");
            var request = new RoomTransitionRequest(RoomConnectionType.Door, primary, string.Empty, 0, fallback);

            var result = RoomTransitionResolver.Resolve(request, currentFlagValue: 0);

            Assert.AreEqual("RoomB", result.RoomId);
        }

        [Test]
        public void Resolve_ConditionMet_ReturnsPrimaryDestination()
        {
            var primary = new RoomDestination("RoomB", "FromA");
            var fallback = new RoomDestination("RoomC", "FromA");
            var request = new RoomTransitionRequest(RoomConnectionType.Door, primary, "quartos.governanta.libertada", 1, fallback);

            var result = RoomTransitionResolver.Resolve(request, currentFlagValue: 1);

            Assert.AreEqual("RoomB", result.RoomId);
        }

        [Test]
        public void Resolve_ConditionNotMet_ReturnsFallbackDestination()
        {
            var primary = new RoomDestination("RoomB", "FromA");
            var fallback = new RoomDestination("RoomC", "FromA");
            var request = new RoomTransitionRequest(RoomConnectionType.Door, primary, "quartos.governanta.libertada", 1, fallback);

            var result = RoomTransitionResolver.Resolve(request, currentFlagValue: 0);

            Assert.AreEqual("RoomC", result.RoomId);
        }
    }
}
