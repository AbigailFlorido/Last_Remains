using UnityEngine;

public class CamaraSeguir3 : MonoBehaviour
{
    public Transform jugador;
    public float speed = 10.0f;
    public float offsetY = 2.8f;
    public float offsetX = 3.79f;
    public float zoom = 3f;

    [Header("Límites del Mapa")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public Transform[] fondos;
    public float[] velocidadParallax;

    private Vector3 posAnteriorCamara;

    void Start()
    {
        posAnteriorCamara = transform.position;
        Camera.main.orthographicSize = zoom;
    }

    void Update()
    {
        if (jugador == null) return;

        // 1. Calculamos el destino ideal basándonos en el jugador
        float targetX = jugador.position.x + offsetX;
        float targetY = jugador.position.y + offsetY;

        // --- LAS LÍNEAS QUE FALTABAN: APLICAR LOS LÍMITES ---
        targetX = Mathf.Clamp(targetX, minX, maxX);
        targetY = Mathf.Clamp(targetY, minY, maxY);
        // ----------------------------------------------------

        // 2. Creamos el vector de destino ya limitado
        Vector3 destino = new Vector3(targetX, targetY, transform.position.z);

        // 3. Movemos la cámara con suavizado
        transform.position = Vector3.Lerp(transform.position, destino, speed * Time.deltaTime);

        // Parallax
        Vector3 deltaCamara = transform.position - posAnteriorCamara;
        for (int i = 0; i < fondos.Length; i++)
        {
            float factor = velocidadParallax[i];
            fondos[i].position += new Vector3(deltaCamara.x * factor, deltaCamara.y * factor, 0);
        }
        posAnteriorCamara = transform.position;
    }
}