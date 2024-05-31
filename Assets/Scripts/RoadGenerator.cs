using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public float ForwardBlocks = 20;
    public float BackwardBlocks = 10;
    public float lastBlock = -10;
    public float BlockLength = 2;
    public GameObject CameraObject;
    public GameObject[] RoadObjects;

    private List<GameObject> spawnedObjects;

    void Start()
    {
        while(lastBlock < 0)
            SpawnNewBlock();
    }

    void Update()
    {
        UpdateRoad();
    }

    private void UpdateRoad()
    {
        float CameraX = CameraObject.transform.position.x;
        while(CameraX + (ForwardBlocks * BlockLength) > lastBlock)
        {
            SpawnNewBlock();
        }
    }

    private void SpawnNewBlock()
    {
        int id = (int) Mathf.Floor(Random.Range(0, RoadObjects.Length));
        GameObject newBlock = Instantiate(RoadObjects[id]) as GameObject; 
    	newBlock.transform.SetParent(transform);
    	newBlock.transform.position = (new Vector3(lastBlock, 0, 0));
        lastBlock += BlockLength;
    }
}
