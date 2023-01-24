using UnityEngine;

namespace UnityTemplateProjects.AI.Enemy
{
    public class EnemyAttackBehaviour : StateMachineBehaviour
    {
        private EnemyController _myEnemyController;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _myEnemyController = animator.gameObject.GetComponent<EnemyController>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _myEnemyController.LeaveAttackState();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            if(_myEnemyController.CanAttack) _myEnemyController.Attack();
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
        }
    }
}