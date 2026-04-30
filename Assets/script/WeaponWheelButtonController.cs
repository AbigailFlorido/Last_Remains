using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelButtonController : MonoBehaviour
{
	public int	ID;
	private Animator anim;
	public string itemName;
	public Image selectedItem;
	private bool selected = false;
	public Sprite icon;
	
	
    void Start()
    {
	    anim = GetComponent<Animator>();
    }

   
    void Update()
    {
	    if (selected)
	    {
	    	selectedItem.sprite = icon;
	    }
    }
    
	public void Selected()
	{
		selected = true;
		WeaponWheelController.weaponID = ID;
	}
	
	public void deselected()
	{
		selected = false;
		WeaponWheelController.weaponID = 0;
	}
	
	public void HoverEnter()
	{
		anim.SetBool("Hover", true);
	}
	
	public void HoverExit()
	{
		anim.SetBool("Hover", false);
	}
}
