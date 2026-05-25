using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;
using Stormriders.Helpers;
using Stormriders.Scriptables.ScriptableAmounts;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedHealSelfEqualToWaterOnEnemiesInRow : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"Restore <keyword=health> to self equal to total {Stormriders.KeywordTag(Keywords.Water.Name)} of enemies in the row")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Heal");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.scriptableAmount = new Script<ScriptableStatusOnBoard>("Water on enemies in row", script =>
                {
                    script.statusType = "water";
                    script.self = false;
                    script.allies = false;
                    script.enemies = true;
                    script.countStatus = true;
                    script.inRow = true;
                });
            });
    }
}