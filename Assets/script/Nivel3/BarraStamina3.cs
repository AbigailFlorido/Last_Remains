using UnityEngine;
using UnityEngine.UI;

public class BarraStamina3 : MonoBehaviour
{
	public Image barra;
	public Movimiento3 movimiento;

	void Update()
	{
		barra.fillAmount = movimiento.staminaActual / movimiento.staminaMax;
	}
}
