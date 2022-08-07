using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public SceneInfo sceneToLoad;
    public int entryPoint = 0;
    public Vector2 newFacing = Vector2.down;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.instance.LoadNewScene(sceneToLoad, entryPoint, newFacing);
            gameObject.SetActive(false);    
        }
    }
}
