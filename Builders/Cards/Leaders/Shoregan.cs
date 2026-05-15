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
public class Shoregan : ILeaderBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Shoregan",
                "TargetModeBasic",
                "Blood Profile Blue (x2)")
            .SetStats(8, 1, 3)
            .SetSprites(
                Stormriders.GetSprite("Shoregan"),
                Stormriders.GetSprite("Shoregan_BG"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(Thunder.Name),
                ];
                card.traits =
                [
                    Stormriders.TStack(Captain.Name),
                    Stormriders.TStack("Barrage"),
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public bool LeaderExclusive => true;

    public bool InPool => true;
    
    public ILeaderBuilder.LeaderModifier LeaderModifiers => new();
}