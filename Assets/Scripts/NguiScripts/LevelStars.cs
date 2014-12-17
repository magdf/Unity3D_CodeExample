using UnityEngine;
using System.Collections;

/// <summary>
/// Set stars sprites in GUI elements in level manager scene
/// </summary>
public class LevelStars : MonoBehaviour
{
    [SerializeField]
    private Consts.SceneNames _level;

    private UISprite[] _stars = new UISprite[3];

    void Start()
	{
        string sceneName = _level.GetStringValue<StringValueAttribute>();
        int sceneNum = Getters.Application.GetBattleSceneNumber(sceneName);
        int starsCount = SaveManager.LoadStarsCount(sceneNum);
        var btn = GetComponent<UIButton>();

        _stars[0] = transform.Find("Star1").GetComponent<UISprite>();
        _stars[1] = transform.Find("Star2").GetComponent<UISprite>();
        _stars[2] = transform.Find("Star3").GetComponent<UISprite>();

        switch (starsCount)
        {
            case 1:
                _stars[0].spriteName = "full";
                break;
            case 2:
                _stars[0].spriteName = "full";
                _stars[1].spriteName = "full";
                break;
            case 3:
                _stars[0].spriteName = "full";
                _stars[1].spriteName = "full";
                _stars[2].spriteName = "full";
                break;
        }
	}

}
