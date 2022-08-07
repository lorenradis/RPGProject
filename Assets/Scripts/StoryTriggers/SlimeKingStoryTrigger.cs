using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingStoryTrigger : StoryTrigger
{
    public UnitInfo slimeKing;
    public SpriteRenderer spriteRenderer;

    private int slimesToKill = 10;

    public Dialog[] dialogs;

    public override bool ConditionMet()
    {
        if (EnemyManager.instance.allEnemies[0].numberKilled >= slimesToKill)
        {
            return true;
        }
        return false;
    }

    public override void OnConditionMet()
    {
        isActive = false;
        spriteRenderer.enabled = true;
        StartCoroutine(SlimeKingEncounter());
        base.OnConditionMet();
    }

    private IEnumerator SlimeKingEncounter()
    {
        for (int i = 0; i < dialogs.Length; i++)
        {
            DialogManager.instance.ShowDialog(dialogs[i]);
            yield return null;
        }
        while(GameManager.instance.gameState == GameManager.GameState.DIALOG)
        {
            yield return null;
        }
        slimeKing.Setup();
        slimeKing.SetLevel(6);
        GameManager.instance.StartBattle(slimeKing);
    }
}
