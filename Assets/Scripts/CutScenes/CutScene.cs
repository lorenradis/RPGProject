using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutScene
{
    public AudioClip sceneBGM;
    public List<CutSceneEvent> events = new List<CutSceneEvent>();

}

[System.Serializable]
public class CutSceneEvent
{
    public CutsceneActor[] actors;

    public Transform cameraTarget;

    public Vector2[] actorMoveDirections;
    public Vector2 cameraMoveDirection;

    public float duration;

    public string[] animatorTriggers;

    public Dialog[] dialogs;
}