using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MOB
{
    public UnitInfo enemyUnit;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float sightRadius = 8f;

    private enum MoveState { IDLE, WANDER, CHASE, FLEE }
    private MoveState moveState;
    private float timeInState = 0f;
    private float actionTimer = 3f;
    private float randomTime;
    private void ChangeState(MoveState newState)
    {
        if(moveState != newState)
        {
            moveState = newState;
            timeInState = 0f;
            randomTime = Random.Range(-2f, 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.StartBattle(enemyUnit);
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {

        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("ActivateTrigger", 1.75f);
    }

    private void ActivateTrigger()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    private void Update()
    {
        if(GameManager.instance.gameState == GameManager.GameState.NORMAL)
        {
            ManageState();
            UpdateAnimator();
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isMoving", isMoving);
        spriteRenderer.flipX = facingVector.x > 0;
    }

    private void ManageState()
    {
        timeInState += Time.deltaTime;
        switch (moveState)
        {
            case MoveState.IDLE:
                if (timeInState > (actionTimer + randomTime))
                {
                    if (PlayerInRange())
                    {
                        int dif = enemyUnit.level - GameManager.instance.playerInfo.level;
                        if (dif > -2)
                        {
                            StartChasing(GameManager.instance.Player);
                        }
                        else
                        {
                            StartFleeing(GameManager.instance.Player);
                        }
                    }
                    else
                    {
                        Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                        MoveInDirection(newDirection);
                        SetMoveMod(.5f);
                        ChangeState(MoveState.WANDER);
                    }
                }
                break;
            case MoveState.WANDER:
                if (PlayerInRange())
                {
                    if (enemyUnit.level >= GameManager.instance.playerInfo.level)
                    {
                        StartChasing(GameManager.instance.Player);
                    }
                    else
                    {
                        StartFleeing(GameManager.instance.Player);
                    }
                }
                else
                {
                    if (timeInState > (actionTimer + randomTime))
                    {
                        StopMoving();
                        ChangeState(MoveState.IDLE);
                    }
                }

                break;
            case MoveState.CHASE:
                if (!PlayerInRange())
                {
                    StopMoving();
                    ChangeState(MoveState.IDLE);
                }
                break;
            case MoveState.FLEE:
                if (!PlayerInRange())
                {
                    StopMoving();
                    ChangeState(MoveState.IDLE);
                }
                break;
        }
    }

    private bool PlayerInRange()
    {
        float playerDist = Vector2.Distance(transform.position, GameManager.instance.Player.position);
        return playerDist < sightRadius;
    }

    private void StartChasing(Transform newTarget)
    {
        StartCoroutine(StartChasingRoutine(newTarget));
    }

    private IEnumerator StartChasingRoutine(Transform newTarget)
    {
        //play some kind of alert animation
        yield return new WaitForSeconds(.25f);
        MoveToTarget(newTarget);
        SetMoveMod(1.5f);
    }

    private void StartFleeing(Transform newTarget)
    {
        StartCoroutine(StartFleeingRoutine(newTarget));
    }

    private IEnumerator StartFleeingRoutine(Transform newTarget)
    {
        //play alert animation
        yield return new WaitForSeconds(.25f);

        Vector2 newDirection = (transform.position - newTarget.position).normalized;
        MoveInDirection(newDirection);
        ChangeState(MoveState.FLEE);
    }

    public void SetEnemyUnit(UnitInfo unitInfo)
    {
        enemyUnit = unitInfo;
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = enemyUnit.overworldAnimator;
        SetMoveMod(enemyUnit.moveMod);
    }
}
