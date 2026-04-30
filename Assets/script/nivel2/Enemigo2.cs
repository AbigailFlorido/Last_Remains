using UnityEngine;

public class Enemigo2 : MonoBehaviour
{
    [Header("Configuración de Movimiento2")]
    public float velocidad = 2f;
    public float distanciaVision = 8f;
    public float attackRange = 1.8f;
    public float attackCooldown = 1.2f;
    public float danoQueHace = 1f;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        fuenteDeAudio = GetComponent<AudioSource>();

        gameObject.tag = "Enemy";
    }

    void FixedUpdate()
    {
        if (player == null || Vida <= 0) return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool viendoAlPlayer = distance <= distanciaVision;
        bool enRangoAtaque = distance <= attackRange;

        // --- DETECCIÓN DE SUELO ---
        bool haySuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);

        // --- DETECCIÓN DE PARED (AJUSTADA) ---
        // Movimos el origen un poco más afuera (0.7f) y acortamos el rayo (0.2f) para que no se choque a sí mismo
        Vector2 rayOriginPared = (Vector2)transform.position + (Vector2.right * direccion * 0.7f);
        RaycastHit2D hitPared = Physics2D.Raycast(rayOriginPared, Vector2.right * direccion, 0.2f, capaSuelo);

        if (viendoAlPlayer)
        {
            int nuevaDir = player.position.x > transform.position.x ? 1 : -1;
            if (nuevaDir != direccion) Voltear();

            if (enRangoAtaque)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
            else if (haySuelo && hitPared.collider == null)
            {
                rb.linearVelocity = new Vector2(velocidad * direccion, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else // --- MODO PATRULLA ---
        {
            if ((!haySuelo || hitPared.collider != null) && tiempoEspera <= 0f)
            {
                Voltear();
                tiempoEspera = 0.5f;
            }

            rb.linearVelocity = new Vector2(velocidad * direccion, rb.linearVelocity.y);

            if (tiempoEspera > 0) tiempoEspera -= Time.fixedDeltaTime;
        }

        if (animator != null) animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void Attack()
    {
        if (animator != null) animator.SetTrigger("DogAttack");

        if (Vector2.Distance(transform.position, player.position) <= attackRange + 0.5f)
        {
            player.SendMessage("RecibirDano", danoQueHace, SendMessageOptions.DontRequireReceiver);
        }
    }

    void Voltear()
    {
        direccion *= -1;
        if (spriteRenderer != null) spriteRenderer.flipX = direccion > 0;

        if (puntoSuelo != null)
        {
            Vector3 pos = puntoSuelo.localPosition;
            pos.x = Mathf.Abs(pos.x) * direccion;
            puntoSuelo.localPosition = pos;
        }
    }

    public void TomarDano(float dano)
    {
        Debug.Log("Perro recibe daño: " + dano);
        Vida -= dano;
        if (Vida <= 0) Morir();
    }

    public void Morir()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        if (animator != null) animator.SetTrigger("DogDeath");
        Destroy(gameObject, 1.8f);
    }

    // --- DIBUJAR SENSORES EN LA ESCENA ---
    void OnDrawGizmos()
    {
        // 1. DIBUJAR SENSOR DE SUELO (Verde = Detectado, Rojo = Vacío)
        if (puntoSuelo != null)
        {
            bool detectandoSuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
            Gizmos.color = detectandoSuelo ? Color.green : Color.red;
            Gizmos.DrawLine(puntoSuelo.position, puntoSuelo.position + Vector3.down * distanciaSuelo);
        }

        // 2. DIBUJAR SENSOR DE PARED (Amarillo = Pared detectada, Azul = Libre)
        Vector2 rayOriginPared = (Vector2)transform.position + (Vector2.right * direccion * 0.7f);
        bool hayPared = Physics2D.Raycast(rayOriginPared, Vector2.right * direccion, 0.2f, capaSuelo);

        Gizmos.color = hayPared ? Color.yellow : Color.blue;
        Gizmos.DrawLine(rayOriginPared, rayOriginPared + (Vector2.right * direccion * 0.2f));
    }
}