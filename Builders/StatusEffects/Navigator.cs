using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;
using WildfrostHopeMod.VFX;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class Navigator : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectNavigator>(Name)
            .WithText("Navigator")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectNavigator>(status =>
            {
                status.stages =
                [
                    Stormriders.GetTrait("Aimless"),
                    Stormriders.GetTrait("Longshot"),
                    Stormriders.GetTrait("Barrage"),
                    Stormriders.GetStatus("Hit All Enemies"),
                ];
            });
    }
}