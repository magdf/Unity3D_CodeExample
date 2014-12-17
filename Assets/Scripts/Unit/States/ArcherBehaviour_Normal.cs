using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour_Normal : BaseUnitBehaviour
{
    private readonly List<Checkpoint> _traversedCheckpoints = new List<Checkpoint>();

    private void Start()
    {
    }

    private bool _hasPointForMoving;

    private void Update()
    {
        if (Conditions.Unit.CanAttackFromPosition(gameObject, BattleManager.GetPlayer(), ((ArcherStats)_mediator.Stats).AttackDistance, transform.position))
        {
            _mediator.StateController.SetState(UnitState.Attack);
            _hasPointForMoving = false;
        }
        else if (Conditions.GameBounds.IsCloserThanPlayerShootingBounds(transform.position))
        {
            if (!_hasPointForMoving)
            {
                Vector3 newPos = RandomUtils.PointInsideCircle(transform.position, 10, Consts.LayerMasks.GroundForUnits);
                if (Conditions.Unit.CanMoveAndAttackFromPosition(gameObject, BattleManager.GetPlayer(), ((ArcherStats)_mediator.Stats).AttackDistance, newPos))
                {
                    _mediator.RichAI.target = _mediator.FsmVariables.SecondTargetForFollow;
                    _mediator.FsmVariables.SecondTargetForFollow.position = newPos;
                    _mediator.RichAI.StartMove();
                    _hasPointForMoving = true;
                }
            }
        }
    }

    private void OnCheckpointEnter(Checkpoint checkPoint)
    {
        if (Conditions.GameBounds.IsCloserThanPlayerShootingBounds(transform.position))
            return;

        Actions.AIAction.TryChangeTarget(this, _mediator.RichAI, checkPoint, _traversedCheckpoints);
    }
}
