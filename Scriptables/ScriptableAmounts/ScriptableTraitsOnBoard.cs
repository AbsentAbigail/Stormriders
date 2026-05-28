#region

using System.Linq;
using UnityEngine;

#endregion

namespace Stormriders.Scriptables.ScriptableAmounts;

internal class ScriptableTraitsOnBoard : ScriptableAmount
{
    public TraitData trait;
    public bool self;
    public bool allies;
    public bool enemies;
    public float multiplier = 1;

    public override int Get(Entity entity)
    {
        var result = 0;
        if (self)
        {
            result += Count(entity);
        }
        if (allies)
        {
            result += entity.GetAllies().Sum(Count);
        }
        if (enemies)
        {
            result += entity.GetAllEnemies().Sum(Count);
        }

        return Mathf.FloorToInt(result * multiplier);
    }

    private int Count(Entity entity)
    {
        var t = entity?.traits.FirstOrDefault(t => t.data == trait);
        return t != null ? 1 : 0;
    }
}