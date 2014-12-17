using System;
using UnityEngine;
using Pathfinding;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _prefab;

    [SerializeField]
    private Transform _target;

    public bool CanUseForImmediateSpawn = true;

    public bool Looped;

    [SerializeField]
    private bool _canIgnoreSpawnRate;

    [SerializeField]
    private float _timeBeforeFirstSpawn;

    [SerializeField]
    private int _minMobsPerWave = 3;

    [SerializeField]
    private int _maxMobsPerWave = 5;

    [SerializeField]
    private float _cooldown = 10f;

    private float _remainCooldownTime = 3;

    private void Start()
    {
        _remainCooldownTime = _timeBeforeFirstSpawn;
    }

    private void Update()
    {
        Profiler.BeginSample("_SpawnWave");
        if (BattleManager.CurrentGameMode != GameMode.Normal)
            return;

        _remainCooldownTime -= Time.deltaTime;
        if (_remainCooldownTime < 0)
        {
            if (CanDoSpawn)
                SpawnWave();
        }
        Profiler.EndSample();
    }

    public void ImmediateSpawn()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        _remainCooldownTime = _cooldown;

        int mobsInCurrentWave = UnityEngine.Random.Range(_minMobsPerWave, _maxMobsPerWave + 1);

        var squareSize = GetSpawnSquareSize();
        Vector3 gridCenter = transform.position + new Vector3(-squareSize / 2f + 0.5f, 0f, -squareSize / 2f + 0.5f);
        for (int i = 0; i < squareSize; i++)
        {
            for (int j = 0; j < squareSize; j++)
            {
                if (mobsInCurrentWave <= i * squareSize + j)
                    return;
                if (!CanDoSpawn)
                    return;

                var cellpos = gridCenter + new Vector3(i, 0, j);

                var mob = (Transform)Instantiate(_prefab, cellpos, transform.rotation);
                mob.parent = SceneContainers.Units;
                mob.GetComponent<RichAI>().target = _target;
            }
        }

        if (!Looped)
            Destroy(gameObject);
    }

    private bool CanDoSpawn
    {
        get { return (_canIgnoreSpawnRate || SpawnManager.Instance.CanDoMobSpawn(_prefab.GetComponent<UnitStats>().SpawnRate)); }
    }

    private int GetSpawnSquareSize()
    {
        return Mathf.CeilToInt(Mathf.Sqrt(_maxMobsPerWave));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue.WithAlpha(0.3f);
        GizmosUtils.DrawSquareGrid(transform.position, GetSpawnSquareSize(), _maxMobsPerWave);

        GizmosUtils.DrawText(CustomEditorPrefsProxy.GizmoGuiSkin, "spawnpoint:\n" + ((_prefab != null) ? _prefab.name : "prefab=null"),
                             transform.position, Color.cyan,
                             (int)(CustomEditorPrefsProxy.GizmoGuiSkin.GetStyle("Label").fontSize * 0.8));
    }

    private void OnDrawGizmosSelected()
    {
        if (_target == null)
            return;
        Gizmos.color = Color.cyan.WithAlpha(0.5f);
        Gizmos.DrawSphere(_target.transform.position, 3f);
        Gizmos.DrawWireSphere(_target.transform.position, 3f);
    }
}
