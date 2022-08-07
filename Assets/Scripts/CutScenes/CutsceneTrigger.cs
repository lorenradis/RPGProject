using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public CutScene cutScene;

    private bool isActive = false;

    public GameObject[] toDeactivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActive)
        {
            StartCutscene();
        }
    }

    public virtual void StartCutscene()
    {
        if (isActive)
            return;
        isActive = true;

        StartCoroutine(RenderCutScene());
    }

    protected virtual IEnumerator RenderCutScene()
    {
        GameManager.instance.EnterCutSceneState();
        
        Camera.main.GetComponent<CameraMovement>().SetCutSceneMode();

        AudioClip previousBGM = GameManager.instance.audioManager.bgmSource.clip;

        if(cutScene.sceneBGM != null)
        {
            GameManager.instance.audioManager.SetBGM(cutScene.sceneBGM);
        }

        int index = 0;
        while (index < cutScene.events.Count)
        {

            yield return RenderCutSceneEvent(cutScene.events[index]);
            index++;
        }
        GameManager.instance.ExitCutSceneState();
        for (int i = 0; i < toDeactivate.Length; i++)
        {
            toDeactivate[i].SetActive(false);
        }

        GameManager.instance.audioManager.SetBGM(previousBGM);

        Camera.main.GetComponent<CameraMovement>().SetTarget(GameManager.instance.Player);
        Camera.main.GetComponent<CameraMovement>().SetFollowMode();

    }

    private IEnumerator RenderCutSceneEvent(CutSceneEvent sceneEvent)
    {
        float elapsedTime = 0f;

        CameraMovement camera = Camera.main.GetComponent<CameraMovement>();

        camera.SetTarget(null);
        camera.SetMovementVector(Vector2.zero);

        if(sceneEvent.cameraTarget != null)
        {
            camera.SetTarget(sceneEvent.cameraTarget);
        }

        if (sceneEvent.cameraMoveDirection != Vector2.zero)
        {
            camera.SetMovementVector(sceneEvent.cameraMoveDirection);
        }

        for (int i = 0; i < sceneEvent.actors.Length; i++)
        {
            sceneEvent.actors[i].GetComponent<Animator>().SetTrigger(sceneEvent.animatorTriggers[i]);
            sceneEvent.actors[i].MoveInDirection(sceneEvent.actorMoveDirections[i]);
        }

        for (int i = 0; i < sceneEvent.dialogs.Length; i++)
        {
            DialogManager.instance.ShowDialog(sceneEvent.dialogs[i]);
        }

        while (elapsedTime < sceneEvent.duration)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        camera.SetMovementVector(Vector2.zero);
        for (int i = 0; i < sceneEvent.actors.Length; i++)
        {
            sceneEvent.actors[i].StopMoving();
        }

        while (GameManager.instance.gameState == GameManager.GameState.DIALOG)
        {
            yield return null;
        }
        yield return null;
    }
}
