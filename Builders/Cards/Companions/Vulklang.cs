using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Helpers;

namespace Stormriders.Builders.Cards.Companions;

[UsedImplicitly]
public class Vulklang : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Vulklang",
                "TargetModeBasic",
                "Blood Profile Normal",
                "Heartbeat2AnimationProfile")
            .SetStats(11, 3, 5)
            .SetSprites(
                Stormriders.GetSprite("Vulklang"),
                Stormriders.GetSprite("Vulklang_BG"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(WhenHitSmeltAllyBehind.Name, 2),
                ];
                card.greetMessages =
                [
                    "These baddies think they know storms? Let's show them a real storm.",
                    "The work never ends, young'in. I'm in if you need me.",
                    "Your weapons are weak. You'll never make it without a proper smith.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}