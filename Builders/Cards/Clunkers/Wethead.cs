using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class Wethead : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Wethead")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(null)
            .WithValue(45)
            .SetSprites(
                Stormriders.GetSprite("Wethead"),
                Stormriders.GetSprite("Wethead_bg"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap", 2),
                    Stormriders.SStack(WhenHitApplyWaterToAttacker.Name, 2)
                ];
            });
    }
}