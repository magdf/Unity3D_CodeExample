using UnityEngine;
using System.Collections;

/// <summary>
/// Shooting controller for player
/// </summary>
public class ShootingController : MonoBehaviour
{
    [SerializeField]
    private Transform _cannonballPrefab;

    [SerializeField, Tooltip("Magnitude of increase to starting shooting position relative a current height")]
    private float _startShootHeightUnder = 20f;

    [SerializeField]
    private float _projectorSizeAfterShoot = 0.5f;

    private Projector _projector;
    private Color _startColor;

    private bool _isCooldownInProgress;

    void Start()
    {
        _projector = GetComponent<Projector>();
        _startColor = _projector.material.color;
        _projector.orthoGraphicSize = PlayerStats.Instance.CurrentExplosionSize;

        Material m = new Material(_projector.material);
        _projector.material = m;

        EventAggregator.Subscribe(GameEvent.EngGameProcess, this, () => { _projector.material.color = Color.white; });
    }

    /// <summary>
    /// Time counter between touchBegin and touchEnd
    /// </summary>
    private float _tapDuration = 10f;

    private void Update()
    {
        if (BattleManager.Instance.Pause)
            _tapDuration = 10;

        if (BattleManager.CurrentGameMode != GameMode.Normal || BattleManager.Instance.Pause)
            return;

        _tapDuration += Time.deltaTime;

        if (_projector.orthographicSize != PlayerStats.Instance.CurrentExplosionSize && !_isCooldownInProgress)
            _projector.orthographicSize = PlayerStats.Instance.CurrentExplosionSize;

        if (_isCooldownInProgress)
            return;


        if (Application.platform.In(RuntimePlatform.Android, RuntimePlatform.IPhonePlayer))
        {
            for (var i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    if (Input.GetTouch(i).deltaPosition.magnitude > 10)  //if there was a large movement then reset the timer
                        _tapDuration = 10;
                }
            }
            if (Input.GetMouseButtonDown(0))
                _tapDuration = 0;
            if (Input.GetMouseButtonUp(0))
                TryShoot();

        }
        else if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void TryShoot()
    {
        if (_tapDuration < 0.5f)
            Shoot();

        _tapDuration = 0;
    }

    private void Shoot()
    {
        var uicam = BattleManager.Instance.UICamera;
        if (Conditions.GUI.ClickOverGUI(uicam.camera, Consts.LayerMasks.UI))
            return;

        Vector3 pos = transform.position + new Vector3(0, _startShootHeightUnder, 0);
        Instantiate(_cannonballPrefab, pos, Quaternion.identity);
        StartCoroutine(ProjectorSizeCoroutine());
    }


    private IEnumerator ProjectorSizeCoroutine()
    {
        _isCooldownInProgress = true;
        _projector.orthographicSize = _projectorSizeAfterShoot;
        _projector.material.color = Color.white;
        float coolDownStartTime = Time.time;

        var cooldownDurationAtStart = PlayerStats.Instance.CurrentShootCooldown;
        float offset = 0f;
        while (!Mathf.Approximately(_projector.orthographicSize, PlayerStats.Instance.CurrentExplosionSize))
        {
            var elapsedTime = Time.time - coolDownStartTime;//elapsed time relatively cooldown start

            if (!Mathf.Approximately(cooldownDurationAtStart, PlayerStats.Instance.CurrentShootCooldown))
            {            
                //elapsed time adjustment with an allowance cooldown changes through bonuses.
                offset = elapsedTime * (PlayerStats.Instance.CurrentShootCooldown / cooldownDurationAtStart) - elapsedTime;
                cooldownDurationAtStart = PlayerStats.Instance.CurrentShootCooldown;
            }
            elapsedTime += offset;

            _projector.orthographicSize = Mathf.Lerp(_projectorSizeAfterShoot, PlayerStats.Instance.CurrentExplosionSize, elapsedTime / cooldownDurationAtStart);
            yield return null;
        }

        _projector.orthographicSize = PlayerStats.Instance.CurrentExplosionSize;
        if (BattleManager.CurrentGameMode != GameMode.Victory)
            _projector.material.color = _startColor;
        _isCooldownInProgress = false;
    }

}
