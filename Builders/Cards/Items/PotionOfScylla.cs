#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class PotionOfScylla : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Potion of Scylla")
            .SetDamage(0)
            .SetSprites(
                Stormriders.GetSprite("Potion_of_Scylla"),
                Stormriders.GetSprite("Scylla_Bg"))
            .WithValue(50)
            .CanPlayOnHand(false)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack("MultiHit"),
                    Stormriders.SStack(InstantDealDamage.Name, 6),
                ];
            });
    }
}