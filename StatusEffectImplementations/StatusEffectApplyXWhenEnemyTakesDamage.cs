#region

using System.Collections;
using System.Linq;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectApplyXWhenEnemyTakesDamage : StatusEffectApplyX
{
    public bool ignoreOwnDamage;
    public TargetConstraint[] unitConstraints;
    public bool self;
    public bool enemy = true;
    public bool ally;
    
    public override void Init()
    {
        PostHit += CheckHit;
    }

    public override bool RunPostHitEvent(Hit hit)
    {
        if (!hit.target.alive)
        {
            return false;
        }
        
        if (ignoreOwnDamage && hit.attacker == target)
        {
            return false;
        }

        if (!self && hit.target == target)
        {
            return false;
        }

        if (!ally && hit.target.owner == target.owner)
        {
            return false;
        }

        if (!enemy && hit.target.owner != target.owner)
        {
            return false;
        }
        
        if (unitConstraints.Any(constraint => !constraint.Check(hit.target)))
        {
            return false;
        }
        
        return target.enabled && target.alive && hit.Offensive && hit.damage > 0 && Battle.IsOnBoard(target);
    }

    private IEnumerator CheckHit(Hit hit) => Run(GetTargets(hit));
}