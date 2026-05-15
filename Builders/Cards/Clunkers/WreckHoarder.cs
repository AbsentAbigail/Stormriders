#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class WreckHoarder : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Wreck Hoarder",
                idleAnim: "SwayAnimationProfile")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Wreck_hoarder"),
                Stormriders.GetSprite("Wreck_hoarder_bg"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap", 2),
                    Stormriders.SStack(WhenWaterdEnemyDiesGainScrap.Name),
                ];
            });
    }
}