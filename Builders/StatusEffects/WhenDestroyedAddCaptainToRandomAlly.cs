using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenDestroyedAddCaptainToRandomAlly : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenDestroyed>(Name)
            .WithText($"When destroyed, apply {Stormriders.KeywordTag("captain")} to a random ally")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(status =>
            {
                status.targetMustBeAlive = false;
                status.effectToApply = Stormriders.GetStatus(TemporaryCaptain.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAlly;
                status.applyConstraints =
                [
                    TargetConstraintHelper.HasTrait(Captain.Name, not: true)
                ];
            });
    }
}