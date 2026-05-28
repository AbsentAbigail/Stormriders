#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenDestroyedSmeltAllCaptainAllies : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenDestroyed>(Name)
            .WithText($"When destroyed, {Stormriders.KeywordTag("smelt")} <{{a}}> to all {Stormriders.KeywordTag("captain")} allies")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(status =>
            {
                status.targetMustBeAlive = false;
                status.effectToApply = Stormriders.GetStatus(Smelt.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                status.applyConstraints =
                [
                    TargetConstraintHelper.HasTrait(Captain.Name)
                ];
            });
    }
}