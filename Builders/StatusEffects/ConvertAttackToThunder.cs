using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;
using WildfrostHopeMod.VFX;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class ConvertAttackToThunder : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantApplyEffect>(Name)
            .WithText($"Convert {Stormriders.VanillaKeywordTag("attack")} to {Stormriders.KeywordTag(Keywords.Thunder.Name)}")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectInstantApplyEffect>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(Thunder.Name);
                status.targetConstraints =
                [
                    TargetConstraintHelper.AttackMoreThan(0)
                ];
                status.scriptableAmount = new Script<ScriptableCurrentAttack>();
            });
    }
}