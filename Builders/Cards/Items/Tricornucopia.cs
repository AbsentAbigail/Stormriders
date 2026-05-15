#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class Tricornucopia : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Tricornucopia")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Tricornucopia"),
                Stormriders.GetSprite("Tricorn_bg"))
            .WithValue(50)
            .CanPlayOnHand()
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(InstantApplyCaptain.Name),
                ];
                card.traits =
                [
                    Stormriders.TStack("Consume"),
                ];
            });
    }
}