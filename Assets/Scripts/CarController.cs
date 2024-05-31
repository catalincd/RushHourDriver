using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // TO DO: auto find?
    public GameObject car;
    public float maxSteeringAngle = 30;
    public float maxAcceleration = 5;

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

    private Rigidbody carBody;
    private GameObject wheelColliders;

    void Start()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        transform.eulerAngles = new Vector3(0.0f, 90f, 0.0f);
        // TO DO: remove this
        

        wheelColliders = new GameObject("WheelColliders");
        wheelColliders.transform.SetParent(car.transform);

        wheelColliders.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        wheelColliders.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        wheelColliders.transform.eulerAngles = new Vector3(0.0f, 90f, 0.0f);
        

        foreach (Transform child in car.transform)
        {
            if(child.name == "wheel-front-left") leftFrontWheel = child;
            if(child.name == "wheel-front-right") rightFrontWheel = child;
            if(child.name == "wheel-back-left") leftBackWheel = child;
            if(child.name == "wheel-back-right") rightBackWheel = child;

            if(child.name.Contains("wheel-"))
            {
                //child.gameObject.layer = LayerMask.NameToLayer("Wheels"); 

                MeshFilter meshFilter = child.gameObject.GetComponent<MeshFilter>();
                float wheelSize = meshFilter.mesh.bounds.size.z;
                float wheelWidth = meshFilter.mesh.bounds.size.z;

                GameObject newCollider = new GameObject(child.name + "-collider");
                newCollider.transform.SetParent(wheelColliders.transform);

                newCollider.transform.localPosition = child.transform.localPosition + new Vector3(wheelWidth * Mathf.Sign(child.transform.localPosition.x) / 2.0f, 0.0f, 0.0f);
                newCollider.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                newCollider.gameObject.AddComponent<WheelCollider>();
                var wheelCollider = newCollider.gameObject.GetComponent<WheelCollider>();

                JointSpring spring = wheelCollider.suspensionSpring;
                Debug.Log($"{spring.spring}, {spring.damper}");
                
                spring.spring = 3500f;
                spring.damper = 400f;
                wheelCollider.suspensionSpring = spring;


                

                wheelCollider.radius = wheelSize / 2.0f;
                wheelCollider.center = new Vector3(0.0f, 0.3f, 0.0f); // 0.15f

                

                if(child.name == "wheel-front-left") frontWheelLeft = wheelCollider;
                if(child.name == "wheel-front-right") frontWheelRight = wheelCollider;
                if(child.name == "wheel-back-left") rearWheelLeft = wheelCollider;
                if(child.name == "wheel-back-right") rearWheelRight = wheelCollider;

            }

            if(child.name.Contains("body"))
            {
                child.gameObject.layer = LayerMask.NameToLayer("CarBodies"); 
            }
        }
        
        carBody = car.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //if(verticalInput > 0.01f)
        //Debug.Log($"Acc: {acceleration * verticalInput}");

        //if(horizontalInput > 0.01f)
        //Debug.Log($"Steer: {maxSteeringAngle * horizontalInput}");

        rearWheelLeft.motorTorque = maxAcceleration * verticalInput;
        rearWheelRight.motorTorque = maxAcceleration * verticalInput;

        frontWheelLeft.motorTorque = maxAcceleration * verticalInput;
        frontWheelRight.motorTorque = maxAcceleration * verticalInput;

        frontWheelLeft.steerAngle  = maxSteeringAngle * horizontalInput;
        frontWheelRight.steerAngle  = maxSteeringAngle * horizontalInput;

        UpdateMeshes();
    }

    private void UpdateMeshes()
    {

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
}
