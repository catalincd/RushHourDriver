using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public float ForwardBlocks = 20;
    public float BackwardBlocks = 10;
    public float MaxBlocks = 25;
    public float BlockLength = 3;
    public int width = 3;
    public float stride = 1;
    public float offset = 0.75f;
    public float lastBlock = -10;
    public GameObject CameraObject;
    public GameObject[] SpawnableObjects;
    private List<List<GameObject>> blocks;

    void Start()
    {
        blocks = new List<List<GameObject>>();

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

    private int GetRandomId()
    {
        return (int) Mathf.Floor(Random.Range(0, SpawnableObjects.Length));
    }

    private void SpawnNewBlock()
    {
        List<GameObject> currentBlocks = new List<GameObject>();

        for(int i=0;i<width;i++)
        {
            int leftId = GetRandomId();
            int rightId = GetRandomId();

            GameObject newBlock = Instantiate(SpawnableObjects[leftId]) as GameObject; 
            newBlock.transform.SetParent(transform);
            newBlock.transform.position = (new Vector3(lastBlock, 0, offset + i * stride));
            
            GameObject newBlockRight = Instantiate(SpawnableObjects[rightId]) as GameObject; 
            newBlockRight.transform.SetParent(transform);
            newBlockRight.transform.position = (new Vector3(lastBlock, 0, -1.0f * (offset + i * stride)));
            newBlockRight.transform.eulerAngles = (new Vector3(0, 180.0f, 0));
        
            currentBlocks.Add(newBlock);
            currentBlocks.Add(newBlockRight);
        }
        

        blocks.Add(currentBlocks);

        lastBlock += BlockLength;

        while(blocks.Count > MaxBlocks)
        {

            for(int i=0;i<blocks[0].Count;i++)
            {
                Destroy(blocks[0][i]);
            }
            
            blocks.RemoveAt(0);
        }
    }
}
