﻿using UnityEngine;
using System.Collections;

public class BattleManager : RequiredMonoSingleton<BattleManager>
{
    [SerializeField]
    public float BattleStartDuration = 180f;

    [SerializeField]
    public Color BattleAmbientLight;

    public static GameMode CurrentGameMode
    {
        get { return Instance._currentGameMode; }
        set { Instance._currentGameMode = value; }
    }

    private GameMode _currentGameMode;

    [HideInInspector]
    public float UnitsSpeedFactor = 1f;

    public static Transform GetPlayer()
    {
        return PlayerStats.Instance.transform;
    }

    public UICamera UICamera { get; private set; }

    public bool Pause
    {
        get { return _isPause; }
        set
        {
            if (value == _isPause)
                return;

            if (value == true)
            {
                _isPause = true;
                Time.timeScale = 0f;
                Screen.showCursor = true;
            }
            else
            {
                _isPause = false;
                Time.timeScale = 1f;
                Screen.showCursor = false;
            }
        }
    }

    private bool _isPause;


    private void Start()
    {
        UICamera = UIRoot.list[0].transform.GetComponentInChildren<UICamera>();
        EventAggregator.Subscribe(GameEvent.EngGameProcess, this, EndGameProcess);
        Invoke("StartGameProcess", 1f);
    }

    private void StartGameProcess()
    {
        Screen.showCursor = false;
        CurrentGameMode = GameMode.Normal;
        EventAggregator.Publish(GameEvent.StartGameProcess, this);
    }

    private void EndGameProcess()
    {
        CurrentGameMode = GameMode.Victory;
        Screen.showCursor = true;

        GlobalVariables.AdditionalExplosionLevel = 0;

        var score = PlayerStats.Instance.LevelScore;
        SaveManager.SaveScoreCount(score, Getters.Application.GetBattleSceneNumber(Application.loadedLevelName));
        SaveManager.SaveStarsCount(ScoreCounter.CalculateStarsNumber(score), Getters.Application.GetBattleSceneNumber(Application.loadedLevelName));
        SaveManager.Save();
    }

    private void OnDestroy()
    {
        Screen.showCursor = true;
    }
}

