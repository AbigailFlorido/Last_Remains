using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarraVida3 : MonoBehaviour
{
    public Image rellenoBarraVida;
	private Movimiento3 movimiento;
    private float vidaMaxima;
    
    void Start()
    {
	    movimiento = GameObject.Find("Player").GetComponent<Movimiento3>();
        vidaMaxima = movimiento.vidas;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount = movimiento.vidas / vidaMaxima;
    }
}
