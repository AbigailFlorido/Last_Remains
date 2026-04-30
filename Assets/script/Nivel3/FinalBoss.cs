using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 1.5f;
    public float attackRange = 1.2f;
    public float distanciaVision = 10f;
    public float danoQueHace = 1f;
    public float limiteAltura = 2.0f; // <-- NUEVO: Máxima diferencia de altura para que te vea

    [Header("Detección")]
    public Transform player;
    public Transform puntoSuelo;
    public float distanciaSuelo = 0.5f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private int direccion = 1;
    private float nextAttackTime = 0f;
    public float Vida = 300f;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        transform.SetParent(null);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (rb.sharedMaterial == null)
        {
            PhysicsMaterial2D mat = new PhysicsMaterial2D("BossMat");
            mat.friction = 0f;
            rb.sharedMaterial = mat;
        }
    }

    void FixedUpdate()
    {
        if (player == null || Vida <= 0) return;

        // Calculamos distancias en ambos ejes
        float diffX = player.position.x - transform.position.x;
        float diffY = Mathf.Abs(player.position.y - transform.position.y); // Diferencia de altura

        // Distancia real (en diagonal)
        float distanciaReal = Vector2.Distance(transform.position, player.position);

        bool haySuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);

        // 1. ¿ESTÁ EN RANGO DE VISIÓN Y EN EL MISMO PISO?
        if (distanciaReal <= distanciaVision && diffY <= limiteAltura)
        {
            // Girar
            if (diffX > 0.2f && direccion == -1) Voltear();
            else if (diffX < -0.2f && direccion == 1) Voltear();

            // 2. ¿ESTÁ EN RANGO DE ATAQUE?
            if (distanciaReal <= attackRange)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                if (Time.time >= nextAttackTime)
                {
                    if (animator != null) animator.SetTrigger("AttackFB");
                    HacerDanoAlPlayer();
                    nextAttackTime = Time.time + 1.5f;
                }
            }
            // 3. PERSEGUIR (Solo si hay suelo adelante)
            else if (haySuelo)
            {
                rb.linearVelocity = new Vector2(velocidad * direccion, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else
        {
            // Si el jugador está muy arriba, muy abajo o muy lejos, el jefe se queda quieto
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (animator != null) animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void Voltear()
    {
        direccion *= -1;
        Vector3 escala = transform.localScale;
        escala.x = 1.5215f * -direccion;
        transform.localScale = escala;
    }

    void HacerDanoAlPlayer()
    {
        // Doble verificación: solo hace daño si al momento del golpe sigues cerca
        float distanciaAlGolpe = Vector2.Distance(transform.position, player.position);
        float diffY = Mathf.Abs(player.position.y - transform.position.y);

        if (distanciaAlGolpe <= attackRange + 0.5f && diffY <= limiteAltura)
        {
            player.SendMessage("RecibirDano", danoQueHace, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Jefe atacando al jugador en el mismo piso");
        }
    }

    public void TomarDano(float d)
    {
        Vida -= d;
        if (Vida <= 0)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetTrigger("DeathFB");
            Destroy(gameObject, 1.5f);
        }
    }
}