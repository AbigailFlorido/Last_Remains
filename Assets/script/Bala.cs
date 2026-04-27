using UnityEngine;

public class Bala : MonoBehaviour
{
	[SerializeField] private float velocidad;
	[SerializeField] private float dano;
	
	private void Update()
	{
		transform.Translate(Vector2.right * velocidad * Time.deltaTime);
		Debug.Log ("La bala balea");
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<Enemigo>().TomarDano(dano);
			Destroy(gameObject);
			Debug.Log ("Daño a enemy");
		}
	}
}
