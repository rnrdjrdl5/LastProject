using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInteractionSkill : Photon.MonoBehaviour, IPunObservable {
    /**** public ****/

    /**** private ****/


    private GameObject inGameCanvas;                       // UI 캔버스
    private GameObject interactiveObject;                  // 상호작용 오브젝트

    private int interViewID;                    // viewID 동기화 용 , 상호작용 물체에서 물체확인용으로 사용
    private Vector3 OriginalCameraPosition;             //플레이어 첫 카메라 위치

    private Animator animator;                           // 애니메이터 컴포넌트

    private FindObject findObject;                         // 탐지 정보
    private TimeBar timeBar;                            // 타임 바 정보
    private PlayerCamera playerCamera;                       // 카메라 정보
    private PlayerState playerState;                        // 플레이어 상태 정보
    private InteractiveState interactiveState;                   // 상호작용 물체 정보
    private PlayerMove boxPlayerMove;                // 플레이어 이동 스크립트

    /**** 접근자 ****/


    public int GetinterViewID() { return interViewID; }
    public void SetinterViewID(int IV) { interViewID = IV; }

    public GameObject GetinteractiveObject() { return interactiveObject; }
    public void SetinteractiveObject(GameObject IO) { interactiveObject = IO; }

    public InteractiveState GetinteractiveState() { return interactiveState; }
    public void SetinteractiveState(InteractiveState IS) { interactiveState = IS; }

    /**** 유니티 함수 ****/


    private void Awake()
    {
        inGameCanvas = GameObject.Find("InGameObject");                               // 캔버스 설정

        animator = GetComponent<Animator>();                                      // 애니메이터 설정

        findObject = GetComponent<FindObject>();                                    // 탐지 오브젝트 설정
        timeBar = GetComponent<TimeBar>();                                       // 타임바 설정
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();  // 카메라 설정
        playerState = GetComponent<PlayerState>();                                   // 플레이어 상태 설정
        boxPlayerMove = GetComponent<PlayerMove>();                        //플레이어 이동 스크립트


        OriginalCameraPosition = Vector3.zero;

        // 애니메이션 포톤 뷰 설정
        gameObject.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("InteractionType", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
    }

    // 동기화, 플레이어 물체정보 동기화
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(interViewID);
            stream.SendNext(OriginalCameraPosition);
        }

        else
        {
            interViewID = (int)stream.ReceiveNext();
            OriginalCameraPosition = (Vector3)stream.ReceiveNext();
        }
    }

    // 캐릭터 스킬 사용 여부 체크
    private void Update()
    {
        // 본인이라면
        if (photonView.isMine)
        {

            // 상호작용이 가능하다면
            if (findObject.GetIsInteraction())
            {

                // 상태를 살핀다.
                if (CheckState())
                {

                    // 키 눌렀는지 판단한다.
                    if (CheckInterKey())
                    {



                        // 1.  상호작용 물체 등록
                        interactiveObject = findObject.GetObjectTarget();
                        interactiveState = interactiveObject.GetComponent<InteractiveState>();

                        // 2. 상호작용 탐지 해제
                        findObject.SetisUseFindObject(false);

                        // 3. 상호작용 설정 디폴트로 변경
                        findObject.BackDefault();

                        // 4. 카메라 설정 변경
                        playerCamera.SetCameraRadX(-transform.eulerAngles.y);
                        playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FREE);

                        // 5. 타임 바 기본으로 설정
                        BaseTimeBarScript();

                        // 6. 상태 NONE 으로 변경
                        playerState.SetPlayerCondition(PlayerState.ConditionEnum.NONE);
                        


                        // 스피드 0으로 설정
                        boxPlayerMove.SetVSpeed(0.0f);
                        boxPlayerMove.SetHSpeed(0.0f);

                        // 애니메이션 블렌드 위치 0 으로 설정
                        animator.SetFloat("DirectionX", 0);
                        animator.SetFloat("DirectionY", 0);



                        // 7. 전송
                        interactiveState.CallInterMessage(photonView.viewID, this);

                        // 8. ViewID 동기화
                        // 애니메이션 동기화하고 viewID 동기화시간이 다름, 최대 0.1초
                        interViewID = interactiveObject.GetPhotonView().viewID;

                        // 9. 플레이어 카메라 위치 저장, 동기화
                        OriginalCameraPosition = playerCamera.transform.position;
                    }
                }
            }

        }

    }



    // 함수


    // 서버에게 함수 받음
    public void UseSkill()
    {
        // 1. 애니메이션 재생, 상호작용 물체에 따라 다름
        animator.SetInteger("InteractionType", (int)interactiveState.interactiveObjectType);

    }

    // 서버에게 함수 받음
    public void DontUseSkill()
    {

        // 1. 스킬 실패에 따른 상태값 다시 주기
        playerState.SetPlayerCondition(PlayerState.ConditionEnum.IDLE);

        // 2. 스킬 실패했다고 알려줌. 누가 사용중이니까.
        Debug.Log("누가 사용중. + "+ interactiveState.photonView.viewID);

        ResetSkill();


    }

    // 상태 체크
    private bool CheckState()
    {
        if (
             playerState.EqualPlayerCondition(PlayerState.ConditionEnum.IDLE) ||
             playerState.EqualPlayerCondition(PlayerState.ConditionEnum.RUN)
             )
        {
            return true;
        }
        else
            return false;
    }

    // 키 체크
    private bool CheckInterKey()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }
        return false;
    }

    // 타임 바 기본 설정
    private void BaseTimeBarScript()
    {
        // 타임바 패널을 생성합니다.
        timeBar.CreateTimeBarPanel();

        // 타임바 시간을 조절합니다.
        timeBar.SetNowTime(0);
        timeBar.SetMaxTime(findObject.GetObjectState().InteractiveTime);

        // 타임바 카운트를 시작합니다.
        timeBar.SetisCount(true);
    }

    // 스킬 탈출로 인한 첫상태로 복구 , 애니메이션 스테이트 동일내용사용, 이 함수를 쓰지는 않음
    private void ResetSkill()
    {
        // 상호작용 탐지 가능
        findObject.SetisUseFindObject(true);

        // 타임바 파괴
        timeBar.DestroyObjects();

        // 카메라 원래대로 복귀
        playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);
    }

    /**** 애니메이션 이벤트 ****/



    // 액션 사용, 물리나 애니를 호출함
    private void CallAction()
    {
        Debug.Log(interactiveState);
        // 물리 일 경우 날라갈 위치의 노말벡터 전달
        interactiveState.UseAction(/*transform.position - playerCamera.transform.position*/transform.position - OriginalCameraPosition);
    }

    private void OffInteraction()
    {
        Debug.Log("애니메이션 이벤트로 종료하기");
        animator.SetInteger("InteractionType", 0);
    }




}

