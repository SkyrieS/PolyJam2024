using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComebackTest : MonoBehaviour
{
    public List<Vector3> line;
    public Rigidbody2D rb;
    public float speed = 1f;
    public float rotationSpeed = 2f;
    public float threshold;
    public float moveVal;

    public Queue<Vector3> positions = new Queue<Vector3>();

    [ContextMenu("Begin")]
    public void Begin()
    {
        foreach (var lin in line)
            positions.Enqueue(lin);
    }

    private void Start()
    {
        
    }

    public void FixedUpdate()
    {
        if (positions.Count == 0)
            return;

        float distance = Vector3.Distance(rb.transform.position, positions.Peek());
        if(distance < threshold)
        {
            positions.Dequeue();
            return;
        }

        float normalized = moveVal / distance;

        Vector3 targetPos = Vector3.Lerp(rb.transform.position, positions.Peek(), normalized * Time.deltaTime);
        Vector3 dir = targetPos - rb.transform.position;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        Quaternion currentRot = Quaternion.Lerp(rb.transform.rotation, targetRot, Time.deltaTime * rotationSpeed);

        rb.MovePosition(targetPos);
        rb.MoveRotation(currentRot);

        //if((rb.transform.position - targetPos).magnitude < threshold)
            //positions.Dequeue();
    }

    private void OnDrawGizmos()
    {
        for(int i = 1; i < line.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(line[i - 1], line[i]);
        }
    }
}
