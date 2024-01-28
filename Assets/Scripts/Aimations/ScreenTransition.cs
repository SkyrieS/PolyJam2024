using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private SpriteRenderer screenTransition;

    public bool update;

    private float startVal;
    private float targetVal;
    private float startTime;
    private UnityAction OnTarget;

    private int CACHED_SHADER_VAL = Shader.PropertyToID("_Progress");

    private void Start()
    {
        screenTransition.material.SetFloat(CACHED_SHADER_VAL, 1f);
    }

    public void SetTarget(float target, UnityAction OnTargetAchieved)
    {
        startTime = Time.time;
        startVal = screenTransition.material.GetFloat("_Progress");
        targetVal = target;
        update = true;
        OnTarget = OnTargetAchieved;
    }

    private void Update()
    {
        if (!update)
            return;

        float progress = (Time.time - startTime) / duration;
        float val = Mathf.Lerp(startVal, targetVal, progress);
        screenTransition.material.SetFloat(CACHED_SHADER_VAL, val);
        if(progress >= 1f)
        {
            OnTarget?.Invoke();
            OnTarget = null;
            update = false;
        }
    }
}
