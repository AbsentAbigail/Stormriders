#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantRemoveCaptain : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantRemoveTrait>(Name)
            .WithText($"Remove {Stormriders.KeywordTag(Captain.Name)}")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantRemoveTrait>(status =>
            {
                status.trait = Stormriders.GetTrait(Traits.Captain.Name);
                status.targetConstraints =
                [
                    TargetConstraintHelper.HasTrait(Traits.Captain.Name)
                ];
            });
    }
}