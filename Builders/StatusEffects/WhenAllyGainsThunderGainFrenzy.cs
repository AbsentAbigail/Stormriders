#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenAllyGainsThunderGainFrenzy : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenYAppliedToAlly>(Name)
            .WithText($"When an ally's {Stormriders.KeywordTag("thunder")} increases, gain <x{{a}}>{Stormriders.VanillaKeywordTag("frenzy")}")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedToAlly>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("MultiHit");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.whenAppliedType = "thunder";
            });
    }
}