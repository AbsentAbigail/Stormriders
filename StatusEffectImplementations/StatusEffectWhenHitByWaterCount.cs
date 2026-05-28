#region

using System.Collections;

#endregion

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