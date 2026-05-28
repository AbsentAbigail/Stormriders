#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedNavigateAllyAhead : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"{Stormriders.KeywordTag(Navigate.Name)} ally ahead")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.AllyInFrontOf;
                status.effectToApply = Stormriders.GetStatus(Navigator.Name);
            });
    }
}