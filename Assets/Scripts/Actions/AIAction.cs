using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Actions
{
    public static class AIAction
    {
        /// <summary>
        /// Trying to change the target and to update a traversedCheckpoints list
        /// </summary>
        public static void TryChangeTarget(MonoBehaviour self, RichAI richaAI, Checkpoint checkPoint, List<Checkpoint> traversedCheckpoints)
        {
            if (!self.enabled)
                return;
            if (!traversedCheckpoints.Contains(checkPoint))
            {
                var target = RandomUtils.GetRandomWithoutExcludeds(checkPoint.NextCheckpoints, traversedCheckpoints.ToArray()).transform;
                if (target == null)
                {
                    Debug.LogError("next target not found in checkpoint", self);
                    return;
                }
                richaAI.target = target;

                traversedCheckpoints.Add(checkPoint);
            }
        }
    }
}
