using UnityEngine;
using UnityEngine.UI;

public class BarraStamina2 : MonoBehaviour
{
    public Image barra;
	public Movimiento2 movimiento;

    void Update()
    {
        barra.fillAmount = movimiento.staminaActual / movimiento.staminaMax;
    }
}
