using Game.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveManager : MonoBehaviour
{
    [SerializeField] private ReceiveZone zonePrefab;
    [SerializeField] private Transform zoneParent;
    [SerializeField] private SpawnedItemsContainer spawnedItemsContainer;
    [SerializeField] private float requestWaitTime;

    private List<ReceiveZone> spawnedZones;

    private ScoreManager scoreManager;
    private MapGenerator mapGenerator;

    private Coroutine requestFoodCoroutine;

    private void OnDestroy()
    {
        StopCoroutine(requestFoodCoroutine);
    }

    public void Initialize(ScoreManager scoreManager, MapGenerator mapGenerator)
    {
        this.scoreManager = scoreManager;
        this.mapGenerator = mapGenerator;
    }

    public void SpawnReceivers()
    {
        spawnedZones = mapGenerator.GenerateZones(zonePrefab, zoneParent);

        foreach (var zone in spawnedZones)
        {
            zone.Initialize(spawnedItemsContainer, scoreManager);
        }

        requestFoodCoroutine = StartCoroutine(RequestFoodAfterTime(requestWaitTime));
    }

    private IEnumerator RequestFoodAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        ReceiveZone zone = GetRandomEmptyZone();
        ItemData itemData = spawnedItemsContainer.RequestItemData();

        if (zone != null && itemData != null)
        {
            zone.RequestFood(itemData);
        }

        requestFoodCoroutine = StartCoroutine(RequestFoodAfterTime(requestWaitTime));
    }

    private ReceiveZone GetRandomEmptyZone()
    {
        List<ReceiveZone> emptyZones = new List<ReceiveZone>();
        foreach(var zone in spawnedZones)
        {
            if (!zone.HasRequest)
                emptyZones.Add(zone);
        }

        if (emptyZones.Count == 0)
            return null;

        int randomIndex = Random.Range(0, emptyZones.Count);
        return emptyZones[randomIndex];
    }
}
