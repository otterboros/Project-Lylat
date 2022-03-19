// EnvironmentCollisionProcessor.cs - Determine whether collision with environmental object deals damage
//------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCollisionProcessor : MonoBehaviour
{
    // Write class which takes in the object colliding and object collided with
    //  For the player ship, take eitehr
    //      normals
    //      or current velocity
    //      if current velocity is above a threshold, player takes damage and is pushed away
    //      otherwise, player takes no damage
}
