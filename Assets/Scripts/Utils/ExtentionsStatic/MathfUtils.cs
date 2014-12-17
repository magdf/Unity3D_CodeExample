using System;
using System.Collections;

namespace UnityEngine
{
    public static class MathfUtils
    {
        /// <summary>
        /// Unlike SmoothDamp - used changing speed instead of changing time
        /// </summary>
        public static float SmoothChangeValue(float currentValue, float targetValue, float changingSpeed, float deltaTime, float min, float max)
        {
            float retValue = currentValue;
            if (currentValue > targetValue)
            {
                retValue -= changingSpeed * deltaTime;
                if (retValue < targetValue)
                    retValue = targetValue;
            }
            else if (currentValue < targetValue)
            {
                retValue += changingSpeed * deltaTime;
                if (retValue > targetValue)
                    retValue = targetValue;
            }

            retValue = Mathf.Clamp(retValue, min, max);
            return retValue;
        }

        public static IEnumerator LerpWithDuration(float from, float to, float duration, Action<float> setterFunc, Action OnComplete = null)
        {
            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime / duration;
                setterFunc(Mathf.Lerp(from, to, progress));
                yield return null;
            }
            if (OnComplete != null)
                OnComplete();
        }
    }
}
