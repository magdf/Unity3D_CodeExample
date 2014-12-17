using UnityEngine;
using System.Collections;
using Pathfinding.RVO;

public class UnitAnimatorController : MonoBehaviour
{
    [SerializeField]
    private float _changingSpeed = 2;

    private Vector3 _prevPos;
    private float _prevSpeed;
    private Animator _animator;
    private RVOController _rvo;

    private int _prevState;

	void Start ()
	{
	    _animator = GetComponent<Animator>();
        _prevState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;

        _rvo = GetComponent<RVOController>();
	    _prevPos = transform.position;
	}

    private void Update()
    {
        UpdateSpeed();
        UpdateCurrentState();
    }


    void UpdateSpeed()
	{
        float dist  = Vector3.Distance(transform.position, _prevPos);
        var newSpeed = Mathf.Lerp(0, _rvo.maxSpeed, dist / (_rvo.maxSpeed * Time.deltaTime));

        newSpeed = MathfUtils.SmoothChangeValue(_prevSpeed, newSpeed, _changingSpeed, Time.deltaTime, 0, _rvo.maxSpeed);

        _animator.SetFloat("Speed", newSpeed);
	    _prevPos = transform.position;
        _prevSpeed = newSpeed;
	}

    public void StartAttack()
    {
         _animator.SetTrigger("StartAttackMode");        
    }
    public void StoptAttack()
    {
        _animator.SetTrigger("StopAttackMode");
    }

    private void UpdateCurrentState()
    {
        int currState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;
        if (_prevState != currState)
            OnChangeState();

        _prevState = currState;
    }

    private void OnChangeState()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Base.move"))
        {
            SendMessage("Shoot");
            SendMessage("OnAttackEnd");
        }
    }
}