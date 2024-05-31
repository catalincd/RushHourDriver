using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    public int width = 6;
    public int height = 5;
    public float offset = 1;
    public float stride = 1;
    public float ForwardBlocks = 20;
    public float BackwardBlocks = 10;
    public float MaxBlocks = 30;
    public float BlockLength = 6;
    public float lastBlock = -10;
    public GameObject grass;
    public GameObject CameraObject;

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

    private void SpawnNewBlock()
    {
        List<GameObject> currentBlocks = new List<GameObject>();
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                GameObject newBlock = Instantiate(grass) as GameObject; 
                newBlock.transform.SetParent(transform);
                newBlock.transform.position = (new Vector3(lastBlock + j, 0, offset + stride * i));

                GameObject newBlockRight = Instantiate(grass) as GameObject; 
                newBlockRight.transform.SetParent(transform);
                newBlockRight.transform.position = (new Vector3(lastBlock + j, 0, -1.0f * (offset + stride * i)));

                currentBlocks.Add(newBlock);
                currentBlocks.Add(newBlockRight);
            }
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
