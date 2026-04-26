using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private Movimiento movimiento;
    private float vidaMaxima;
    
    void Start()
    {
        movimiento = GameObject.Find("Player").GetComponent<Movimiento>();
        vidaMaxima = movimiento.vidas;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount = movimiento.vidas / vidaMaxima;
    }
}
