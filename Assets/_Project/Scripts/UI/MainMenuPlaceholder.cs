#nullable enable

using UnityEngine;
using UnityEngine.UIElements;

namespace Aurora.UI
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainMenuPlaceholder : MonoBehaviour
    {
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.style.flexGrow = 1;
            root.style.alignItems = Align.Center;
            root.style.justifyContent = Justify.Center;

            var label = new Label("Hotel Aurora")
            {
                style = { color = Color.white, fontSize = 48 }
            };
            root.Add(label);
        }
    }
}
