#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class HailBottle : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Hail Bottle")
            .SetDamage(0)
            .SetSprites(
                Stormriders.GetSprite("Hail_bottle"),
                Stormriders.GetSprite("Hail_bottle_bg"))
            .WithValue(50)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(InstantSnowEqualToWater.Name),
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack("MultiHit", 2),
                ];
                card.traits =
                [
                    Stormriders.TStack("Aimless"),
                ];
            });
    }
}