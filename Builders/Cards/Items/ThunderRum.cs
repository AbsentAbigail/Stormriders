#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class ThunderRum : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Thunder Rum")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Thunder_rum"),
                Stormriders.GetSprite("Thunder_rum_bg"))
            .WithValue(50)
            .CanPlayOnHand()
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(ConvertAttackToThunder.Name),
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedIncreaseThunderOfCaptains.Name)
                ];
                card.traits =
                [
                    Stormriders.TStack("Consume")
                ];
            });
    }
}