using System.Collections.Generic;
using UnityEngine;
using Pathfinding.RVO;

/// <summary>
/// Class with links for unit components for easy access
/// </summary>
public class UnitMediator : MonoBehaviour 
{
    public MyRichAI RichAI { get; private set; }
    public RVOController RVOController { get; private set; }
    public UnitStats Stats { get; private set; }
    public UnitAnimatorController AnimatorController { get; private set; }
    public BaseUnitBehaviour CurrentBehaviourState { get; private set; }

    public ArcherFSMVariables FsmVariables { get; private set; }
    public UnitStatesController StateController { get; private set; }

	void Awake () 
	{
        RichAI = GetComponent<MyRichAI>();
        RVOController = GetComponent<RVOController>();
        Stats = GetComponent<UnitStats>();
        AnimatorController = GetComponent<UnitAnimatorController>();
        StateController = GetComponent<UnitStatesController>();
	    FsmVariables = GetComponent<ArcherFSMVariables>();
	}

}
