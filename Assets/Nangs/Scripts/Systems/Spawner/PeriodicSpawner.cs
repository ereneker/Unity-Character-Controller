using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PeriodicSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn = null;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Renderer arenaRenderer = null;
    [SerializeField] [Range(0.25f, 4f)] private float minScaleSize = 1f;
    [SerializeField] private float maxScaleSize = 1f;
    [SerializeField] private Vector3 transformBounds;
    [SerializeField] private bool waitForString = false;
    [SerializeField] private string stringToWaitFor = "";

    private float nextSpawn = 0f;
    public List<GameObject> objectList = new List<GameObject>();

    private void Awake()
    {
        nextSpawn = spawnInterval;
        transformBounds = arenaRenderer.bounds.extents * 0.9f;
        MusicManager.beatUpdated += SpawnObject;
    }

    private void Update()
    {
        if (nextSpawn <= 0f)
        {
            SpawnObject();
        }

        nextSpawn -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        MusicManager.beatUpdated -= SpawnObject;
    }

    private void SpawnObject()
    {

        if (!waitForString)
        {
            if (nextSpawn > 0)
            {
                nextSpawn--;
            }
            else
            {
                float xRand = Random.Range(-transformBounds.x, transformBounds.x);
                float zRand = Random.Range(-transformBounds.z, transformBounds.z);
                float randScale = Random.Range(minScaleSize, maxScaleSize);
                var obj = Instantiate(objectToSpawn, new Vector3(xRand, 0, zRand), Quaternion.identity);
                obj.transform.localScale = new Vector3(randScale * obj.transform.lossyScale.x,
                    randScale * obj.transform.lossyScale.y, randScale * obj.transform.lossyScale.z);
                nextSpawn = spawnInterval - 1;
            }
        }
    }
}
