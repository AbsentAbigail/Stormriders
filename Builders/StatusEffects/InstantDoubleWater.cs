using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantDoubleWater : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantDoubleX>(Name)
            .WithText($"Double the target's {Stormriders.KeywordTag(Keywords.Water.Name)}")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantDoubleX>(status =>
            {
                status.statusToDouble = Stormriders.GetStatus(Water.Name);
                status.targetConstraints =
                [
                    TargetConstraintHelper.HasStatus(Water.Name)
                ];
            });
    }
}