using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneezeReceiverSkill : BaseReceiveSkill
{
    [SerializeField] private AreaEffector2D effector;
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private float distance;
    [SerializeField] private float duration;
    [SerializeField] private float startForce;
    [SerializeField] private float angle;
    [SerializeField] private AnimationCurve forceOverDistance;
    [SerializeField] private AnimationCurve movementCurve;

    private float startTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public override void Perform()
    {
        startTime = Time.time;
        startPosition = transform.position;
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-angle / 2f, angle / 2f)) * transform.parent.rotation;
        targetPosition = transform.position + (-transform.up * distance);
        effector.transform.position = startPosition;
        effector.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (effector.gameObject.activeSelf)
        {
            float progress = (Time.time - startTime) / duration;
            effector.transform.position = Vector3.Lerp(startPosition, targetPosition, movementCurve.Evaluate(progress));
            effector.forceMagnitude = startForce * forceOverDistance.Evaluate(progress);

            Color col = Color.white;
            col.a = 1f - progress;
            rend.color = col;
            if(progress >= 1f)
            {
                effector.gameObject.SetActive(false);
            }
        }
    }
}
