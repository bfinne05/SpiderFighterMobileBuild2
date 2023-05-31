using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
	public bool hasParticleSystem;
	public bool Player = false;

	public GameManager gameManager;

	

    private void Start()
    {
		 gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    private void OnCollisionEnter(Collision collision)
	{
		if (Player)
		{
			if (collision.gameObject.CompareTag("Player"))
			{
				if (hasParticleSystem)
				{
					CreateParticleSystem();
				}
				gameManager.SetScore(150);
				Destroy(gameObject);
			}
		}

		if (collision.gameObject.CompareTag("Ground"))
		{
			if (hasParticleSystem)
			{
				CreateParticleSystem();
			}
			gameManager.SetScore(100);
			Destroy(gameObject);
		}
	}

	void CreateParticleSystem()
	{

	}
}
