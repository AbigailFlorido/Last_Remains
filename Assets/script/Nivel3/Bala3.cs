using UnityEngine;

public class Bala3 : MonoBehaviour
{
	[SerializeField] private float velocidad;
	[SerializeField] private float dano;
	
	private void Update()
	{
		//transform.Translate(Vector2.right * velocidad * Time.deltaTime);
		transform.Translate(Vector2.right * velocidad * Time.deltaTime);
		Debug.Log ("La bala balea");
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			Enemigo3 scriptEnemigo = other.GetComponentInParent<Enemigo3>();
			//other.GetComponent<Enemigo>().TomarDano(dano);
			//Destroy(gameObject);
			//Debug.Log ("Daño a enemy");
			if (scriptEnemigo != null)
			{
				scriptEnemigo.TomarDano(dano);
				Debug.Log("Impacto en: " + other.name); // Esto te dirá si le diste a la cabeza o al cuerpo
				Destroy(gameObject);
			}
			else 
			{
				Debug.LogWarning("Se detectó tag Enemy en " + other.name + " pero no tiene el script Enemigo3");
			}
		}
	}
}
