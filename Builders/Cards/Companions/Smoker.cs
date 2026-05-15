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
public class Smoker : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Smoker",
                "TargetModeBasic",
                "Blood Profile Husk",
                "SwayAnimationProfile")
            .SetStats(6, null, 8)
            .SetSprites(
                Stormriders.GetSprite("Smoker"),
                Stormriders.GetSprite("Smoker_BG"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedSmeltAlliesInRow.Name),
                    Stormriders.SStack(WhenAnyWaterCountsDownTrigger.Name),
                ];
                card.greetMessages =
                [
                    "The cold all the way up there won't freeze MY vents over. I'll make sure of it.",
                    "A little more heat, a little more smoke, and the water boils away into something STRONG.",
                    "I'm the best smith in the sea. I'd like to meet the one on land and trade notes.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}