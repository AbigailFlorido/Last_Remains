using UnityEngine;

public class Enemigo2 : MonoBehaviour
{
    [Header("Configuración de Movimiento3")]
    public float velocidad = 2f;
    public float distanciaVision = 8f;
    public float attackRange = 1.8f;   // Lo subí un poco para que no tenga que estar "pegado"
    public float attackCooldown = 1.2f;
    public float danoQueHace = 1f;     // Nuevo: Daño del perro

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

        // Asegúrate de que el perro tenga el Tag "Enemy" en el Inspector
        gameObject.tag = "Enemy";
    }

    void FixedUpdate()
    {
        if (player == null || Vida <= 0) return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool viendoAlPlayer = distance <= distanciaVision;
        bool enRangoAtaque = distance <= attackRange;

        bool haySuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        Vector2 rayOrigin = (Vector2)transform.position + (Vector2.right * direccion * 0.5f);
        RaycastHit2D hitPared = Physics2D.Raycast(rayOrigin, Vector2.right * direccion, 0.5f, capaSuelo);

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
        }
        else
        {
            if ((!haySuelo || hitPared.collider != null) && tiempoEspera <= 0f)
            {
                Voltear();
                tiempoEspera = 0.5f;
            }
            rb.linearVelocity = new Vector2(velocidad * direccion, rb.linearVelocity.y);
            tiempoEspera -= Time.fixedDeltaTime;
        }

        if (animator != null) animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void Attack()
    {
        if (animator != null) animator.SetTrigger("DogAttack");

        // Hacer daño al jugador mediante mensaje (ajusta el nombre si tu función de daño es distinta)
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
        Destroy(gameObject, 1.5f);
    }
}