using System.Collections;
using System.Linq;

namespace Stormriders.StatusEffectImplementations;

internal class StatusEffectApplyXWhenAllyDealsDamage : StatusEffectApplyX
{
    public TargetConstraint[] attackerConstraints;
    
    public override void Init()
    {
        PostHit += CheckHit;
    }

    public override bool RunPostHitEvent(Hit hit)
    {
        if (!hit.attacker)
        {
            return false;
        }
        var attacker = hit.attacker;
        return target.enabled && target.alive && hit.Offensive && Battle.IsOnBoard(target)
               && hit.target.alive
               && target.owner == attacker.owner && Battle.IsOnBoard(attacker)
               && attackerConstraints.All(constraint => constraint.Check(hit.attacker));
    }

    private IEnumerator CheckHit(Hit hit) => Run(GetTargets(hit), hit.damage + hit.damageBlocked);
}