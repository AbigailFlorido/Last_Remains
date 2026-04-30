using UnityEngine;

public class Bala2 : MonoBehaviour
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
        // 1. Verificamos si chocamos con algo que tenga el Tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // 2. BUSCAMOS EL SCRIPT DEL PERRO (Enemigo2)
            Enemigo2 scriptPerro = other.GetComponent<Enemigo2>();

            // Si no lo encuentra en el objeto, busca en el padre
            if (scriptPerro == null) scriptPerro = other.GetComponentInParent<Enemigo2>();

            if (scriptPerro != null)
            {
                scriptPerro.TomarDano(dano); // Llama a la función del perro
                Debug.Log("¡Impacto en el perro confirmado!");
                Destroy(gameObject);
            }
            else
            {
                // Por si golpeas al Jefe Final u otro enemigo con script distinto
                other.SendMessage("TomarDano", dano, SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);
            }
        }

        // Destruir la bala si toca el suelo
        if (other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            Destroy(gameObject);
        }
    }
}