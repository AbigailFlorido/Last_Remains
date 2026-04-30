using UnityEngine;

public class Bala3 : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float dano = 1f;
    [SerializeField] private float tiempoDeVida = 3f; // Para que no viajen infinitamente

    private void Start()
    {
        // Destruir la bala después de X segundos si no golpea nada
        Destroy(gameObject, tiempoDeVida);
    }

    private void Update()
    {
        // Mueve la bala hacia adelante relativo a su rotación
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Verificamos si chocamos con algo que tenga el Tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // 2. Intentamos obtener el script FinalBoss
            FinalBoss scriptBoss = other.GetComponent<FinalBoss>();

            // Si no lo encuentra en este objeto, lo busca en el padre (por si el collider está en un hijo)
            if (scriptBoss == null) scriptBoss = other.GetComponentInParent<FinalBoss>();

            if (scriptBoss != null)
            {
                scriptBoss.TomarDano(dano);
                Debug.Log("¡Impacto confirmado en el Jefe!");
                Destroy(gameObject);
            }
            else
            {
                // Aquí podrías agregar lógica para otros enemigos comunes si tienen scripts distintos
                Debug.LogWarning("Golpeé a " + other.name + " pero no tiene el script FinalBoss");
            }
        }

        // Opcional: Destruir la bala si toca el suelo/paredes
        if (other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            Destroy(gameObject);
        }
    }
}