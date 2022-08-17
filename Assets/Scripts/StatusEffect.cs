using UnityEngine;

public class StatusEffect : ScriptableObject {

    /*
    effects we want to be able to render
    poison - damage
    bad poison - more damage
    burn - damage
    freeze - damage, can't act
    stone - can't act
    sleep - can't act
    confuse - act at random
    blind - very low accuracy
    silenced - can't use spells
    stunned - can't act
    charmed? - won't attack enemy
    */

    public string statusName;               //poison                        //petrified                     
    public string statusEnactDescription; //was poisoned                    //turned to stone!          
    public string statusOcurDescription; //takes damage from the poison  //""
    public string statusResolvedDescription; //is no longer poisoned        //recovered from petrification

    public float damageFactor = 0f; //poison is .0625, damagefactor is multiplied by max hp for damage amount
    public float accuracyFactor = 1f; //blind is .125f, sand is .5f, gets multiplied by accuracy
    
    public int chanceToRecover; //every turn this will get rolled against, some statuses should get more likely to recover from as they go on.

    public bool canAct;
    public bool canCastSpells;
    public bool doesDrain;
    public bool actAtRandom;
    public bool endsOnHit;
    public bool endsOnSourceDeath;
    public bool endsAfterBattle; 

    public UnitInfo source; //unit that caused the effect (can't be hit if charmed, receives health if 'drain', etc)

}