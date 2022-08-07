using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private Vector2 minBounds;
    [SerializeField]
    private Vector2 maxBounds;

    private Vector2 movementVector;

    private float smoothing = .125f;

    private float cutSceneMoveSpeed = 2f;

    private Vector2 offset;

    private enum CameraMode { NORMAL, CUTSCENE }
    private CameraMode cameraMode;

    private enum CutSceneMode { FOLLOW, STILL, MOVE }
    private CutSceneMode cutSceneMode;

    private void LateUpdate()
    {
        if (GameManager.instance.gameState == GameManager.GameState.MAINMENU)
            return;
        Vector3 targetPosition;
        Vector3 newPosition;
        switch (cameraMode)
        {
            case CameraMode.NORMAL:
                if (target == null)
                    target = GameManager.instance.Player;
                targetPosition = new Vector3(target.position.x, target.position.y, -10f);
                targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
                newPosition = Vector3.Lerp((Vector2)transform.position, targetPosition + (Vector3)offset, smoothing);

                transform.position = new Vector3(newPosition.x, newPosition.y, -10f);

                break;
            case CameraMode.CUTSCENE:

                if (target != null)
                {
                    newPosition = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * cutSceneMoveSpeed);
                    transform.position = new Vector3(newPosition.x, newPosition.y, -10f);
                }
                else if (movementVector != Vector2.zero)
                {
                    newPosition = Vector2.MoveTowards(transform.position, transform.position + (Vector3)movementVector, Time.deltaTime * cutSceneMoveSpeed);
                    transform.position = new Vector3(newPosition.x, newPosition.y, -10f);
                }
                break;
            default:
                break;
        }

    }

    public void SetMinBounds(Vector2 newBounds)
    {
        Vector2 camSizeOffset = new Vector2(Camera.main.orthographicSize * 16f / 9f, Camera.main.orthographicSize);
        minBounds = newBounds + camSizeOffset;
    }

    public void SetMaxBounds(Vector2 newBounds)
    {
        Vector2 camSizeOffset = new Vector2(Camera.main.orthographicSize * 16f / 9f, Camera.main.orthographicSize);
        maxBounds = newBounds - camSizeOffset;
    }

    public void SetSmoothing(float _smoothing)
    {
        smoothing = _smoothing;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetFollowMode()
    {
        cameraMode = CameraMode.NORMAL;
    }

    public void SetCutSceneMode()
    {
        cameraMode = CameraMode.CUTSCENE;
    }

    public void SetMovementVector(Vector2 _movementVector)
    {
        movementVector = _movementVector;
    }

    public void SetOffset(float x, float y)
    {
        offset = new Vector2(x, y);
    }
}