using System.Collections;

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectSmelt : StatusEffectInstant
{
    public StatusEffectData effectToApply;
    
    public override IEnumerator Process()
    {
        var damage = target.damage.current;
        if (damage > 0)
        {
            target.damage.current = 0;
            target.damage.max = 0;
            target.data.hasAttack = false;
            target.display.RemoveStatusIcon("damage", "damage");
            yield return StatusEffectSystem.Apply(target, applier, effectToApply, damage);
            yield return Remove();
            yield break;
        }
        
        var thunder = target.FindStatus("thunder");
        if (thunder == null)
        {
            yield break;
        }
        yield return StatusEffectSystem.Apply(target, applier, effectToApply, count);
        yield return Remove();

    }
}