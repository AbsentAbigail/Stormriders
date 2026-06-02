namespace Stormriders.StatusEffectImplementations;

public class StatusEffectApplyXWhenYAppliedToOnlyAlly : StatusEffectApplyXWhenYAppliedToAlly
{
    public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
    {
        return apply.target != target && base.RunPostApplyStatusEvent(apply);
    }
}