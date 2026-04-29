using UnityEngine;


public class CamaraSeguir00 : MonoBehaviour
{
    public Transform jugador;
    public float speed = 10.0f;
	public float offsetY = 2.8f;
	public float offsetX = 3.79f;
    
	//Efecto parallax
	//Arrastramos los fondos que tengamos
	public	Transform[] fondos;
	//valores entre 0 y 1 ( 0 es estatico y 1 se muevve igual que la camara)
	public float[] velocidadParallax;
	
	private	Vector3 posAnteriorCamara;
	
	void start()
	{
		posAnteriorCamara = transform.position;
	}
    void Update()
	{
		//Sigue al jugador
		Vector3 destino = new Vector3 (jugador.position.x + offsetX, jugador.position.y + offsetY, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, destino, speed * Time.deltaTime);
		//Parallax
		Vector3 deltaCamara = transform.position - posAnteriorCamara;
		for (int i = 0; i < fondos.Length; i++) 
		{
			float factor = velocidadParallax[i];
			fondos[i].position += new Vector3(deltaCamara.x * factor, deltaCamara.y * factor, 0);
		}
		posAnteriorCamara = transform.position;
    }
}
