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

    private List<GameObject> leftCars;
    private List<GameObject> rightCars;

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
        leftCars = new List<GameObject>();
        rightCars = new List<GameObject>();

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
        for(int i=0;i<leftCars.Count;i++)
        {
            if(Mathf.Abs(leftCars[i].transform.position.z) > zLimit || leftCars[i].transform.position.y < -0.5f)
            {
                Destroy(leftCars[i]);
                leftCars.RemoveAt(i);
                i--;
                Debug.Log("Deleted car");
            }
        }

        for(int i=0;i<rightCars.Count;i++)
        {
            if(Mathf.Abs(rightCars[i].transform.position.z) > zLimit || rightCars[i].transform.position.y < -0.5f)
            {
                Destroy(rightCars[i]);
                rightCars.RemoveAt(i);
                i--;
                Debug.Log("Deleted car");
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
        leftCars.Add(newCar);

    
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
        rightCars.Add(newCar);

    
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
