using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficMovement : MonoBehaviour
{
    private Transform[] waypoints;
    private int currentWaypoint;
    private Vector3 start;
    private float speed;
    private float waypointReachedThreshold = 0.1f;

    public void SetMovementParameters(Transform[] waypoints,float carSpeed)
    {
        this.waypoints = waypoints;
        start = transform.position;
        speed = carSpeed;
        currentWaypoint = 0;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;
        
        Vector3 targetPosition = waypoints[currentWaypoint].position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < waypointReachedThreshold)
        {
            transform.position = start;
        }

    }
}