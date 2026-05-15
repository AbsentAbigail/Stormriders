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
public class InstantApplyFrostOrWaterIfCaptainInRow : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantApplyXConditionalElseY>(Name)
            .WithText($"Apply <{{a}}>{Stormriders.VanillaKeywordTag("frost")}, or, if there's an ally {Stormriders.KeywordTag("captain")} in the row, apply <{{a}}>{Stormriders.KeywordTag("water")} instead")
            .WithStackable(true)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantApplyXConditionalElseY>(status =>
            {
                status.conditionalEffectToApply = Stormriders.GetStatus(Water.Name);
                status.otherEffectToApply = Stormriders.GetStatus("Frost");
                status.applierConditions =
                [
                    TargetConstraintHelper.General<TargetConstraintAllyInRowMatchesConstraint>("Ally in row has Captain",
                        constraint =>
                        {
                            constraint.constraints =
                            [
                                TargetConstraintHelper.HasTrait(Captain.Name)
                            ];
                        })
                ];
            });
    }
}