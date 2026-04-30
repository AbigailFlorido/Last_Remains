using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
	
	public Animator anim;
	private bool weaponWheelSelected = false;
	public Image selectedItem;
	public Sprite noImage;
	public static int weaponID;
	
    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.Tab))
	    {
	    	weaponWheelSelected = !weaponWheelSelected;
	    }
	    
	    if (weaponWheelSelected)
	    {
	    	anim.SetBool("OpenWeaponWheel", true);
	    }
	    
	    else
	    {
	    	anim.SetBool("OpenWeaponWheel", false);
	    }
	    
	    switch(weaponID)
	    {
	    	case 1: //hacha
		    	Debug.Log("Hacha en rueda");
		    	break;
	    	case 2: //pistola
		    	Debug.Log("pistola");
		    	break;
	    	case 3: //lanzallamas
		    	Debug.Log("lanzallamas");
		    	break;
	    }
    }
}
