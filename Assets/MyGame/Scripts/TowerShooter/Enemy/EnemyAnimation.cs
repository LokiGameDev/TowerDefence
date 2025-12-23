
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator animator;

    public void EnemyAnimationTrigger(string triggerName)
    {
        if(animator!=null) animator.SetTrigger(triggerName);
        else Debug.Log("Shit");
    }
}
