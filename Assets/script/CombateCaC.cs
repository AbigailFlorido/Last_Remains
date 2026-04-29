using UnityEngine;

public class CombateCaC : MonoBehaviour
{
	[SerializeField] private Transform controladorGolpe;
	[SerializeField] private float radioGolpe;
	[SerializeField] private float danoGolpe;
    
	private Animator animator; // Para controlar las animaciones

	private void Start()
	{
		// Obtenemos el Animator que está en el personaje
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		// Usamos Fire2 o una tecla distinta si quieres separar el disparo del hachazo
		if (Input.GetButtonDown("Fire1")) 
		{
			Golpe();
		}
	}
    
	private void Golpe()
	{
		// 1. ACTIVAR ANIMACIÓN
		// Asegúrate de que el nombre entre comillas sea EXACTO al de tu animación
		animator.SetTrigger("Ataque");

		// 2. DETECTAR ENEMIGOS
		Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        
		foreach (Collider2D colisionador in objetos)
		{
			if (colisionador.CompareTag("Enemy"))
			{
				// Usamos GetComponentInParent por si le pegas a la cabeza o un hijo
				Enemigo enemigo = colisionador.GetComponentInParent<Enemigo>();
				if (enemigo != null)
				{
					enemigo.TomarDano(danoGolpe);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (controladorGolpe != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
		}
	}
}