using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class audiomanager : MonoBehaviour
{
    public AudioClip damaged;
    public AudioClip shoot;
    public AudioClip sword;

    List<AudioSource> audios=new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<3;i++)
        {
            var audio=this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }

    void play(int index,string name,bool isloop)
    {
        var clip=GetAudioClip(name);
        if(clip!=null)
        {
            var audio=audios[index];
            audio.clip=clip;
            audio.loop=isloop;
            audio.Play();
        }
    }

    AudioClip GetAudioClip(string name)
    {
        switch(name)
        {
            case "damaged":
                return damaged;
            case "shoot":
                return shoot;
            case "sword":
                return sword;
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
