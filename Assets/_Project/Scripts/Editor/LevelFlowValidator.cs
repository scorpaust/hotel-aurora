#nullable enable

using System.Collections.Generic;
using Aurora.Hotel;

namespace Aurora.Editor
{
    /// <summary>
    /// Checks a LevelFlowDefinition is well-formed before LevelGeneratorWindow uses it —
    /// kept separate from the window so it's testable without an EditorWindow instance.
    /// </summary>
    public static class LevelFlowValidator
    {
        public static ContentDefinitionValidator.ValidationResult Validate(LevelFlowDefinition flow)
        {
            var issues = new List<string>();

            if (flow.Beats.Count == 0)
            {
                issues.Add($"LevelFlowDefinition '{flow.Id}' has no beats.");
                return new ContentDefinitionValidator.ValidationResult(issues);
            }

            for (var i = 0; i < flow.Beats.Count; i++)
            {
                var beat = flow.Beats[i];
                switch (beat.Type)
                {
                    case LevelBeatType.EnemyEncounter:
                        if (beat.Enemies.Count == 0)
                            issues.Add($"Beat {i} ({beat.Type}) has no EnemyDefinition assigned.");
                        break;
                    case LevelBeatType.TrapGauntlet:
                        if (beat.Traps.Count == 0)
                            issues.Add($"Beat {i} ({beat.Type}) has no TrapDefinition assigned.");
                        break;
                    case LevelBeatType.Cutscene:
                        if (beat.Cutscene == null)
                            issues.Add($"Beat {i} ({beat.Type}) has no CutsceneDefinition assigned.");
                        break;
                    case LevelBeatType.NarrativeInteraction:
                        if (beat.NarrativeBeat == null)
                            issues.Add($"Beat {i} ({beat.Type}) has no NarrativeBeatDefinition assigned.");
                        break;
                    case LevelBeatType.Boss:
                    case LevelBeatType.MiniBoss:
                        if (beat.Boss == null)
                            issues.Add($"Beat {i} ({beat.Type}) has no BossDefinition assigned.");
                        break;
                    case LevelBeatType.Exploration:
                    case LevelBeatType.Maze:
                    case LevelBeatType.BonusLevel:
                        // Driven by scene layout alone — no required reference yet.
                        break;
                }
            }

            return new ContentDefinitionValidator.ValidationResult(issues);
        }
    }
}
