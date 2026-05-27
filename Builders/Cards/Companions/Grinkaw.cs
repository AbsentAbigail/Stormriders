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
public class Grinkaw : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Grinkaw",
                "TargetModeBasic",
                "Blood Profile Pink Wisp",
                "HangAnimationProfile")
            .SetStats(3, 2, 3)
            .SetSprites(
                Stormriders.GetSprite("Grinkaw"),
                Stormriders.GetSprite("Grinkaw_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.attackEffects =
                [
                    Stormriders.SStack(InstantApplyFrostOrWaterIfCaptainInRow.Name),
                ];
                card.traits =
                [
                    Stormriders.TStack("Longshot")
                ];
                card.AddToPets();
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}