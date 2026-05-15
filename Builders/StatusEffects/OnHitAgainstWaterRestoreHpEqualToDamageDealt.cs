using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnHitAgainstWaterRestoreHpEqualToDamageDealt : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnHitType>(Name)
            .WithText($"When attacking an enemy with {Stormriders.KeywordTag("water")}, restore {Stormriders.VanillaKeywordTag("health")} to self equal to damage dealt")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnHitType>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Heal");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.hitTargetConstraints =
                [
                    TargetConstraintHelper.HasStatus("Water")
                ];
            });
    }
}