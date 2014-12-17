using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnManager : RequiredMonoSingleton<SpawnManager>
{
    [SerializeField]
    private int _minSpawnRateSumBeforeImmediateSpawn = 5;

    [SerializeField]
    private int _maxSpawnRateSum = 30;

    public int CurrentSpawnRateSum { get; private set; }

    [SerializeField]
    private Transform _parentForLoopedSpawnPoints;

    private SpawnPoint[] _loopedSpawnPoints;


    private void Start()
    {
        EventAggregator.Subscribe<int>(GameEvent.OnAddUnitToGame, this, (rate) =>
            {
                CurrentSpawnRateSum += rate;
            });
        EventAggregator.Subscribe<int>(GameEvent.OnRemoveUnitFromGame, this, (rate) =>
            {
                CurrentSpawnRateSum -= rate;
            });

        _loopedSpawnPoints = _parentForLoopedSpawnPoints.GetComponentsInChildren<SpawnPoint>().Where(c => c.enabled && c.gameObject.activeInHierarchy).ToArray();
    }


    void Update()
    {
        if (BattleManager.CurrentGameMode != GameMode.Normal)
            return;

        if (CurrentSpawnRateSum < _minSpawnRateSumBeforeImmediateSpawn)
            ImmediateSpawn();
    }

    public bool CanDoMobSpawn(int unitSpawnRate)
    {
        return (CurrentSpawnRateSum + unitSpawnRate <= _maxSpawnRateSum);
    }

    public Transform[] GetAllLoopedSpawnPoints()
    {
        return _loopedSpawnPoints.Select(c => c.transform).ToArray();
    }

    private int GetFreeSpawnRates()
    {
        return _maxSpawnRateSum - CurrentSpawnRateSum;
    }

    private void ImmediateSpawn()
    {
        var spawnPoints = _loopedSpawnPoints.Where(c => c.CanUseForImmediateSpawn);
        var spawnPoint = RandomUtils.GetRandomItem(spawnPoints);
        if (spawnPoint == null)
            return;
        spawnPoint.ImmediateSpawn();
    }

}
