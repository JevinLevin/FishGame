using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishManager : MonoBehaviour
{
    [Header("Assets")] 
    [SerializeField] private GameObject fishObject;

    [Header("Attributes")] 
    [SerializeField] private Vector2 spawnDelayBounds;

    private float spawnTimer;
    private float spawnDelay;

    private void Awake()
    {
        ResetTimer();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            SpawnFish();
        }
        
    }

    private void ResetTimer()
    {
        spawnTimer = 0.0f;
        spawnDelay = Random.Range(spawnDelayBounds.x, spawnDelayBounds.y);
    }

    private void SpawnFish()
    {
        // Spawning logic
        LootScriptableObject lootType = LootManager.Instance.GenerateLoot(LootManager.fishScriptableObjects);
        Fish newFish = Instantiate(fishObject, transform).GetComponent<Fish>();
        newFish.Setup(lootType);
        
        ResetTimer();
    }
}
