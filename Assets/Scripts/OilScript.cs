using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilScript : MonoBehaviour
{
	public AudioClip splashClip;
	
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController >();

        if (controller != null)
        {
			if (controller.InvincibleSet == false)
			{
				controller.OilSplash();
			}
        }
    }
	
	void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController >();

        if (controller != null)
        {
			if (controller.InvincibleSet == false)
			{
				controller.PlaySound(splashClip);
			}
        }
    }

}