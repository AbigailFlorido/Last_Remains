using UnityEngine;
using UnityEngine.UI;

public class BarraStamina : MonoBehaviour
{
    public Image barra;
    public Movimiento movimiento;

    void Update()
    {
        barra.fillAmount = movimiento.staminaActual / movimiento.staminaMax;
    }
}
