using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActor : MonoBehaviour
{
    protected bool isMoving = false;

    private Animator animator;
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
        animator = GetComponent<Animator>();
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

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("moveX", facingVector.x);
        animator.SetFloat("moveY", facingVector.y);
        animator.SetBool("isMoving", isMoving);
    }

    private void FixedUpdate()
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