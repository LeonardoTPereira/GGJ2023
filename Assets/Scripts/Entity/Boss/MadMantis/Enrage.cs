using UnityEngine;

namespace Boss.MadMantis
{
    public class Enrage : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<ShaderManager>().Enrage();
        }
    }
}