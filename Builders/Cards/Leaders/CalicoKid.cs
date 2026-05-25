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
public class CalicoKid : ILeaderBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Calico Kid",
                "TargetModeBasic",
                "Blood Profile Normal")
            .SetStats(9, null, 8)
            .SetSprites(
                Stormriders.GetSprite("Calico"),
                Stormriders.GetSprite("Calico_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack(Thunder.Name, 2),
                    Stormriders.SStack(OnCardPlayedLoseThunder.Name),
                    Stormriders.SStack(WhenHitGainThunder.Name, 2),
                ];
                card.traits =
                [
                    Stormriders.TStack(Captain.Name),
                    Stormriders.TStack("Smackback"),
                ];
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public bool LeaderExclusive => true;

    public bool InPool => true;
    
    public ILeaderBuilder.LeaderModifier LeaderModifiers => new();
}