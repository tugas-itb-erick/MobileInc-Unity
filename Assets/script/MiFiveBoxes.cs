using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiFiveBoxes : MonoBehaviour {
	public GameObject orderBox;
	public GameObject prefab;
	public void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			Globals.mi5QuantityLocal-=1;
			Globals.mi5Inbox+=1;
			orderBox.SetActive(true);
			//Debug.Log("Left Click");
		}else if(Input.GetMouseButtonDown(1)){
			if(Globals.mi5QuantityLocal<10){
				Globals.mi5Inbox+=Globals.mi5QuantityLocal;	
				Globals.mi5QuantityLocal = 0;
			}else{
				Globals.mi5QuantityLocal-=10;
				Globals.mi5Inbox+=10;
			}
			
			orderBox.SetActive(true);
			//Debug.Log("Right Click");
		}else if(Input.GetMouseButtonDown(2)){
			//Debug.Log("Middle Click");
		}
		//Debug.Log(Globals.mi5Quantity.ToString());
	}
}
