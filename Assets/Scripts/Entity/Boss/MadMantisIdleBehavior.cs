using UnityEngine;

namespace Boss
{
    public class MadMantisIdleBehavior : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<MadMantisManager>().StartAttack();
        }
    }
}