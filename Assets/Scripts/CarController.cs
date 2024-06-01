using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // TO DO: auto find?
    public GameObject car;
    public float maxSteeringAngle = 45;
    public float maxAcceleration = 15;
    public float maxBrake = 7.5f;

    public bool updateWheels = true;

    public Vector3 centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);

    private float steeringAngle = 0.0f;
    private float acceleration = 0.0f;
    private float brakeforce = 0.0f;
    private Transform leftFrontWheel;
    private Transform rightFrontWheel;
    private Transform leftBackWheel;
    private Transform rightBackWheel;

    private WheelCollider rearWheelRight;
    private WheelCollider rearWheelLeft;

    private WheelCollider frontWheelRight;
    private WheelCollider frontWheelLeft;

    private BoxCollider carCollider;
    private Rigidbody carBody;
    private GameObject wheelColliders;

    private bool followingTrafficWaypoints = false;
    private bool isLeft = true;
    private List<Vector3> waypointsToFollow;

    public CarController(GameObject _car)
    {
        car = _car;
    }

    void Start()
    {
        //transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //transform.eulerAngles = new Vector3(0.0f, 90f, 0.0f);
        // TO DO: remove this
        

        wheelColliders = new GameObject("WheelColliders");
        wheelColliders.transform.SetParent(car.transform);

        wheelColliders.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        wheelColliders.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        wheelColliders.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        

        car.AddComponent<Rigidbody>();
        carBody = car.GetComponent<Rigidbody>();
        carBody.centerOfMass = centerOfMass;
        carBody.mass = 150;

        car.AddComponent<BoxCollider>();
        carCollider = car.GetComponent<BoxCollider>();
        carCollider.center = new Vector3(0.0f, 0.65f, 0.05f);
        carCollider.size = new Vector3(0.8f, 0.5f, 2.0f);

        foreach (Transform child in car.transform)
        {
            if(child.name == "wheel-front-left") leftFrontWheel = child;
            if(child.name == "wheel-front-right") rightFrontWheel = child;
            if(child.name == "wheel-back-left") leftBackWheel = child;
            if(child.name == "wheel-back-right") rightBackWheel = child;

            if(child.name.Contains("wheel-") && !child.name.Contains("collider"))
            {
                //child.gameObject.layer = LayerMask.NameToLayer("Wheels"); 

                MeshFilter meshFilter = child.gameObject.GetComponent<MeshFilter>();
                float wheelSize = meshFilter.mesh.bounds.size.z;
                float wheelWidth = meshFilter.mesh.bounds.size.z;

                GameObject newCollider = new GameObject(child.name + "-collider");
                newCollider.transform.SetParent(wheelColliders.transform);

                newCollider.transform.localPosition = child.transform.localPosition + new Vector3(wheelWidth * Mathf.Sign(child.transform.localPosition.x) * 0.65f, 0.0f, 0.0f);
                newCollider.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                newCollider.gameObject.AddComponent<WheelCollider>();
                var wheelCollider = newCollider.gameObject.GetComponent<WheelCollider>();

                JointSpring spring = wheelCollider.suspensionSpring;
                // Debug.Log($"{spring.spring}, {spring.damper}");
                
                spring.spring = 3500f;
                spring.damper = 400f;
                wheelCollider.suspensionSpring = spring;


                

                wheelCollider.radius = wheelSize / 2.0f;
                wheelCollider.center = new Vector3(0.0f, 0.15f, 0.0f); // 0.15f

                

                if(child.name == "wheel-front-left") frontWheelLeft = wheelCollider;
                if(child.name == "wheel-front-right") frontWheelRight = wheelCollider;
                if(child.name == "wheel-back-left") rearWheelLeft = wheelCollider;
                if(child.name == "wheel-back-right") rearWheelRight = wheelCollider;

            }

            if(child.name.Contains("body"))
            {
                //child.gameObject.layer = LayerMask.NameToLayer("CarBodies"); 
            }
        }
    }

    void Update()
    {
        if(followingTrafficWaypoints) UpdateFollower();
    }

    void FixedUpdate()
    {
        rearWheelLeft.motorTorque = maxAcceleration * acceleration;
        rearWheelRight.motorTorque = maxAcceleration * acceleration;

        frontWheelLeft.motorTorque = maxAcceleration * acceleration;
        frontWheelRight.motorTorque = maxAcceleration * acceleration;


        rearWheelLeft.brakeTorque = maxBrake * brakeforce;
        rearWheelRight.brakeTorque = maxBrake * brakeforce;

        frontWheelLeft.brakeTorque = maxBrake * brakeforce;
        frontWheelRight.brakeTorque = maxBrake * brakeforce;


        frontWheelLeft.steerAngle  = maxSteeringAngle * steeringAngle;
        frontWheelRight.steerAngle  = maxSteeringAngle * steeringAngle;


        if(updateWheels)
            UpdateMeshes();
    }

    private void UpdateMeshes()
    {
        Vector3 pos;
        Quaternion rot; 

        rearWheelLeft.GetWorldPose(out pos, out rot);
        leftBackWheel.rotation = rot;
       // leftBackWheel.position = pos;

        rearWheelRight.GetWorldPose(out pos, out rot);
        rightBackWheel.rotation = rot;
       // rightBackWheel.position = pos;

        frontWheelLeft.GetWorldPose(out pos, out rot);
        leftFrontWheel.rotation = rot;
        //leftFrontWheel.position = pos;

        frontWheelRight.GetWorldPose(out pos, out rot);
        rightFrontWheel.rotation = rot;
        //rightFrontWheel.position = pos;
    }

    private void SetWheelPosition(WheelCollider xCollider, Transform xTransform)
    {
        var meshFilter = xTransform.gameObject.GetComponent<MeshFilter>();
        float wheelWidth = meshFilter.mesh.bounds.size.z;

        Vector3 pos;
        Quaternion rot; 

        rearWheelLeft.GetWorldPose(out pos, out rot);
        leftBackWheel.rotation = rot;
        leftBackWheel.position = pos;
    }

    public void Steer(float _steering)
    {
        steeringAngle = _steering;
    }

    public void Accelerate(float _acceleration)
    {
        acceleration = _acceleration;
    }

    public void Brake(float _brakeforce)
    {
        brakeforce = _brakeforce;
    }

    public Vector3 GetPosition()
    {
        return car.transform.position;
    }

    public Transform GetTransform()
    {
        return car.transform;
    }    

    public void StartFollowingTraffic(bool _isLeft, List<Vector3> waypoints)
    {
        followingTrafficWaypoints = true;
        isLeft = _isLeft;
        waypointsToFollow = new List<Vector3>(waypoints);
    }

    private void UpdateFollower()
    {
        if(waypointsToFollow.Count == 0) return;

        if(isLeft)
        {
            while(waypointsToFollow.Count > 0 && waypointsToFollow[0].z > car.transform.position.z)
            {
                waypointsToFollow.RemoveAt(0);
            }
        }
        else
        {
            while(waypointsToFollow.Count > 0 && waypointsToFollow[0].z < car.transform.position.z)
            {
                waypointsToFollow.RemoveAt(0);
            }
        }

        if(waypointsToFollow.Count == 0) return;

        Vector3 carForward = GetTransform().forward;
        Vector3 target = waypointsToFollow[0] - GetPosition(); 
        target.Normalize();
        Vector2 targetDirection = new Vector2(target.x, target.z);
        Vector2 currentDirection = new Vector2(carForward.x, carForward.z);
        float angle = Vector2.SignedAngle(targetDirection, currentDirection);
        float steeringDelta = Mathf.Clamp(angle / maxSteeringAngle, -1.0f, 1.0f);
        Steer(steeringDelta);
    }
}
