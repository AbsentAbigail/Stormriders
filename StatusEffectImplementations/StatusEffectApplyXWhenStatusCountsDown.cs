namespace Stormriders.StatusEffectImplementations;

internal class StatusEffectApplyXWhenStatusCountsDown : StatusEffectApplyX
{
    public string statusType = "water";
    public ApplyToFlags? CountDownFlags;
    public override void Init()
    {
        Events.OnStatusEffectCountDown += Check;
    }
    
    public void OnDestroy()
    {
        Events.OnStatusEffectCountDown -= Check;
    }
    
    private void Check(StatusEffectData status, ref int amount)
    {
        if (status.type != statusType)
        {
            return;
        }

        if (!CheckUnit(status.target))
        {
            return;
        }

        ActionQueue.Stack(new ActionSequence(Run(GetTargets(), amount)));
    }

    private bool CheckUnit(Entity unit)
    {
        if (CountDownFlags == null)
        {
            return true;
        }
        
        var oldFlags = applyToFlags;
        applyToFlags = CountDownFlags.Value;
        var result = GetTargets().Contains(unit);
        applyToFlags = oldFlags;
        return result;
    }
}