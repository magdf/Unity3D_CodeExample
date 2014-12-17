using UnityEngine;
using System.Collections;

public class ArcherFSMVariables : MonoBehaviour
{ 
    /// <summary>
    /// Target for unit. It is necessary that a unit been stopped or moved to a desired point, but not checkpoints.
    /// </summary>
    [HideInInspector]
    public Transform SecondTargetForFollow;

	void Awake () 
	{
        SecondTargetForFollow = transform.FindChild("SecondTarget");
        SecondTargetForFollow.parent = SceneContainers.Targets;
	}
	
	void Update () 
	{
	
	}
}
