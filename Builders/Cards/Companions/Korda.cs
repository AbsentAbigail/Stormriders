#region

using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Stormriders.Builders.Cards.Companions;

[UsedImplicitly]
public class Korda : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Korda",
                bloodProfile: "Blood Profile Berry")
            .SetStats(9, 3, 5)
            .SetSprites(
                Stormriders.GetSprite("Korda"),
                Stormriders.GetSprite("Korda_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(WhenCaptainHitApplyWaterToAttacker.Name, 4),
                    Stormriders.SStack(OnKillGainCaptain.Name),
                ];
                card.greetMessages =
                [
                    "I'll follow your orders, Captain. But if the crew votes I take your place, I won't say no.",
                    "You're getting some charm and bling on this venture, aren't you? Care to share some of that with the rest of us?",
                    "I'll keep everything ship-shape, my friend.",
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}