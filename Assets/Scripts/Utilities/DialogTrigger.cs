using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog[] dialogs;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ShowDialogs();
        }
    }

    private void ShowDialogs()
    {
        for (int i = 0; i < dialogs.Length; i++)
        {
            DialogManager.instance.ShowDialog(dialogs[i]);
        }
    }
}
