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
public class Maryanne : ILeaderBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Maryanne",
                "TargetModeBasic",
                "Blood Profile Normal",
                "ShakeAnimationProfile")
            .SetStats(9, null, 4)
            .SetSprites(
                Stormriders.GetSprite("Maryanne"),
                Stormriders.GetSprite("Maryanne_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(Thunder.Name, 2),
                    Stormriders.SStack("MultiHit"),
                ];
                card.traits =
                [
                    Stormriders.TStack("Aimless"),
                    Stormriders.TStack(Captain.Name),
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public bool LeaderExclusive => true;

    public bool InPool => true;
    
    public ILeaderBuilder.LeaderModifier LeaderModifiers => new();
}