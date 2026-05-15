using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stormriders.Helpers;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectApplyXOnHitType : StatusEffectApplyX
{
    public string[] hitTypes = ["basic", "thunder"];
    public TargetConstraint[] attackerConstraints;
    public TargetConstraint[] hitTargetConstraints;
    public override void Init()
    {
        OnHit += CheckHit;
    }

    public override bool RunHitEvent(Hit hit)
    {
        if (hit.attacker != target)
        {
            return false;
        }
     
        if (!hitTypes.Contains(hit.damageType))
        {
            return false;
        }

        return attackerConstraints.All(constraint => constraint.Check(target))
               && hitTargetConstraints.All(constraint => constraint.Check(hit.target));
    }

    private IEnumerator CheckHit(Hit hit)
    {
        yield return Run(GetTargets(hit), hit.damage + hit.damageBlocked);
    }
}