using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolBehaviour : StateMachineBehaviour
{
    private EnemyController _myEnemyController;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _myEnemyController = animator.gameObject.GetComponent<EnemyController>();
        _myEnemyController.SetSphereRadius(_myEnemyController.FollowDistance);
        _myEnemyController.SetDestinationToPatrol();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_myEnemyController.CheckPatrolPointReached())
        {
            _myEnemyController.UpdatePatrolPoint();
            _myEnemyController.SetDestinationToPatrol();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
