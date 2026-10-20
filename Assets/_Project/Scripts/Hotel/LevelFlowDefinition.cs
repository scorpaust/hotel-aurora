#nullable enable

using System;
using System.Collections.Generic;
using Aurora.Core;
using UnityEngine;

namespace Aurora.Hotel
{
    [CreateAssetMenu(menuName = "Aurora/Content/Level Flow Definition", fileName = "SO_NewLevelFlow")]
    public sealed class LevelFlowDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private LevelBeat[] _beats = Array.Empty<LevelBeat>();

        public string Id => _id;
        public IReadOnlyList<LevelBeat> Beats => _beats;
    }
}
