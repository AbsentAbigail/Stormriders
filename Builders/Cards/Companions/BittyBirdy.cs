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
public class BittyBirdy : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Bitty Birdy",
                "TargetModeBasic",
                "Blood Profile Normal",
                "WaveAnimationProfile")
            .SetStats(4, 1, 5)
            .SetSprites(
                Stormriders.GetSprite("Bitty_Birdie"),
                Stormriders.GetSprite("Bitty_BG"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(WhenAllyGainsThunderGainFrenzy.Name),
                ];
                card.greetMessages =
                [
                    "These waves are wild, but so am I, Captain!",
                    "This may be the worst storm I've seen... I can't wait to sail right into it!",
                    "The noise may scare the other birds, but it only makes me bolder!",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}