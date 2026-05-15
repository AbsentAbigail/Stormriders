using System.Linq;

namespace Stormriders.Scriptables.TargetConstraints;

public class TargetConstraintAllyInRowMatchesConstraint : TargetConstraint
{
    public TargetConstraint[] constraints;
    
    public override bool Check(Entity target)
    {
        var alliesInRow = target.GetAlliesInRow();
        return alliesInRow.Any(entity => constraints.All(constraint => constraint.Check(entity)));
    }

    public override bool Check(CardData targetData)
    {
        return false;
    }
}