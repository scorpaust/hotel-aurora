namespace Aurora.Core
{
    /// <summary>
    /// Empty until F1-05 builds the real tools/items inventory — PickUp (F1-03) already
    /// calls it, so nothing in Gameplay needs to change once it's implemented.
    /// </summary>
    public interface IInventoryService
    {
        void AddItem(string itemId);
    }
}
