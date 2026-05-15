#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class Leviathinner : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Leviathinner",
                idleAnim: "WaveAnimationProfile")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(0)
            .SetSprites(
                Stormriders.GetSprite("Leviathinner"),
                Stormriders.GetSprite("Leviathinner_bg"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(Water.Name, 2)
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap"),
                    Stormriders.SStack("Trigger When Ally In Row Attacks")
                ];
            });
    }
}