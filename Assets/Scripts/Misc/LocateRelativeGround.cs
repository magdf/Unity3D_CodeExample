using UnityEngine;
using System.Collections;

/// <summary>
/// For editor only. Set position.y to ground position after start game.
/// </summary>
public class LocateRelativeGround : MonoBehaviour 
{
    [SerializeField]
    private float _positionAboveGround = 0f;

    private bool _isDoneRaycastToGround;

    void Start()
    {
        enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (!_isDoneRaycastToGround)
        {
            transform.position = PhysicsUtils.RaycastFromUpToDown(transform.position, Consts.LayerMasks.GroundForUnits).point + new Vector3(0, _positionAboveGround, 0);
            _isDoneRaycastToGround = true;
        }
    }


}
