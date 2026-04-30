using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaNivel1 : MonoBehaviour
{
	public string escenaGanar = "NivelCompletado";

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Buscamos si tiene el componente Movimiento en cualquier parte del objeto
		if (other.GetComponent<Movimiento2>() != null || other.GetComponentInParent<Movimiento2>() != null)
		{
			Debug.Log("<color=green>¡Meta detectada! Forzando carga de escena: " + escenaGanar + "</color>");
			
			// Forzamos el tiempo a 1 por si acaso
			Time.timeScale = 1f; 
			
			// Cargamos la escena
			SceneManager.LoadScene(escenaGanar);
		}
		else
		{
			Debug.Log("Trigger detectó algo, pero no es el jugador: " + other.name);
		}
	}
}