using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Movimiento : MonoBehaviour
{
	public float velocidad = 4f;
	public float velocidadCarrera = 8f;
	public float fuerzaSalto = 5.5f;
	private Rigidbody2D rb;

	[Header("Ground Check")]
	public Transform groundCheck;
	public float radioGroundCheck = 0.2f;
	public LayerMask capaSuelo;
	private bool enSuelo = true;

	public int slime;
	public int vidas = 5;

	public TMP_Text textoPuntos;
	public float tiempoDano = 0.5f;
	public float fuerzaEmpujeX = 4f;
	public float fuerzaEmpujeY = 3f;
	private bool recibiendoDano = false;
	private Color colorOriginal;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	public float staminaMax = 5f;
	public float staminaActual;
	public float gastoStamina = 1f;
	public float recuperacion = 2f;
	public float delayRecuperacion = 12f;
	private float tiempoSinCorrer = 0f;

	public AudioClip sonidoMoneda;
	public AudioClip sonidoDano;
	public AudioClip sonidoSalto;
	private AudioSource fuenteDeAudio;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		colorOriginal = spriteRenderer.color;
		ActualizarPuntos();
		staminaActual = staminaMax;

		fuenteDeAudio = GetComponent<AudioSource>();
		if (fuenteDeAudio == null)
		{
			fuenteDeAudio = gameObject.AddComponent<AudioSource>();
		}
	}

	void Update()
	{
		float movimientoX = Input.GetAxis("Horizontal");
		float velocidadActual = velocidad;

		enSuelo = Physics2D.OverlapCircle(groundCheck.position, radioGroundCheck, capaSuelo);

		bool intentandoCorrer = Input.GetKey(KeyCode.LeftShift) && movimientoX != 0;

		if (intentandoCorrer && staminaActual > 0)
		{
			velocidadActual = velocidadCarrera;
			staminaActual -= gastoStamina * Time.deltaTime;
			tiempoSinCorrer = 0f;
		}
		else
		{
			tiempoSinCorrer += Time.deltaTime;
			if (tiempoSinCorrer >= delayRecuperacion)
			{
				staminaActual += recuperacion * Time.deltaTime;
			}
		}
		staminaActual = Mathf.Clamp(staminaActual, 0, staminaMax);

		// Si usas una versión vieja de Unity y te da error en .linearVelocity, cámbialo a .velocity
		rb.linearVelocity = new Vector2(movimientoX * velocidadActual, rb.linearVelocity.y);

		if (movimientoX > 0) spriteRenderer.flipX = false;
		else if (movimientoX < 0) spriteRenderer.flipX = true;

		if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
			fuenteDeAudio.PlayOneShot(sonidoSalto);
		}

		// Animaciones
		// 1. Enviamos la velocidad al parámetro "Speed" (Mathf.Abs asegura que siempre sea positivo)
		animator.SetFloat("Speed", Mathf.Abs(movimientoX));

		// 2. Enviamos si estamos en el suelo al parámetro "isGrounded"
		animator.SetBool("isGrounded", enSuelo);
	} // <--- AQUÍ ESTABA EL ERROR (había una llave extra abajo de esta)

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "slime")
		{
			slime = slime + 1;
			ActualizarPuntos();
			fuenteDeAudio.PlayOneShot(sonidoMoneda);
			Destroy(other.gameObject);
		}
        
		if (other.CompareTag("eskeleton") && vidas > 0)
		{
			RecibirDano();
		}

		if (other.CompareTag("Cabeza_enemigo"))
		{
			// OJO: Asegúrate de tener el script "Enemigo" creado
			Enemigo enemigo = other.GetComponentInParent<Enemigo>();
			if (enemigo != null)
			{
				enemigo.Morir();
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto * 0.7f);
			}
		}
	}

	void RecibirDano()
	{
		if (recibiendoDano) return;

		vidas--;
		recibiendoDano = true;
		spriteRenderer.color = Color.red;
		fuenteDeAudio.PlayOneShot(sonidoDano);

		float direccionEmpuje = spriteRenderer.flipX ? 1f : -1f;
		rb.linearVelocity = new Vector2(direccionEmpuje * fuerzaEmpujeX, fuerzaEmpujeY);

		if (vidas <= 0)
		{
			Invoke("ReiniciarNivel", 0.5f);
		}

		Invoke("VolverANormal", tiempoDano);
	}

	void VolverANormal()
	{
		recibiendoDano = false;
		spriteRenderer.color = colorOriginal;
	}

	void ReiniciarNivel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy") && vidas > 0 && !recibiendoDano)
		{
			RecibirDano();
		}
	}

	void ActualizarPuntos()
	{
		if (textoPuntos != null)
			textoPuntos.text = "Slime: " + slime;
	}

	void OnDrawGizmosSelected()
	{
		if (groundCheck == null) return;
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(groundCheck.position, radioGroundCheck);
	}
}