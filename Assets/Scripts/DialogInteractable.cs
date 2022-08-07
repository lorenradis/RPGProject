using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : Interactable
{
    public List<Dialog> greetings = new List<Dialog>();
    public List<Dialog> dialogs = new List<Dialog>();
    public List<Dialog> farewells = new List<Dialog>();

    public override void OnInteract(Transform player)
    {
        SpeakToPlayer();
        base.OnInteract(player);
    }

    private void SpeakToPlayer()
    {
        if (greetings.Count > 0)
        {
            Dialog greeting = greetings[Random.Range(0, greetings.Count)];
            DialogManager.instance.ShowDialog(greeting);
        }
        if (dialogs.Count > 0)
        {
            Dialog dialog = dialogs[Random.Range(0, dialogs.Count)];
            DialogManager.instance.ShowDialog(dialog);
        }
        if (farewells.Count > 0)
        {
            Dialog farewell = farewells[Random.Range(0, farewells.Count)];
            DialogManager.instance.ShowDialog(farewell);
        }
    }

    public void ClearDialogs()
    {
        greetings.Clear();
        dialogs.Clear();
        farewells.Clear();
    }
}
