#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class LuminSandJar : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Lumin Sand Jar")
            .SetDamage(null)
            .SetSprites(
                Stormriders.GetSprite("Lumin_Sand"),
                Stormriders.GetSprite("Lumin_Sand_BG"))
            .WithValue(50)
            .CanPlayOnHand()
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(IncreaseEffectsText.Name),
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack(ReduceAllWater.Name, 3),
                ];
            });
    }
}