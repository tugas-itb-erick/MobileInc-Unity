using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Data.SQLite;
using System.Net.Http;
using System.Collections.Specialized;

public class UIManager : MonoBehaviour {

    public Game GM;
    public GameObject itemsQuantityText;
    public Text mi5QuantityText;
    public Text mimaxQuantityText;
    public Text redmiQuantityText;
    public Text galaxynote5QuantityText;
    public Text galaxynote8QuantityText;
    public Text galaxys8QuantityText;
    public GameObject itemsInBoxText;
    public Text mi5InBoxText;
    public Text mimaxInBoxText;
    public Text redmiInBoxText;
    public Text galaxynote5InBoxText;
    public Text galaxynote8InBoxText;
    public Text galaxys8InBoxText;
    public GameObject nextOrder;
    public Text mi5Order;
    public Text mimaxOrder;
    public Text redmiOrder;
    public Text galaxynote5Order;
    public Text galaxynote8Order;
    public Text galaxys8Order;
    public GameObject day;
    public GameObject happiness;
    public GameObject money;
    public GameObject popularity;
    public GameObject trust;
    public Light sun;

    public GameObject promo;
    public Button promoButton;
    public Button shop, storage, supply;
    public GameObject shopPanel;
    public Button[] menus;

    public GameObject infoPanel;
    public GameObject supplyPanel;
    public Button[] supplies;

    public GameObject pausePanel;

    public GameObject newsPanel;

    public Camera camera;

    private int tima;
    private bool weatherState;
    void Awake ()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start () {
        weatherState = true;
        InvokeRepeating("UpdateNews", 0f, 15.0f);
        InvokeRepeating("RemoveNews", 0f, 1.0f);

        promo.SetActive(true);
        itemsQuantityText.SetActive(false);
        itemsInBoxText.SetActive(false);
        promoButton.onClick.AddListener(PromoOnClick);
        shop.onClick.AddListener(ShopOnClick);
        storage.onClick.AddListener(StorageOnClick);
        shopPanel.SetActive(false);
        menus = shopPanel.GetComponentsInChildren<Button>();
        infoPanel.SetActive(false);
        infoPanel.GetComponentInChildren<Button>().onClick.AddListener(HireOnClick);
        supply.onClick.AddListener(SupplyOnClick);
        supplyPanel.SetActive(false);
        supplies = supplyPanel.GetComponentsInChildren<Button>();
        newsPanel.SetActive(false);

        pausePanel.SetActive(false);

        SQLiteDatabase.OpenConnection();
        Globals.client = new HttpClient( new HttpClientHandler { UseProxy = false });
        Globals.use_promo = 0;

        for (int i=0; i<12; i++)
        {
            int local = i;
            menus[local].onClick.AddListener(delegate { OnClickItem(local); });
        }

        string query = "SELECT * FROM supplier";

        try {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            for (int i = 0; i < 18; i++)
            {
                int local = i;

                reader.Read();
                supplies[local].GetComponentsInChildren<Text>()[1].text = reader["min_order"].ToString();
                supplies[local].GetComponentsInChildren<Text>()[2].text = reader["price"].ToString();

                supplies[local].onClick.AddListener(delegate { OnSupplyBuy(local); });
            }
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }
	
	// Update is called once per frame
	void Update () {
        day.GetComponentsInChildren<Text>()[0].text = "Day " + Game.day.ToString(); // day
        day.GetComponentsInChildren<Text>()[1].text = TimeToString(Game.time); // time
        sun.transform.eulerAngles = new Vector3((float)(Game.time - Game.INITIAL_TIME)*0.6f, 0.0f, 0.0f); // rotate sun
        if (GM.weatherValue == -1)
        {
            sun.intensity = 0.2f;
        }
        else if(GM.weatherValue == 0)
        {
            sun.intensity = 0.015f;
            if(weatherState){
                Game.trust = Game.trust - 1;
                weatherState = false;
            }
        }
        else if (GM.weatherValue == 1)
        {
            sun.intensity = 1f;
            if(!weatherState){
                Game.trust = Game.trust + 1;
                weatherState = true;
            }
        }

        happiness.GetComponentInChildren<Text>().text = Game.happiness.ToString() + "%";
        money.GetComponentInChildren<Text>().text = Game.money.ToString();
        popularity.GetComponentInChildren<Text>().text = Game.popularity.ToString();
        trust.GetComponentInChildren<Text>().text = Game.trust.ToString();
        SetItemInBoxCount();
        ShowNextOrder();
        MouseClick();
        WinLose();
    }   

    public void ShowNextOrder()
    {
        string query = "SELECT id, phone_type, quantity, due_time FROM orders WHERE player_id = " + Globals.player_id.ToString() + " ORDER BY due_time ASC";
        SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
        SQLiteDataReader reader = command.ExecuteReader();

        try
        {
            if(reader.HasRows)
            {
                reader.Read();
                string type = reader["phone_type"].ToString();

                Globals.mi5Order = 0;
                Globals.mimaxOrder = 0;
                Globals.redmiOrder = 0;
                Globals.galaxynote5Order = 0;
                Globals.galaxynote8Order = 0;
                Globals.galaxys8Order = 0;

                if (type == "Mi 5") Globals.mi5Order = Convert.ToInt32(reader["quantity"]);
                else if (type == "Mi Max") Globals.mimaxOrder = Convert.ToInt32(reader["quantity"]);
                else if (type == "Redmi 3s") Globals.redmiOrder = Convert.ToInt32(reader["quantity"]);
                else if (type == "Galaxy Note 5") Globals.galaxynote5Order = Convert.ToInt32(reader["quantity"]);
                else if (type == "Galaxy Note 8") Globals.galaxynote8Order = Convert.ToInt32(reader["quantity"]);
                else Globals.galaxys8Order = Convert.ToInt32(reader["quantity"]);

                //update UI
                mi5Order.text = "Mi5: " + Globals.mi5Order.ToString();
                mimaxOrder.text = "MiMax: " + Globals.mimaxOrder.ToString();
                redmiOrder.text = "Redmi: " + Globals.redmiOrder.ToString();
                galaxynote5Order.text = "GalaxyNote5: " + Globals.galaxynote5Order.ToString();
                galaxynote8Order.text = "GalaxyNote8: " + Globals.galaxynote8Order.ToString();
                galaxys8Order.text = "GalaxyS8+: " + Globals.galaxys8Order.ToString();

                Globals.nextOrderTime = Convert.ToInt32(reader["due_time"]);
                Globals.order_id = Convert.ToInt32(reader["id"]);
            }
            else
            {
                //update UI
                mi5Order.text = "Mi5: 0";
                mimaxOrder.text = "MiMax: 0";
                redmiOrder.text = "Redmi: 0";;
                galaxynote5Order.text = "GalaxyNote5: 0";
                galaxynote8Order.text = "GalaxyNote8: 0";
                galaxys8Order.text = "GalaxyS8+: 0";

                Globals.nextOrderTime = 0;
                Globals.order_id = 0;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    
    private string TimeToString(int time)
    {
        int converted = (int)((float)time * 2.4f);
        int min = converted / 60;
        int sec = converted % 60;

        string smin = "", ssec = "", ti = "AM";
        if (min > 11)
            ti = "PM";
        min %= 12;
        if (min < 10) smin += "0";
        if (sec < 10) ssec += "0";
        smin += min.ToString();
        ssec += sec.ToString();
        
        return smin + ":" + ssec + " " + ti;
    }

    public void ShopOnClick()
    {
        shop.onClick.AddListener(ContinueFromShopOnClick);
        shop.onClick.RemoveListener(ShopOnClick);
        shop.GetComponentInChildren<Text>().text = "Back";
        shopPanel.SetActive(true);

        camera.transform.position = new Vector3(camera.transform.position.x - 300, 
            camera.transform.position.y, camera.transform.position.z);
        camera.transform.eulerAngles = new Vector3(0.0f,
            camera.transform.rotation.y, camera.transform.rotation.z);

        for(int i=0; i<12; i++)
        {
            menus[i].interactable = !Game.stuff[i];
        }
    }

    public void ContinueFromShopOnClick()
    {
        shop.onClick.AddListener(ShopOnClick);
        shop.onClick.RemoveListener(ContinueFromShopOnClick);
        shop.GetComponentInChildren<Text>().text = "Shop";
        shopPanel.SetActive(false);

        camera.transform.position = new Vector3(camera.transform.position.x + 300,
            camera.transform.position.y, camera.transform.position.z);
        camera.transform.eulerAngles = new Vector3(15.49f,
            camera.transform.rotation.y, camera.transform.rotation.z);
    }

    public void StorageOnClick()
    {
        promo.SetActive(false);
        itemsQuantityText.SetActive(true);
        itemsInBoxText.SetActive(true);
        storage.onClick.AddListener(ContinueFromStorageOnClick);
        storage.onClick.RemoveListener(StorageOnClick);
        storage.GetComponentInChildren<Text>().text = "Back to Game";
        GM.gameObject.SetActive(false);
        SceneManager.LoadScene("Storage", LoadSceneMode.Additive);
    }

    public void ContinueFromStorageOnClick()
    {
        promo.SetActive(true);
        itemsQuantityText.SetActive(false);
        itemsInBoxText.SetActive(false);
        storage.onClick.AddListener(StorageOnClick);
        storage.onClick.RemoveListener(ContinueFromStorageOnClick);
        storage.GetComponentInChildren<Text>().text = "Go to Storage";
        GM.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync("Storage");
    }

    public void SetItemInBoxCount(){
        mi5QuantityText.text = "Mi5: " + Globals.mi5QuantityLocal.ToString();
        mimaxQuantityText.text = "MiMax: " + Globals.mimaxQuantityLocal.ToString();
        redmiQuantityText.text = "Redmi: " + Globals.redmiQuantityLocal.ToString();
        galaxynote5QuantityText.text = "GalaxyNote5: " + Globals.galaxynote5QuantityLocal.ToString();
        galaxynote8QuantityText.text = "GalaxyNote8: " + Globals.galaxynote8QuantityLocal.ToString();
        galaxys8QuantityText.text = "GalaxyS8+: " + Globals.galaxys8QuantityLocal.ToString();

        mi5InBoxText.text = "Mi5: " + Globals.mi5Inbox.ToString();
        mimaxInBoxText.text = "MiMax: " + Globals.mimaxInbox.ToString();
        redmiInBoxText.text = "Redmi: " + Globals.redmiInbox.ToString();
        galaxynote5InBoxText.text = "GalaxyNote5: " + Globals.galaxynote5Inbox.ToString();
        galaxynote8InBoxText.text = "GalaxyNote8: " + Globals.galaxynote8Inbox.ToString();
        galaxys8InBoxText.text = "GalaxyS8+: " + Globals.galaxys8Inbox.ToString();
    }

    public void OnClickItem(int i)
    {
        Debug.Log(i);

        Game.money -= GM.price[i];
        

        Game.stuff[i] = true;
        menus[i].interactable = false;
        GM.SetActiveInventory();
    }

    public void MouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "table1")
                {
                    if (!Game.stuff[12])
                    {
                        infoPanel.SetActive(true);
                        infoPanel.GetComponentsInChildren<Text>()[2].text = "1";
                        infoPanel.GetComponentsInChildren<Text>()[3].text = "1000";
                    }
                }
                else if (hitInfo.transform.gameObject.tag == "table2")
                {
                    if (!Game.stuff[13])
                    {
                        infoPanel.SetActive(true);
                        infoPanel.GetComponentsInChildren<Text>()[2].text = "2";
                        infoPanel.GetComponentsInChildren<Text>()[3].text = "2000";
                    }
                }
                else if (hitInfo.transform.gameObject.tag == "table3")
                {
                    if (!Game.stuff[14])
                    {
                        infoPanel.SetActive(true);
                        infoPanel.GetComponentsInChildren<Text>()[2].text = "3";
                        infoPanel.GetComponentsInChildren<Text>()[3].text = "3000";
                    }
                }
                else
                {
                    infoPanel.SetActive(false);
                }
            }
        }
    }

    public void HireOnClick()
    {
        Debug.Log("HireOnClick");
        int num = Convert.ToInt32(infoPanel.GetComponentsInChildren<Text>()[2].text);
        int price = Convert.ToInt32(infoPanel.GetComponentsInChildren<Text>()[3].text);

        Game.money -= price;
        Game.stuff[11 + num] = true;
        GM.SetActiveInventory();
        infoPanel.SetActive(false);
    }

    public void SupplyOnClick()
    {
        supply.onClick.AddListener(ContinueFromSupplyOnClick);
        supply.onClick.RemoveListener(SupplyOnClick);
        supply.GetComponentInChildren<Text>().text = "Back";
        supplyPanel.SetActive(true);

        camera.transform.position = new Vector3(camera.transform.position.x + 300,
            camera.transform.position.y, camera.transform.position.z);
        camera.transform.eulerAngles = new Vector3(0.0f,
            camera.transform.rotation.y, camera.transform.rotation.z);
    }

    public void PromoOnClick(){
        promoButton.onClick.AddListener(SendPromo);
    }

    public void ContinueFromSupplyOnClick()
    {
        supply.onClick.AddListener(SupplyOnClick);
        supply.onClick.RemoveListener(ContinueFromSupplyOnClick);
        supply.GetComponentInChildren<Text>().text = "Supply";
        supplyPanel.SetActive(false);

        camera.transform.position = new Vector3(camera.transform.position.x - 300,
            camera.transform.position.y, camera.transform.position.z);
        camera.transform.eulerAngles = new Vector3(15.49f,
            camera.transform.rotation.y, camera.transform.rotation.z);
    }

    public void OnSupplyBuy(int i)
    {
        int count = Convert.ToInt32(supplies[i].GetComponentsInChildren<Text>()[1].text.ToString());
        int price = Convert.ToInt32(supplies[i].GetComponentsInChildren<Text>()[2].text.ToString());
        Debug.Log(i.ToString());
        Game.money -= price;
        if(i>=0 && i<=2){
            Globals.mi5Quantity += count;
        }else if(i>=3 && i<=5){
            Globals.mimaxQuantity += count;
        }else if(i>=6 && i<=8){
            Globals.redmiQuantity += count;
        }else if(i>=9 && i<=11){
            Globals.galaxynote5Quantity += count; 
        }else if(i>=12 && i<=14){
            Globals.galaxynote8Quantity += count;
        }else if(i>=15 && i<=17){
            Globals.galaxys8Quantity += count;
        }
    }

    public void UpdateNews()
    {
        tima = 4;
        int idx = GM.UpdateNews();

        if (idx < 8)
        {
            string berita = GM.news[idx];
            newsPanel.SetActive(true);

            newsPanel.GetComponentInChildren<Text>().text = "News: " + berita;
        }
    }

    public void RemoveNews()
    {
        if (tima <= 0)
            newsPanel.SetActive(false);
        else
            tima--;
    }

    public void WinLose()
    {
        if (Game.day > 7 || Game.money < -3000 || Game.popularity < -50 || Game.trust < -20)
        {
            PlayerPrefs.SetInt("Win", 0); // lose
            SceneManager.LoadScene("WinLose", LoadSceneMode.Single);
            enabled = false;
            Destroy(GM.gameObject);
            Destroy(gameObject);
        }
        else if (Game.day <= 10 && Game.money >= 60000)
        {
            PlayerPrefs.SetInt("Win", 1); // win
            SceneManager.UnloadSceneAsync("Main Game");
            SceneManager.LoadScene("WinLose", LoadSceneMode.Single);
            enabled = false;
            Destroy(GM.gameObject);
            Destroy(gameObject);
        }
    }

    public async void SendPromo()
    {
        var values = new Dictionary<string, string>
        {
            { "player", Globals.player_name }
        };

        var data = new FormUrlEncodedContent(values);
        var response = await Globals.client.PostAsync("http://mobileinc.herokuapp.com/api/manage/promotion/send", data);
        var contents = await response.Content.ReadAsStringAsync();

        Debug.Log("Response = " + contents);

        Game.popularity += 8;

        System.Random random = new System.Random();
        Globals.use_promo += random.Next(1,10);

        Debug.Log("Promo for next " + Globals.use_promo + " orders");
    }
}
