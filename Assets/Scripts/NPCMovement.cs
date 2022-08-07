using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovement : MOB
{

    /*
    should contain decision making about when and where to move.
    should this also inherit from interactable so that it can respond to player interaction, or would that be a separate monobehaviour?
        we want this to stop moving and face the player on interact so... yes?
   
    */
    public NPCInfo npcInfo;
    public DialogInteractable dialogInteraction;

    public enum NPCState { IDLE, MOVE, TALK, INTERACT }
    public NPCState npcState;
    private float timeInState = 0f;
    private void ChangeState(NPCState newState)
    {
        if (npcState != newState)
        {
            npcState = newState;
            timeInState = 0f;
            randomTime = Random.Range(-3f, 3f);
        }
    }

    private float idleTimer = 7f;
    private float moveTimer = 10f;
    private float randomTime = 0f;

    private Animator animator;

    private void Start()
    {
        randomTime = Random.Range(-3f, 3f);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.NORMAL)
        {
            ManageState();
            UpdateAnimator();
        }
    }

    private void ManageState()
    {
        timeInState += Time.deltaTime;
        switch (npcState)
        {
            case NPCState.IDLE:
                if (timeInState > idleTimer + randomTime)
                {
                    MoveToDestination(new Vector2(transform.position.x + Random.Range(-5f, 5f), transform.position.y + Random.Range(-5f, 5f)));
                    ChangeState(NPCState.MOVE);
                }
                break;
            case NPCState.MOVE:
                if(timeInState > moveTimer + randomTime)
                {
                    StopMoving();
                    ChangeState(NPCState.IDLE);
                }
                break;
            case NPCState.TALK:

                break;
            case NPCState.INTERACT:

                break;
            default:
                break;
        }
    }

    public void OnInteract()
    {
        StopMoving();
        FacePlayer();
        //ChangeState(NPCState.TALK);
    }

    private void FacePlayer()
    {
        facingVector = (GameManager.instance.Player.position - transform.position).normalized;
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("inputX", facingVector.x);
        animator.SetFloat("inputY", facingVector.y);
        animator.SetBool("isMoving", npcState == NPCState.MOVE);
    }

    public void SetNPCInfo(NPCInfo newNPC)
    {
        npcInfo = newNPC;
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = npcInfo.animatorController;
        DialogInteractable dialog = GetComponent<DialogInteractable>();
        dialog.ClearDialogs();
        for (int i = 0; i < npcInfo.greetings.Length; i++)
        {
            dialog.greetings.Add(npcInfo.greetings[i]);
        }

        for (int i = 0; i < npcInfo.dialogs.Length; i++)
        {
            dialog.dialogs.Add(npcInfo.dialogs[i]);
        }

        for (int i = 0; i < npcInfo.farewells.Length; i++)
        {
            dialog.farewells.Add(npcInfo.farewells[i]);
        }
    }
}