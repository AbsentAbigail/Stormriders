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
public class Tunar : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Tunar",
                "TargetModeBasic",
                "Blood Profile Blue (x2)")
            .SetStats(5, 0, 5)
            .SetSprites(
                Stormriders.GetSprite("Tunar"),
                Stormriders.GetSprite("Tunar_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(InstantDoubleWater.Name),
                ];
                card.startWithEffects =
                [
                    Stormriders.SStack("Hit All Enemies")
                ];
                card.greetMessages =
                [
                    "Looks like the school field trip is being redirected right into the heart of the storm!",
                    "It was scary being frozen in ice, but at least I wasn't alone.",
                    "Let's teach those frosties a lesson they'll never forget!",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}