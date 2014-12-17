using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour 
{
    public int StartHP = 1;

    [TooltipAttribute("unit cost in spawn system")]
    public int SpawnRate = 1;

    [TooltipAttribute("cost for killing unit")]
    public int Cost = 10;

    public int AttackPower = 10;

    private int _currentHP = 1;
    private float _defaultspeed;

    private UnitMediator _mediator;

    protected virtual  void Awake()
    {
        _currentHP = StartHP;
        _mediator = GetComponent<UnitMediator>();
        _defaultspeed = _mediator.RVOController.maxSpeed;
        if (!EventAggregator.IsApplicationShuttingDown)
        {
            EventAggregator.PublishT(GameEvent.OnAddUnitToGame, this, SpawnRate);
        }
    }

    void Update()
    {
        float factor = 1;
        if (Conditions.GameBounds.IsInsideCameraMovementBounds(transform.position))
            factor = BattleManager.Instance.UnitsSpeedFactor;

        _mediator.RVOController.maxSpeed = _defaultspeed * factor;
    }

    private void Damage(int value)
    {
        _currentHP -= value;
        if (_currentHP < 1)
        {
            EventAggregator.PublishT(GameEvent.OnRemoveUnitFromGame, this, SpawnRate);         
            Destroy(gameObject);
        }
    }

    public bool WillKilledByCurrenHit
    {
        get { return (_currentHP == 1); }
    }
}
