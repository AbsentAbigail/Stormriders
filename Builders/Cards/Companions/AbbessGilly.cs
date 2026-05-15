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
public class AbbessGilly : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Abbess Gilly",
                "TargetModeBasic",
                "Blood Profile Fungus")
            .SetStats(10, null, 5)
            .SetSprites(
                Stormriders.GetSprite("Abbess_Gilly"),
                Stormriders.GetSprite("Abbess_Gilly_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedIncreaseHealthOfAlliesInRow.Name, 3),
                    Stormriders.SStack(WhenEnemyGainsWaterCountDownRedrawBell.Name),
                ];
                card.greetMessages =
                [
                    "The bells toll for all of us, of land and sea and in between",
                    "I am in your service, our hearts are in union.",
                    "Prayer alone is not enough, we must rise to the call.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}