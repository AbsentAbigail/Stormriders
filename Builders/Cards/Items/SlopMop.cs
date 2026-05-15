#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class SlopMop : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Slop Mop")
            .SetDamage(1)
            .SetSprites(
                Stormriders.GetSprite("Slop_mop"),
                Stormriders.GetSprite("Slop_mop_bg"))
            .WithValue(50)
            .CanPlayOnHand(false)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(Water.Name, 2)
                ];
            });
    }
}