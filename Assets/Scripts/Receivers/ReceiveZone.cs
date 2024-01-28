using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Items;
using Game.Score;
using UnityEngine.SceneManagement;

public class ReceiveZone : MonoBehaviour
{
    [SerializeField] private List<BaseReceiveSkill> skills;
    [SerializeField] private float timeBetweenUsingSkills;
    [SerializeField] private float timeVariation;
    [SerializeField] private ScoreType scoreType;
    [SerializeField] private SpriteRenderer foodSprite;
    [SerializeField] private GameObject foodObj;

    private SpawnedItemsContainer spawnedItemsContainer;
    private ScoreManager scoreManager;

    private int foodRequested_id;
    private bool hasRequest;
    public bool HasRequest => hasRequest;

    private float currentTime;
    private float waitTime;

    private void Start()
    {
        SetPerformTime(2f);
    }

    public void Initialize(SpawnedItemsContainer spawnedItemsContainer, ScoreManager scoreManager)
    {
        this.spawnedItemsContainer = spawnedItemsContainer;
        this.scoreManager = scoreManager;
    }

    public void RequestFood(ItemData itemData)
    {
        foodRequested_id = itemData.item_id;
        foodObj.gameObject.SetActive(true);
        foodSprite.sprite = itemData.item_sprite;
        foodSprite.transform.localScale = new Vector3(itemData.sprite_size, itemData.sprite_size, itemData.sprite_size);
        foodSprite.transform.rotation = Quaternion.identity;
        hasRequest = true;
    }

    private void SetPerformTime(float modifier = 1f)
    {
        currentTime = 0;
        waitTime = (timeBetweenUsingSkills + Random.Range(-timeVariation / 2f, timeVariation / 2f)) * modifier;
    }

    private void Update()
    {
        if(currentTime >= waitTime)
        {
            BaseReceiveSkill randomSkill = skills[Random.Range(0, skills.Count)];
            randomSkill.Perform();
            currentTime = 0;
            SetPerformTime();
            return;
        }

        currentTime += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BaseItem item = collision.transform.GetComponent<BaseItem>();
        if(item != null && item.Item_id == foodRequested_id)
        {
            foreach (var player in item.currentTags)
                Debug.Log(player);

            foodObj.gameObject.SetActive(false);
            hasRequest = false;
            RemoveItem(item);
        }
    }

    private void RemoveItem(BaseItem item)
    {
        AddScore(item);
        spawnedItemsContainer.RemoveItem(item);
    }

    private void AddScore(BaseItem item)
    {
        switch (scoreType)
        {
            case ScoreType.RelativeToDistance:
                AddScoreRelativeToDistance(item.currentTags);
                break;
            case ScoreType.LastHit:
                AddScoreByLastHit(item.lastTag);
                break;
        }
    }

    private void AddScoreRelativeToDistance(List<PlayerTag> closeByTags)
    {
        foreach (var tag in closeByTags)
        {
            scoreManager.IncreaseScoreByOne(tag.type);
        }
        scoreManager.CheckGameWon();
    }

    private void AddScoreByLastHit(PlayerTag lastTag)
    {
        scoreManager.IncreaseScoreByOne(lastTag.type);
        scoreManager.CheckGameWon();
    }

    public enum ScoreType
    {
        RelativeToDistance,
        LastHit
    }
}
