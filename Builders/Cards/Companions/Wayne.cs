#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.Cards.Companions;

[UsedImplicitly]
public class Wayne : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Wayne",
                "TargetModeBasic",
                "Blood Profile Normal",
                "WaveAnimationProfile")
            .SetStats(12, 10, 7)
            .SetSprites(
                Stormriders.GetSprite("Wayne"),
                Stormriders.GetSprite("Wayne_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnKillReduceCounterAndApplyWaterToEnemiesInRow.Name),
                ];
                card.greetMessages =
                [
                    "I've been through worse wars than this one, and I'll make it through this one, too.",
                    "How did I lose my arm? Heh... why don't you ask Clover?",
                    "You've got the Thunder, but I've got the pure, unrelenting muscle.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}