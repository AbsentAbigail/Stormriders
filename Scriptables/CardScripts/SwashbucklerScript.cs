#region

using System.Linq;
using Stormriders.Builders.Traits;

#endregion

namespace Stormriders.Scriptables.CardScripts;

public class SwashbucklerScript : CardScript
{
    public TraitData trait = Stormriders.GetTrait(Captain.Name);
    public int increase = 3;
    
    public override void Run(CardData target)
    {
        if (!target.traits.RemoveWhere(traitstacks => traitstacks.data.name == trait.name))
        {
            return;
        }
        
        var thunder = target.startWithEffects.FirstOrDefault(status => status.data.type == "thunder");
        if (thunder is not null)
        {
            thunder.count += increase;
        }
        else
        {
            target.damage += increase;
        }
    }
}