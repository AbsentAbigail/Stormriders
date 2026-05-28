#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.StatusEffectImplementations;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantSmeltSelfAndCaptains : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantMultiple>(Name)
            .WithText($"{Stormriders.KeywordTag("smelt")} <{{a}}> to self and all {Stormriders.KeywordTag("captain")}s")
            .WithStackable(true)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantMultiple>(status =>
            {
                status.effects =
                [
                    Stormriders.GetStatusOf<StatusEffectSmelt>(Smelt.Name)
                ];
                status.applyXEffects =
                [
                    Stormriders.GetStatusOf<StatusEffectApplyXInstant>(InstantSmeltCaptains.Name)
                ];
            });
    }
}