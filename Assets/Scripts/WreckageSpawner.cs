using System.Diagnostics;
using UnityEngine;

public class WreckageSpawner : MonoBehaviour
{   
    public GameObject[] prefab;


    public void TriggerSpawn(Vector3 pos)
    {
        for (int i = 0; i < 4; i++)
        {
            
            for (int ii = 0; ii < prefab.Length; ii++)
            {

                GameObject spawnedObject = Instantiate(prefab[ii], pos + new Vector3(0.2f * Random.Range(1f, 3f) - 0.2f, 0, 0.2f * Random.Range(1f, 2f) - 0.2f), Quaternion.identity);
                Rigidbody newBody = (Rigidbody) spawnedObject.AddComponent<Rigidbody>();
                MeshCollider newMesh = (MeshCollider) spawnedObject.AddComponent<MeshCollider>();

                newMesh.convex = true;
                newBody.mass = 10;
                newBody.AddForce(600.0f * new Vector3(Random.Range(-2f, 2f), Random.value * 5, Random.Range(-1f, 1f)));
                newBody.AddTorque(300.0f * new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-1f, 1f)));

                Transform objectTransform = spawnedObject.transform;
                objectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
    }

}