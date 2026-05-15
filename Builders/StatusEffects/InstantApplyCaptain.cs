using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;
using Stormriders.Scriptables.TargetConstraints;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantApplyCaptain : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantApplyEffect>(Name)
            .WithText($"Apply the {Stormriders.KeywordTag("captain")} trait")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantApplyEffect>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(TemporaryCaptain.Name);
                status.targetConstraints =
                [
                    TargetConstraintHelper.General<TargetConstraintIsUnit>(),
                    TargetConstraintHelper.HasTrait(Captain.Name, not: true)
                ];
            });
    }
}