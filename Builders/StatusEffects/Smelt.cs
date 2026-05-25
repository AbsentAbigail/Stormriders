using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class Smelt : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectSmelt>(Name)
            .WithText($"{Stormriders.KeywordTag("smelt")} <{{a}}>")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectSmelt>(status =>
            {
                status.type = "smelt";
                status.effectToApply = Stormriders.GetStatus(Thunder.Name);
                status.targetConstraints =
                [
                    TargetConstraintHelper.AttackOrThunder()
                ];
            });
    }
}