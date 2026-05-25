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
public class FatherGilly : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Father Gilly",
                "TargetModeBasic",
                "Blood Profile Fungus")
            .SetStats(12, 3, 6)
            .SetSprites(
                Stormriders.GetSprite("Father_Gilly"),
                Stormriders.GetSprite("Father_Gilly_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedHealSelfEqualToWaterOnEnemiesInRow.Name),
                    Stormriders.SStack(WhenRedrawBellHitApplyWaterToEnemiesInRow.Name, 2),
                ];
                card.greetMessages =
                [
                    "The bells toll for all of us, of sea and land, both hand in hand.",
                    "I am in your service, our souls ring as one.",
                    "Prayer alone is not enough, we must stand up against the evils.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}