#region

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectHaltX : StatusEffectData
{
    public StatusEffectData effectToHalt;
    public bool ignoreSilence = true;

    public override void Init()
    {
        Events.OnStatusEffectCountDown += StatusCountDown;
    }

    public void OnDestroy()
    {
        Events.OnStatusEffectCountDown -= StatusCountDown;
    }

    public void StatusCountDown(StatusEffectData status, ref int amount)
    {
        if (status.type != effectToHalt.type || status.target != target || Silenced())
            return;
        amount = 0;
    }

    public bool Silenced() => target.silenced && !ignoreSilence;
}
