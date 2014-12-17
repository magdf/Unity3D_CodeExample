using System;
using UnityEngine;

public class ArcherBehaviour_Attack : BaseUnitBehaviour
{
    [SerializeField]
    private Transform _arrowPrefab;

    [SerializeField]
    private float _attackCooldown = 5f;

    [SerializeField]
    private float _rotationSpeed = 150f;

    private Transform _targetForFollow;

    private Transform _attackableTarget;

    private ArrowController _arrow;

    protected override void Awake() 
    {
        base.Awake();
        _attackableTarget = BattleManager.GetPlayer();
        _mediator.RichAI.OnEndMoveToTarget += OnMoveEnd;
    }

    private void OnMoveEnd()
    {
        _hasPointForMoving = false;
    }

    private void OnEnable()
    {
        _targetForFollow = _mediator.RichAI.target;
        _mediator.FsmVariables.SecondTargetForFollow.position = transform.position;
        _mediator.RichAI.target = _mediator.FsmVariables.SecondTargetForFollow;

        StopMove();
        PreparationsForAttack();
    }

    private void PreparationsForAttack()
    {
        _attackInProgress = true;
        _hasPointForMoving = false;
        Action attack = () =>
            {
                _remainTimeBeforeNextAttack = _attackCooldown;
                StopMove();
                _mediator.AnimatorController.StartAttack();
                _mediator.AnimatorController.StoptAttack();
            };

        var arrow = (Transform)Instantiate(_arrowPrefab, transform.position, Quaternion.LookRotation(Vector3.up));
        arrow.parent = transform;
        _arrow = arrow.GetComponent<ArrowController>();

        StartCoroutine(Actions.Position.RotateToTargetCoroutine(transform, _attackableTarget, _rotationSpeed, 10, OnComplete: attack));
    }

    //Call from animation
    private void OnAttackEnd() 
    {
        _attackInProgress = false;
    }

    //Call from animation
    private void Shoot()
    {
        _arrow.Shoot(BattleManager.GetPlayer(), _mediator.Stats.AttackPower);
    }
    

    private float _remainTimeBeforeNextAttack;
    private bool _hasPointForMoving;
    private bool _attackInProgress;

    private void Update()
    {
        _remainTimeBeforeNextAttack -= Time.deltaTime;

        if (_attackInProgress)
            return;

        if (_remainTimeBeforeNextAttack > 0)
        {
            if (!_hasPointForMoving)
            {
                Vector3 newPos = RandomUtils.PointInsideCircle(transform.position, 10, Consts.LayerMasks.GroundForUnits);
                if (Conditions.Unit.CanMoveAndAttackFromPosition(gameObject, _attackableTarget, ((ArcherStats) _mediator.Stats).AttackDistance, newPos))
                {
                    _mediator.FsmVariables.SecondTargetForFollow.position = newPos;
                    _mediator.RichAI.StartMove();
                    _hasPointForMoving = true;
                }
            }
        }
        else
        {
            if (Conditions.Unit.CanAttackFromPosition(gameObject, _attackableTarget, ((ArcherStats) _mediator.Stats).AttackDistance, transform.position))
            {
                StopMove();
                PreparationsForAttack();
                _hasPointForMoving = false;
            }
            else
                if (!Conditions.Unit.CanMoveAndAttackFromPosition(gameObject, _attackableTarget, ((ArcherStats) _mediator.Stats).AttackDistance, _mediator.FsmVariables.SecondTargetForFollow.position)
                    || !_hasPointForMoving)
                {
                    _hasPointForMoving = false;
                    _mediator.RichAI.target = _targetForFollow;
                    _mediator.RichAI.StartMove();
                    _mediator.StateController.SetState(UnitState.Normal);
                }
        }
    }

    private void StopMove()
    {
        _mediator.FsmVariables.SecondTargetForFollow.position = transform.position;
        _mediator.RichAI.StopMove(_mediator.FsmVariables.SecondTargetForFollow.position);
    }

    private void OnDestroy()
    {
        if (_mediator != null && _mediator.RichAI != null && _mediator.RichAI.OnEndMoveToTarget!=null)
            _mediator.RichAI.OnEndMoveToTarget -= OnMoveEnd;
    }

}
