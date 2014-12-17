using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using System;

/// <summary>
/// Displays active bonuses
/// </summary>
public class CooldownsBar : MonoBehaviour
{
    private CooldownItem[] _icons;
    private readonly List<BaseBonus> _activeBonuses = new List<BaseBonus>();

    private void Awake()
    {
        var sprites = this.GetComponentsInDirectChildrens<UISprite>().OrderBy(c => c.gameObject.name).ToArray();

        _icons = new CooldownItem[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            _icons[i] = new CooldownItem(sprites[i]);
            sprites[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        EventAggregator.Subscribe<BaseBonus>(GameEvent.OnStartBonusEffect, this, OnAddBonus);
        EventAggregator.Subscribe<BaseBonus>(GameEvent.OnEndBonusEffect, this, OnRemoveBonus);
    }

    void Update()
    {
        foreach (var icon in _icons)
        {
            if (icon.FilledSprite.gameObject.activeSelf == false)
                break;

            icon.FilledSprite.fillAmount = 1 - icon.CurrentBonus.RemainingTime / icon.CurrentBonus.Duration;
        }
    }

    private void OnAddBonus(BaseBonus newBonus)
    {
        if (_activeBonuses.Contains(newBonus))
        {
            return;
        }
        if (_activeBonuses.Count > _icons.Length)
            return;

        var firstEmptyIcon = _icons[_activeBonuses.Count];

        firstEmptyIcon.SetBonus(newBonus);
        _activeBonuses.Add(newBonus);
        firstEmptyIcon.FilledSprite.gameObject.SetActive(true);
    }

    private void OnRemoveBonus(BaseBonus bonus)
    {
        var index = GetAssociatedIconIndex(bonus);

        //leftward shift of icons that the right of the current
        int lastVisibleIconindex = _activeBonuses.Count - 1;
        for (int i = index; i < lastVisibleIconindex; i++)
        {
            _icons[i].SetBonus(_icons[i + 1].CurrentBonus);
        }

        _icons[lastVisibleIconindex].FilledSprite.gameObject.SetActive(false);
        _activeBonuses.Remove(bonus);
    }

    private int GetAssociatedIconIndex(BaseBonus bonus)
    {
        for (int i = 0; i < _activeBonuses.Count; i++)
        {
            if (_icons[i].CurrentBonus == bonus)
                return i;
        }
        Debug.LogError("Associated Icon Index not found");
        return -1;
    }


    [Serializable]
    private class CooldownItem
    {
        public UISprite FilledSprite;
        public BaseBonus CurrentBonus;

        [SerializeField]
        private UISprite _icon;

        public CooldownItem(UISprite sprite)
        {
            FilledSprite = sprite;
            _icon = sprite.GetComponentsInDirectChildrens<UISprite>().First();
            CurrentBonus = null;
        }

        public void SetBonus(BaseBonus bonus)
        {
            if (bonus == null)
            {
                Debug.LogWarning("bonus=null");
                return;
            }
            CurrentBonus = bonus;
            _icon.spriteName = bonus.SpriteName;
        }
    }
}
