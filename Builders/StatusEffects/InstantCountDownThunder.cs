using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantCountDownThunder : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantCountDownStatus>(Name)
            .WithText($"Count down {Stormriders.KeywordTag(Keywords.Thunder.Name)} by <{{a}}>")
            .WithStackable(true)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantCountDownStatus>(status =>
            {
                status.types = ["thunder"];
                status.targetConstraints =
                [
                    TargetConstraintHelper.HasStatus(Thunder.Name)
                ];
            });
    }
}