using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private bool _isFinalCheckpoint;

    [SerializeField]
    [TooltipAttribute("Checkpoints which has a roads from the current checkpoint")]
    private Checkpoint[] _nextCheckpoints;


    public Checkpoint[] NextCheckpoints
    {
        get { return _nextCheckpoints; }
    }

    private void OnTriggerEnter(Collider unit)
    {
        if (!_isFinalCheckpoint)
            unit.SendMessage(Consts.Events.OnCheckpointEnter, this, SendMessageOptions.DontRequireReceiver);
        else
            unit.SendMessage(Consts.Events.OnFinalCheckpointEnter, this,SendMessageOptions.DontRequireReceiver);
    }


    private void OnDrawGizmos()
    {
        GizmosUtils.DrawText(CustomEditorPrefsProxy.GizmoGuiSkin, name, transform.position);
    }

    private  void OnDrawGizmosSelected()
    {
        foreach (var c in _nextCheckpoints)
        {
            Gizmos.color = Color.cyan.WithAlpha(0.5f);
            Gizmos.DrawSphere(c.transform.position, 3f);
            Gizmos.DrawWireSphere(c.transform.position, 3f);
        }
    }

}
