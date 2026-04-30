using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    public Transform jugador;
    public float speed = 10.0f;
    public float offsetY = 2.8f;
    public float offsetX = 3.79f;

    [Header("Límites del Mapa")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Efecto parallax
    public Transform[] fondos;
    public float[] velocidadParallax;

    private Vector3 posAnteriorCamara;

    void Start() // Corregido 'start' a 'Start' con mayúscula
    {
        posAnteriorCamara = transform.position;
    }

    void Update()
    {
        if (jugador == null) return;

        // 1. Calculamos el destino ideal
        float destinoX = jugador.position.x + offsetX;
        float destinoY = jugador.position.y + offsetY;

        // 2. RESTRINGIMOS ese destino para que no se salga de los límites
        destinoX = Mathf.Clamp(destinoX, minX, maxX);
        destinoY = Mathf.Clamp(destinoY, minY, maxY);

        // 3. Creamos el vector de destino final limitado
        Vector3 destinoLimitado = new Vector3(destinoX, destinoY, transform.position.z);

        // 4. Movemos la cámara
        transform.position = Vector3.Lerp(transform.position, destinoLimitado, speed * Time.deltaTime);

        // --- Lógica de Parallax (se mantiene igual) ---
        Vector3 deltaCamara = transform.position - posAnteriorCamara;
        for (int i = 0; i < fondos.Length; i++)
        {
            float factor = velocidadParallax[i];
            fondos[i].position += new Vector3(deltaCamara.x * factor, deltaCamara.y * factor, 0);
        }
        posAnteriorCamara = transform.position;
    }
}