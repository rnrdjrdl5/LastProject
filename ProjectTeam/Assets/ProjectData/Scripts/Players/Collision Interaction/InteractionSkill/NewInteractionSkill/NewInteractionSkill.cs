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
    //private TimeBar timeBar;                            // 타임 바 정보
    private PlayerCamera playerCamera;                       // 카메라 정보
    private PlayerState playerState;                        // 플레이어 상태 정보
    private InteractiveState interactiveState;                   // 상호작용 물체 정보
    private PlayerMove boxPlayerMove;                // 플레이어 이동 스크립트

    public ObjectManager objectManager;             // 오브젝트 매니저



    /**** 접근자 ****/


    public int GetinterViewID() { return interViewID; }
    public void SetinterViewID(int IV) { interViewID = IV; }

    public GameObject GetinteractiveObject() { return interactiveObject; }
    public void SetinteractiveObject(GameObject IO) { interactiveObject = IO; }

    public InteractiveState GetinteractiveState() { return interactiveState; }
    public void SetinteractiveState(InteractiveState IS) { interactiveState = IS; }

    public ObjectManager GetobjectManager() { return objectManager; }
    public void SetobjectManager(ObjectManager om) { objectManager = om; }

    /**** 유니티 함수 ****/


    private void Awake()
    {

        inGameCanvas = GameObject.Find("InGameObject");                               // 캔버스 설정

        animator = GetComponent<Animator>();                                      // 애니메이터 설정

        findObject = GetComponent<FindObject>();                                    // 탐지 오브젝트 설정
        //timeBar = GetComponent<TimeBar>();                                       // 타임바 설정
        //playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();  // 카메라 설정
        playerCamera = PlayerCamera.GetInstance();
        playerState = GetComponent<PlayerState>();                                   // 플레이어 상태 설정
        boxPlayerMove = GetComponent<PlayerMove>();                        //플레이어 이동 스크립트
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();             // 오브젝트 매니저 초기화
        


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
    //    Debug.Log(PhotonNetwork.player.CustomProperties["MouseScore"]);

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


                        // 5. 타임 바 보이게하고, 정보 설정함
                        //                        BaseTimeBarScript();
                        UIManager.GetInstance().timerBarPanelScript.PreTimeBar(
                            interactiveState.InteractiveTime);

                        // 6. 상태 NONE 으로 변경
                        playerState.SetPlayerCondition(PlayerState.ConditionEnum.NONE);
                        


                        // 스피드 0으로 설정
                        boxPlayerMove.SetVSpeed(0.0f);
                        boxPlayerMove.SetHSpeed(0.0f);
                        boxPlayerMove.SetMoveDir(Vector3.up * boxPlayerMove.GetMoveDir().y);


                        // 애니메이션 블렌드 위치 0 으로 설정
                        animator.SetFloat("DirectionX", 0);
                        animator.SetFloat("DirectionY", 0);

                        // 플레이어 입력 불가상태로 변경 , 한번 모두 떼야 가능하도록
                        boxPlayerMove.SetisCanKey(false);



                        // 7. 전송
                        interactiveState.CallInterMessage(photonView.viewID, this);

                        // 8. ViewID 동기화
                        // 애니메이션 동기화하고 viewID 동기화시간이 다름, 최대 0.1초
                        
                        interViewID = interactiveObject.GetPhotonView().viewID;
                        Debug.Log("실제 번호 : " + interViewID);

                        // 9. 플레이어 카메라 위치 저장, 동기화
                        OriginalCameraPosition = playerCamera.transform.position;
                    }
                }
            }

        }

    }



    // 서버에게 함수 받음
    public void UseSkill()
    {

        // 물리, 애니메이션 모두다 플레이어의 뒤 방향으로 설정
        // cameraRadX : Free카메라용 오프셋
        // cameraRadX는 Free 상태에서만 갱신되기 때문에 이렇게 첫 값을 지정해 주어야 함.


        // 플레이어 카메라 이전 좌표 저장 , 
        // 애니메이션 액션 보간에서 사용

        // 1. 카메라 이전 위치 지정
        playerCamera.PreCameraRadX = -playerCamera.transform.rotation.eulerAngles.y;

        // 2. 카메라 free 위치 지정
        playerCamera.SetCameraRadX(-playerCamera.transform.rotation.eulerAngles.y);

        // 3. 카메라 타입 free로 변경
        playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FREE);


        // ++1. 애니메이션이면 추가로
        if (interactiveState.ActionType != InteractiveState.EnumAction.PHYSICS)
        {

            // 애니메이션의 타입에 따라 고정위치가 달라진다.


            // 플레이어타입이 Position으로 고정형이라면.
            Vector3 v3 = Vector3.zero;
            if (interactiveState.InterPosType == InteractiveState.EnumInterPos.POSITION)
            {

                // 1. 플레이어 위치 고정
                gameObject.transform.position =
                interactiveState.PlayerInterPosition.transform.position;




                // 2. 플레이어 각도 고정
                v3 = new Vector3
                {
                    x = gameObject.transform.rotation.eulerAngles.x,
                    y = interactiveState.PlayerInterPosition.transform.rotation.eulerAngles.y,
                    z = gameObject.transform.rotation.eulerAngles.z
                };

                gameObject.transform.rotation = Quaternion.Euler(v3);
            }

            else
            {

                // 1. 플레이어 위치 고정
                gameObject.transform.position =
                interactiveState.PlayerInterPosition.transform.position;




                // 2. 플레이어 각도 고정
                v3 = new Vector3
                {
                    x = gameObject.transform.rotation.eulerAngles.x,
                    y = interactiveState.PlayerInterPosition.transform.rotation.eulerAngles.y,
                    z = gameObject.transform.rotation.eulerAngles.z
                };

                gameObject.transform.rotation = Quaternion.Euler(v3);





            }


            // 3, 플레이어 각도 변경때문에 free위치 재조절
            playerCamera.CameraRadX = -v3.y;

            // 4. 액션 시전 후 돌아오는 위치를 지정
            boxPlayerMove.SetPlayerRotateEuler(-v3.y);

            // 5, 보간 사용 on
            playerCamera.ISPreUseLerp = true;

            // 6. 물체 애니메이션 적용 , 레이어 설정
            interactiveState.CallActionAnimation();
            
            
            
        }
        
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

   /* // 타임 바 기본 설정
    private void BaseTimeBarScript()
    {
        UIManager.GetInstance().TimeBarPanel.SetActive();


        // 타임바 패널을 생성합니다.
        timeBar.CreateTimeBarPanel();

        // 타임바 시간을 조절합니다.
        timeBar.SetNowTime(0);
        timeBar.SetMaxTime(findObject.GetObjectState().InteractiveTime);

        // 타임바 카운트를 시작합니다.
        timeBar.SetisCount(true);
    }*/

    // 스킬 탈출로 인한 첫상태로 복구 , 애니메이션 스테이트 동일내용사용, 이 함수를 쓰지는 않음
    private void ResetSkill()
    {
        // 상호작용 탐지 가능
        findObject.SetisUseFindObject(true);

        // 타임바 파괴
        //timeBar.DestroyObjects();
        UIManager.GetInstance().timerBarPanelScript.DestroyTimebar();

        // 카메라 원래대로 복귀
        playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);
    }

    /**** 애니메이션 이벤트 ****/



    // 액션 사용, 물리나 애니를 호출함
    // interactive 정보가 필요함.
    private void CallAction()
    {
        // 물리 일 경우 날라갈 위치의 노말벡터 전달
        interactiveState.UseAction(transform.position - OriginalCameraPosition);

        // 사용했을 때 스코어 추가
        if (photonView.isMine)
        {
            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { {"MouseScore"
                    ,(int)PhotonNetwork.player.CustomProperties["MouseScore"] + interactiveState.InterObjectScore } });

        }
    }

    private void OffInteraction()
    {
        Debug.Log("애니메이션 이벤트로 종료하기");
        animator.SetInteger("InteractionType", 0);

        if (photonView.isMine)
        {

            // 애니메이션 타입이고
            // 액션이 사용되지 않은 상태라면
            if (interactiveState.ActionType == InteractiveState.EnumAction.ANIMATION &&
                interactiveState.IsUseAction == false)
            {
                interactiveState.CallRPCCancelActionAnimation();
            }


            // FIndObject의 활성화 탐지 시작
            findObject.SetisUseFindObject(true);

            //timeBar.DestroyObjects();
            UIManager.GetInstance().timerBarPanelScript.DestroyTimebar();


            // 1. 카메라를 따라가는 상태로 변경.
            playerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FOLLOW);

            // 2. 플레이어의 회전값을 free값으로 변경
            animator.GetComponent<PlayerMove>().SetPlayerRotateEuler(-playerCamera.GetCameraRadX());




            Debug.Log("해제해제해제 , + " + interactiveState.photonView.viewID);

            // 모든 클라이언트에게 RPC전송 하는 함수 콜
            //interactiveState.SetisCanFirstCheck(true);
            // ** 마스터에서 처리하기에는 애니메이션 동기화 문제가 있어서 안됨
            interactiveState.CallOnCanFirstCheck();
        }
    }

    private void InitInterState()
    {

    }

    /***** RPC *****/

}

