using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class Sinkapacitor : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Sinkapacitor",
                idleAnim: "ShakeAnimationProfile")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(null)
            .WithValue(45)
            .SetSprites(
                Stormriders.GetSprite("Sinkapacitor"),
                Stormriders.GetSprite("Sinkapacitor_bg"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap"),
                    Stormriders.SStack(WhenThunderAllyTakesDamageApplyThunder.Name),
                ];
            });
    }
}