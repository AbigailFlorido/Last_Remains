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

	public int Cartas;
	public int vidas = 5;
	public TMP_Text textoPuntos;

	[Header("Daño y Empuje")]
	public float tiempoDano = 0.5f;
	public float fuerzaEmpujeX = 4f;
	public float fuerzaEmpujeY = 3f;
	private bool recibiendoDano = false; // Esta variable ahora sí se usa
	private Color colorOriginal;
	private Animator animator;
	private SpriteRenderer spriteRenderer;

	[Header("Stamina")]
	public float staminaMax = 5f;
	public float staminaActual;
	public float gastoStamina = 1f;
	public float recuperacion = 2f;
	public float delayRecuperacion = 12f;
	private float tiempoSinCorrer = 0f;
	public Animator miAnimatorUI;

	[Header("Audio")]
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
		staminaActual = staminaMax;
		ActualizarPuntos(); // Inicializamos el texto al empezar

		fuenteDeAudio = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
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
			if (tiempoSinCorrer >= delayRecuperacion) staminaActual += recuperacion * Time.deltaTime;
		}
		staminaActual = Mathf.Clamp(staminaActual, 0, staminaMax);

		rb.linearVelocity = new Vector2(movimientoX * velocidadActual, rb.linearVelocity.y);

		if (movimientoX > 0) spriteRenderer.flipX = false;
		else if (movimientoX < 0) spriteRenderer.flipX = true;

		if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
			fuenteDeAudio.PlayOneShot(sonidoSalto);
		}

		animator.SetFloat("Speed", Mathf.Abs(movimientoX));
		animator.SetBool("isGrounded", enSuelo);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Cartas"))
		{
			other.enabled = false; // Para que no cuente doble
			Cartas++;
			ActualizarPuntos();

			if (miAnimatorUI != null)
			{
				// Fíjate bien: miAnimatorUI es la variable, .Play es la función
				miAnimatorUI.Play("Carta" + Cartas);
			}

			Destroy(other.gameObject,12f);
		}

		if (other.CompareTag("Enemy") && vidas > 0)
		{
			RecibirDano();
		}

		if (other.CompareTag("Cabeza_enemigo"))
		{
			Enemigo enemigo = other.GetComponentInParent<Enemigo>();
			if (enemigo != null)
			{
				enemigo.Morir();
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto * 0.7f);
			}
		}
	}

	public void RecibirDano()
	{
		if (recibiendoDano) return; // AQUÍ SE USA LA VARIABLE PARA NO RECIBIR DAÑO REPETIDO

		vidas--;
		recibiendoDano = true; // Activamos el escudo temporal
		spriteRenderer.color = Color.red;
		fuenteDeAudio.PlayOneShot(sonidoDano);

		float direccionEmpuje = spriteRenderer.flipX ? 1f : -1f;
		rb.linearVelocity = new Vector2(direccionEmpuje * fuerzaEmpujeX, fuerzaEmpujeY);

		if (vidas <= 0)
		{
			SceneManager.LoadScene("GameOverScene");
		}
		else
		{
			Invoke("VolverANormal", tiempoDano);
		}
	}

	void VolverANormal()
	{
		recibiendoDano = false; // Desactivamos el escudo
		spriteRenderer.color = colorOriginal;
	}

	void ActualizarPuntos() // EL MÉTODO QUE FALTABA
	{
		if (textoPuntos != null)
			textoPuntos.text = "Cartas: " + Cartas;
	}
}