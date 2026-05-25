using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantReduceWaterBy4 : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXInstant>(Name)
            .WithText($"Reduce {Stormriders.KeywordTag(Keywords.Water.Name)} by 4")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXInstant>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(InstantCountDownWater.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.scriptableAmount = new Script<ScriptableFixedAmount>("Fixed 4", script => script.amount = 4);
                status.targetConstraints =
                [
                    TargetConstraintHelper.General<TargetConstraintStatusMoreThan>("Has 4 Water",
                        script =>
                        {
                            script.amount = 3;
                            script.status = Stormriders.GetStatus(Water.Name);
                        })
                ];
            });
    }
}