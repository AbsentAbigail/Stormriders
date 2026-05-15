using System.Collections;
using System.Linq;
using Stormriders.Helpers;

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectNavigator : StatusEffectInstant
{
    public DataFile[] stages;
    
    public override IEnumerator Process()
    {
        for (var i = stages.Length - 2; i >= 0; i--)
        {
            var data = stages[i];
            if (!HasEffect(data))
            {
                continue;
            }
            yield return RemoveEffect(data);
            yield return ApplyEffect(i+1);
            yield return target.UpdateTraits();
            target.display.promptUpdateDescription = true;
            target.PromptUpdate();
            yield return Remove();
            yield break;
        }
        
        yield return Remove();
    }

    private bool HasEffect(DataFile dataFile)
    {
        switch (dataFile)
        {
            case TraitData when target.traits.Any(t => t.data.name == dataFile.name):
            case StatusEffectData when target.statusEffects.Any(s => s.name == dataFile.name):
                return true;
            default:
                return false;
        }
    }

    private IEnumerator ApplyEffect(int i)
    {
        if (i >= stages.Length)
        {
            yield break;
        }
        var data = stages[i];
        if (data is StatusEffectData status)
        {
            
            LogHelper.Log($"Applying status {data.name}");
            yield return StatusEffectSystem.Apply(target, applier, status, 1);
            yield break;
        }

        
        LogHelper.Log($"Applying trait {data.name}");
        target.GainTrait((TraitData)data, 1);
    }

    private IEnumerator RemoveEffect(DataFile data)
    {
        if (data is StatusEffectData status)
        {
            var targetStatus = target.FindStatus(status);
            if (!targetStatus)
            {
                yield break;
            }

            yield return targetStatus.Remove();
            yield break;
        }

        if (data is not TraitData trait)
        {
            yield break;
        }

        var traitStacks = target.traits.FirstOrDefault(t => t.data.name == trait.name);

        traitStacks?.count = 0;
    }
}