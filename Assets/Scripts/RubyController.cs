using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

﻿public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    
    public GameObject projectilePrefab;
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
	
	public ParticleSystem hitEffect;
	public ParticleSystem fruitEffect;
	
	AudioSource audioSource;
	
	public AudioClip tossClip;
	
	private static int score = 0;
	public Text winText;
	public Text fixText;
	public Text ammoText;
	
	public int ammo = 4;
	
	public MusicController musicController;
	
	//private FixText fixText;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
		
		audioSource= GetComponent<AudioSource>();
		
		fixText.text = "Score: " + score;
		ammoText.text = "Ammo: " + ammo;
		
		//GameObject fixTextObject = GameObject.FindWithTag("FixTag");
		//if (fixText != null)
		//{
		//	fixText = fixTextObject.GetComponent<FixText>();
		//	fixText.text = "Score: " + score;
		//}
    }

    // Update is called once per frame
    void Update()
    {
		if (currentHealth > 0)
		{
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");
        
			Vector2 move = new Vector2(horizontal, vertical);
        
			if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
			{
				lookDirection.Set(move.x, move.y);
				lookDirection.Normalize();
			}
        
			animator.SetFloat("Look X", lookDirection.x);
			animator.SetFloat("Look Y", lookDirection.y);
			animator.SetFloat("Speed", move.magnitude);
		}
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
			if (ammo > 0)
			{
				Launch();
				PlaySound(tossClip);
				ammo -= 1;
				ammoText.text = "Ammo: " + ammo;
			}
        }
		
		if (Input.GetKeyDown(KeyCode.X))
		{
			RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
			if (hit.collider != null)
			{
				if (hit.collider != null)
				{
					NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
					if (character != null)
					{
						if (score < 4)
						{
							character.DisplayDialog();
						}
						
						if (score >= 4)
						{
							SceneManager.LoadScene("Level2");
							winText.text = "";
						}
					}  
				}
			}
		}
		
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (score >= 8)
			{
				SceneManager.LoadScene("Main");
				score -= 8;
			}
			if (currentHealth <= 0)
			{
				Restart();
			}
		}
    }
    
    void FixedUpdate()
    {
		if (currentHealth > 0)
		{
			Vector2 position = rigidbody2d.position;
			position.x = position.x + speed * horizontal * Time.deltaTime;
			position.y = position.y + speed * vertical * Time.deltaTime;

			rigidbody2d.MovePosition(position);
		}
    }

    public void ChangeHealth(int amount)
    {
		if (currentHealth > 0)
		{	
			if (amount < 0)
			{
				if (isInvincible)
					return;
            
				isInvincible = true;
				invincibleTimer = timeInvincible;
			}
        
			currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
			UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
		
			if (currentHealth == 0)
			{
				winText.text = "You lose! Game developed by Russell Goodrich. Press R to restart.";
				print ("Test here!");
				MusicSound();
			}
		}
    }
	
	public void ChangeAmmo()
	{
		ammo += 4;
		ammoText.text = "Ammo: " + ammo;
	}
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
	
	public void PlaySound(AudioClip clip)
	{
		audioSource.PlayOneShot(clip);
	}
	
	public void ChangeFix()
	{
		score += 1;
		print ("Score changed!");
		fixText.text = "Score: " + score;
		if (score == 4)
		{
			winText.text = "Talk to Jambi to visit stage two!";
		}
		
		if (score >= 8)
		{
			winText.text = "You win! Game developed by Russell Goodrich. Press R to restart.";
			MusicSound();
		}
	}
	
	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	public void MusicSound()
	{
		GameObject musicObject = GameObject.FindWithTag("MusicTag");
		if (musicObject != null)
		{
			musicController = musicObject.GetComponent<MusicController>();
			if (musicController != null)
			{
				if (score >= 8)
				{
					musicController.winCommand();
				}
				
				if (currentHealth == 0)
				{
					musicController.loseCommand();
				}
			}
		}
	}
}