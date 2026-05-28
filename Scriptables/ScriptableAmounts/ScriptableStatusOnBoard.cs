#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Stormriders.Scriptables.ScriptableAmounts;

internal class ScriptableStatusOnBoard : ScriptableAmount
{
    public string statusType;
    public bool self;
    public bool allies;
    public bool enemies;
    public float multiplier = 1;
    public bool countStatus;
    public bool inRow;

    public override int Get(Entity entity)
    {
        var result = 0;

        if (self)
        {
            result += Count(entity);
        }
        if (allies)
        {
            result += (inRow ? entity.GetAlliesInRow() : entity.GetAllies()).Sum(Count);
        }
        if (enemies)
        {
            result += (inRow ? GetEnemiesInRow(entity) : entity.GetAllEnemies()).Sum(Count);
        }

        return Mathf.FloorToInt(result * multiplier);
    }

    private int Count(Entity entity)
    {
        var status = entity.statusEffects.FirstOrDefault(s => s.type.Equals(statusType));
        if (status == null)
            return 0;
        return countStatus ? status.count : 1;
    }

    private static List<Entity> GetEnemiesInRow(Entity entity)
    {
        List<Entity> result = [];
        foreach (var rowContainer in entity.containers)
        {
            var enemiesInRow = entity.GetEnemiesInRow(References.Battle.GetRowIndex(rowContainer));
            if (enemiesInRow is { Count: > 0 })
                result.AddRange(enemiesInRow);
        }
        return result;
    }
}