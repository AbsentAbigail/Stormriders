#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class ShockHammer : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Shock Hammer")
            .SetDamage(2)
            .SetSprites(
                Stormriders.GetSprite("Shock_hammer"),
                Stormriders.GetSprite("Shock_hammer_bg"))
            .WithValue(50)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(InstantDoubleThunder.Name),
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack("On Card Played Apply Attack To Self", 2),
                ];
            });
    }
}