using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BtnLoadNextLevel : MonoBehaviour
{
    /// <summary>
    /// Battle scenes number
    /// </summary>
    private int _levelsNumber;

    private void Start()
    {
    }

    private void OnClick()
    {
        if (Application.loadedLevelName.Contains(Getters.Application.BattleScenePrefixName))
        {
            int level = Getters.Application.GetBattleSceneNumber(Application.loadedLevelName);

            if (level == _levelsNumber)
            {
                Application.LoadLevel(Getters.Application.BattleScenePrefixName + level);
            }
            else
            {
                level++;
                Application.LoadLevel(Getters.Application.BattleScenePrefixName + level);
            }
        }
    
    }
}
