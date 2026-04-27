using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
	[SerializeField] private Transform controladorDisparo;
	[SerializeField] private GameObject bala;
	
	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			//disparar
			Disparar();
			Debug.Log("Dispara");
		}
	}
	private void Disparar()
	{
		Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
	}
}
