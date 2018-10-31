using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyNoteFiveBoxes : MonoBehaviour {
	public GameObject orderBox;
	public void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			Globals.galaxynote5QuantityLocal-=1;
			Globals.galaxynote5Inbox+=1;
			orderBox.SetActive(true);
			//Debug.Log("Left Click");
		}else if(Input.GetMouseButtonDown(1)){
			if(Globals.galaxynote5QuantityLocal<10){
				Globals.galaxynote5Inbox+=Globals.galaxynote5QuantityLocal;	
				Globals.galaxynote5QuantityLocal = 0;
			}else{
				Globals.galaxynote5QuantityLocal-=10;
				Globals.galaxynote5Inbox+=10;
			}
			
			orderBox.SetActive(true);
			//Debug.Log("Right Click");
		}else if(Input.GetMouseButtonDown(2)){
			//Debug.Log("Middle Click");
		}
		//Debug.Log(Globals.mi5Quantity.ToString());
	}
}
