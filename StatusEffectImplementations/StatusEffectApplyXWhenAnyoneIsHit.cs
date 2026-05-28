#region

using System.Collections;
using System.Linq;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectApplyXWhenAnyoneIsHit : StatusEffectApplyX
{
    public TargetConstraint[] constraints;
    public TargetConstraint[] attackerConstraints;

    public override void Init()
    {
        PostHit += Check;
    }

    public override bool RunPostHitEvent(Hit hit)
    {
        if (!target.enabled || !Battle.IsOnBoard(target))
        {
            return false;
        }

        if (!ReactToHit(hit))
        {
            return false;
        }

        if (!(hit.target && Battle.IsOnBoard(hit.target)))
        {
            return false;
        }

        return attackerConstraints.All(constraint => constraint.Check(hit.attacker))
               && constraints.All(constraint => constraint.Check(hit.target));
    }

    private IEnumerator Check(Hit hit) => Run(GetTargets(hit));

    private static bool ReactToHit(Hit hit)
    {
        return hit.canRetaliate && hit.Offensive && hit.BasicHit;
    }
}