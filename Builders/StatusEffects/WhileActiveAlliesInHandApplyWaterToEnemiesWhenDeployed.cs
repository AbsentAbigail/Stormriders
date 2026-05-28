#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhileActiveAlliesInHandApplyWaterToEnemiesWhenDeployed : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectWhileActiveX>(Name)
            .WithText($"When an ally is deployed, apply <{{a}}>{Stormriders.KeywordTag("water")} to all enemies")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(WhenDeployedApplyWaterToEnemiesNoText.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Hand;
            });
    }
}