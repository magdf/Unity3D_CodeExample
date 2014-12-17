using System;
using UnityEngine;
using System.Collections;

public class LabelTimer : MonoBehaviour
{
    private UILabel _label;

    public float RemainTime { get; private set; }

    void Start()
    {
        _label = transform.FindChild("Label1").GetSafeComponent<UILabel>();

        RemainTime = BattleManager.Instance.BattleStartDuration;

        EventAggregator.Subscribe(GameEvent.StartGameProcess, this, () => StartCoroutine(UpdateTimeCoroutine(0.1f)));
        EventAggregator.Subscribe<int>(GameEvent.AddTime, this, OnAddTime);
    }

    private IEnumerator UpdateTimeCoroutine(float frequency)
    {
        SetText(((int)RemainTime).ToString());

        while (RemainTime > 0)
        {
            yield return new WaitForSeconds(frequency);
            RemainTime -= frequency;
            int remainSec = (int)Mathf.Floor(RemainTime + 0.01f); //+0.01f for fix float inaccuracy  
            SetText(remainSec.ToString());
        }

        SetText("0");
        EventAggregator.Publish(GameEvent.EngGameProcess, this);
    }

    private void SetText(string label1)
    {
        _label.text = label1;
    }

    private void OnAddTime(int time)
    {
        RemainTime += time;
    }
}
