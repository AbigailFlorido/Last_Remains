using UnityEngine;

public class CambiadorMusica : MonoBehaviour
{
    public AudioClip musicaParaEsteNivel;

    void Start()
    {
        // Buscamos al MusicManager que sobrevivió desde el menú
        if (MusicManager.instance != null)
        {
            MusicManager.instance.CambiarMusica(musicaParaEsteNivel);
        }
    }
}