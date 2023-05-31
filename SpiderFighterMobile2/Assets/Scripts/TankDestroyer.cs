using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDestroyer : MonoBehaviour
{
	public void DestroyTank()
	{
		Debug.Log("Destroying tank");
		if (gameObject.activeSelf)
		{
			StartCoroutine(DestroyTankDelayed());
		}
	}

	private IEnumerator DestroyTankDelayed()
	{
		yield return new WaitForSeconds(5);
		if (gameObject.activeSelf)
		{
			Destroy(gameObject);
		}
	}
}