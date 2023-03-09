using UnityEngine;

namespace Boss.MadMantis
{
    public class EnragedIdle : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Manager>().StartJump();
            animator.GetComponent<Manager>().StartAttack();
        }
    }
}