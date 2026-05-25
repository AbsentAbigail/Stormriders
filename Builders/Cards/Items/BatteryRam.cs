#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class BatteryRam : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Battery Ram")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Battery_ram"),
                Stormriders.GetSprite("Battery_ram_bg"))
            .WithValue(50)
            .CanPlayOnHand()
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack("Increase Max Counter"),
                    Stormriders.SStack(Smelt.Name, 5),
                ];
                card.traits =
                [
                    Stormriders.TStack("Consume"),
                ];
            });
    }
}