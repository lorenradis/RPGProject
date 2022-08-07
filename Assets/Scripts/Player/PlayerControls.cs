using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D col2d;
    private Rigidbody2D rb2d;
    [SerializeField]
    private SpriteRenderer grassRenderer;

    [SerializeField]
    private AudioClip[] footSteps;

    [SerializeField]
    private LayerMask interactLayers;

    [SerializeField]
    private Transform checkSource;

    private Vector2 facingVector;

    private bool isMoving = false;

    private float moveSpeed = 0f;
    private float maxMoveSpeed = 5f;
    private float minMoveSpeed = .25f;

    private bool isRunning = false;
    private float moveMod = 1f;
    private float accel = .5f;

    private float checkDist = 1f;

    private float footstepFrequency = .35f;
    private float footstepTimer = 0f;

    private void Start()
    {
        GameManager.instance.Player = transform;
        facingVector = Vector2.down;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col2d = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.NORMAL && !GameManager.instance.hasReceivedInput)
        {
            if (isMoving)
            {
                footstepTimer += Time.deltaTime;
                if (footstepTimer > footstepFrequency)
                {
                    footstepTimer -= footstepFrequency;
                    GameManager.instance.audioManager.PlaySoundEffect(footSteps[Random.Range(0, footSteps.Length)]);
                }
            }
            ManageInput();
            UpdateAnimator();
        }
    }

    private void ManageInput()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movementVector != Vector2.zero)
        {
            isMoving = true;
            facingVector = movementVector.normalized;
        }
        else
        {
            isMoving = false;
        }

        //A Button - interact
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Return))
        {
            AttemptInteract();
        }
        else if (Input.GetKey("joystick button 0"))
        {

        }
        else if (Input.GetKeyUp("joystick button 0"))
        {

        }

        //B Button - run, cancel
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Space))
        {
            StartRunning();
        }
        else if (Input.GetKey("joystick button 1") || Input.GetKey(KeyCode.Space))
        {
            if (!isRunning)
            {
                StartRunning();
            }
        }
        else if (Input.GetKeyUp("joystick button 1") || Input.GetKeyUp(KeyCode.Space))
        {
            StopRunning();
        }

        //X Button - bring up map, menu, something?
        if (Input.GetKeyDown("joystick button 2") || Input.GetKeyDown(KeyCode.X))
        {
            GameManager.instance.ShowInGameMenu();
        }
        else if (Input.GetKey("joystick button 2"))
        {

        }
        else if (Input.GetKeyUp("joystick button 2"))
        {

        }

        //Y button - use item?
        if (Input.GetKeyDown("joystick button 3"))
        {

        }
        else if (Input.GetKey("joystick button 3"))
        {

        }
        else if (Input.GetKeyUp("joystick button 3"))
        {

        }

        //L Button
        if (Input.GetKeyDown("joystick button 4"))
        {

        }
        else if (Input.GetKey("joystick button 4"))
        {

        }
        else if (Input.GetKeyUp("joystick button 4"))
        {

        }

        //R Button
        if (Input.GetKeyDown("joystick button 5"))
        {

        }
        else if (Input.GetKey("joystick button 5"))
        {

        }
        else if (Input.GetKeyUp("joystick button 5"))
        {

        }

        //Select Button
        if (Input.GetKeyDown("joystick button 6"))
        {

        }
        else if (Input.GetKey("joystick button 6"))
        {

        }
        else if (Input.GetKeyUp("joystick button 6"))
        {

        }
    }

    public void StopMoving()
    {
        isMoving = false;
        UpdateAnimator();
    }

    private void StartRunning()
    {
        isRunning = true;
        moveMod = 2f;
    }

    private void StopRunning()
    {
        isRunning = false;
        moveMod = 1f;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameState != GameManager.GameState.NORMAL)
            return;
        if (isMoving)
        {
            moveSpeed += (1f * accel);

        }
        else
        {
            moveSpeed -= (2f * accel);
        }
        moveSpeed = Mathf.Clamp(moveSpeed, 0f, maxMoveSpeed);

        if (moveSpeed > minMoveSpeed)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb2d.position, rb2d.position + facingVector, moveSpeed * Time.deltaTime * moveMod);
            rb2d.MovePosition(newPosition);
        }

    }

    private void UpdateAnimator()
    {
        animator.SetFloat("inputX", facingVector.x);
        animator.SetFloat("inputY", facingVector.y);
        animator.SetBool("isMoving", isMoving);
        spriteRenderer.flipX = facingVector.x < 0;
    }

    private void AttemptInteract()
    {
        Debug.Log("Trying to interact");
        col2d.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(checkSource.position, facingVector, checkDist, interactLayers);
        col2d.enabled = true;
        if (hit.transform != null)
        {
            Debug.Log("I hit a " + hit.transform.name);
            hit.transform.GetComponent<Interactable>().OnInteract(transform);
        }
    }

    public void ActivateGrassOverlay()
    {
        grassRenderer.enabled = true;
    }

    public void DeactivateGrassOverlay()
    {
        grassRenderer.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(checkSource.position, facingVector);
    }
}