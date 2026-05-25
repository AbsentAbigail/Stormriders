#region

using System.Collections;
using DeadExtensions;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectApplyXInstantChance : StatusEffectApplyXInstant
{
    public int numerator;
    public int denominator;

    public override void Init()
    {
        OnBegin += Process;
    }

    private new IEnumerator Process()
    {
        if (PettyRandom.Range(0, denominator) > numerator)
        {
            yield return Remove();
            yield break;
        }
        
        yield return base.Process();
    }
}