using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOverlayTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerControls>().ActivateGrassOverlay();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerControls>().DeactivateGrassOverlay();
        }
    }
}
