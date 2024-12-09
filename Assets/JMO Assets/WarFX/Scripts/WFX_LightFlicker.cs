using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	[SerializeField]
	private bool isOneShot = false;

	public float time = 0.05f;
	
	private float timer;

	private int count = 0;
	
	void Start ()
	{
		timer = time;
		StartCoroutine("Flicker");
	}
	
	IEnumerator Flicker()
	{
		while(true)
		{
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;

			if (isOneShot)
            {
				count++;
				if (count >= 2)
					break;
            }
			

			do
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			while(timer > 0);
			timer = time;
		}
	}
}
