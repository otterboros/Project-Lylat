// PositionUpdater.cs - Update the position of a given transform by a given offset
//                      with overload parameters for pinning to another transform
//-------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUpdater
{
    public static void UpdatePosition(Transform thisTransform, Vector3 targetPositionOffset)
    {
        thisTransform.position += targetPositionOffset;
    }

    public static void UpdatePosition(Transform thisTransform, Transform targetTransform)
    {
        thisTransform.position = targetTransform.position;
    }

    public static void UpdatePosition(Transform thisTransform, Transform targetTransform, Vector3 targetPositionOffset)
    {
        thisTransform.position = targetTransform.position + targetPositionOffset;
    }
}
