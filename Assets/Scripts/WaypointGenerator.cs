using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaypointGenerator : MonoBehaviour 
{
    public virtual List<Vector3> YieldWaypoints() 
    {
        return new List<Vector3>();
    }
}

