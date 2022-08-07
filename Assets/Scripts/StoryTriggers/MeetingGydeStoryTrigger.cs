using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingGydeStoryTrigger : StoryTrigger
{

    [SerializeField]
    private Dialog[] dialogs;

    public GameObject[] toActivate;

    public CutsceneTrigger cutsceneTrigger;

    public MapRegion mapRegion;

    public Dialog gameIntroDialog;

    public override bool ConditionMet()
    {
        return !GameManager.instance.storyProgression.hasMetGyde;
    }

    public override void OnConditionMet()
    {
        isActive = false;
        GameManager.instance.storyProgression.hasMetGyde = true;

        for (int i = 0; i < toActivate.Length; i++)
        {
            toActivate[i].SetActive(true);
        }
        cutsceneTrigger.gameObject.SetActive(true);

        DialogManager.instance.ShowDialog(gameIntroDialog);

        mapRegion.OnDeactivate();
    }
}
