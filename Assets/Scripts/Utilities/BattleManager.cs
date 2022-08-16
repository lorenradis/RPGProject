using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;

    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private Button magicButton;
    [SerializeField]
    private Button itemButton;
    [SerializeField]
    private Button runButton;

    private Button previousButton;

    private List<string> dialogs = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("There's two battle managers");
            Destroy(gameObject);
        }
    }

    public enum BattleState { SETUP, IDLE, PLAYERTURN, ENEMYTURN, RESOLVE, LOSE, WIN, DIALOG }
    public BattleState battleState;
    private BattleState previousState;
    private float timeInState = 0f;
    private void ChangeState(BattleState newState)
    {
        if (newState != battleState)
        {
            if(battleState != BattleState.DIALOG)
                previousState = battleState;
            battleState = newState;
            timeInState = 0f;
            Debug.Log("Changed state to " + battleState);
        }
    }

    private List<CombatTurn> combatTurns = new List<CombatTurn>();
    private List<CombatTurn> pastTurns = new List<CombatTurn>();

    public UnitInfo playerUnit;
    public UnitInfo enemyUnit;

    //Player UI Region
    [SerializeField]
    private GameObject playerCommandsPanel;
    [SerializeField]
    private GameObject playerSpellsPanel;
    [SerializeField]
    private SpellSlot[] spellSlots;
    [SerializeField]
    private GameObject playerItemsPanel;
    [SerializeField]
    private ItemSlot[] itemSlots;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI mpText;
    [SerializeField]
    private TextMeshProUGUI lvlText;
    [SerializeField]
    private TextMeshProUGUI gpText;
    [SerializeField]
    private TextMeshProUGUI expText;

    [SerializeField]
    private TextMeshProUGUI dialogText;

    [SerializeField]
    private Image damageFlash;

    [SerializeField]
    private GameObject enemyBattleObject;
    [SerializeField]
    private GameObject continueArrow;

    public int actThreshold = 1000;

    private int playerTimer = 0;
    private int enemyTimer = 0;

    private bool enemyActed = false;

    private void Start()
    {
        SetUnit(GameManager.instance.playerInfo, true);
        SetUnit(GameManager.instance.encounteredEnemy, false);

        ChangeState(BattleState.IDLE);

        DisplayDialog("A " + enemyUnit.unitName + " approaches!");
        continueArrow.SetActive(false);
    }

    public void SetUnit(UnitInfo unit, bool isPlayer)
    {
        if (isPlayer)
        {
            playerUnit = unit;
            hpText.text = "" + unit.HP.TotalCurrent;
            mpText.text = "" + unit.MP.TotalCurrent;
            lvlText.text = "" + unit.level;
            expText.text = "" + unit.experience;
            gpText.text = "" + unit.gold;
        }
        else
        {
            enemyUnit = unit;
            enemyBattleObject.GetComponent<Animator>().runtimeAnimatorController = unit.battleAnimator;
        }
    }

    private void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.BATTLE)
        {
            ManageState();
        }
    }

    private void ManageState()
    {
        if(dialogs.Count > 0 && battleState != BattleState.DIALOG)
        {
            StartCoroutine(RenderDialog(battleState));
        }
        switch (battleState)
        {
            case BattleState.SETUP:

                break;
            case BattleState.IDLE:
                if (playerTimer >= actThreshold)
                {
                    playerTimer -= actThreshold;
                    StartPlayerTurn();
                }
                else
                {
                    playerTimer += playerUnit.Speed.Value;
                    if (enemyTimer >= actThreshold)
                    {
                        enemyTimer -= actThreshold;
                        StartEnemyTurn();
                    }
                    else
                    {
                        enemyTimer += enemyUnit.Speed.Value;
                    }
                }
                break;
            case BattleState.PLAYERTURN:
                //listen for player to back out of a menu
                if(Input.GetKeyDown("joystick button 1"))
                {
                    CloseCurrentMenu();
                }
                //listen for a combat turn to be generated

                if (combatTurns.Count > 0)
                {
                    ExecuteCombatTurn();
                }
                break;
            case BattleState.ENEMYTURN:
                if (!enemyActed)
                {
                    CombatTurn combatTurn = new CombatTurn(enemyUnit, playerUnit, enemyUnit.weapon.combatAction);
                    combatTurns.Add(combatTurn);
                    if (combatTurns.Count > 0)
                    {
                        enemyActed = true;
                        ExecuteCombatTurn();
                    }
                }
                break;
            case BattleState.RESOLVE:

                break;
            case BattleState.LOSE:
                GameManager.instance.ReturnFromBattle();
                break;
            case BattleState.WIN:
                GameManager.instance.ReturnFromBattle();
                break;
            case BattleState.DIALOG:

                break;
            default:
                break;
        }
    }

    public void CloseCurrentMenu()
    {
        if(playerSpellsPanel.gameObject.activeSelf)
        {
            HidePlayerSpellsPanel();
            ShowPlayerCommandsPanel();
        }else if(playerItemsPanel.gameObject.activeSelf)
        {
            HidePlayerItemsPanel();
            ShowPlayerCommandsPanel();
        }
    }

    private void StartPlayerTurn()
    {
        //resolve status effects?
        ChangeState(BattleState.PLAYERTURN);
        DisplayDialog("What will you do?");
        ShowPlayerCommandsPanel();
    }

    private void StartEnemyTurn()
    {
        ChangeState(BattleState.ENEMYTURN);
    }

    private void ExecuteCombatTurn()
    {
        CombatTurn combatTurn = combatTurns[0];
        combatTurns.RemoveAt(0);
        StartCoroutine(RenderCombatTurn(combatTurn));

    }

    private IEnumerator RenderCombatTurn(CombatTurn combatTurn)
    {
        combatTurn.CalculateTurn();

        combatTurn.actor.UseMP(combatTurn.combatAction.mpCost);

        mpText.text = ""+playerUnit.MP.TotalCurrent;

        DisplayDialog(combatTurn.actor.unitName + " " + combatTurn.combatAction.description + "!");
        yield return null;
        while (battleState == BattleState.DIALOG)
        {
            yield return null;
        }

        if (combatTurn.didHit)
        {
            if(battleState == BattleState.ENEMYTURN)
            {
                enemyBattleObject.GetComponent<Animator>().SetTrigger("attack");
                yield return null;
                while(enemyBattleObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("enemyBattleAttack"))
                {
                    yield return null;
                }
            }

            if (combatTurn.damageAmount > 0)
            {
                if(combatTurn.didCrit)
                {
                    DisplayDialog("It's a critical hit!");
                    yield return null;
                    while(battleState == BattleState.DIALOG)
                    {
                        yield return null;
                    }
                }
                combatTurn.target.TakeDamage(combatTurn.damageAmount);

                DisplayDialog("The " + combatTurn.target.unitName + " took " + combatTurn.damageAmount + " damage!");
                if (combatTurn.target == enemyUnit)
                {
                    StartCoroutine(EnemyFlicker(.35f));
                }
                else
                {
                    StartCoroutine(PlayerFlicker(.35f));
                }
                yield return null;
                while (battleState == BattleState.DIALOG)
                {
                    yield return null;
                }
                if (combatTurn.target == playerUnit)
                {
                    hpText.text = "" + playerUnit.HP.TotalCurrent;
                }
            }

            if (combatTurn.statusHit)
            {

            }

            if (combatTurn.healthRecoveryAmount > 0)
            {
                DisplayDialog(combatTurn.actor.unitName + " recovered " + combatTurn.healthRecoveryAmount + " health!");
                yield return null;
                while(battleState == BattleState.DIALOG)
                {
                    yield return null;
                }
                combatTurn.actor.RecoverHealth(combatTurn.healthRecoveryAmount);
                if(combatTurn.actor == playerUnit)
                {
                    hpText.text = "" + playerUnit.HP.TotalCurrent;
                }
            }

            if (combatTurn.magicRecoveryAmount > 0)
            {
                DisplayDialog(combatTurn.actor.unitName + " recovered " + combatTurn.magicRecoveryAmount + " magic!");
                yield return null;
                while (battleState == BattleState.DIALOG)
                {
                    yield return null;
                }
                combatTurn.actor.RecoverMP(combatTurn.magicRecoveryAmount);
                if (combatTurn.actor == playerUnit)
                {
                    mpText.text = "" + playerUnit.MP.TotalCurrent;
                }
            }

            if (combatTurn.combatAction.statusToRecover != CombatAction.StatusEffect.NONE)
            {
                //if we have the status effect this cures, resolve it.
            }

        }
        else
        {
            //show flavor text showing the player missed
            DisplayDialog("It missed!");
            yield return null;
            while (battleState == BattleState.DIALOG)
            {
                yield return null;
            }
        }

        //
        pastTurns.Add(combatTurn);
        ResolveTurn();
    }

    private IEnumerator RenderDialog(BattleState previousState)
    {
        while (dialogs.Count > 0)
        {
            string message = dialogs[0];
            dialogs.RemoveAt(0);
            ChangeState(BattleState.DIALOG);
            dialogText.text = message;
            bool anyKey = false;
            float waitTime = .02f;
            float speedUp = .5f;
            for (int i = 0; i < message.Length + 1; i++)
            {
                dialogText.maxVisibleCharacters = i;
                speedUp = Input.anyKey ? .2f : .5f;
                yield return new WaitForSeconds(waitTime * speedUp);
            }

            continueArrow.SetActive(true);

            while (!anyKey)
            {
                anyKey = Input.anyKeyDown;
                yield return null;
            }

            continueArrow.SetActive(false);
        }
        ChangeState(previousState);

    }

    public void AttemptRun()
    {
        if(CanRun())
        {
            FleeBattle();
        }
        else
        {
            StartCoroutine(RenderFleeBattle(false));
        }

    }

    private bool CanRun()
    {
        int roll = Random.Range(1, 100);

        float playerSpeed = playerUnit.Speed.Value;
        float enemySpeed = enemyUnit.Speed.Value;
        float totalSpeed = playerSpeed + enemySpeed;

        float runChance = 0;

        if(battleState == BattleState.PLAYERTURN)
        {
            runChance = 100f / totalSpeed * playerSpeed;
        }
        else
        {
            runChance = 100f / totalSpeed * enemySpeed;
        }

        return roll < runChance;
    }

    public void FleeBattle()
    {
        StartCoroutine(RenderFleeBattle(true));
    }

    private IEnumerator RenderFleeBattle(bool succeeded)
    {
        if (succeeded)
        {
            switch (battleState)
            {
                case BattleState.ENEMYTURN:
                    DisplayDialog("The enemy fled in terror!");
                    yield return null;
                    while (battleState == BattleState.DIALOG)
                    {
                        yield return null;
                    }
                    break;
                case BattleState.PLAYERTURN:
                    DisplayDialog("You escaped from the " + enemyUnit.unitName + "!");
                    yield return null;
                    while (battleState == BattleState.DIALOG)
                    {
                        yield return null;
                    }
                    break;
            }
            GameManager.instance.ReturnFromBattle();
        }
        else
        {
            switch (battleState)
            {
                case BattleState.ENEMYTURN:
                    DisplayDialog("The enemy tried to run from you, but you were too fast!");
                    yield return null;
                    while (battleState == BattleState.DIALOG)
                    {
                        yield return null;
                    }
                    break;
                case BattleState.PLAYERTURN:
                    DisplayDialog("You tried to escape, but the " + enemyUnit.unitName + " caught you!");
                    yield return null;
                    while (battleState == BattleState.DIALOG)
                    {
                        yield return null;
                    }
                    break;
            }
        }
    }

    public void ShowPlayerCommandsPanel()
    {
        playerCommandsPanel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
    }

    public void ShowPlayerSpellsPanel()
    {
        playerSpellsPanel.gameObject.SetActive(true);
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if(i < playerUnit.availableSpells.Count)
            {
                spellSlots[i].gameObject.SetActive(true);
                spellSlots[i].SetSpell(playerUnit.availableSpells[i]);
            }
            else
            {
                spellSlots[i].gameObject.SetActive(false);
            }
            
        }
        if(playerUnit.availableSpells.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(spellSlots[0].gameObject);
        }
        else
        {
            //set focus on close menu button
        }
    }

    public void ShowPlayerItemsPanel()
    {
        Inventory inventory = GameManager.instance.inventory;
        playerItemsPanel.gameObject.SetActive(true);
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                itemSlots[i].gameObject.SetActive(true);
                itemSlots[i].SetItem(inventory.items[i]);
            }
            else
            {
                itemSlots[i].gameObject.SetActive(false);
            }
        }
        if(inventory.items.Count > 0)
            EventSystem.current.SetSelectedGameObject(itemSlots[0].gameObject);
    }

    public void HidePlayerCommandsPanel()
    {
        playerCommandsPanel.gameObject.SetActive(false);
    }

    public void HidePlayerSpellsPanel()
    {
        playerSpellsPanel.gameObject.SetActive(false);
    }

    public void HidePlayerItemsPanel()
    {
        playerItemsPanel.gameObject.SetActive(false);
    }

    public void BasicAttack()
    {
        CombatAction combatAction = playerUnit.weapon.combatAction;
        PerformAction(combatAction);
    }

    public void SpecialAttack(CombatAction combatAction)
    {
        if(playerUnit.MP.TotalCurrent >= combatAction.mpCost)
        {
            PerformAction(combatAction);
        }
        else
        {
            Debug.Log("Not enough MP");
        }
    }

    public void UseItem(Item item)
    {
        if(item.isConsumable)
        {
            item.quantity--;
            if(item.quantity < 1)
            {
                GameManager.instance.inventory.RemoveItemFromList(item);
            }
        }
        PerformAction(item.combatAction);
    }

    public void PerformAction(CombatAction combatAction)
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            CombatTurn combatTurn = new CombatTurn(playerUnit, enemyUnit, combatAction);
            combatTurns.Add(combatTurn);
            HidePlayerCommandsPanel();
            HidePlayerItemsPanel();
            HidePlayerSpellsPanel();
        }
        else if (battleState == BattleState.ENEMYTURN)
        {
            CombatTurn combatTurn = new CombatTurn(enemyUnit, playerUnit, combatAction);
            combatTurns.Add(combatTurn);
        }
        else
            return;
    }

    public void DisplayDialog(string message)
    {
        dialogs.Add(message);
    }

    private IEnumerator EnemyFlicker(float duration)
    {
        int counter = 0;
        int frequency = 3;
        float elapsedTime = 0f;
        SpriteRenderer renderer = enemyBattleObject.GetComponent<SpriteRenderer>();
        while(elapsedTime < duration)
        {
            counter++;
            if(counter % frequency == 0)
            {
                renderer.enabled = !renderer.enabled;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if(EnemyIsDead())
        {
            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                renderer.enabled = !renderer.enabled;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            renderer.enabled = false;
        }
        else
        {
            renderer.enabled = true;
        }
    }

    private IEnumerator PlayerFlicker(float duration)
    {
        int counter = 0;
        int frequency = 3;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            counter++;
            if (counter % frequency == 0)
            {
                damageFlash.enabled = !damageFlash.enabled;
            }
            yield return null;
        }
        damageFlash.enabled = false;
    }

    private void ResolveTurn()
    {
        ChangeState(BattleState.RESOLVE);
        StartCoroutine(RenderTurnResolution());
    }

    private bool PlayerIsDead()
    {
        return playerUnit.HP.IsDepleted();
    }

    private bool EnemyIsDead()
    {
        return enemyUnit.HP.IsDepleted();
    }

    private IEnumerator RenderTurnResolution()
    {
        enemyActed = false;

        //if player is dead, lose
        //if enemy is dead, win
        //change state to idle
        if (playerUnit.HP.IsDepleted())
        {
            LoseBattle();
            yield break;
        }
        else if (enemyUnit.HP.IsDepleted())
        {
            EnemyManager.instance.UpdateKillCounts(enemyUnit);
            DisplayDialog("The " + enemyUnit.unitName + " perished!");
            yield return null;
            while (battleState == BattleState.DIALOG)
            {
                yield return null;
            }

            WinBattle();
            yield break;
        }
        else
        {
            ChangeState(BattleState.IDLE);
        }
    }

    private void LoseBattle()
    {
        ChangeState(BattleState.LOSE);
        DisplayDialog("You lost the battle!");
    }

    private void WinBattle()
    {
        StartCoroutine(RenderBattleWin());
    }

    private IEnumerator RenderBattleWin()
    {
        bool levelUp = false;
        ChangeState(BattleState.WIN);
        if(playerUnit.experience + enemyUnit.experience >= playerUnit.nextLevel)
        {
            levelUp = true;
        }
        playerUnit.GainExperience(enemyUnit.experience);

        if(levelUp)
        {
            yield return RenderLevelUp();
        }

        playerUnit.GainGold(enemyUnit.gold);
        DisplayDialog("You won the battle!");
        yield return null;
        while (battleState == BattleState.DIALOG)
        {
            yield return null;
        }
        yield return null;
        DisplayDialog("You gained " + enemyUnit.experience + " experience, and " + enemyUnit.gold + " gold!");
        yield return null;
        while (battleState == BattleState.DIALOG)
        {
            yield return null;
        }
        yield return null;

    }

    private IEnumerator RenderLevelUp()
    {
        DisplayDialog("You reached level " + playerUnit.level + "!");
        DisplayDialog("Your stats have increased!");
        DisplayDialog("Your health points now total " + playerUnit.HP.TotalMax + "!");
        DisplayDialog("Your mana points now total " + playerUnit.MP.TotalMax + "!");
        if(playerUnit.LearnedAbility(playerUnit.level))
        {
            DisplayDialog("You learned the awesome power of " + playerUnit.GetAbilityAtLevel(playerUnit.level).actionName + "!");
        }
        yield return null;
        while (battleState == BattleState.DIALOG)
        {
            yield return null;
        }
    }
}