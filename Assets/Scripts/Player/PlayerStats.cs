using UnityEngine;
using System.Collections;

public class PlayerStats : RequiredMonoSingleton<PlayerStats>
{
    [SerializeField]
    private float _explosionSizeIncrement = 0.5f;

    [SerializeField]
    private float _startigExplosionSize = 3f;


    public float StartingShootCooldown = 2f;
    public float CurrentShootCooldown;

    [HideInInspector]
    public bool HasShieldFromBullets;

    [HideInInspector]
    public float DefenseFactor = 1;

    public float CurrentExplosionSize
    {
        get { return _startigExplosionSize + _explosionLevel * _explosionSizeIncrement; }
    }

    /// <summary>
    /// Now always return 1
    /// </summary>
    public int CurrentExplosionPower
    {
        get { return 1; }
    }

    public int ExplosionLevel
    {
        get { return _explosionLevel; }
        set { _explosionLevel = Mathf.Clamp(value, 0, 5); }
    }

    private int _explosionLevel = 0;


    public int LevelScore
    {
        get { return _levelScore; }
    }

    /// <summary>
    /// For dataBinding in NGUI
    /// </summary>
    public string LevelScoreText
    {
        get { return _levelScore.ToString(); }
        set { }
    }

    private int _levelScore = 0;


    private void Start()
    {
        _explosionLevel = GlobalVariables.AdditionalExplosionLevel + SaveManager.LoadStarsCount(Getters.Application.GetBattleSceneNumber(Application.loadedLevelName));
        CurrentShootCooldown = StartingShootCooldown;
        EventAggregator.Subscribe<Damage>(GameEvent.OnPlayerDamage, this, GetDamage);
        EventAggregator.Subscribe<int>(GameEvent.OnCalculateScore, this, AddScore);
    }

    private void AddScore(int value)
    {
        if (BattleManager.CurrentGameMode != GameMode.Normal)
            return;
        _levelScore += value;
        _levelScore = Mathf.Clamp(_levelScore, 0, 99999);
    }

    private void GetDamage(Damage damage)
    {
        if (BattleManager.CurrentGameMode != GameMode.Normal)
            return;
        if (damage.Type == DamageType.Far && HasShieldFromBullets)
            return;
        SubtractScore(Mathf.RoundToInt(damage.Value * DefenseFactor));
    }

    private void SubtractScore(int value)
    {
        if (BattleManager.CurrentGameMode != GameMode.Normal)
            return;

        _levelScore -= value;
        _levelScore = Mathf.Clamp(_levelScore, 0, 99999);
    }
}
