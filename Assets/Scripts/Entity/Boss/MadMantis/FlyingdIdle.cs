using UnityEngine;

namespace Boss.MadMantis
{
    public class FlyingdIdle : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Manager>().Fly();
        }
    }
}