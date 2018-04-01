using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAnimator : StateMachineBehaviour {

    private PlayerCamera playerCamera;
    private FindObject findObject;
    private TimeBar timeBar;
    private PlayerState playerState;
    private InteractiveState interactiveState;
    private NewInteractionSkill newInteractionSkill;
    private PhotonView photonView;

    void InitPhotonView(Animator animator)
    {
        photonView = animator.gameObject.GetComponent<PhotonView>();
    }

    void InitScripts(Animator animator)
    {
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

            findObject = animator.gameObject.GetComponent<FindObject>();

            timeBar = animator.gameObject.GetComponent<TimeBar>();

            playerState = animator.gameObject.GetComponent<PlayerState>();
    }

    void InitinteractiveState(Animator animator)
    {
            interactiveState = animator.GetComponent<NewInteractionSkill>().GetinteractiveState();
    }


	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        // 상호작용 상태 받기
        InitinteractiveState(animator);

        // 포톤뷰 스크립트 받기
        InitPhotonView(animator);

        // 본인이면
        if (photonView.isMine)
        {

            // 다른 각종 스크립트 받기
            InitScripts(animator);

            // 상태이상 변경
            playerState.SetPlayerCondition(PlayerState.ConditionEnum.INTERACTION);
        }

    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (photonView.isMine)
        {
            

            
            // FIndObject의 활성화 탐지 시작
            findObject.SetisUseFindObject(true);

            timeBar.DestroyObjects();

            playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);



            Debug.Log("해제해제해제 , + " + interactiveState.photonView.viewID);

            // 모든 클라이언트에게 RPC전송 하는 함수 콜
            //interactiveState.SetisCanFirstCheck(true);
            // ** 마스터에서 처리하기에는 애니메이션 동기화 문제가 있어서 안됨
            interactiveState.CallOnCanFirstCheck();
        }

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
