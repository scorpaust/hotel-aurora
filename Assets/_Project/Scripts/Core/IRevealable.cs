namespace Aurora.Core
{
    /// <summary>
    /// Hook for monsters to show their human face when lit by the lantern's focused beam
    /// (F1-04 spec item 6) — used by boss/enemy confrontations (F5-02), no consumer yet.
    /// </summary>
    public interface IRevealable
    {
        void OnRevealed(float duration);
    }
}
