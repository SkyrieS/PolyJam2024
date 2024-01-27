using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComebackTest : MonoBehaviour
{
    public List<Collider2D> collidersToDisable;
    public PlayerHandMovement handMovement;
    public HandLineDrawer drawer;

    public Rigidbody2D rb;
    public float rotationSpeed = 2f;
    public float threshold;
    public float moveVal;
    public float maxLenght;

    public Stack<Vector3> positions = new Stack<Vector3>();
    public Stack<float> rotations = new Stack<float>();

    [ContextMenu("Begin")]
    public void Begin()
    {
        foreach (var lin in drawer.pointsV2List)
            positions.Push(lin);

        foreach (var rot in drawer.rotations)
            rotations.Push(rot);
            

        drawer.comingBack = true;
        handMovement.comingBack = true;

        foreach (var col in collidersToDisable)
            col.enabled = false;
    }

    private void Update()
    {
        if (positions.Count == 0 && drawer.totalDistance > maxLenght)
        {
            drawer.totalDistance = 0;
            Begin();
        }

        if (positions.Count == 0)
            return;

        float distance = Vector3.Distance(rb.transform.position, positions.Peek());
        if (distance < threshold)
        {
            positions.Pop();
            rotations.Pop();
            if (drawer.pointsV2List.Count > 0)
                drawer.pointsV2List.RemoveAt(drawer.pointsV2List.Count - 1);

            if (positions.Count == 0)
            {
                drawer.comingBack = false;
                handMovement.comingBack = false;
                foreach (var col in collidersToDisable)
                    col.enabled = true;

                drawer.prevPosition = drawer.rb.transform.position;
                drawer.pointsV2List.Add(drawer.prevPosition);
                handMovement.RefreshRot();
                drawer.rotations.Add(handMovement.currentAngle);
            }

            return;
        }

        targetPos = Vector3.Lerp(rb.transform.position, positions.Peek(), moveVal * Time.deltaTime);

        Quaternion savedRot = Quaternion.Euler(0f, 0f, rotations.Peek());
        currentRot = Quaternion.Lerp(rb.transform.rotation, savedRot, rotationSpeed * Time.deltaTime);

    }

    Vector3 targetPos;
    Quaternion currentRot;

    public void FixedUpdate()
    {
        if (positions.Count == 0)
            return;

        rb.MovePosition(targetPos);
        rb.MoveRotation(currentRot);
    }
}
