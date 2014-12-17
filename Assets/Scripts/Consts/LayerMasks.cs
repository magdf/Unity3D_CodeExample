using UnityEngine;

namespace Consts
{
    public static class LayerMasks
    {   
        public static readonly LayerMask GroundForCamera = LayerMaskExt.Create(Layers.Ground);

        public static readonly LayerMask BallCollisions = LayerMaskExt.Create(Layers.Ground);

        public static readonly LayerMask GroundForUnits = LayerMaskExt.Create(Layers.Ground);

        public static readonly LayerMask Units = LayerMaskExt.Create(Layers.Unit);
        public static readonly LayerMask UI = LayerMaskExt.Create(Layers.UI);
    }
}
