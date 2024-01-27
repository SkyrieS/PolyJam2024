using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private float directionTreshold = 0.2f;
    [SerializeField] private float maxLineDistance = 15f;
    [SerializeField] private float scanTime = 0.5f;

    public List<Vector3> pointsV3List = new List<Vector3>();
    public List<Vector2> pointsV2List = new List<Vector2>();
    public Vector2 previousPoint;
    public float _deltaDistance;
    public float _lastTime;

    private void Start()
    {
        previousPoint = lineRenderer.transform.position;
    }

    private void Update()
    {
        if (_deltaDistance >= maxLineDistance)
        {
            return;
        }

        if (Time.time > _lastTime)
        {
            _lastTime = Time.time + scanTime;

            Vector2 currentPositon = lineRenderer.transform.position;
            Vector2 previousPositon = previousPoint;
            float distance = Vector2.Distance(previousPositon, currentPositon);

            Debug.Log($"dis {distance} tresh {directionTreshold}");
            if (distance > directionTreshold)
            {
                previousPoint = currentPositon;
                pointsV3List.Add(currentPositon);
                pointsV2List.Add(currentPositon);
                _deltaDistance += distance;
                DrawLine();
                GeneratePolygonCollider();
            }
        }
    }

    private void DrawLine()
    {
        lineRenderer.positionCount = pointsV3List.Count;
        lineRenderer.SetPositions(pointsV3List.ToArray());
    }

    private void GeneratePolygonCollider()
    {
        polygonCollider.points = pointsV2List.ToArray();
    }
}