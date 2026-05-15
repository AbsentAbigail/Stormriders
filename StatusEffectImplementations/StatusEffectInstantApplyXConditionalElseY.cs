#region

using System.Collections;
using System.Linq;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectInstantApplyXConditionalElseY : StatusEffectInstantApplyEffect
{
    public TargetConstraint[] applierConditions;
    public TargetConstraint[] targetConditions;
    public StatusEffectData conditionalEffectToApply;
    public StatusEffectData otherEffectToApply;

    public override IEnumerator Process()
    {
        if (applierConditions.Any(condition => !condition.Check(applier))
            || targetConditions.Any(condition => !condition.Check(target)))
        {
            effectToApply = otherEffectToApply;
        }
        else
        {
            effectToApply = conditionalEffectToApply;
        }
        
        yield return base.Process();
    }
}