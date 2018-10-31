using System;
using System.Data.SQLite;
using UnityEngine;

public class StorageController : MonoBehaviour {

	public int mi5Quantity;
	public int mimaxQuantity;
	public int redmiQuantity;
	public int galaxynote5Quantity;
	public int galaxynote8Quantity;
	public int galaxys8Quantity;

	public GameObject mi5;
	public GameObject mimax;
	public GameObject redmi;
	public GameObject galaxynote5;
	public GameObject galaxynote8;
	public GameObject galaxys8;

	public GameObject mi51;
	public GameObject mi52;
	public GameObject mi53;
	public GameObject mi54;
	public GameObject mi55;

	public GameObject mimax1;
	public GameObject mimax2;
	public GameObject mimax3;
	public GameObject mimax4;
	public GameObject mimax5;

	public GameObject redmi1;
	public GameObject redmi2;
	public GameObject redmi3;
	public GameObject redmi4;
	public GameObject redmi5;
	
	public GameObject galaxynote51;
	public GameObject galaxynote52;
	public GameObject galaxynote53;
	public GameObject galaxynote54;
	public GameObject galaxynote55;

	public GameObject galaxynote81;
	public GameObject galaxynote82;
	public GameObject galaxynote83;
	public GameObject galaxynote84;
	public GameObject galaxynote85;

	public GameObject galaxys81;
	public GameObject galaxys82;
	public GameObject galaxys83;
	public GameObject galaxys84;
	public GameObject galaxys85;
	
	public GameObject orderMi5;
	public GameObject orderMiMax;
	public GameObject orderRedmi;
	public GameObject orderGalaxyNote5;
	public GameObject orderGalaxyNote8;
	public GameObject orderGalaxyS8;
	
	public GameObject itemsQuantityText;

	public GameObject packetBox;
	public Animator boxAnimLeft;
	public Animator boxAnimRight;

    // Use this for initialization
	void Start () {
		//STUB, aslinya ambil dari variabel global (diinit di Game.cs)
		Globals.mi5QuantityLocal = Globals.mi5Quantity;
		Globals.mimaxQuantityLocal = Globals.mimaxQuantity;
		Globals.redmiQuantityLocal = Globals.redmiQuantity;
		Globals.galaxynote5QuantityLocal = Globals.galaxynote5Quantity;
		Globals.galaxynote8QuantityLocal = Globals.galaxynote8Quantity;
		Globals.galaxys8QuantityLocal = Globals.galaxys8Quantity;

		Globals.mi5Inbox = 0;
		Globals.mimaxInbox = 0;
		Globals.redmiInbox = 0;
		Globals.galaxynote5Inbox = 0;
		Globals.galaxynote8Inbox = 0;
		Globals.galaxys8Inbox = 0;

		boxAnimLeft.enabled = false;
		boxAnimRight.enabled = false;

		orderMi5.SetActive(false);
		orderMiMax.SetActive(false);
		orderRedmi.SetActive(false);
		orderGalaxyNote5.SetActive(false);
		orderGalaxyNote8.SetActive(false);
		orderGalaxyS8.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		mi5Quantity = Globals.mi5QuantityLocal;
		mimaxQuantity = Globals.mimaxQuantityLocal;
		redmiQuantity = Globals.redmiQuantityLocal;
		galaxynote5Quantity = Globals.galaxynote5QuantityLocal;
		galaxynote8Quantity = Globals.galaxynote8QuantityLocal;
		galaxys8Quantity = Globals.galaxys8QuantityLocal;
		//mi5 Quantity
		if(mi5Quantity > 160){
			//do nothing
		}else if(mi5Quantity >120){
			mi51.SetActive(false);
		}else if(mi5Quantity >80){
			mi51.SetActive(false);
			mi52.SetActive(false);
		}else if(mi5Quantity >40){
			mi51.SetActive(false);
			mi52.SetActive(false);
			mi53.SetActive(false);
		}else if(mi5Quantity >0){
			mi51.SetActive(false);
			mi52.SetActive(false);
			mi53.SetActive(false);
			mi54.SetActive(false);
		}else {
			mi51.SetActive(false);
			mi52.SetActive(false);
			mi53.SetActive(false);
			mi54.SetActive(false);
			mi55.SetActive(false);
		}

		//mimax Quantity
		if(mimaxQuantity > 160){
			//do nothing
		}else if(mimaxQuantity >120){
			mimax1.SetActive(false);
		}else if(mimaxQuantity >80){
			mimax1.SetActive(false);
			mimax2.SetActive(false);
		}else if(mimaxQuantity >40){
			mimax1.SetActive(false);
			mimax2.SetActive(false);
			mimax3.SetActive(false);
		}else if(mimaxQuantity >0){
			mimax1.SetActive(false);
			mimax2.SetActive(false);
			mimax3.SetActive(false);
			mimax4.SetActive(false);
		}else {
			mimax1.SetActive(false);
			mimax2.SetActive(false);
			mimax3.SetActive(false);
			mimax4.SetActive(false);
			mimax5.SetActive(false);
		}

		//redmi Quantity
		if(redmiQuantity > 160){
			//do nothing
		}else if(redmiQuantity >120){
			redmi1.SetActive(false);
		}else if(redmiQuantity >80){
			redmi1.SetActive(false);
			redmi2.SetActive(false);
		}else if(redmiQuantity >40){
			redmi1.SetActive(false);
			redmi2.SetActive(false);
			redmi3.SetActive(false);
		}else if(redmiQuantity >0){
			redmi1.SetActive(false);
			redmi2.SetActive(false);
			redmi3.SetActive(false);
			redmi4.SetActive(false);
		}else {
			redmi1.SetActive(false);
			redmi2.SetActive(false);
			redmi3.SetActive(false);
			redmi4.SetActive(false);
			redmi5.SetActive(false);
		}

		//galaxynote5 Quantity
		if(galaxynote5Quantity > 160){
			//do nothing
		}else if(galaxynote5Quantity >120){
			galaxynote51.SetActive(false);
		}else if(galaxynote5Quantity >80){
			galaxynote51.SetActive(false);
			galaxynote52.SetActive(false);
		}else if(galaxynote5Quantity >40){
			galaxynote51.SetActive(false);
			galaxynote52.SetActive(false);
			galaxynote53.SetActive(false);
		}else if(galaxynote5Quantity >0){
			galaxynote51.SetActive(false);
			galaxynote52.SetActive(false);
			galaxynote53.SetActive(false);
			galaxynote54.SetActive(false);
		}else {
			galaxynote51.SetActive(false);
			galaxynote52.SetActive(false);
			galaxynote53.SetActive(false);
			galaxynote54.SetActive(false);
			galaxynote55.SetActive(false);
		}

		//galaxynote8 Quantity
		if(galaxynote8Quantity > 160){
			//do nothing
		}else if(galaxynote8Quantity >120){
			galaxynote81.SetActive(false);
		}else if(galaxynote8Quantity >80){
			galaxynote81.SetActive(false);
			galaxynote82.SetActive(false);
		}else if(galaxynote8Quantity >40){
			galaxynote81.SetActive(false);
			galaxynote82.SetActive(false);
			galaxynote83.SetActive(false);
		}else if(galaxynote8Quantity >0){
			galaxynote81.SetActive(false);
			galaxynote82.SetActive(false);
			galaxynote83.SetActive(false);
			galaxynote84.SetActive(false);
		}else {
			galaxynote81.SetActive(false);
			galaxynote82.SetActive(false);
			galaxynote83.SetActive(false);
			galaxynote84.SetActive(false);
			galaxynote85.SetActive(false);
		}

		//galaxys8 Quantity
		if(galaxys8Quantity > 160){
			//do nothing
		}else if(galaxys8Quantity >120){
			galaxys81.SetActive(false);
		}else if(galaxys8Quantity >80){
			galaxys81.SetActive(false);
			galaxys82.SetActive(false);
		}else if(galaxys8Quantity >40){
			galaxys81.SetActive(false);
			galaxys82.SetActive(false);
			galaxys83.SetActive(false);
		}else if(galaxys8Quantity >0){
			galaxys81.SetActive(false);
			galaxys82.SetActive(false);
			galaxys83.SetActive(false);
			galaxys84.SetActive(false);
		}else {
			galaxys81.SetActive(false);
			galaxys82.SetActive(false);
			galaxys83.SetActive(false);
			galaxys84.SetActive(false);
			galaxys85.SetActive(false);
		}
	}
}
