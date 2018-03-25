using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAnimator : StateMachineBehaviour {

    private PlayerCamera playerCamera;

    private FindObject findObject;

    private TimeBar timeBar;

    private PlayerState playerState;

    void InitScripts(Animator animator)
    {
        if (playerCamera == null)
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

        if (findObject == null)
            findObject = animator.gameObject.GetComponent<FindObject>();

        if (timeBar == null)
            timeBar = animator.gameObject.GetComponent<TimeBar>();

        if (playerState == null)
            playerState = animator.gameObject.GetComponent<PlayerState>();
        
    }


	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        InitScripts(animator);

        playerState.SetPlayerCondition(PlayerState.ConditionEnum.INTERACTION);

    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        // FIndObject의 활성화 탐지 시작
        findObject.SetisUseFindObject(true);

        timeBar.DestroyObjects();

        playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);

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
