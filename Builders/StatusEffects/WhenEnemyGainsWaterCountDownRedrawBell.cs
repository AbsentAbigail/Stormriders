using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.Scriptables.ScriptableAmounts;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenEnemyGainsWaterCountDownRedrawBell : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenYAppliedTo>(Name)
            .WithText($"When an enemy gains {Stormriders.KeywordTag("water")}, count down <Redraw Bell> by <{{a}}>")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(InstantChargeRedrawBell.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.whenAppliedToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                status.whenAppliedTypes = ["water"];
            });
    }
}