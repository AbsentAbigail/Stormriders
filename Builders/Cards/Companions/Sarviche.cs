#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.Cards.Companions;

[UsedImplicitly]
public class Sarviche : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Sarviche",
                "TargetModeBasic",
                "Blood Profile Berry")
            .SetStats(7, 1, 5)
            .SetSprites(
                Stormriders.GetSprite("Sarviche"),
                Stormriders.GetSprite("Sarviche_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(Water.Name, 3),
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedHealCaptainAllies.Name, 3)
                ];
                card.greetMessages =
                [
                    "You look hungry, Captain. Can't fight evil on an empty stomach now can you?",
                    "Didn't anyone tell these guys I always use fresh food, never frozen?",
                    "The audacity of these creatures leaves an awful bitter taste in my mouth.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}