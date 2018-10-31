using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedmiBoxes : MonoBehaviour {
	public GameObject orderBox;
	public void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			Globals.redmiQuantityLocal-=1;
			Globals.redmiInbox+=1;
			orderBox.SetActive(true);
			//Debug.Log("Left Click");
		}else if(Input.GetMouseButtonDown(1)){
			if(Globals.redmiQuantityLocal<10){
				Globals.redmiInbox+=Globals.redmiQuantityLocal;
				Globals.redmiQuantityLocal = 0;	
			}else{
				Globals.redmiQuantityLocal-=10;
				Globals.redmiInbox+=10;
			}
			
			orderBox.SetActive(true);
			//Debug.Log("Right Click");
		}else if(Input.GetMouseButtonDown(2)){
			//Debug.Log("Middle Click");
		}
		//Debug.Log(Globals.mi5Quantity.ToString());
	}
	
}
