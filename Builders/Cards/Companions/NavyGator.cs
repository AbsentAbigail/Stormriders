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
public class NavyGator : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Navy Gator",
                "TargetModeBasic",
                "Blood Profile Fungus")
            .SetStats(6, 2, 6)
            .SetSprites(
                Stormriders.GetSprite("Navy_Gator"),
                Stormriders.GetSprite("Navy_Gator_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedNavigateAllyAhead.Name),
                    Stormriders.SStack(WhileActiveEnemiesRetainWater.Name),
                ];
                card.greetMessages =
                [
                    "Well hullo there, Captain! Ready to chart a course for success?",
                    "Keep sight of your goals, and you'll never lose your way!",
                    "A messy storm this is, but I still know my way through it.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}