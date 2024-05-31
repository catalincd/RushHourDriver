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

        blocks.Add(newBlock);

        lastBlock += BlockLength;

        while(blocks.Count > MaxBlocks)
        {
            Destroy(blocks[0]);   
            blocks.RemoveAt(0);
        }
    }
}
