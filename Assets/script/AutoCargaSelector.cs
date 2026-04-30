using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoCargaSelector : MonoBehaviour
{
	public float tiempoEspera = 4f; // Tiempo para que vean tu imagen de victoria
	public string escenaSelector = "SelectorNiveles";

	void Start()
	{
		StartCoroutine(EsperarYIrAlSelector());
	}

	IEnumerator EsperarYIrAlSelector()
	{
		yield return new WaitForSeconds(tiempoEspera);
		SceneManager.LoadScene(escenaSelector);
	}
}