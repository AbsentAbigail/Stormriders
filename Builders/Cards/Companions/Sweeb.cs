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
public class Sweeb : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Sweeb")
            .SetStats(4, 1, 3)
            .SetSprites(
                Stormriders.GetSprite("Sweeb"),
                Stormriders.GetSprite("Sweeb_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedSmeltAlliedCaptains.Name),
                    Stormriders.SStack(OnCardPlayedApplyWaterToRandomEnemy.Name),
                ];
                card.greetMessages =
                [
                    "Looks like a proper mess out there, Captain. Need ole Sweeb to lend a hand?",
                    "You could use a bit of support, huh? Let Sweeb buff up your blade.",
                    "Feel like I was trapped in that ice for a hundred years. Could have been worse though.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}