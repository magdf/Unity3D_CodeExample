using System;
using UnityEngine;
using System.Collections;

namespace Actions
{
    public static class Position
    {
        public static void MoveToDirection(Transform self, Vector3 normalizedDirection, float speed)
        {
            self.position += (normalizedDirection * Time.deltaTime * speed);
        }

        public static IEnumerator ConstantMoveCoroutine(Vector3 normalizedDirection, float speed, Action<Vector3> setterFunc)
        {
            while (true)
            {
                setterFunc(normalizedDirection * Time.deltaTime * speed);
                yield return null;
            }
        }

        /// <summary>
        /// Rotate with slowdown speed
        /// </summary>
        public static void SlowdownRotate(Transform self, Vector3 direction, float rotationSpeed)
        {
            if (direction != Vector3.zero)
                self.rotation = Quaternion.Lerp(self.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Rotate with constant speed
        /// </summary>
        /// <param name="rotationSpeed">Speed in degrees per second</param>
        public static void ConstantRotate(Transform self, Vector3 direction, float rotationSpeed)
        {
            if (direction != Vector3.zero)
            {
                self.rotation = Quaternion.RotateTowards(self.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Rotate to target with constant speed
        /// </summary>
        /// <param name="rotationSpeed">Speed in degrees per second</param>
        /// <param name="endRotationAngle">For stop rotation if (min angle between current and target objects > endRotationAngle)</param>
        public static IEnumerator RotateToTargetCoroutine(Transform self, Transform target, float rotationSpeed, float endRotationAngle, Action OnComplete)
        {
            while (Getters.Base.GetAngleByXZ(self, target) > endRotationAngle)
            {
                Vector3 direction = Getters.Base.GetDirectionByXZ(self.position, target.position);
                self.rotation = Quaternion.RotateTowards(self.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                yield return null;
            }
            OnComplete();
        }
    }
}