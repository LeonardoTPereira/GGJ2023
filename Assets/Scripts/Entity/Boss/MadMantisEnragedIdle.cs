using Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadMantisEnragedIdle : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MadMantisManager>().StartJump();
        animator.GetComponent<MadMantisManager>().StartAttack();
    }
}