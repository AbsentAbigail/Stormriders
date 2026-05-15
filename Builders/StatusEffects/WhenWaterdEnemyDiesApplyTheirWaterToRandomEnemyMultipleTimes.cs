using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenWaterdEnemyDiesApplyTheirWaterToRandomEnemyMultipleTimes : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenUnitIsKilledMultipleTimes>(Name)
            .WithText($"When a {Stormriders.KeywordTag("water")}'d enemy dies, apply their {Stormriders.KeywordTag("water")} to <{{a}}> random enemies.")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenUnitIsKilledMultipleTimes>(status =>
            {
                status.ally = false;
                status.enemy = true;
                
                status.effectToApply = Stormriders.GetStatus(Water.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomEnemy;
                status.applyEqualAmount = true;
                status.unitConstraints =
                [
                    TargetConstraintHelper.HasStatus(Water.Name)
                ];
                status.contextEqualAmount =
                    new Script<ScriptableCurrentStatus>("Current Water", script => script.statusType = "water");
            });
    }
}