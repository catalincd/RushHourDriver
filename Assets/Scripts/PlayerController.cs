using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraController cameraObject;
    public GameObject[] CarObjects;
    public CarController controller;

    public float maxAcceleration = 12;
    public float maxBrake = 7.5f;

    public float carScale = 0.175f;

    public bool Reversed = false;

    void Start()
    {
        SpawnCar();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        controller.Steer(horizontalInput);
        // float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey("space") ^ Reversed)
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
        controller.maxSteeringAngle = 45;
        controller.centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
        controller.updateWheels = true;

        cameraObject.SetTarget(newCar.transform);
    }
}
