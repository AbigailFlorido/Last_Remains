using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    public Transform jugador;
    public float speed = 10.0f;
    public float offsetY = 2.08f;
    void Update()
    {
        Vector3 destino = new Vector3 (jugador.position.x, jugador.position.y + offsetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destino, speed * Time.deltaTime);
    }
}
