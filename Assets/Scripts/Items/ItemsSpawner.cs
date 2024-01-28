using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] private SpawnedItemsContainer spawnedItemsContainer;
    [SerializeField] private Vector2 randomSpawnTimeRanges;
    [SerializeField] private Vector2 randomForceRanges;
    [SerializeField] private Vector2 randomTorgueRanges;
    [SerializeField] private int maxItemCount;
    [SerializeField] private List<Vector2> spawnPoints;

    private float spawnTime;
    private bool canSpawnItems;
    public bool CanSpawnItems => canSpawnItems && spawnedItemsContainer.SpawnedItems.Count < maxItemCount;

    public void StartSpawning()
    {
        canSpawnItems = true;
        spawnTime = Time.time + Random.Range(randomForceRanges.x, randomForceRanges.y);
    }

    public void StopSpawning()
    {
        canSpawnItems = false;
    }

    public void Clear()
    {
        spawnedItemsContainer.Clear();
    }

    private void Update()
    {
        if (!CanSpawnItems)
        {
            return;
        }

        if (Time.time > spawnTime)
        {
            spawnTime = Time.time + Random.Range(randomSpawnTimeRanges.x, randomSpawnTimeRanges.y);

            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        ItemData item = spawnedItemsContainer.ItemsDatabase.GetRandomItem();
        Vector2 spawnPoint = GetRandomSpawnPoint();
        Vector2 direction = -spawnPoint.normalized;
        float randomForce = Random.Range(randomForceRanges.x, randomForceRanges.y);
        float randomTorque = Random.Range(randomForceRanges.x, randomForceRanges.y);

        BaseItem itemObj = Instantiate(item.item_prefab);
        itemObj.transform.position = spawnPoint;
        itemObj.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
        itemObj.AddForce(direction * randomForce, randomTorque);
        itemObj.Item_id = item.item_id;
        spawnedItemsContainer.AddItem(itemObj);
    }

    private Vector2 GetRandomSpawnPoint()
    {
        int random = Random.Range(0, spawnPoints.Count);
        return spawnPoints[random];
    }

    private void OnDrawGizmos()
    {
        foreach (var pos in spawnPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pos, 0.5f);
        }
    }
}
