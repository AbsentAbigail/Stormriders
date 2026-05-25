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
public class Suturgeon : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Suturgeon",
                "TargetModeBasic",
                "Blood Profile: Blue x2")
            .SetStats(8, 1, 4)
            .SetSprites(
                Stormriders.GetSprite("Suturgeon"),
                Stormriders.GetSprite("Suturgeon_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedIncreaseLowestHealthAllyHealthEqualToWater.Name),
                    Stormriders.SStack(WhileActiveAlliesInHandApplyWaterToEnemiesWhenDeployed.Name),
                ];
                card.greetMessages =
                [
                    "Do you require medical assistance, little one? Allow me to help",
                    "There is love in these waters... and surgical tools in my bag",
                    "We are all in this together, and that makes us strong.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}