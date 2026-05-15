#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class DampFlogger : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Damp Flogger",
                idleAnim: "ShakeAnimationProfile")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(0)
            .SetSprites(
                Stormriders.GetSprite("Dampflogger"),
                Stormriders.GetSprite("Dampflogger_BG"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(Water.Name, 4)
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap", 4),
                    Stormriders.SStack("On Card Played Lose Scrap To Self"),
                    Stormriders.SStack(WhenWaterdEnemyTakesDamageTrigger.Name),
                ];
                card.traits =
                [
                    Stormriders.TStack("Aimless")
                ];
            });
    }
}