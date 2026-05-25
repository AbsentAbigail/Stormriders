#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class Hurricanon : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Hurricanon")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Hurricanon"),
                Stormriders.GetSprite("Hurricanon_bg"))
            .WithValue(50)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(Thunder.Name, 8),
                    Stormriders.SStack(OnCardPlayedDealDamageToFrontAlly.Name, 4),
                ];
            });
    }
}