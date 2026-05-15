namespace Stormriders.GameSystems;

public class ChargeRedrawBellSystem : GameSystem
{
    private int _amount;
    private bool _chargeRedraw;

    public void OnEnable()
    {
        Events.OnRedrawBellRevealed += RedrawBellRevealed;
    }

    public void OnDisable()
    {
        Events.OnRedrawBellRevealed -= RedrawBellRevealed;
    }

    public void ChargeRedrawBell(int amount)
    {
        var redrawBellSystem = FindObjectOfType<RedrawBellSystem>(true);
        _chargeRedraw = true;
        _amount = amount;
        if (redrawBellSystem.bell.activeSelf)
        {
            ChargeBell(redrawBellSystem);
        }
    }

    private void RedrawBellRevealed(RedrawBellSystem redrawBellSystem)
    {
        if (!_chargeRedraw)
        {
            return;
        }
        ChargeBell(redrawBellSystem);
    }

    private void ChargeBell(RedrawBellSystem redrawBellSystem)
    {
        if (_amount == 0)
        {
            redrawBellSystem.SetCounter(0);
        }
        else
        {
            for (var i = 0; i < _amount; i++)
            {
                redrawBellSystem.Counter();
            }
        }

        _chargeRedraw = false;
    }
}