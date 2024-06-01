using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    public Vector3[] waypoints;
    public Vector3[] trafficWaypoints;

    public bool trafficLeft = true;
    public bool trafficRight = true;
}
