using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : WaypointGenerator
{
    public float ForwardBlocks = 20;
    public float BackwardBlocks = 10;
    public float MaxBlocks = 25;
    public float lastBlock = -10;
    public float BlockLength = 2;
    public GameObject CameraObject;
    public GameObject[] RoadObjects;
    private List<GameObject> blocks;
    private List<Vector3> waypoints;

    void Start()
    {
        waypoints = new List<Vector3>();

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
        int id = (int) Mathf.Floor(Random.Range(0, RoadObjects.Length));
        GameObject newBlock = Instantiate(RoadObjects[id]) as GameObject; 
    	newBlock.transform.SetParent(transform);
    	newBlock.transform.position = (new Vector3(lastBlock, 0, 0));

        WaypointContainer waypointContainer = (WaypointContainer) newBlock.GetComponent<WaypointContainer>();
        List<Vector3> localWaypoints = new List<Vector3>(waypointContainer.waypoints);
        for(var i=0;i<localWaypoints.Count;i++)
        {
            localWaypoints[i] = localWaypoints[i] + new Vector3(lastBlock, 0, 0);
        }

        waypoints.AddRange(localWaypoints);

        blocks.Add(newBlock);

        lastBlock += BlockLength;

        while(blocks.Count > MaxBlocks)
        {
            Destroy(blocks[0]);   
            blocks.RemoveAt(0);
        }


    }

    public override List<Vector3> YieldWaypoints() 
    {
        var yieldList = new List<Vector3>(waypoints);
        waypoints.Clear();  
        return yieldList;
    }
}
