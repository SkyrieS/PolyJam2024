using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandLineDrawer : MonoBehaviour
{
    public ComebackTest comeback;
    public Rigidbody2D rb;

    [SerializeField] private Gradient colorGradient;
    [SerializeField] private SpriteRenderer sleeve;
    [SerializeField] private AnimationCurve colorCurve;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PolygonCollider2D polygonCollider;

    [SerializeField] private float deltaThreshold = 0.01f;
    [SerializeField] private float maxLineDistance = 15f;

    public List<Vector3> pointsV2List = new List<Vector3>();
    public List<float> rotations = new List<float>();
    public float totalDistance;

    public Vector2 prevPosition;
    public bool comingBack;

    private void Start()
    {
        prevPosition = rb.transform.position;
        pointsV2List.Add(prevPosition);
    }

    private void Update()
    {
        UpdateLine();
        UpdateSleeveColor();

        if (comingBack)
            return;

        float distance = Vector2.Distance(prevPosition, rb.transform.position);
        if (distance > deltaThreshold)
        {
            totalDistance += distance;
            pointsV2List.Add(rb.transform.position);
            rotations.Add(rb.transform.rotation.eulerAngles.z);
            prevPosition = rb.transform.position;
        }
    }

    public void UpdateSleeveColor()
    {
        var color = colorGradient.Evaluate(Mathf.InverseLerp(0f, comeback.maxLenght, totalDistance));
        lineRenderer.endColor = color;
        lineRenderer.startColor = colorGradient.Evaluate(0f);
        color.a = 1f;
        sleeve.color = color;
    }

    public void UpdateLine()
    {
        lineRenderer.positionCount = pointsV2List.Count;
        lineRenderer.SetPositions(pointsV2List.ToArray());
    }

    public Mesh mesh;
    [ContextMenu("Bake")]
    private void DrawLine()
    { 
        lineRenderer.BakeMesh(mesh);
        //lineRenderer.positionCount = pointsV2List;
        //lineRenderer.SetPositions(pointsV3List.ToArray());
    }

    private void OnDrawGizmos()
    {
        for(int i = 1; i < pointsV2List.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointsV2List[i - 1], pointsV2List[i]);
        }
    }
}