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
public class Powder : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Powder",
                "TargetModeBasic",
                "Blood Profile Normal",
                "Heartbeat2AnimationProfile")
            .SetStats(3, 0, 2)
            .SetSprites(
                Stormriders.GetSprite("Powder"),
                Stormriders.GetSprite("Powder_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(OnCardPlayedBoostSelf.Name),
                    Stormriders.SStack(WhenDestroyedSmeltAllCaptainAllies.Name),
                ];
                card.traits =
                [
                    Stormriders.TStack("Explode")
                ];
                card.greetMessages =
                [
                    "(excited monkey noises)",
                    "(mock explosion noises, followed by a big grin)",
                    "(the rhythmic thumping of a monkey hopping on a canon)",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}