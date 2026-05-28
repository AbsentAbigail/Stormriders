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
public class Shawreck : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Shawreck",
                bloodProfile: "Blood Profile: Blue x2")
            .SetStats(5, 5, 5)
            .SetSprites(
                Stormriders.GetSprite("Shawreck"),
                Stormriders.GetSprite("Shawreck_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(WhenCardDestroyedApplyWaterToRandomEnemyAndHealthToSelf.Name),
                ];
                card.greetMessages =
                [
                    "Did you dredge me up from the sunken graveyard? No? A pillar of ice? Who the heck put me in there?",
                    "I'm coming along to find what precious garbage you might discard.",
                    "You never know what you're going to find out there. Isn't that fun?",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}