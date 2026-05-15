#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;

#endregion

namespace Stormriders.Builders.Cards.Items;

[UsedImplicitly]
public class SlushBolas : ICardBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateItem(Name, "Slush Bolas",
                idleAnim: "ShakeAnimationProfile")
            .SetDamage(0)
            .SetSprites(
                Stormriders.GetSprite("Slush_Bolas"),
                Stormriders.GetSprite("Slush_Bolas_BG"))
            .WithValue(50)
            .CanPlayOnHand(false)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack("Snow"),
                    Stormriders.SStack(Water.Name)
                ];
                card.traits = [Stormriders.TStack("Noomlin")];
            });
    }
}