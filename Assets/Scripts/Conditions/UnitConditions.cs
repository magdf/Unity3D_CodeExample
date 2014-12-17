using UnityEngine;
using System.Collections;

namespace Conditions
{
    public static class Unit
    {
        public static bool IsAllowableDistanceForAttack(Vector3 fromPosition, Vector3 targetPosition, float unitAttackDistance)
        {
            return (Vector3.Distance(fromPosition, targetPosition) <= unitAttackDistance);
        }

        public static bool CanAttackFromPosition(GameObject self, Transform attackableTarget, float unitAttackDistance, Vector3 position)
        {
            return IsAllowableDistanceForAttack(position, attackableTarget.position, unitAttackDistance) &&
                   GameBounds.IsInsidePlayerShootingBounds(position);
        }

        public static bool CanMoveAndAttackFromPosition(GameObject self, Transform attackableTarget, float unitAttackDistance, Vector3 position)
        {
            return IsAllowableDistanceForAttack(position, attackableTarget.position, unitAttackDistance) &&
                   IsFreePosition(position, 0.6f, Consts.LayerMasks.Units.AddToMask(Consts.Layers.Obstacle), self) &&
                   GameBounds.IsInsidePlayerShootingBounds(position);
        }

        /// <summary>
        /// Find Objects around position using OverlapSphere
        /// </summary>
        public static bool IsFreePosition(Vector3 position, float radius, LayerMask mask, GameObject excludedObject = null)
        {
            Collider[] colls = Physics.OverlapSphere(position, radius, mask);
            if (colls.Length == 0)
                return true;
            if (colls.Length > 1 || excludedObject == null)
                return false;
            return colls[0].gameObject == excludedObject;
        }
    }
}
