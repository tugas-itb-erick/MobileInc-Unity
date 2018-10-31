using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketBoxController : MonoBehaviour {

	public GameObject mi5;
	public GameObject mimax;
	public GameObject redmi;
	public GameObject galaxynote5;
	public GameObject galaxynote8;
	public GameObject galaxys8;

	private Vector3 screenPoint;
 	private Vector3 offset;
	private Vector3 scanPos;

	public Animator boxAnimLeft;
	public Animator boxAnimRight;

	public bool isOrderTrue;
	public bool isValidated;
	void Start(){
		isOrderTrue = false;
		isValidated = false;
	}

	// Update is called once per frame
	void Update () {
		
	}

	 void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(transform.position);
		offset =  transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,screenPoint.z));
	
		boxAnimLeft.enabled = true;
		boxAnimRight.enabled = true;
		
		StartCoroutine(LateCall());
	}

	void OnMouseDrag(){
		Vector3 curScreenPoint;
		Vector3 curPosition;
		
		curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	
	}

	IEnumerator LateCall()
     {
        yield return new WaitForSeconds(3);
 
		mi5.SetActive(false);
		mimax.SetActive(false);
		redmi.SetActive(false);
		galaxynote5.SetActive(false);
		galaxynote8.SetActive(false);
		galaxys8.SetActive(false);
     }

	void OnTriggerEnter(Collider other){
		//Destroy(other.gameObject); --> hapus objek
		if(other.gameObject.CompareTag("SendPacket")&& !isValidated && Globals.nextOrderTime!=-1){
			ValidateOrder();
			if(isOrderTrue){
				Debug.Log("Order Sesuai");
				Game.trust += 1;
			}else{
				Debug.Log("Order Tidak Sesuai");
				Game.trust -= 1;
			}
			isValidated = true;

			Globals.mi5Quantity = Globals.mi5QuantityLocal;
			Globals.mimaxQuantity = Globals.mimaxQuantityLocal;
			Globals.redmiQuantity = Globals.redmiQuantityLocal;
			Globals.galaxynote5Quantity = Globals.galaxynote5QuantityLocal;
			Globals.galaxynote8Quantity = Globals.galaxynote8QuantityLocal;
			Globals.galaxys8Quantity = Globals.galaxys8QuantityLocal;
		}
	}

	//cocokin sama next order
	public void ValidateOrder(){
        int current_time = (Game.day - 1) * 600 + Game.time;
		isOrderTrue = (Globals.mi5Order <= Globals.mi5Inbox && Globals.mimaxOrder <= Globals.mimaxInbox && Globals.redmiOrder <= Globals.redmiInbox && Globals.galaxynote5Order <= Globals.galaxynote5Inbox && Globals.galaxynote8Order <= Globals.galaxynote8Inbox && Globals.galaxys8Order <= Globals.galaxys8Inbox && current_time <= Globals.nextOrderTime);

        Debug.Log("Order id : " + Globals.order_id.ToString());

        if(!isOrderTrue)
        {
            Game.money -= 500;
            Game.popularity -= 5;
            Game.trust--;
        }
        else
        {
            Game.popularity += 2;
            Game.trust += 2;
        }

        string query = "DELETE FROM orders WHERE id = " + Globals.order_id.ToString();
        SQLiteDatabase.UpdateQuery(query);
	}
}
