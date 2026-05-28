#region

using UnityEngine;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectApplyXWhenYAppliedToNotStatus : StatusEffectApplyXWhenYAppliedTo
{
    private new bool CheckType(StatusEffectData effectData)
    {
        return (whenAnyApplied && effectData.isStatus) || whenAppliedTypes.Contains(effectData.type);
    }

    public override bool RunApplyStatusEvent(StatusEffectApply apply)
    {
        if ((!adjustAmount && !instead) || !target.enabled || TargetSilenced() ||
            (!target.alive && targetMustBeAlive) || !apply.effectData || apply.count <= 0 ||
            !CheckType(apply.effectData) || !CheckTarget(apply.target))
        {
            return false;
        }

        if (instead)
        {
            apply.effectData = effectToApply;
        }

        if (!adjustAmount)
        {
            return false;
        }
        apply.count += addAmount;
        apply.count = Mathf.RoundToInt(apply.count * multiplyAmount);
        return false;
    }
}