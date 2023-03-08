using UnityEngine;

namespace Boss
{
    public class MadMantisFlyingdIdle : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<MadMantisManager>().Fly();
        }
    }
}