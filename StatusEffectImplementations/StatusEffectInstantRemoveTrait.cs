#region

using System.Collections;
using System.Linq;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectInstantRemoveTrait : StatusEffectInstant
{
    public TraitData trait;

    public override IEnumerator Process()
    {
        var traitStacks = target.traits.FirstOrDefault(t => t.data.name == trait.name);

        if (traitStacks == null)
        {
            yield return Remove();
            yield break;
        }
        
        traitStacks.count = 0;
        yield return target.UpdateTraits();
        target.display.promptUpdateDescription = true;
        target.PromptUpdate();
        yield return Remove();
    }
}