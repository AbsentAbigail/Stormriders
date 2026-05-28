#region

using System.Collections;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectWater : StatusEffectData
{
    public override void Init()
    {
        OnStack += Stack;
    }

    private IEnumerator Stack(int stacks)
    {
        var spice = target.FindStatus("spice");
        if (spice is null)
        {
            yield break;
        }
        yield return spice.Remove();

    }
}