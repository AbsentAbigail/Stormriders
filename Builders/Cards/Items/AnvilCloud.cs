#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class AnvilCloud : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Anvil Cloud")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Anvil_cloud"),
                Stormriders.GetSprite("Anvil_cloud_bg"))
            .WithValue(50)
            .CanPlayOnHand()
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(Smelt.Name, 4),
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack("On Card Played Reduce Attack Effect 1 To Self"),
                ];
            });
    }
}