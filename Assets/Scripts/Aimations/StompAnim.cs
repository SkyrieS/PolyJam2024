using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompAnim : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Vector3 startrSize;
    public Vector3 targetSize;
    public AnimationCurve alphaCurve;
    public AnimationCurve animationCurve;
    public float duration;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    public void SetTargetSize(Vector3 target)
    {
        targetSize = target;
    }

    private void Update()
    {
        Color col = Color.white;
        float progress = (Time.time - startTime) / duration;
        sprite.gameObject.transform.localScale = Vector3.Lerp(startrSize, targetSize, animationCurve.Evaluate(progress));
        col.a = alphaCurve.Evaluate(progress);
        sprite.color = col;

        if (progress >= 1f)
            Destroy(gameObject);
    }
}
