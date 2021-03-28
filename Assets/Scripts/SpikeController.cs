using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
	public AudioClip hitClip;
	
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController >();

        if (controller != null)
        {
			controller.hitEffect.Play();
            controller.ChangeHealth(-1);
        }
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController >();

        if (controller != null)
        {
			controller.PlaySound(hitClip);
        }
    }

}