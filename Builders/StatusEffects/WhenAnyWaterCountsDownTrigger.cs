using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenAnyWaterCountsDownTrigger : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenStatusCountsDown>(Name)
            .WithText($"Trigger whenever any {Stormriders.KeywordTag("water")} counts down")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenStatusCountsDown>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Trigger (High Prio)");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.CountDownFlags = null;

                status.isReaction = true;
                status.descColorHex = "F99C61";
            });
    }
}