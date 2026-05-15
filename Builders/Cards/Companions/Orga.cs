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
public class Orga : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Orga",
                "TargetModeBasic",
                "Blood Profile Blue (x2)",
                "FloatAnimationProfile")
            .SetStats(5, 4, 5)
            .SetSprites(
                Stormriders.GetSprite("Orga_Sprite"),
                Stormriders.GetSprite("Orga_BG"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedGainHpEqualToWaterOnBoard.Name),
                    Stormriders.SStack(WhenDeployedApplyWaterToEnemies.Name, 2),
                ];
                card.greetMessages =
                [
                    "It's too cold in these waters, even for me. Let's fix that.",
                    "You caught me in the middle of a song. Wanna hear an encore?",
                    "What are we huntin', captain? I sure could go for a bite.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}