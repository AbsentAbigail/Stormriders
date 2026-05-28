#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class ConvertThunderToAttack : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantConvertThunderToAttack>(Name)
            .WithText($"Convert {Stormriders.KeywordTag(Keywords.Thunder.Name)} to {Stormriders.VanillaKeywordTag("attack")}")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantConvertThunderToAttack>(status =>
            {
                status.targetConstraints =
                [
                    TargetConstraintHelper.HasStatus(Thunder.Name),
                    TargetConstraintHelper.HealthMoreThan(0),
                ];
            });
    }
}