using UnityEngine;

public class DisparoJugador2 : MonoBehaviour

{
	public GameObject Bala2Prefab;
	[SerializeField] private Transform controladorDisparo;
	[SerializeField] private GameObject Bala2;
	
	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			//disparar
			Disparar();
			Debug.Log("Dispara");
		}
	}
	//private void Disparar()
	//{
	//Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
	//}
	public void Disparar()
	{
		// Creamos la bala
		GameObject nuevaBala = Instantiate(Bala2Prefab, controladorDisparo.position, controladorDisparo.rotation);

		// Revisamos la orientación del jugador (puedes usar el spriteRenderer.flipX)
		// Suponiendo que tienes acceso al SpriteRenderer de tu personaje:
		if (GetComponent<SpriteRenderer>().flipX)
		{
			// Si el personaje mira a la izquierda, rotamos la bala 180 grados
			nuevaBala.transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		else
		{
			// Si mira a la derecha, rotación normal
			nuevaBala.transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
