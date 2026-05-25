using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantReduceOwnCounterAndApplyWaterToEnemies : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantMultiple>(Name)
            .WithStackable(true)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantMultiple>(status =>
            {
                status.effects = [
                    Stormriders.GetStatusOf<StatusEffectInstant>("Reduce Max Counter"),
                ];
                status.applyXEffects =
                [
                    Stormriders.GetStatusOf<StatusEffectApplyXInstant>(InstantApplyWaterToEnemiesInRow.Name),
                ];
            });
    }
}