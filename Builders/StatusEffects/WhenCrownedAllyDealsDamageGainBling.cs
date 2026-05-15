using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenCrownedAllyDealsDamageGainBling : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenAllyDealsDamage>(Name)
            .WithText("When a <sprite name=crown>'d ally deals damage, gain <{a}><keyword=blings>")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenAllyDealsDamage>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Gain Gold");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.attackerConstraints =
                [
                    TargetConstraintHelper.General<TargetConstraintHasCrown>("Has Crown")
                ];
            });
    }
}