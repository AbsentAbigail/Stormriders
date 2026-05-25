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
public class Clover : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Clover",
                "TargetModeBasic",
                "Blood Profile Normal",
                "WaveAnimationProfile")
            .SetStats(4, 4, 4)
            .SetSprites(
                Stormriders.GetSprite("Clover"),
                Stormriders.GetSprite("Clover_BG"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedDealDamageToAllyBehind.Name),
                    Stormriders.SStack(OnCardPlayedDealDamageToAllyAhead.Name, 3),
                    Stormriders.SStack(OnCardPlayedChance1In4Bling.Name, 4),
                ];
                card.traits =
                [
                    Stormriders.TStack("Barrage")
                ];
                card.greetMessages =
                [
                    "It's your lucky day, I'm the best gunner around these parts!",
                    "I go BLAM and BLAM and BLAM and sometimes I hit someone I didn't mean to but then I shrug it off and KEEP SHOOTING",
                    "Stay outta my crosshairs and we'll get along just fine.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}