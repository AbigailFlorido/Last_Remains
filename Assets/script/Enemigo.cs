using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 2f;
    public float distanciaVision = 5f;
    public Transform player;
    public Transform puntoSuelo;
    public float distanciaSuelo = 0.5f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int direccion = 1;
    private float tiempoEspera = 0f;

    public AudioClip sonidoAplastado;
	private AudioSource fuenteDeAudio;
    
	public float attackRange = 5f;
	public float attackCooldown = 1f;

	private Animator animator;
	private float nextAttackTime = 0f;
	
	[SerializeField] private float Vida;
	[SerializeField] private GameObject efectoMuerte;
	
	public void TomarDano(float dano)
	{
		Vida -= dano;
		if (Vida <= 0)
		{
			Muerte();
			Debug.Log("Se muerexd");
		}
	}
	private void Muerte()
	{
		Instantiate(efectoMuerte, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	
	{
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();
		Debug.Log(animator);

	    //Añadir AudioSource
        fuenteDeAudio = GetComponent<AudioSource>();
        if (fuenteDeAudio == null)
        {
            fuenteDeAudio = gameObject.AddComponent<AudioSource>();
        }
	}
    

	// Update is called once per frame
	void Update()
	
	{
		float distance = Vector2.Distance(transform.position, player.position);
		

		if (distance <= attackRange && Time.time >= nextAttackTime)
		{
			rb.linearVelocity = Vector2.zero;
			Attack();
			nextAttackTime = Time.time + attackCooldown;
		}
	}

	void Attack()
	{
		Debug.Log("ATAQUE ACTIVADO");
		animator.SetTrigger("Attack");
	}
	void FixedUpdate()
		
	
    {
        if (puntoSuelo == null) return;
        tiempoEspera -= Time.fixedDeltaTime;
        bool haySuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);

        // Sin suelo adelante: voltear (solamente si no acaba de voltear)
        if (haySuelo && tiempoEspera <= 0f)
        {
            Voltear();
            //va a esperar antes de voltear otra vez
            tiempoEspera = 0.3f;
            Debug.Log("Viste el suelo");
        }
        //Si el player esta cerca va a perseguirlo
         if (player != null && Vector2.Distance(transform.position, player.position) <= distanciaVision)
        {
            int nuevaDir = player.position.x > transform.position.x ? 1 : -1;
            if (nuevaDir != direccion) Voltear();
            Debug.Log("Viste al jugador");
        }
         //Siempre se mueva no se detenga al voltear
         rb.linearVelocity = new Vector2(velocidad*direccion, rb.linearVelocity.y);
	    //Debug.Log("Caminando");
    }
    void Voltear()
    {
        direccion *= -1;
        spriteRenderer.flipX = direccion < 0;

        Vector3 pos = puntoSuelo.localPosition;
        pos.x = Mathf.Abs(pos.x) * direccion;
        puntoSuelo.localPosition = pos;
    }
	public void Morir ()
	{
        //fuenteDeAudio.PlayOneShot(sonidoAplastado);
        AudioSource.PlayClipAtPoint(sonidoAplastado, transform.position);
        Destroy(gameObject);
	}
    private void OnDrawGizmosSelect()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(puntoSuelo.position, puntoSuelo.position + Vector3.down * distanciaSuelo);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaVision);
    }
}
