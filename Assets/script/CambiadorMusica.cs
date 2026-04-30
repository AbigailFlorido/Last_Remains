using UnityEngine;

public class CambiadorMusica : MonoBehaviour
{
	public AudioClip musicaParaEsteNivel;

	void Start()
	{
		// Buscamos al MusicManager que sobrevive
		if (MusicManager.instance != null)
		{
			MusicManager.instance.CambiarMusica(musicaParaEsteNivel);
		}
		else
		{
			Debug.LogWarning("No se encontró el MusicManager. ¿Está el objeto en la escena inicial?");
		}
	}
}