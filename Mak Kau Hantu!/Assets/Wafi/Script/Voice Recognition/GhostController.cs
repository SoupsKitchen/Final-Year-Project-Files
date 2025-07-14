using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    public void RunAwayFromPlayer()
    {
        Debug.Log("Ghost (Sphere): Running away!");
        // Move backward from current facing direction
        targetPosition = transform.position - transform.forward * moveDistance;
        isMoving = true;
    }

    public void ChasePlayerFaster()
    {
        Debug.Log("Ghost (Sphere): Chasing player!");
        // Move forward from current facing direction
        targetPosition = transform.position + transform.forward * moveDistance;
        isMoving = true;
    }
}