using UnityEngine;
using TMPro;

public class SpellSlot : MonoBehaviour
{
    [SerializeField]
    private CombatAction combatAction;
    [SerializeField]
    private TextMeshProUGUI spellText;


    public void SetSpell(CombatAction newAction)
    {
        combatAction = newAction;
        spellText.text = newAction.actionName + " " + newAction.mpCost;
    }

    public void CastThisSpell()
    {
        BattleManager.instance.SpecialAttack(combatAction);
    }
}
