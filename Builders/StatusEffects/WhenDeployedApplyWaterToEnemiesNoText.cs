#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenDeployedApplyWaterToEnemiesNoText : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenDeployed>(Name)
            .WithStackable(true)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(Water.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
            });
    }
}