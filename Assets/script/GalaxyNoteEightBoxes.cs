using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyNoteEightBoxes : MonoBehaviour {
	public GameObject orderBox;
	public void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			Globals.galaxynote8QuantityLocal-=1;
			Globals.galaxynote8Inbox+=1;
			orderBox.SetActive(true);
			//Debug.Log("Left Click");
		}else if(Input.GetMouseButtonDown(1)){
			if(Globals.galaxynote8QuantityLocal<10){
				Globals.galaxynote8Inbox+=Globals.galaxynote8QuantityLocal;
				Globals.galaxynote8QuantityLocal = 0;	
			}else{
				Globals.galaxynote8QuantityLocal-=10;
				Globals.galaxynote8Inbox+=10;
			}
			
			orderBox.SetActive(true);
			//Debug.Log("Right Click");
		}else if(Input.GetMouseButtonDown(2)){
			//Debug.Log("Middle Click");
		}
		//Debug.Log(Globals.mi5Quantity.ToString());
	}
}
