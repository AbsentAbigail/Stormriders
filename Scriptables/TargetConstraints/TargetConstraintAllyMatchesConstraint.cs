using System.Linq;

namespace Stormriders.Scriptables.TargetConstraints;

public class TargetConstraintAllyMatchesConstraint : TargetConstraint
{
    public TargetConstraint[] constraints;
    public bool inRow = true;
    
    public override bool Check(Entity target)
    {
        var allies = inRow ? target.GetAlliesInRow() : target.GetAllAllies();
        return allies.Any(entity => constraints.All(constraint => constraint.Check(entity)));
    }

    public override bool Check(CardData targetData)
    {
        return false;
    }
}