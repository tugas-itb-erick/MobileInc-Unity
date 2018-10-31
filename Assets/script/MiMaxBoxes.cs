using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiMaxBoxes : MonoBehaviour {
	public GameObject orderBox;
	public void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			Globals.mimaxQuantityLocal-=1;
			Globals.mimaxInbox+=1;
			orderBox.SetActive(true);
			//Debug.Log("Left Click");
		}else if(Input.GetMouseButtonDown(1)){
			if(Globals.mimaxQuantityLocal<10){
				Globals.mimaxInbox+=Globals.mimaxQuantityLocal;	
				Globals.mimaxQuantityLocal = 0;
			}else{
				Globals.mimaxQuantityLocal-=10;
				Globals.mimaxInbox+=10;
			}
			orderBox.SetActive(true);
			//Debug.Log("Right Click");
		}else if(Input.GetMouseButtonDown(2)){
			//Debug.Log("Middle Click");
		}
		//Debug.Log(Globals.mi5Quantity.ToString());
	}
}
