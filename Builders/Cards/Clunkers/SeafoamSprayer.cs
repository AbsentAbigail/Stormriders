#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class SeafoamSprayer : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Seafoam Sprayer",
                idleAnim: "SwayAnimationProfile")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Seafoam_sprayer"),
                Stormriders.GetSprite("Seafoam_sprayer_bg"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap"),
                    Stormriders.SStack(WhenWaterdEnemyDiesApplyTheirWaterToRandomEnemyMultipleTimes.Name, 2),
                ];
            });
    }
}