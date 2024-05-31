using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] CarObjects;
    public CarController controller;

    void Start()
    {
        SpawnCar();
    }

    void Update()
    {
        if (Input.GetKey("space"))
        {
            //Debug.Log("KEYDOWN");
            //controller.Accelerate(100000.0f);
            //controller.Brake(0.0f);
        }
        else
        {
            //controller.Accelerate(0.0f);
            //controller.Brake(50.0f);
        }
    }

    public void SpawnCar(float startingX = 0.0f)
    {

    }
}
