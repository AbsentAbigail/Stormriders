using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stormriders.Helpers;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectWhenHitByWaterCount : StatusEffectApplyXWhenHit
{
    private string statusType = "water";
    // Used by Kokolockays scriptable image
    public int eatCount;
    
    public override void Init()
    {
        PostHit += Check;
    }

    private IEnumerator Check(Hit hit)
    {
        yield return CheckHit(hit);
        eatCount++;
    }

    public override bool RunPostHitEvent(Hit hit)
    {
        return base.RunPostHitEvent(hit) && hit.attacker.FindStatus(statusType)?.count >= GetAmount();
    }
}