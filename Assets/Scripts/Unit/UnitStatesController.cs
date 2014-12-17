using System.Collections.Generic;
using UnityEngine;

public class UnitStatesController : MonoBehaviour
{
    public BaseUnitBehaviour CurrentState { get; private set; }

    private readonly Dictionary<UnitState, BaseUnitBehaviour> _states = new Dictionary<UnitState, BaseUnitBehaviour>();

    void Awake()
    {
        var startingState = GetComponent<WarriorBehaviour_Normal>();
        if (startingState != null)
        {
            _states.Add(UnitState.Normal, GetComponent<WarriorBehaviour_Normal>());
            _states.Add(UnitState.Retreat, GetComponent<UnitBehaviour_Retreat>());
        }
        else //if unit is archer
        {
            _states.Add(UnitState.Normal, GetComponent<ArcherBehaviour_Normal>());
            _states.Add(UnitState.Attack, GetComponent<ArcherBehaviour_Attack>());
            _states.Add(UnitState.Retreat, GetComponent<UnitBehaviour_Retreat>());
        }

        CurrentState = _states[UnitState.Normal];
    }

    void Start()
    {
        EventAggregator.Subscribe(GameEvent.EngGameProcess, this, SetRetreatState);
    }

    public void SetState(UnitState state)
    {
        if (CurrentState == null)
            return;

        CurrentState.enabled = false;
        CurrentState = _states[state];
        CurrentState.enabled = true;
    }

    private void SetRetreatState()
    {
        SetState(UnitState.Retreat);
    }

}
