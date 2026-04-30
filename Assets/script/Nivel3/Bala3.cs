using UnityEngine;

public class Bala3 : MonoBehaviour
{
	[SerializeField] private float velocidad = 10f;
	[SerializeField] private float dano = 1f;
	[SerializeField] private float tiempoDeVida = 3f;

	private void Start()
	{
		Destroy(gameObject, tiempoDeVida);
	}

	private void Update()
	{
		transform.Translate(Vector2.right * velocidad * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("La bala chocó con: " + other.name); // <--- ESTO ES LO MÁS IMPORTANTE

		IDamageable objetivo = other.GetComponent<IDamageable>();
		if (objetivo == null) objetivo = other.GetComponentInParent<IDamageable>();

		if (objetivo != null)
		{
			objetivo.TomarDano(dano);
			Destroy(gameObject);
		}
	}
}