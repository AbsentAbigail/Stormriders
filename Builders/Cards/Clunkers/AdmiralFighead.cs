#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class AdmiralFighead : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Admiral Fighead",
                idleAnim: "SwayAnimationProfile")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Fighead"),
                Stormriders.GetSprite("Fighead_bg"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap", 2),
                    Stormriders.SStack(WhenDestroyedAddCaptainToRandomAlly.Name),
                ];
            });
    }
}