using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficGenerator : MonoBehaviour
{
    public GameObject[] cars;
    public float carScale;

    public float carSpeed = 10.0f;
    public float speedVariation = 1.2f; 
    public float spawnStride = 500;
    public float strideVariation = 2.0f;
    public float zLimit = 7.5f;

    private float lastSpawnLeft = 0;
    private float lastStrideLeft = 0;

    private float lastSpawnRight = 0;
    private float lastStrideRight = 0;

    private WaypointContainer waypointContainer;
    
    private Vector3[] leftWaypoints;
    private Vector3[] rightWaypoints;

    private List<GameObject> carObjects;

    public void Setup(GameObject[] _cars, float _carScale = 0.175f, float _carSpeed = 10.0f, float _speedVariation = 1.2f, float _spawnStride = 500, float _strideVariation = 2.0f, float _zLimit = 7.5f)
    {
        cars = _cars;
        carSpeed = _carSpeed;
        carScale = _carScale;
        speedVariation = _speedVariation;
        spawnStride = _spawnStride;
        strideVariation = _strideVariation;
    }

    void Start()
    {
        waypointContainer = (WaypointContainer) gameObject.GetComponent<WaypointContainer>();
        carObjects = new List<GameObject>();

        leftWaypoints = waypointContainer.trafficWaypoints;
        rightWaypoints = (Vector3[])leftWaypoints.Clone();

        for(int i=0;i<leftWaypoints.Length;i++)
        {
            rightWaypoints[i] = transform.position - leftWaypoints[i];
            leftWaypoints[i] = transform.position + leftWaypoints[i];
        }
    }

    void Update()
    {
        if(waypointContainer.trafficLeft) SpawnCarLeft();
        if(waypointContainer.trafficRight) SpawnCarRight();

        DeleteCars();
    }

    private void DeleteCars()
    {
        for(int i=0;i<carObjects.Count;i++)
        {
            if(carObjects[i] == null || Mathf.Abs(carObjects[i].transform.position.z) > zLimit || carObjects[i].transform.position.y < -0.5f)
            {
                if(carObjects[i] != null) Destroy(carObjects[i]);
                carObjects.RemoveAt(i);
                i--;
            }
        }
    }

    void SpawnCarLeft()
    {
        if(Time.unscaledTime * 1000.0f - lastSpawnLeft < lastStrideLeft) return;

        lastSpawnLeft = Time.unscaledTime * 1000.0f;
        float variationBias = Mathf.Lerp(1.0f / strideVariation, strideVariation, Random.value);
        lastStrideLeft = spawnStride * variationBias;

        int id = (int) Mathf.Floor(Random.Range(0, cars.Length));

        GameObject newCar = Instantiate(cars[id]) as GameObject; 
        newCar.transform.SetParent(transform);
        newCar.transform.position = leftWaypoints[0];
        newCar.transform.localScale = (new Vector3(carScale, carScale, carScale));
        newCar.transform.eulerAngles = (new Vector3(0.0f, 180.0f, 0.0f));
        carObjects.Add(newCar);

    
        CarController controller = newCar.AddComponent<CarController>();
        controller.transform.SetParent(newCar.transform);
        controller.car = newCar;
        controller.maxAcceleration = carSpeed;
        controller.maxBrake = 7.0f;
        controller.maxSteeringAngle = 45.0f;
        controller.centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
        controller.updateWheels = true;
        controller.Accelerate(Mathf.Lerp(1.0f / speedVariation, speedVariation, Random.value));
        controller.StartFollowingTraffic(true, new List<Vector3>(leftWaypoints));
    }

    void SpawnCarRight()
    {
        if(Time.unscaledTime * 1000.0f - lastSpawnRight < lastStrideRight) return;

        lastSpawnRight = Time.unscaledTime * 1000.0f;
        float variationBias = Mathf.Lerp(1.0f / strideVariation, strideVariation, Random.value);
        lastStrideRight = spawnStride * variationBias;

        int id = (int) Mathf.Floor(Random.Range(0, cars.Length));

        GameObject newCar = Instantiate(cars[id]) as GameObject; 
        newCar.transform.SetParent(transform);
        newCar.transform.position = rightWaypoints[0];
        newCar.transform.localScale = (new Vector3(carScale, carScale, carScale));
        newCar.transform.eulerAngles = (new Vector3(0.0f, 0.0f, 0.0f));
        carObjects.Add(newCar);

    
        CarController controller = newCar.AddComponent<CarController>();
        controller.transform.SetParent(newCar.transform);
        controller.car = newCar;
        controller.maxAcceleration = carSpeed;
        controller.maxBrake = 7.0f;
        controller.maxSteeringAngle = 45.0f;
        controller.centerOfMass = new Vector3(0.0f, 0.0f, 0.0f);
        controller.updateWheels = true;
        controller.Accelerate(Mathf.Lerp(1.0f / speedVariation, speedVariation, Random.value));
        controller.StartFollowingTraffic(false, new List<Vector3>(rightWaypoints));
    }
}
