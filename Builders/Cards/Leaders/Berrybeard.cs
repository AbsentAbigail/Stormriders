#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.Cards.Leaders;

[UsedImplicitly]
public class Berrybeard : ILeaderBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Berrybeard",
                "TargetModeBasic",
                "Blood Profile Berry",
                "FloatSquishAnimationProfile")
            .SetStats(12, null, 5)
            .SetSprites(
                Stormriders.GetSprite("Berrybeard"),
                Stormriders.GetSprite("Berrybeard_BG"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(Thunder.Name),
                    Stormriders.SStack(OnHitAgainstWaterRestoreHpEqualToDamageDealt.Name),
                ];
                card.traits =
                [
                    Stormriders.TStack(Captain.Name)
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public bool LeaderExclusive => true;

    public bool InPool => true;
    
    public ILeaderBuilder.LeaderModifier LeaderModifiers => new();
}