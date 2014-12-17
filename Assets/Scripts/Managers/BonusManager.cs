using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class BonusManager : RequiredMonoSingleton<BonusManager>
{
    [SerializeField, Tooltip("Bonus icon prefab appearing at explosion point")]
    private GameObject _attachedBonusIconPrefab;

    [SerializeField, Tooltip("Combo text prefab appearing at explosion point")]
    private GameObject _attachedComboTextPrefab;

    [SerializeField, TooltipAttribute("Min sum of start HP of killed units for which will always be given bonus")]
    private int _minSumOfStartHpOfKilledUnitsForBonus = 5;

    [SerializeField, TooltipAttribute("Min start HP of killed unit for which will always be given bonus")]
    private int _minStartHPForBonus = 3;

    private BaseBonus[] _bonuses;

    void Start()
    {
        _bonuses = GetComponentsInChildren<BaseBonus>().Where(c => c.gameObject.activeInHierarchy && enabled).ToArray();
    }

    public void CalculateBonusesForExplosion(Collider[] units, Vector3 position)
    {
        if (BattleManager.CurrentGameMode != GameMode.Normal)
            return;

        UnitStats[] stats = units.Select(c => c.GetComponent<UnitStats>()).ToArray();
        int totalStartHpOfKilled = stats.Where(c => c.WillKilledByCurrenHit).Sum(c => c.StartHP);
        string bonusDescitption = null;

        bool hasBonus = HasBonus(totalStartHpOfKilled, stats);
        if (totalStartHpOfKilled > 1)
        {
            AttachComboText(position, totalStartHpOfKilled, hasBonus);
        }

        if (hasBonus)
        {
            var bonus = GetRandomBonus();
            RunBonus(bonus);
            AttachBonus(bonus, position);
            bonusDescitption = Localization.Get(bonus.LocalizationKeyForDescription);
        }
        if (totalStartHpOfKilled > 0)
        {
            float factor = 1 + 0.1f * (totalStartHpOfKilled - 1);

            int totalScore = 0;
            foreach (var stat in stats)
            {
                totalScore += (int)(factor * stat.Cost);
            }
            EventAggregator.PublishT(GameEvent.OnCalculateScore, this, totalScore);
            HitInfoBar.Instance.Show(factor.ToString(), totalScore.ToString(), bonusDescitption);
        }
    }

    private bool HasBonus(int totalStartHpOfKilled, IEnumerable<UnitStats> stats)
    {
        return (totalStartHpOfKilled >= _minSumOfStartHpOfKilledUnitsForBonus || stats.Any(c => c.StartHP >= _minStartHPForBonus && c.WillKilledByCurrenHit));
    }

    private void AttachBonus(BaseBonus bonus, Vector3 position)
    {
        var bonusGO = NGUITools.AddChild(UIRoot.list[0].gameObject, _attachedBonusIconPrefab);
        var uifollover = bonusGO.GetComponent<UIFollowerToPoint>();
        uifollover.TargetPos = position;

        var sprite = bonusGO.GetComponentInChildren<UISprite>();
        sprite.spriteName = bonus.SpriteName;
    }

    private void AttachComboText(Vector3 position, int count, bool hasBonus)
    {
        var bonusGO = NGUITools.AddChild(UIRoot.list[0].gameObject, _attachedComboTextPrefab);
        var uifollover = bonusGO.GetComponent<UIFollowerToPoint>();
        uifollover.TargetPos = position;

        var lbl = bonusGO.GetComponentInChildren<UILabel>();
        lbl.text += count;

        if (hasBonus)
            uifollover.Offset.y += 0.95f;
    }

    private BaseBonus GetRandomBonus()
    {
        return RandomUtils.GetRandomItem(_bonuses);
    }

    private void RunBonus(BaseBonus bonus)
    {
        if (bonus.IsConstant)
        {
            bonus.RunEffect();
            return;
        }

        if (!bonus.IsCurrentlyExecuting)
            bonus.RunEffect();
        else
            bonus.ResetDuration();
    }

}
