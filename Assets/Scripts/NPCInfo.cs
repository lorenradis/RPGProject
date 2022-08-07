using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class NPCInfo : ScriptableObject
{
    
    public string npcName;
    public Sprite portrait;

    public RuntimeAnimatorController animatorController;

    public Dialog[] greetings;
    public Dialog[] dialogs;
    public Dialog[] questions;
    public Dialog[] farewells;

}

