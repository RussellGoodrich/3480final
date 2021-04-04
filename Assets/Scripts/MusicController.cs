using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	public AudioClip winClip;
	public AudioClip loseClip;
	
	AudioSource audioSource;
	
    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void winCommand()
	{
		audioSource.Stop();
		audioSource.clip = winClip;
		audioSource.Play();
		audioSource.loop = true;
	}
	
	public void loseCommand()
	{
		audioSource.Stop();
		audioSource.clip = loseClip;
		audioSource.Play();
		audioSource.loop = true;
	}
}
