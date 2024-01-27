using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    public UnityAction<Collision2D> OnCollisionStarted;
    public UnityAction<Collision2D> OnCollisionEnded;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionStarted?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionEnded?.Invoke(collision);
    }
}
