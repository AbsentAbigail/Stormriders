using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;
using Stormriders.Helpers;
using Stormriders.Scriptables.ScriptableAmounts;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedIncreaseHealthOfAlliesInRow : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"Increase {Stormriders.VanillaKeywordTag("health")} of allies in row by <{{a}}>")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Increase Max Health");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.AlliesInRow;
            });
    }
}