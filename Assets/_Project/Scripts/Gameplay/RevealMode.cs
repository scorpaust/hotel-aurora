namespace Aurora.Gameplay
{
    /// <summary>
    /// How a PastLayerObject's present/past textures relate (F1-04 spec item 4) — purely an
    /// authoring convention: give the shader a transparent texture on one side to get
    /// ShowWhenLit/HideWhenLit, or two opaque ones for a straight Swap.
    /// </summary>
    public enum RevealMode
    {
        ShowWhenLit,
        HideWhenLit,
        Swap
    }
}
