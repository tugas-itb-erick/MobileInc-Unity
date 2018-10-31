using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySEightBoxes : MonoBehaviour {
	public GameObject orderBox;
	public void OnMouseOver(){
		if(Input.GetMouseButtonDown(0)){
			Globals.galaxys8QuantityLocal-=1;
			Globals.galaxys8Inbox+=1;
			orderBox.SetActive(true);
			//Debug.Log("Left Click");
		}else if(Input.GetMouseButtonDown(1)){
			if(Globals.galaxys8QuantityLocal<10){
				Globals.galaxys8Inbox+=Globals.galaxys8QuantityLocal;
				Globals.galaxys8QuantityLocal = 0;
			}else{
				Globals.galaxys8QuantityLocal-=10;
				Globals.galaxys8Inbox+=10;
			}
			
			orderBox.SetActive(true);
			//Debug.Log("Right Click");
		}else if(Input.GetMouseButtonDown(2)){
			//Debug.Log("Middle Click");
		}
		//Debug.Log(Globals.mi5Quantity.ToString());
	}
}
