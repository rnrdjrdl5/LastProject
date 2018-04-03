using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimator : StateMachineBehaviour {

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("isAttack", false);

        animator.gameObject.GetComponent<PlayerState>().SetPlayerCondition(PlayerState.ConditionEnum.ATTACK);
        
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {


        NewCatAttack NCA = animator.GetComponent<NewCatAttack>();

        PlayerState ps = animator.gameObject.GetComponent<PlayerState>();

        // 해당 스테이트에서 나갈 때 공격 조건이 꺼져있으면 킵니다.
        if (NCA.GetisCanAttack() == false && ps.GetPlayerCondition() != PlayerState.ConditionEnum.ATTACK)
        {
            Debug.Log("sdf");
            NCA.SetisCanAttack(true);
        }

        // 해당 스테이트에서 나갈 때 프라이팬에 정보가 남아있다면 삭제합니다.
        NCA.DeleteFryPanOption();

       
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
