using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	private static MusicManager _instance;
	public static MusicManager instance 
	{
		get 
		{
			if (_instance == null)
			{
				// Si no existe, buscamos uno en la escena
				_instance = FindFirstObjectByType<MusicManager>();

				// Si aún no existe, creamos uno nuevo vacío
				if (_instance == null)
				{
					GameObject singleton = new GameObject("MusicManager");
					_instance = singleton.AddComponent<MusicManager>();
					DontDestroyOnLoad(singleton);
				}
			}
			return _instance;
		}
	}

	private AudioSource audioSource;

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(gameObject);
		audioSource = GetComponent<AudioSource>();
	}

	public void CambiarMusica(AudioClip nuevaMusica)
	{
		if (audioSource == null) audioSource = GetComponent<AudioSource>();
		if (audioSource.clip == nuevaMusica) return;

		audioSource.clip = nuevaMusica;
		audioSource.loop = true;
		audioSource.Play();
	}
}