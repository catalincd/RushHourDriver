using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionGenerator : MonoBehaviour
{
    public float ForwardBlocks = 20;
    public float BackwardBlocks = 10;
    public float BlockLength = 2;
    public float lastBlock = -10;
    public float MaxBlocks = 25;
    public GameObject CameraObject;
    public GameObject carPrefab; // what why when
    public GameObject[] SpawnableObjects;

    private List<GameObject> blocks;

    
    void Start()
    {
        blocks = new List<GameObject>();

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
        int id = (int) Mathf.Floor(Random.Range(0, SpawnableObjects.Length));
        // id = 0;
        GameObject newBlock = Instantiate(SpawnableObjects[id]) as GameObject; 
    	newBlock.transform.SetParent(transform);
    	newBlock.transform.position = (new Vector3(lastBlock, 0, 0));

        
        GameObject startPoint = new GameObject("StartPoint");
        startPoint.transform.SetParent(newBlock.transform);
        startPoint.transform.localPosition = (new Vector3(0, 0, 6));

        GameObject endPoint = new GameObject("EndPoint");
        endPoint.transform.SetParent(newBlock.transform);
        endPoint.transform.localPosition = (new Vector3(0, 0, -6));
        
        if (newBlock.name == "Intersection3(Clone)")
        {
            // traseu sens here
            AddCarSpawner(newBlock, new Transform[]{startPoint.transform, endPoint.transform}, 0f, 0.4f, -6f);
            AddCarSpawner(newBlock, new Transform[]{endPoint.transform, startPoint.transform}, 180f, -0.4f, 6f);
        }
        else
        {
            AddCarSpawner(newBlock, new Transform[]{startPoint.transform, endPoint.transform}, 0f, 0.4f, -6f);
            AddCarSpawner(newBlock, new Transform[]{endPoint.transform, startPoint.transform}, 180f, -0.4f, 6f);
        }
        
        
        blocks.Add(newBlock);

        lastBlock += BlockLength;

        while(blocks.Count > MaxBlocks)
        {
            Destroy(blocks[0]);   
            blocks.RemoveAt(0);
        }
    }

    private void AddCarSpawner(GameObject intersection, Transform[] waypoints, float direction, float road_side, float start)
    {
        int speed = Random.Range(2, 4);
        GameObject NPC = Instantiate(carPrefab, intersection.transform.position + new Vector3(road_side, 0, start), Quaternion.identity);
        TrafficMovement movementNPC1 = NPC.AddComponent<TrafficMovement>();
        waypoints[0].localPosition += (new Vector3(road_side, 0, 0));
        waypoints[1].localPosition += (new Vector3(road_side, 0, 0));
        movementNPC1.SetMovementParameters(waypoints, speed);
        NPC.transform.SetParent(intersection.transform);
        NPC.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        NPC.transform.Rotate(0f, direction, 0f);
    }

}
