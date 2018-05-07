using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    static private SoundManager soundManager;

    static public SoundManager GetInstance() { return soundManager; }

    public AudioClip BackGroundSound;

    public AudioClip[] FrypanHit;

    // 1 배경음악
    // 2 이펙트소리
    private AudioSource[] audioSource;
    public AudioSource GetAudioSource(int number) { return audioSource[number]; }

    private void Awake()
    {
        soundManager = this;
        audioSource = GetComponents<AudioSource>();


        

            

    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayBackGround()
    {
        audioSource[0].clip = BackGroundSound;
        audioSource[0].loop = true;
        audioSource[0].Play();
    }

    public void PlayHitSound(PoolingManager.EffctType effctType)
    {
        if (PoolingManager.EffctType.ATTACK == effctType)
        {
            audioSource[1].PlayOneShot(FrypanHit[Random.Range(0, FrypanHit.Length)]);
        }
    }

}
