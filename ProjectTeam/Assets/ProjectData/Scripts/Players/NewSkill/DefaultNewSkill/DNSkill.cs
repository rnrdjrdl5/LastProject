using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNSkill : MonoBehaviour {

    /**** private ****/

    public CoolDown coolDown;               // 스킬 쿨타임
    public SkillConditionOption skillConditionOption;               // 스킬 조건
    public SkillContinueConditionOption skillContinueConditionOption;       // 지속스킬 조건
    public DefaultInput InputKey;               // 키 입력 조건
    public DefaultInput ExitInputKey;           // 키 탈출 조건

    public float ManaCost;              // 스킬 비용
    public float CtnManaCost;               // 스킬 유지 비용

    public List<DefaultPlayerSkillDebuff> PlayerSkillDebuffs;               // 디버프 종류


    /**** protected ****/

    protected PhotonView photonView;
    public PhotonView GetphotonView() { return photonView; }
    public void SetphotonView(PhotonView pv) { photonView = pv; }

    protected PlayerState playerState;
    public PlayerState GetplayerState() { return playerState; }
    public void SetplayerState(PlayerState ps) { playerState = ps; }

    protected PlayerManaPoint manaPoint;
    public PlayerManaPoint GetmanaPoint() { return manaPoint; }
    public void SetmanaPoint(PlayerManaPoint mp) { manaPoint = mp; }

    protected Animator animator;


}
