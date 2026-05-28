#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class SunMelonade : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Sun Melonade")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Sun_melonade"),
                Stormriders.GetSprite("Sun_melonade_bg"))
            .WithValue(50)
            .CanPlayOnHand(false)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack("Heal", 2),
                    Stormriders.SStack("Reduce Counter"),
                ];
            });
    }
}