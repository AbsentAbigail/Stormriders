using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenHitByWaterEnemyAbsorbAttacker : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectWhenHitByWaterCount>(Name)
            .WithText($"When hit by an enemy with <{{a}}> or more {Stormriders.KeywordTag("water")}, kill the attacker and absorb their <keyword=health> and <keyword=attack>, then increase own effects by <{{a}}>")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectWhenHitByWaterCount>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(InstantEatAndIncreaseApplierEffects.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Attacker;
            });
    }
}