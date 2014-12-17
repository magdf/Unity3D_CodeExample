using UnityEngine;
using System.Collections;

public class CannonballController : MonoBehaviour
{
    [SerializeField]
    private float _flyDuration = 1;

    [SerializeField]
    private float _destroyTimeAfterExplosion = 2f;

    private float _speed;
    private float _targetYpos;

    public AudioClip _clip;
    public Transform _explosionPrefab;

    void Start()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -Vector3.up);
        Physics.Raycast(ray, out hit, Mathf.Infinity, Consts.LayerMasks.BallCollisions);

        _speed = hit.distance / _flyDuration;
        _targetYpos = hit.point.y;

        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        while (transform.position.y > _targetYpos)
        {
            transform.Translate(-Vector3.up * _speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = PhysicsUtils.RaycastFromUpToDown(transform.position, Consts.LayerMasks.BallCollisions).point;
        Explosion();
    }

    private float ExplosionSize
    {
        get { return PlayerStats.Instance.CurrentExplosionSize - 0.1f * PlayerStats.Instance.CurrentExplosionSize; } //-0.1f*PlayerStats.Instance.CurrentExplosionSize - fix visual inaccuracy for projector
    }

    private void Explosion()
    {
        var units = Physics.OverlapSphere(transform.position, ExplosionSize, Consts.LayerMasks.Units);
        BonusManager.Instance.CalculateBonusesForExplosion(units, transform.position);
        foreach (var unit in units)
        {
            unit.SendMessage(Consts.Events.Damage, PlayerStats.Instance.CurrentExplosionPower);
        }

        Invoke("DestroySelf", _destroyTimeAfterExplosion);

        renderer.enabled = false;
        AudioSource.PlayClipAtPoint(_clip, transform.position, 1f);

        Transform fx = (Transform)Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        fx.parent = transform;

        ParticleEmitter pe = fx.GetComponent<ParticleEmitter>();
        pe.minSize = pe.maxSize = PlayerStats.Instance.CurrentExplosionSize + 1;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue.WithAlpha(0.5f);
        Gizmos.DrawSphere(transform.position, ExplosionSize);
    }

}
