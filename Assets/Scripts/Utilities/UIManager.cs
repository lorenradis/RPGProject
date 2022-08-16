using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class UIManager
{
    public static UIManager instance;

    [SerializeField]
    private AudioClip closeMenuSound;

    [SerializeField]
    private RectTransform mainMenuObject;
    [SerializeField]
    private Button[] mainMenuButtons;
    [SerializeField]
    private RectTransform pauseMenuObject;
    [SerializeField]
    private RectTransform inGameMenuObject;
    [SerializeField]
    private ItemSlot[] itemSlots; //set in inspector
    [SerializeField]
    private RectTransform inventoryMenuObject;
    [SerializeField]
    private SpellSlot[] spellSlots; //set in inspector
    [SerializeField]
    private RectTransform spellsMenuObject;
    [SerializeField]
    private RectTransform equipMenuObject;
    [SerializeField]
    private RectTransform statsMenuObject;
    [SerializeField]
    private RectTransform killsMenuObject;
    [SerializeField]
    private KillsSlot[] killsSlots;
    [SerializeField]
    private Button[] pauseMenuButtons;
    [SerializeField]
    private Button[] inGameMenuButtons;


    //player stats window
    [SerializeField]
    private TextMeshProUGUI statsHPText;
    [SerializeField]
    private TextMeshProUGUI statsMPText;
    [SerializeField]
    private TextMeshProUGUI statsSTRText;
    [SerializeField]
    private TextMeshProUGUI statsDEFText;
    [SerializeField]
    private TextMeshProUGUI statsINTText;
    [SerializeField]
    private TextMeshProUGUI statsWISText;
    [SerializeField]
    private TextMeshProUGUI statsCONText;
    [SerializeField]
    private TextMeshProUGUI statsSPDText;
    [SerializeField]
    private TextMeshProUGUI statsLUCKText;
    [SerializeField]
    private TextMeshProUGUI statsSTATUSText;
    [SerializeField]
    private TextMeshProUGUI statsNAMEText;
    [SerializeField]
    private TextMeshProUGUI statsLVLText;
    [SerializeField]
    private TextMeshProUGUI statsEXPText;
    [SerializeField]
    private TextMeshProUGUI statsToNextText;
    [SerializeField]
    private TextMeshProUGUI statsGOLDText;

    [SerializeField]
    private RectTransform enemyInfoMenuObject;
    [SerializeField]
    private Image enemyPortraitImage;
    [SerializeField]
    private TextMeshProUGUI enemyNameText;
    [SerializeField]
    private TextMeshProUGUI enemyEXPText;
    [SerializeField]
    private TextMeshProUGUI enemyBountyText;
    [SerializeField]
    private TextMeshProUGUI enemySTRText;
    [SerializeField]
    private TextMeshProUGUI enemyDEFText;
    [SerializeField]
    private TextMeshProUGUI enemyINTText;
    [SerializeField]
    private TextMeshProUGUI enemyWISText;
    [SerializeField]
    private TextMeshProUGUI enemyCONText;
    [SerializeField]
    private TextMeshProUGUI enemySPDText;
    [SerializeField]
    private TextMeshProUGUI enemyDescriptionText;


    [SerializeField]
    private RectTransform shopKeeperPanel;
    [SerializeField]
    private ItemSlot[] shopItemSlots;
    private ShopkeeperInteraction shopKeeper;

    public void ShowInGameMenu()
    {
        inGameMenuObject.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(inGameMenuButtons[0].gameObject);
    }

    public void HideInGameMenu()
    {
        inGameMenuObject.gameObject.SetActive(false);
    }

    public void ShowMainMenu()
    {
        mainMenuObject.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenuButtons[0].gameObject);
    }

    public void HideMainMenu()
    {
        mainMenuObject.gameObject.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenuObject.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseMenuButtons[0].gameObject);
    }

    public void HidePauseMenu()
    {
        pauseMenuObject.gameObject.SetActive(false);
    }

    public void ShowInventory()
    {
        HideInGameMenu();
        inventoryMenuObject.gameObject.SetActive(true);
        Inventory inventory = GameManager.instance.inventory;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                itemSlots[i].SetItem(inventory.items[i]);
                itemSlots[i].gameObject.SetActive(true);
            }
            else
            {
                itemSlots[i].gameObject.SetActive(false);
            }
        }
        if (inventory.items.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(itemSlots[0].gameObject);
        }
    }

    public void ShowSpellsMenu()
    {
        HideInGameMenu();
        spellsMenuObject.gameObject.SetActive(true);
        List<CombatAction> spells = GameManager.instance.playerInfo.availableSpells;
        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (i < spells.Count)
            {
                spellSlots[i].SetSpell(spells[i]);
                spellSlots[i].gameObject.SetActive(true);
            }
            else
            {
                spellSlots[i].gameObject.SetActive(false);
            }
        }
        if(spellSlots[0].gameObject.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(spellSlots[0].gameObject);
        }
    }

    public void ShowEquipMenu()
    {
        HideInGameMenu();
        equipMenuObject.gameObject.SetActive(true);
    }

    public void ShowStatsMenu(UnitInfo unitInfo)
    {
        HideInGameMenu();
        statsMenuObject.gameObject.SetActive(true);
        statsHPText.text = "HP: " + unitInfo.HP.TotalCurrent + "/" + unitInfo.HP.TotalMax;
        statsMPText.text = "MP: " + unitInfo.MP.TotalCurrent + "/" + unitInfo.MP.TotalMax;
        statsSTRText.text = "STR: " + unitInfo.Strength.Value;
        statsDEFText.text = "DEF: " + unitInfo.Defense.Value;
        statsINTText.text = "INT: " + unitInfo.Intelligence.Value;
        statsWISText.text = "WIS: " + unitInfo.Wisdom.Value;
        statsCONText.text = "CON: " + unitInfo.Constitution.Value;
        statsSPDText.text = "SPD: " + unitInfo.Speed.Value;
        statsLUCKText.text = "LUCK: " + unitInfo.Heart.Value;
        statsNAMEText.text = unitInfo.unitName;
        statsLVLText.text = "LVL: " + unitInfo.level;
        statsEXPText.text = "EXP: " + unitInfo.experience;
        statsToNextText.text = "To Next: " + ((unitInfo.level * unitInfo.level * unitInfo.level) - unitInfo.experience);
        statsGOLDText.text = "GOLD: " + unitInfo.gold;
    }

    public void ShowEnemyInfoMenu(UnitInfo unitInfo)
    {
        enemyInfoMenuObject.gameObject.SetActive(true);
        enemyNameText.text = unitInfo.unitName;
        enemyEXPText.text = "EXP: " + unitInfo.experience;
        enemyBountyText.text = "BOUNTY: " + unitInfo.gold;
        enemySTRText.text = "STR: " + unitInfo.Strength.Value;
        enemyDEFText.text = "DEF: " + unitInfo.Defense.Value;
        enemyINTText.text = "INT: " + unitInfo.Intelligence.Value;
        enemyWISText.text = "WIS: " + unitInfo.Wisdom.Value;
        enemyCONText.text = "CON: " + unitInfo.Constitution.Value;
        enemySPDText.text = "SPD: " + unitInfo.Speed.Value;
        enemyDescriptionText.text = unitInfo.description;
        enemyPortraitImage.sprite = unitInfo.portrait;
    }

    public void HideEnemyInfoMenu()
    {
        enemyInfoMenuObject.gameObject.SetActive(false);
    }

    public void ShowKillsMenu()
    {
        HideInGameMenu();
        killsMenuObject.gameObject.SetActive(true);
        for (int i = 0; i < EnemyManager.instance.allEnemies.Count; i++)
        {
            if(i == 0)
            {
                EventSystem.current.SetSelectedGameObject(killsSlots[0].gameObject);
            }

            if (i >= killsSlots.Length)
                break;

            if(EnemyManager.instance.allEnemies[i].numberKilled > 0)
            {
                killsSlots[i].SetUnitInfo(EnemyManager.instance.allEnemies[i]);
            }
        }
        
    }

    public void ShowShopInventory(ShopkeeperInteraction _shopKeeper)
    {
        shopKeeperPanel.gameObject.SetActive(true);
        shopKeeper = _shopKeeper;
        for (int i = 0; i < shopItemSlots.Length; i++)
        {
            if(i < shopKeeper.items.Count)
            {
                shopItemSlots[i].SetItem(shopKeeper.items[i]);
                shopItemSlots[i].SetShopKeeper(shopKeeper);
            }
            else
            {
                shopItemSlots[i].gameObject.SetActive(false);
            }
        }
        EventSystem.current.SetSelectedGameObject(shopItemSlots[0].gameObject);
    }

    public void HideShopInventory()
    {
        shopKeeperPanel.gameObject.SetActive(false);
    }

    public GameManager.GameState CloseCurrentMenu()
    {
        GameManager.instance.audioManager.PlaySoundEffect(closeMenuSound);

        if (inventoryMenuObject.gameObject.activeSelf)
        {
            HideInventory();
            ShowInGameMenu();
            return GameManager.GameState.MENU;
        }
        else if (spellsMenuObject.gameObject.activeSelf)
        {
            HideSpellsMenu();
            ShowInGameMenu();
            return GameManager.GameState.MENU;
        }
        else if (equipMenuObject.gameObject.activeSelf)
        {
            HideEquipMenu();
            ShowInGameMenu();
            return GameManager.GameState.MENU;
        }
        else if (statsMenuObject.gameObject.activeSelf)
        {
            HideStatsMenu();
            ShowInGameMenu();
            return GameManager.GameState.MENU;

        }
        else if (enemyInfoMenuObject.gameObject.activeSelf)
        {
            HideEnemyInfoMenu();
            return GameManager.GameState.MENU;
        }
        else if (killsMenuObject.gameObject.activeSelf)
        {
            HideKillsMenu();
            ShowInGameMenu();
            return GameManager.GameState.MENU;
        }
        else if (inGameMenuObject.gameObject.activeSelf)
        {
            HideInGameMenu();
            return GameManager.GameState.NORMAL;

        }
        else if (pauseMenuObject.gameObject.activeSelf)
        {
            HidePauseMenu();
            return GameManager.GameState.NORMAL;
        }
        else
        {
            return GameManager.instance.gameState;
        }
    }

    public void HideInventory()
    {
        inventoryMenuObject.gameObject.SetActive(false);
    }

    public void HideKillsMenu()
    {
        killsMenuObject.gameObject.SetActive(false);
    }

    public void HideStatsMenu()
    {
        statsMenuObject.gameObject.SetActive(false);
    }

    public void HideSpellsMenu()
    {
        spellsMenuObject.gameObject.SetActive(false);
    }

    private void HideEquipMenu()
    {
        equipMenuObject.gameObject.SetActive(false);
    }
}