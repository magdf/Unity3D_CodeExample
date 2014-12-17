using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// All bonuses located inside BonusManager in scene. 
/// </summary>
[Serializable]
public abstract class BaseBonus : MonoBehaviour
{
    public string SpriteName;

    public string LocalizationKeyForDescription;

    /// <summary>
    /// If 'true' then this bonus is active all time.
    /// </summary>    
    public virtual bool IsConstant { get; private set; }

    public bool IsCurrentlyExecuting { get; protected set; }


    /// <summary>
    /// Duration for non-constant bonuses
    /// </summary>
    public float Duration;

    protected float _remainingTime;

    public float RemainingTime { get { return _remainingTime; } }

    public virtual void RunEffect()
    {
        if (IsCurrentlyExecuting)
            Debug.LogError("bonus already runned", this);

        IsCurrentlyExecuting = true;
        _remainingTime = Duration;
        StartCoroutine(RemainingTimeCounterCoroutine());
        EventAggregator.PublishT(GameEvent.OnStartBonusEffect, this, this);
    }

    public virtual void ResetDuration()
    {
        _remainingTime = Duration;
    }

    protected virtual void DeleteEffect()
    {
        IsCurrentlyExecuting = false;
        EventAggregator.PublishT(GameEvent.OnEndBonusEffect, this, this);
    }

    protected IEnumerator RemainingTimeCounterCoroutine()
    {
        while (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            yield return null;
        }
        DeleteEffect();
    }

    protected void Test()
    {
        //Duration = 3;
        RunEffect();
    }
}
