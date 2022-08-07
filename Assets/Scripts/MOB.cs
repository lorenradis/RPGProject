using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MOB : MonoBehaviour
{
    protected bool isMoving = false;

    private Rigidbody2D rb2d;

    [SerializeField]
    private float moveSpeed = 3f;
    private float moveMod = 1f;

    private Transform target;
    private Vector2 direction;
    private Vector2 destination;

    protected Vector2 facingVector;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void MoveInDirection(Vector2 newDirection)
    {
        StopMoving();
        direction = newDirection;
        isMoving = true;
    }

    public void MoveToDestination(Vector2 newDestination)
    {
        StopMoving();
        destination = newDestination;
        isMoving = true;
    }

    public void MoveToTarget(Transform newTarget)
    {
        StopMoving();
        target = newTarget;
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
        target = null;
        destination = Vector2.zero;
        direction = Vector2.zero;
    }

    public void SetMoveMod(float newMod)
    {
        moveMod = newMod;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameState == GameManager.GameState.NORMAL)
        {
            if (isMoving)
            {
                Vector2 newPosition = transform.position;
                if (target != null)
                {
                    newPosition = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed * moveMod);
                }
                else if (destination != Vector2.zero)
                {
                    newPosition = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed * moveMod);
                }
                else if (direction != Vector2.zero)
                {
                    newPosition = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, Time.deltaTime * moveSpeed * moveMod);
                }
                facingVector = (newPosition - rb2d.position).normalized;
                rb2d.MovePosition(newPosition);
            }
        }
    }
}