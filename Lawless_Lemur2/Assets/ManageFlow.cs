using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageFlow : MonoBehaviour
{

    [SerializeField] GameObject[] toggleObj,skeletons;
    [SerializeField] GameObject Lemur, Explosion, End;
    private Animator lemurAnim;
    public int currentPos = 0;

    WordScramble scram;
    Animator anim;

    [Header("Audio")]
    AudioSource sauce;
    [SerializeField] AudioClip[] winClip, loseClip, dialogue,guitar;

    private void Start()
    {
        sauce = GetComponent<AudioSource>();
        scram = GetComponent<WordScramble>();
        lemurAnim = Lemur.GetComponent<Animator>();

        ToggleObj();

        Next();
    }
    public void ToggleObj()
    {
        foreach(GameObject gb in toggleObj)
        {
            gb.SetActive(!gb.activeInHierarchy);
        }
        //Scramble words
    }

    public void Next()
    {
        GameObject skele = skeletons[currentPos];
        anim = skele.GetComponent<Animator>();
        anim.SetBool("Talk", true);
        AudioClip selectedDialogue = Clip(dialogue);
        sauce.PlayOneShot(selectedDialogue);
        StartCoroutine(Wait(selectedDialogue.length,"normal"));
        //wait til after clip
    }

    private IEnumerator Wait(float Time,string type)
    {
        yield return new WaitForSeconds(Time);
        switch (type)
        {
            case "normal":
                Debug.Log("Done waiting");
                scram.StartShuffling();
                anim.SetBool("Talk", false);
                break;
            case "strum":
                lemurAnim.SetBool("Talk", false);
                break;
        }
    }

    public void GuitarStrum()
    {
        AudioClip clipToPlay = Clip(guitar);
        lemurAnim.SetBool("Talk", true);
        sauce.PlayOneShot(clipToPlay);
        StartCoroutine(Wait(clipToPlay.length, "strum"));
        
        
    }

    public void KillThem()
    {
        GameObject kill = skeletons[currentPos];
       // Instantiate(Explosion, kill.transform); BETTER PARTICLES  
        anim.SetBool("Die", true);
        //Destroy(kill);
        sauce.PlayOneShot(Clip(winClip));
        //Check for final one then go to next position
        //Lerp to position
        currentPos++;
    }
    public void Death()
    {
        Instantiate(Explosion, Lemur.transform);
        sauce.PlayOneShot(Clip(loseClip));
        Lemur.SetActive(false);
        End.SetActive(true);
        //Play explosion sound
    }

    public AudioClip Clip(AudioClip[] clip)
    {
        return clip[Random.Range(0, clip.Length)];
    }
}
