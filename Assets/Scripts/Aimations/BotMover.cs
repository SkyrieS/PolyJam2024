using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    public Vector3 from;
    public Vector3 to;
    public float speed;

    private void Start()
    {
        speed = Random.Range(1f, 5f);
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(from, to, Mathf.InverseLerp(-1f, 1f, Mathf.Sin(Time.time * speed)));
    }
}
