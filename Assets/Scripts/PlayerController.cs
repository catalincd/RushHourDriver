using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public WaypointGenerator[] waypointGenerators;
    public CameraController cameraObject;
    public GameObject[] CarObjects;

    public float AutoDriverOffset = 0.25f;
    
    public float maxSteeringAngle = 45;
    public float maxAcceleration = 12;
    public float maxBrake = 7.5f;

    public float carScale = 0.175f;

    public bool Reversed = false;

    public bool DrawDebugLines = true;
    public bool DrawDebugAngleLines = true;
    public Material LinesMaterial;

    private CarController controller;

    private List<Vector3> waypoints;

    private LineRenderer lineRenderer;
    private LineRenderer angleRenderer;

    void Start()
    {
        waypoints = new List<Vector3>();
        SetupLines();
        SpawnCar();
    }

    void Update()
    {
        UpdateWaypoints();

        if(DrawDebugLines) UpdateLines();

        float horizontalInput = Input.GetAxis("Horizontal");
        //controller.Steer(horizontalInput);
        // float verticalInput = Input.GetAxis("Vertical");



        if ((Input.GetKey("space") || GetTouch()) ^ Reversed)
        {
            controller.Accelerate(0.0f);
            controller.Brake(1.0f);
        }
        else
        {
            controller.Brake(0.0f);
            controller.Accelerate(1.0f);
        }
    }

    private bool GetTouch()
    {
        for(int i=0;i<Input.touchCount;i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.position.y < Screen.height - 50)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateWaypoints()
    {
        int oldLen = waypoints.Count;

        for(int i=0;i<waypointGenerators.Length;i++)
        {
            List<Vector3> newWaypoints = waypointGenerators[i].YieldWaypoints();
            waypoints.AddRange(newWaypoints);
        }

        if(waypoints.Count > oldLen)
        {
            waypoints = waypoints.OrderBy(p => p.x).ToList();
            Debug.Log(waypoints.Count);
        }

        if(waypoints.Count == 0) return;

        Vector3 currentCarPosition = controller.GetPosition();
        
        while(waypoints[0].x < currentCarPosition.x)
        {
            waypoints.RemoveAt(0);
        }
        
        
        Vector3 carForward = controller.GetTransform().forward;
        Vector3 target = waypoints[0] - currentCarPosition; 
        target.Normalize();
        Vector2 targetDirection = new Vector2(target.x, target.z);
        Vector2 currentDirection = new Vector2(carForward.x, carForward.z);

        float angle = Vector2.SignedAngle(targetDirection, currentDirection);




        float steeringDelta = Mathf.Clamp(angle / maxSteeringAngle, -1.0f, 1.0f);
        controller.Steer(steeringDelta);

        // Debug.Log($"currentDirection: {currentDirection.ToString()} Angle: {angle} Steering: {steeringDelta}");

        

        if(DrawDebugAngleLines)
        {
            angleRenderer.SetPosition(0, controller.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f));
            angleRenderer.SetPosition(1, controller.GetPosition() + new Vector3(1.0f * Mathf.Cos(angle * Mathf.Deg2Rad), 0.5f, 1.0f * Mathf.Sin(angle * Mathf.Deg2Rad)));
        }
    }

    private void SetupLines()
    {
        if(DrawDebugLines)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 10;

            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            // lineRenderer.material = LinesMaterial;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }

        if(DrawDebugAngleLines)
        {
            angleRenderer = gameObject.AddComponent<LineRenderer>();
            angleRenderer.positionCount = 2;

            angleRenderer.startWidth = 0.1f;
            angleRenderer.endWidth = 0.1f;
            // lineRenderer.material = LinesMaterial;
            angleRenderer.startColor = Color.white;
            angleRenderer.endColor = Color.white;
        }
    }

    private void UpdateLines()
    {
        lineRenderer.SetPosition(0, controller.GetPosition() + new Vector3(0.0f, 0.25f, 0.0f));
        for(int i=1;i<10;i++)
        {
            if(waypoints.Count < i - 1) break;

            lineRenderer.SetPosition(i, waypoints[i - 1] + new Vector3(0.0f, 0.25f, 0.0f));
        }
    }

    public void SpawnCar(float startingX = 0.0f)
    {
        int id = (int) Mathf.Floor(Random.Range(0, CarObjects.Length));

        GameObject newCar = Instantiate(CarObjects[id]) as GameObject; 
        newCar.transform.SetParent(transform);
        newCar.transform.position = (new Vector3(startingX, 0.5f, 0.0f));
        newCar.transform.localScale = (new Vector3(carScale, carScale, carScale));
        newCar.transform.eulerAngles = (new Vector3(0.0f, 90.0f, 0.0f));

        controller = newCar.AddComponent<CarController>();
        controller.car = newCar;
        controller.maxAcceleration = maxAcceleration;
        controller.maxBrake = maxBrake;
        controller.maxSteeringAngle = maxSteeringAngle;
        controller.centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
        controller.updateWheels = true;
        controller.isPlayer = true;

        cameraObject.SetTarget(newCar.transform);
    }
}
