using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
	
	SpriteRenderer spriteColor;
	
	public float speed;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
		spriteColor = GetComponent<SpriteRenderer>();
    }
    
    public void Launch(Vector2 direction, float force)
    {
		speed = force;
        rigidbody2d.AddForce(direction * speed);
    }
	
	public void OilSplash()
	{
		spriteColor.color = new Color (0,0,0,1);
	}
    
    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }
		
		HardEnemyController h = other.collider.GetComponent<HardEnemyController>();
        if (h != null)
        {
            h.Fix();
        }
    
        Destroy(gameObject);
    }
}
