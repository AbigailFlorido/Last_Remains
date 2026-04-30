using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip musicaFondo;
    [Range(0f, 1f)]
    public float volumen = 0.3f;
    private AudioSource fuenteDeAudio;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        fuenteDeAudio = GetComponent<AudioSource>();
        if(fuenteDeAudio == null)
        {
            fuenteDeAudio = gameObject.AddComponent<AudioSource>();
        }
        fuenteDeAudio.clip = musicaFondo;
        fuenteDeAudio.volume = volumen;
        fuenteDeAudio.loop = true;
        fuenteDeAudio.playOnAwake = false;
        fuenteDeAudio.Play();

    }



    public void CambiarVolumen (float nuevoVolumen)
    {
        volumen = nuevoVolumen;
        fuenteDeAudio.volume = volumen;
    }

    public void CambiarMusica(AudioClip nuevaMusica)
    {
        // Si no hay música nueva o es la misma que ya suena, no hacemos nada
        if (nuevaMusica == null || fuenteDeAudio.clip == nuevaMusica) return;

        fuenteDeAudio.clip = nuevaMusica;
        fuenteDeAudio.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
