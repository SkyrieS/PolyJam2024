using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveZone : MonoBehaviour
{
    [SerializeField] private List<BaseReceiveSkill> skills;
    [SerializeField] private float timeBetweenUsingSkills;
    [SerializeField] private float timeVariation;

    private float currentTime;
    private float waitTime;

    private void Start()
    {
        SetPerformTime(2f);
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
}
