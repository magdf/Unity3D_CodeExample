using UnityEngine;
using System.Collections;

public class HitInfoBar : RequiredMonoSingleton<HitInfoBar>
{
    [SerializeField]
    private float _duration;

    private float _remainingTime;

    private UILabel _bonusInfo;
    private UILabel _scoreInfo;
    private UILabel _scoreFactorInfo;

    protected override void Awake()
    {
        base.Awake();
        _bonusInfo = transform.FindChild("BonusInfo").GetComponent<UILabel>();
        _scoreInfo = transform.FindChild("NewScore/Lbl_Value").GetComponent<UILabel>();
        _scoreFactorInfo = transform.FindChild("ScoreFactor/Lbl_Value").GetComponent<UILabel>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0)
            gameObject.SetActive(false);
    }

    public void Show(string scoreFactor, string score, string bonusDescription)
    {
        gameObject.SetActive(true);
        _remainingTime = _duration;
        _scoreFactorInfo.text = scoreFactor;
        _scoreInfo.text = score;

        if (string.IsNullOrEmpty(bonusDescription))
            _bonusInfo.gameObject.SetActive(false);
        else
        {
            _bonusInfo.text = bonusDescription;
            _bonusInfo.gameObject.SetActive(true);
        }

    }
}
