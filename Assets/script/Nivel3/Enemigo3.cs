using UnityEngine;

public class Enemigo3 : MonoBehaviour
{
	[Header("Configuración de Movimiento3")]
	public float velocidad = 2f;
	public float distanciaVision = 8f; // Más grande que el ataque
	public float attackRange = 1.5f;   // Más corto, para que se acerque
	public float attackCooldown = 1f;

	[Header("Detección")]
	public Transform player;
	public Transform puntoSuelo;
	public float distanciaSuelo = 0.5f;
	public LayerMask capaSuelo;

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private int direccion = 1;
	private float tiempoEspera = 0f;
	private float nextAttackTime = 0f;

	private Animator animator;
	[SerializeField] private float Vida = 10f;
	private AudioSource fuenteDeAudio;
	public AudioClip sonidoAplastado;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		// Importante: Asegúrate de que el SpriteRenderer y Animator estén en el objeto o hijos
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();

		fuenteDeAudio = GetComponent<AudioSource>();
		if (fuenteDeAudio == null) fuenteDeAudio = gameObject.AddComponent<AudioSource>();
	}

	void FixedUpdate()
	{
		if (player == null || Vida <= 0) return;

		float distance = Vector2.Distance(transform.position, player.position);
		bool viendoAlPlayer = distance <= distanciaVision;
		bool enRangoAtaque = distance <= attackRange;

		// DETECCIÓN DE SUELO Y PAREDES
		bool haySuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
		Vector2 rayOrigin = (Vector2)transform.position + (Vector2.right * direccion * 0.5f);
		RaycastHit2D hitPared = Physics2D.Raycast(rayOrigin, Vector2.right * direccion, 0.5f, capaSuelo);

		tiempoEspera -= Time.fixedDeltaTime;

		if (viendoAlPlayer)
		{
			// 1. Determinar dirección hacia el jugador
			int nuevaDir = player.position.x > transform.position.x ? 1 : -1;
			if (nuevaDir != direccion) Voltear();

			if (enRangoAtaque)
			{
				// 2. DETENERSE Y ATACAR
				rb.linearVelocity = new Vector2(velocidad * direccion, rb.linearVelocity.y);
				if (Time.time >= nextAttackTime)
				{
					Attack();
					nextAttackTime = Time.time + attackCooldown;
				}
			}
			else
			{
				// 3. PERSEGUIR (Solo si hay suelo adelante)
				if (haySuelo && hitPared.collider == null)
				{
					rb.linearVelocity = new Vector2(velocidad * direccion, rb.linearVelocity.y);
				}
				else
				{
					rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
				}
			}
		}
		else
		{
			// 4. MODO PATRULLA
			if ((!haySuelo || hitPared.collider != null) && tiempoEspera <= 0f)
			{
				Voltear();
				tiempoEspera = 0.5f;
			}
			rb.linearVelocity = new Vector2(velocidad * direccion, rb.linearVelocity.y);
		}

		// Actualizar animaciones de caminar (si tienes el parámetro)
		if (animator != null)
		{
			animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
		}
	}

	void Attack()
	{
		Debug.Log("¡ATAQUE!");
		if (animator != null) animator.SetTrigger("Attack");
	}

	void Voltear()
	{
		direccion *= -1;
		if (spriteRenderer != null) spriteRenderer.flipX = direccion < 0;

		if (puntoSuelo != null)
		{
			Vector3 pos = puntoSuelo.localPosition;
			pos.x = Mathf.Abs(pos.x) * direccion;
			puntoSuelo.localPosition = pos;
		}
	}

	public void TomarDano(float dano)
	{
		Vida -= dano;
		if (Vida <= 0) Morir();
	}

	public void Morir()
	{
		rb.linearVelocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Static; // Para que no se mueva al morir
		if (animator != null) animator.SetTrigger("Muerte");
		Destroy(gameObject, 1.5f);
	}
}