using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{

    protected bool isActive = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Something triggered me");
        if (!isActive)
            return;
        if(collision.CompareTag("Player") && ConditionMet())
        {
            Debug.Log("Something met the condition");
            OnConditionMet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive)
            return;

        if(collision.CompareTag("Player") && ConditionMet())
        {
            OnConditionMet();
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public virtual bool ConditionMet()
    {
        return false;
    }

    public virtual void OnConditionMet()
    {
        isActive = false;
    }

}
