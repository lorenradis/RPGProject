/*

map regions will also need to be named scriptableobjects like scenes would have been, they can all have unique bgms and light values
if they're named scriptable objects then we CAN save them in the game manager between scene loads.

what else is still "to do?" other than just content creation?  What systems are needed?

actually create the inventory UI and scripts

i don't think items can be used yet, in or out of battle.

consider adding multiple players and multiple enemies to the battle system.

*/


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class SaveManager
{

    //create a SaveManager saveManager in the GameManager instance.

    public SaveData activeSave;

    public bool hasLoaded = false;

    public void SaveGameData()
    {
        
        string dataPath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".sav", FileMode.Create);

        activeSave.playerLevel = GameManager.instance.playerInfo.level;
        activeSave.playerPosition = GameManager.instance.Player.position;
        activeSave.currentScene = GameManager.instance.currentScene.sceneName;

        serializer.Serialize(stream, activeSave);
        stream.Close();
        
    }

    public void LoadGameData()
    {
        
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".sav"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/" + activeSave.saveName + ".sav", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;

            GameManager.instance.savedPlayerPosition= activeSave.playerPosition;
            GameManager.instance.playerInfo.SetLevel(activeSave.playerLevel);
            SceneManager.LoadScene(activeSave.currentScene);

            stream.Close();
            hasLoaded = true;
        }
        else
        {

        }
        
    }

    public void DeleteSaveData()
    {
        
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".sav"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + ".sav");
        }
        else
        {

        }
        
    }

}

[System.Serializable]
public class SaveData
{

    public string saveName;

    public Vector2 playerPosition;

    public string currentScene;

    //can i just serialize the entire player unit UnitInfo instead of doing each component separately?
    //would that bring with it things like spells, inventory, equipment?

    public int playerLevel;
    public int playerMoney;
    public int playerExperience;
    public int playerHealth;
    public int playerMagic;

    public int slimesKilled;

    public List<Item> inventory = new List<Item>();
    public List<CombatAction> spells = new List<CombatAction>();

}
