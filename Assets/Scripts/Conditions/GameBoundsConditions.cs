using UnityEngine;
using System.Collections;

namespace Conditions
{
    public static class GameBounds
    {
        private const float _nearBoundsOffset = 15;
        private const float _farBoundsOffset = 5;

        public static bool IsInsidePlayerShootingBounds(Vector3 posintion)
        {
            Vector2 pos2d = new Vector2(posintion.x, posintion.z);
            var rect = BattleCamera.Instance.GetBounds();
            rect.yMin += _nearBoundsOffset;
            return rect.Contains(pos2d);
        }

        public static bool IsCloserThanPlayerShootingBounds(Vector3 posintion)
        {
            var rect = BattleCamera.Instance.GetBounds();
            if (posintion.z <= rect.yMin + _nearBoundsOffset)
                return true;
            return false;
        }

        public static bool IsInsideCameraMovementBounds(Vector3 posintion)
        {
            Vector2 pos2d = new Vector2(posintion.x, posintion.z);
            var rect = BattleCamera.Instance.GetBounds();
            rect.yMax += _farBoundsOffset;
            return rect.Contains(pos2d);
        }
    }
}
