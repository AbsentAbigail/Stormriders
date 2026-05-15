#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Clunkers;

[UsedImplicitly]
public class Frobisher : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Frobisher",
                idleAnim: "FloatAnimationProfile")
            .WithCardType("Clunker")
            .SetHealth(null)
            .SetDamage(null)
            .SetCounter(8)
            .SetSprites(
                Stormriders.GetSprite("Frobisher"),
                Stormriders.GetSprite("Frobisher_bg"))
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack("Scrap"),
                    Stormriders.SStack("Destroy Self After Turn"),
                    Stormriders.SStack(WhenAllyDealsDamageApplyEqualSnowToTarget.Name),
                ];
            });
    }
}