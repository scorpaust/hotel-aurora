#nullable enable

using UnityEngine;

namespace Aurora.Core
{
    [CreateAssetMenu(menuName = "Aurora/Events/Game Event (string)", fileName = "SO_NewGameEventString")]
    public sealed class GameEventString : GameEvent<string>
    {
    }
}
