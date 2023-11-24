using UnityEngine;

namespace Boss.MadMantis
{
    public class EnragedIdle : StateMachineBehaviour
    {
        private const float enragedCooldownBoost = 0.2f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Manager>().StartJump();
            animator.GetComponent<Manager>().StartAttack(0f, enragedCooldownBoost);
        }
    }
}