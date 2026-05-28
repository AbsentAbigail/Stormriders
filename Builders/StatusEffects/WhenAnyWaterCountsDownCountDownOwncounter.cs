#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.StatusEffectImplementations;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenAnyWaterCountsDownCountDownOwncounter : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenStatusCountsDown>(Name)
            .WithText($"Count down <keyword=counter> by <{{a}}> whenever any {Stormriders.KeywordTag("water")} counts down")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenStatusCountsDown>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Reduce Counter");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.CountDownFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
            });
    }
}