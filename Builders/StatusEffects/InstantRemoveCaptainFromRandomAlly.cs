#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;
using Stormriders.Scriptables.TargetConstraints;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantRemoveCaptainFromRandomAlly : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXInstant>(Name)
            .WithText($"Remove {Stormriders.KeywordTag("captain")} from a random ally")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXInstant>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(InstantRemoveCaptain.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAlly;
                TargetConstraint[] constraint = [TargetConstraintHelper.HasTrait(Captain.Name)];
                status.targetConstraints =
                [
                    TargetConstraintHelper.General<TargetConstraintAllyMatchesConstraint>("Ally has Captain",
                        script =>
                        {
                            script.inRow = false;
                            script.constraints = constraint;
                        })
                ];
                status.applyConstraints = constraint;
            });
    }
}