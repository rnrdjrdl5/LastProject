using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class Player : Photon.PunBehaviour, IPunObservable
{

    public enum ConditionEnum { IDLE , RUN , WINDBLAST , BLINK , DAMAGE};

    private ConditionEnum PlayerCondition = ConditionEnum.IDLE;

    public ConditionEnum getPlayerCondition()
    {
        return PlayerCondition;
    }

    public void setPlayerCondition(ConditionEnum CE)
    {
        PlayerCondition = CE;
    }

    // Use this for initialization
    public PhotonView pv;

    Vector3 Position;
    Vector3 BeforePosition;

    Quaternion Rotation;

    private Animator PlayerAnimator;
    private AnimatorStateInfo PlayerAniStateInfo;


    public float PlayerSpeed = 10.0f;


    private float MaxHealth = 100.0f;
    public float NowHealth = 100.0f;
    private float recvHealth;

    private bool isMoving = false;

    public float RotationSpeed = 100.0f;        //회전스피드

    private Vector3 PlayersDistance = Vector3.zero; // 플레이어 사이, 노말벡터사용.


    public float PushSpeed = 10.0f; // 어택 시 밀려나가는 거리
    public float PushTime = 1.0f; // 어택 시 밀려나가는 시간

    public bool isAttack = false; //  , RPC로 받아서 서버에서 공격판단함.



    public bool isMasterClient = false;

    private bool otherMoving = false; // 본인이 조종하지 않는 클라이언트 일때, 맞아서 밀려나갈 때 애니메이션 조정하기 위한 용도

    public Image HPBar;

    public GameObject UICanvasPrefab;   // 프리팹으로 받아오기 위한 외부변수

    public GameObject BulletObject; // 원거리 공격 총알 오브젝트

    private GameObject UICanvasObject; // Instantiate로 만든 UI를 저장하기 위한 변수

    private GameObject PlayerCamera;


    public GameObject BlinkObject;  // 블링크위치를 확인하기 위한 오브젝트

    private GameObject BlinkPlace; // 블링크 하나밖에 생성 안되는 오브젝트




    public bool isBlinkOn = false;


    public float BlinkDistance = 20.0f;

    public float RaycastInfinity = 100000.0f;


    //애니메이션 이벤트를 직접 구현합니다. 
    // 메카님 애니메이션 이벤트에서는 이벤트 인식이 안되는 경우가 있다합니다.
    // 이를 위해 대리자 Delegate를 사용합니다.

    private delegate void AnimationDelegate();

    private void Awake()
    {
        PhotonNetwork.playerName = "Player" + " " + GetComponent<PhotonView>().viewID;


        

        pv = GetComponent<PhotonView>();
        Debug.Log(pv.viewID);
        Position = transform.position;
        Rotation = transform.rotation;
        PlayerAnimator = gameObject.GetComponent<Animator>();

                recvHealth = 9999.9f;
        // 이름설정, ID : 클라이언트 고유 번호
        //  Debug.Log(PhotonNetwork.playerName = "Data" + PhotonNetwork.player.ID);
        // PhotonVIew.viewID : 해당 오브젝트의 고유번호, 오브젝트를 소유하는 클라이언트에 비례됨.









        if (pv.isMine)  
        {
            PlayerCamera = GameObject.Find("PlayerCamera");
            PlayerCamera.GetComponent<PlayerCamera>().PlayerObject = gameObject;
            PlayerCamera.GetComponent<PlayerCamera>().isPlayerSpawn = true;
        }

        isMasterClient = PhotonNetwork.isMasterClient;

    }
    void Start()
    {
        if (pv.isMine)
        {
            UICanvasObject = Instantiate(UICanvasPrefab) as GameObject;
            HPBar = UICanvasObject.transform.Find("HPMPPanel/HP_Bar").gameObject.GetComponent<Image>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        isMasterClient = PhotonNetwork.isMasterClient;


        if (PhotonNetwork.isMasterClient) // 모든 처리는 마스터클라이언트에서 해준다.  (언리얼식 : 리슨서버)
        {

        }


        if (pv.isMine)
        {
            HPBar.fillAmount = NowHealth / MaxHealth;


            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log(pv.viewID);

            }

            if ( (!isMoving) && (
                    PlayerCondition == ConditionEnum.RUN ||
                    PlayerCondition == ConditionEnum.IDLE ||
                    PlayerCondition == ConditionEnum.BLINK ) )
            {
                transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * PlayerSpeed, Space.Self);
            }

            transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed * Input.GetAxis("Mouse X"));


            // 이름 찾기
            /* foreach (PhotonPlayer PP in PhotonNetwork.playerList)
             { 

                 Debug.Log(PP.name);

             }
             */

            if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && (!isMoving) 
                && (PlayerCondition==ConditionEnum.IDLE || PlayerCondition==ConditionEnum.RUN ) )
            {

                PlayerAnimator.SetBool("isIdleRun", true);
            }
            else
            {
                PlayerAnimator.SetBool("isIdleRun", false);
            }
            /*
            if (Input.GetMouseButtonDown(0))
            {
                pv.RPC("AttacKAnimation", PhotonTargets.All);
            }
            */

            //임시방편으로 Player  + ID로 설정해놨다.
            //추후에 ID기능 도입하면 ID로 인식해보자.
            if (Input.GetMouseButtonDown(0))
            {
                if (isBlinkOn)
                {
                    MoveBlink();
                    pv.RPC("BlinkPosition", PhotonTargets.Others, transform.position);
                }
                else if ( (!isBlinkOn) && (PlayerCondition != ConditionEnum.WINDBLAST) )
                {
                    // 바로 안쏜다. 이벤트 통해서 쏜다.
                    // pv.RPC("Shooting", PhotonTargets.All, transform.forward, "Player" + pv.viewID);

                    pv.RPC("WindBlastAnimation", PhotonTargets.All);



                }

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isBlinkOn == false)
                {

                    /*Physics.Raycast(transform.position + Vector3.forward * 5, 
                        PlayerCamera.GetComponent<PlayerCamera>().CameraRad*/


                    BlinkFunc();


                }
                else if (isBlinkOn == true)
                {
                    DeleteBlinkFunc();
                }

            }

            if (Input.GetMouseButtonDown(2))
            {
                NowHealth -= 10;
            }

            if (NowHealth <= 0)
            {
                PlayerDead();
            }


            if (isBlinkOn)
            {
                Vector3 BlinkObjectPosition = SetBlink();
                BlinkPlace.transform.position = BlinkObjectPosition;
                BlinkPlace.transform.rotation = transform.rotation;

            }
        }

        else
        {
            float Speed = 0.2f;
            transform.position = Vector3.Lerp(transform.position, Position, Time.deltaTime * 10.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, Time.deltaTime * 10.0f);
            NowHealth = recvHealth;


            if (!otherMoving)
            {
                if ((Position - transform.position).magnitude > Speed) // magnitude한 값으로 서로를 계산하면 오류 발생.
                {
                    PlayerAnimator.SetBool("isIdleRun", true);
                }
                else
                {
                    PlayerAnimator.SetBool("isIdleRun", false);
                }
            }

        }

    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            GetComponent<Rigidbody>().AddForce((PlayersDistance * PushSpeed * Time.fixedDeltaTime), ForceMode.Impulse);
        }
    }

    // 가설1. 해당 Script를 가진 소유자에서만 Writting이 사용되고, 
    // 나머지 클라이언트에서는 else를 통해 Writting에서 보낸 정보를 받는다.
    // 즉, 각 클라이언트간의 동기화.
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(NowHealth);
        }
        else
        {
            Position = (Vector3)stream.ReceiveNext();
            Rotation = (Quaternion)stream.ReceiveNext();
            recvHealth = (float)stream.ReceiveNext();

        }

    }


/*
    [PunRPC]
    void AttacKAnimation()
    {
        PlayerAnimator.SetTrigger("AttackTrigger");
        AniEventStruct.setNowPlayingAnimation(ConditionEnum.ATTACK);
        isAttack = true;
    }
    */

   /* [PunRPC]
    void DamageAnimation()
    {
        PlayerAnimator.SetTrigger("DamageTrigger");
        AniEventStruct.setNowPlayingAnimation(ConditionEnum.DAMAGE);
    }
    */
    // 데미지를 주었을 때 해당 클라이언트가 소유중일 때, PhotonView 를 소유중일때만 체크한다.
    [PunRPC]
    void ApplyDamage(float _Damage)
    {
        Debug.Log("ApplyDamage");
        if (pv.isMine)
        {
            NowHealth = NowHealth - _Damage;
            Debug.Log("맞음." + NowHealth);
        }
    }

    [PunRPC]
    void BackCharacter(Vector3 Vec3)
    {
        // 본인의 클라이언트면 움직임과 MoveStart를 사용해준다.
        if (pv.isMine)
        {
            Debug.Log("backCharacter사용");
            Debug.Log("Vec3 : " + Vec3);
            PlayersDistance = Vec3;
            StartCoroutine("MoveStart");


        }
        // 본인의 클라이언트면 Run 애니메이션을 막아버린다.
        else
        {


            // 다시 RPC를 받을 이유 없이 클라이언트 스스로 같은 시간 후에 꺼버린다. 
            // 다시 받을 이유도 없어서 레이턴씨로 인한 시간이 단축된다.
            StartCoroutine("MoveStartOtherClient");
        }


    }

    /* 2018-01-18 : 아직 자기가 쏜 미사일에 자신이 맞지 않도록 하는 처리는 하지 않았습니다.
     * 미사일에 누가 발사했는지의 대한 정보를 가지고 있어야 할 듯 싶습니다. */

    //[PunRPC]
    // RPC 제거 , 각 애니메이션 일정 시간되면 알아서 생성하도록 변경. 레이턴씨와 통신량을 늘릴 이유가 없음.
    void Shooting(Vector3 Dis, string PlayerName)
    {
        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = Dis * BulletDistance;
        BulletDefaultPlace.y += CharacterHeight;

        GameObject Bullet = Instantiate(BulletObject, transform.position + (BulletDefaultPlace), Quaternion.identity);
        Bullet.GetComponent<Bullet>().BulletDistance = Dis;
        Bullet.GetComponent<Bullet>().ShootPlayer = PlayerName;

    }

    struct AniEventStruct
    {
        public AnimationDelegate _EventDelegate;
        public float _Time ;

        public AniEventStruct(AnimationDelegate _Dele , float _t)
        {
            _EventDelegate = _Dele;
            _Time = _t;
        }

        // 설명 : 특정 시간 후에 애니메이션 실행하고자 할 때 
        //      애니메이션이 도중에 교체되버려도 코루틴은 멈추지않음.
        // 단점 : 애니메이션 교체 시 마다 지정해줘야함.
        // >> 애니메이션 교체를 함수로 만든다.

    }


    [PunRPC]
    void WindBlastAnimation()
    {
        PlayerAnimator.SetTrigger("WindBlastTrigger");

        AniEventStruct a = new AniEventStruct(new AnimationDelegate(initBullet), 0.833f);
        
        StartCoroutine("PlayerAnimationvEvent", a);
    }

    IEnumerator PlayerAnimationvEvent(AniEventStruct _AniEvent)
    {

        yield return new WaitForSeconds(_AniEvent._Time);

        //여기서 현재값을 비교해서, 재생중이 맞으면 다시 설정하는건데..? 
        //이러면 스테이트머신을 써야한다?
        //1. 스테이트머신을 일일히 추가하느냐?
        //2. 아니면 애니메이션 재생시마다 일일히 상태를 변경시키냐?
        //3. 스테이트머신은 되도록이면 쓰지말자? 믿을 수 없음.

            _AniEvent._EventDelegate();
        
        yield return null;
    }

    void initBullet()
    {
        Shooting(transform.forward, "Player" + pv.viewID);
        Debug.Log("불릿호출");

    }




    [PunRPC]
    void BlinkPosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        // 시리얼라이즈에서 이미 받고 나서 
        // update 로 가기 전에
        // RPC가 들어오면,
         // RPC로 인한 위치이동 > update로 인한 보간  이 순서대로 적용되서
         // RPC로 위치 이동했다가 보간위치로 이동함.
         // 그래서 시리얼라이즈로 받는 값도 바꿔준다.
        Position = newPosition;

         // 도착 후 생성? Instantiate(BlinkEffect, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
      /*  if(PhotonNetwork.isMasterClient)
        {
            if (collision.collider.tag == "Punch")
            {
                
                //어택중인 사람하고 받는사람하고 다름. 

                


                if (collision.collider.gameObject.GetComponentInParent<Player>().isAttack)
                {
                    pv.RPC("DamageAnimation", PhotonTargets.All);

                    pv.RPC("BackCharacter", PhotonTargets.All, collision.collider.gameObject.GetComponentInParent<Player>().transform.forward);
                    Debug.Log("충돌처리 성공, Punch");
                }
            }
        }*/
    }



    void OnGUI()
    {
    }


    /*******2018/1/18 문제 발생 **********
     * 내용 : 맞은 사람의 클라이언트에서는 피격모션과 대기모션이 제대로 보이나
     * 때린사람 입장에서는 때린 플레이어가 마지막에 가서 뛰는 모습이 보임.*/
     

    IEnumerator MoveStart()
    {
        PlayerAnimator.SetBool("DamageOnOff", true);
        isMoving = true;

        yield return new WaitForSeconds(PushTime);

        PlayerAnimator.SetBool("DamageOnOff", false);
        isMoving = false;

        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return null;
    }

    IEnumerator MoveStartOtherClient()
    {
        PlayerAnimator.SetBool("DamageOnOff", true);
        otherMoving = true;

        yield return new WaitForSeconds(PushTime);

        PlayerAnimator.SetBool("DamageOnOff", false);
        otherMoving = false;

        yield return null;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "DeadZone")
        {
            PlayerDead();
        }



        //총알 생성 방식도 PhotonNetwork.Instantiate를 사용하지 않음.
        //삭제방식도 개인이 처리하고 단, 서버가 맞았을 때는 처리를 한다.


        //현재 쏘는 입장에서 상대를 맞추면 튕겨나가는 시간이 오래걸려보인다.
        // 이유는, RPC를 통해 상대에게 튕겨나가도록 설정하는  시간 ( RPC 전송시간 ) 과
        // 시리얼라이즈에서 맞은 상대의 좌표값을 받아오는 주기 시간 ( 레이턴씨 + 주기시간 ) 이 포함되어 있기 때문.


        if ((other.tag == "Bullet") &&
            (other.gameObject.GetComponent<Bullet>().ShootPlayer != "Player" + pv.viewID))
        {
            Destroy(other.gameObject);

            if (PhotonNetwork.isMasterClient && other.gameObject.GetComponent<Bullet>().isBulletCheck == false)
            {
                pv.RPC("ApplyDamage", PhotonTargets.All, 10.0f);
                pv.RPC("BackCharacter", PhotonTargets.All, other.gameObject.GetComponent<Bullet>().BulletDistance);


                //Destroy 가 늦어서 총알충돌 인식이 여러번 되는 경우가 있다. 
                //총알의 체크를 한번만 하도록 설정해서 막는다.
                other.gameObject.GetComponent<Bullet>().isBulletCheck = true;

                
            }
        }

    }


    
    void OnDrawGizmos()
    {/*
        //복잡하다. 


        Gizmos.color = Color.red;

        float GizmosDistance = 10.0f;
        // Quaternion을 2단계로 나눈다. x축 회전에서 문제가 발생한다. 

        // 1. y축만 해당하는 회전을 시킨다.
        // ( 회전값은 플레이어의 회전값 )
        Quaternion q2 = Quaternion.Euler(0,
            transform.eulerAngles.y, 0);

        // 2. 회전값을 적용시킨다. ( 이 값은 transfom.forward와 동일하다. ) 
        Vector3 vec3 = q2 * Vector3.forward * GizmosDistance;

        // 3. 두번째 Quaternion을 생성한다.  x축.
        // (회전 값은 카메라를 통해 받아온다. 
       Quaternion q3 = Quaternion.Euler(PlayerCamera.GetComponent<PlayerCamera>().CameraRad, 0, 0);

        // **************!중요!******************
        //q3 * Vector.forward 값은 z축과 y축의 변화를 가진다.
        // 여기서, z축은 첫번째 x축회전 vector에 추가적용시킨다.
        // x,z 축은 첫번째 계산식으로 distance 적용시킴
        // y축은 distance 미적용상태, 추가 적용한다.

        vec3 = vec3 * (q3 * Vector3.forward).z;
        vec3.y = (q3 * Vector3.forward * GizmosDistance).y;


        

        //Gizmos.DrawRay(transform.position + Vector3.up * 1.0f , vec3);
        */
    }

    void BlinkFunc()
    {
        Vector3 BlinkObjectPosition = SetBlink();
        BlinkPlace = Instantiate(BlinkObject,BlinkObjectPosition + Vector3.up * 0.5f,transform.rotation);

        isBlinkOn = true;
    }


    private Vector3 SetBlink()
    {
        RaycastHit hit;

        Vector3 MouseVector3 = MouseRayCast();


        Vector3 BlinkVector3 = Vector3.zero;
        if (Physics.Raycast(PlayerCamera.GetComponent<Transform>().position, MouseVector3, out hit, BlinkDistance, 1 << LayerMask.NameToLayer("Floor")))
        {
            BlinkVector3 = hit.point;
           // BlinkVector3 += Vector3.up * 0.5f;
        }

        else
        {
            // 1. 방향을 거리적용
            Vector3 NotTargetBlink = PlayerCamera.GetComponent<Transform>().position + MouseVector3 * BlinkDistance;

            if (Physics.Raycast(NotTargetBlink, Vector3.down, out hit, RaycastInfinity, 1 << LayerMask.NameToLayer("Floor")))
            {
                BlinkVector3 = hit.point;
             //   BlinkVector3 += Vector3.up * 0.5f;
            }
            else
            {
                Debug.Log("레이캐스트 못찾았다. 위치를  본인위치로 돌려준다.");
                return transform.position;
                
            }
        }

        return BlinkVector3;
    }




    void DeleteBlinkFunc()
    {
        Destroy(BlinkPlace);
        isBlinkOn = false;
    }


    Vector3 MouseRayCast()
    {

        Quaternion q2 = Quaternion.Euler(0,
            transform.eulerAngles.y, 0);

        Vector3 MouseVector3 = q2 * Vector3.forward;


        Quaternion q3 = Quaternion.Euler(PlayerCamera.GetComponent<PlayerCamera>().CameraRad, 0, 0);

        MouseVector3 = MouseVector3 * (q3 * Vector3.forward).z;
        MouseVector3.y = (q3 * Vector3.forward).y;

        return MouseVector3;
        
    }

    void MoveBlink()
    {
        transform.position = BlinkPlace.transform.position;
        Destroy(BlinkPlace);

        isBlinkOn = false;
    }

    void PlayerDead()
    {
        PlayerCamera.GetComponent<PlayerCamera>().isPlayerSpawn = false;
        PhotonNetwork.Destroy(gameObject);
        Destroy(UICanvasObject);
    }

    /*
        public override void OnDisconnectedFromPhoton()
        {
            pv.RPC("Test", PhotonTargets.Others);

        }

        public override void OnLeftRoom()
        {
            pv.RPC("Test", PhotonTargets.Others);
        }

        public override void OnConnectionFail(DisconnectCause cause)
        {
            pv.RPC("Test", PhotonTargets.Others);

        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {


                Debug.Log("클라이언트 접속종료! " + otherPlayer.ID);
            //모두다 false로 나온다.
            Debug.Log("마스터 클라이언트 여부 : " + otherPlayer.IsMasterClient);
        }
        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            Debug.Log("새 클라이언트 : "  + newMasterClient.IsMasterClient);
            Debug.Log("새 클라이언트 ID : " + newMasterClient.ID); 

        }
        */

}
