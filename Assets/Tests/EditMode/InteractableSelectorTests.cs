using System.Collections.Generic;
using Aurora.Core;
using NUnit.Framework;

namespace Aurora.Tests.EditMode
{
    public class InteractableSelectorTests
    {
        private sealed class FakeInteractable : IInteractable
        {
            public string VerbKey => "test";
            public InteractionKind Kind => InteractionKind.Press;
            public float HoldDuration => 0f;
            public int Priority { get; set; }
            public bool CanInteract(InteractionContext context) => true;
            public void Interact(InteractionContext context) { }
        }

        [Test]
        public void SelectBest_NoCandidates_ReturnsNull()
        {
            var result = InteractableSelector.SelectBest(new List<(IInteractable, float)>());

            Assert.IsNull(result);
        }

        [Test]
        public void SelectBest_SingleCandidate_ReturnsIt()
        {
            var candidate = new FakeInteractable();

            var result = InteractableSelector.SelectBest(new List<(IInteractable, float)> { (candidate, 1f) });

            Assert.AreSame(candidate, result);
        }

        [Test]
        public void SelectBest_SamePriority_PicksClosest()
        {
            var near = new FakeInteractable { Priority = 0 };
            var far = new FakeInteractable { Priority = 0 };

            var result = InteractableSelector.SelectBest(new List<(IInteractable, float)>
            {
                (far, 5f),
                (near, 1f)
            });

            Assert.AreSame(near, result);
        }

        [Test]
        public void SelectBest_HigherPriorityWinsOverCloserDistance()
        {
            var closeButLowPriority = new FakeInteractable { Priority = 0 };
            var farButHighPriority = new FakeInteractable { Priority = 10 };

            var result = InteractableSelector.SelectBest(new List<(IInteractable, float)>
            {
                (closeButLowPriority, 0.5f),
                (farButHighPriority, 5f)
            });

            Assert.AreSame(farButHighPriority, result);
        }
    }
}
