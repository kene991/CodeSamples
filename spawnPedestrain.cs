using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPedestrain : MonoBehaviour
{
    public GameObject[] pedestrainsPrefab;

    public GameObject[] spawnPoints;
    public int spawnIndex;
    public int customerSpawnIndex;

    [SerializeField] private float countdown;
    int spawnCount = 5;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnIndex = Random.Range(0, spawnPoints.Length);
        if (countdown > spawnCount)
        {
            customerSpawnIndex = Random.Range(0, pedestrainsPrefab.Length);
            GameObject pedestrains = Instantiate(pedestrainsPrefab[customerSpawnIndex], spawnPoints[spawnIndex].transform.position, spawnPoints[spawnIndex].transform.rotation * Quaternion.Euler(0, 90, 0)) as GameObject;
            countdown = 0;
        }
        countdown += Time.deltaTime;
    }

   
}
