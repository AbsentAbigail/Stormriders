#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class CarversTools : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Carver's Tools")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Carvers_tools"),
                Stormriders.GetSprite("Carvers_tools_bg"))
            .WithValue(50)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack("Instant Add Scrap", 3),
                    Stormriders.SStack(InstantRemoveCaptainFromRandomAlly.Name),
                ];
                card.traits =
                [
                    Stormriders.TStack("Consume"),
                ];
            });
    }
}