using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private EnviroColliders colliders;
    [SerializeField] private float cameraSize;
    [SerializeField, Range(0f, 3f)] private float wallThickness;

    private Vector2Int AspectRatio => new Vector2Int(16, 10);

    public GameObject testOb;

    private void Update()
    {
        float halfSize = (cameraSize * 2f)/ AspectRatio.y;
        float targetX = AspectRatio.x * halfSize;
        float targetY = AspectRatio.y * halfSize;
        cam.orthographicSize = cameraSize;
        testOb.transform.localScale = new Vector3(targetX, targetY, 0f);
    }

    private Vector2 CalculateEnviroSize()
    {
        float halfSize = (cameraSize * 2f) / AspectRatio.y;
        return new Vector2(AspectRatio.x * halfSize, AspectRatio.y * halfSize);
    }

    [ContextMenu("gen")]
    public void Gen()
    {
        colliders.Adjust(CalculateEnviroSize(), wallThickness);
    }

    private void OnValidate()
    {
        //Gen();
    }

    private void OnDrawGizmos()
    {
        colliders.DrawGizmos();
    }
}