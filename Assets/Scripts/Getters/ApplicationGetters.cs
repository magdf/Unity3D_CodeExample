using UnityEngine;

namespace Getters
{
    public static class Application
    {
        public const string BattleScenePrefixName = "Level";

        public static int GetBattleSceneNumber(string sceneName)
        {
            return int.Parse(sceneName.Remove(0, BattleScenePrefixName.Length));
        }
    }
}