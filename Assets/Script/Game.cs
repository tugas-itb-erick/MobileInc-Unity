using System;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SQLite;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;

public class Game : MonoBehaviour {
    
    public static int day;
    public static int time;
    public static int happiness;
    public static int money;
    public static int popularity;
    public static int trust;

    public static string[] products = new string[] { "Mi 5", "Mi Max", "Redmi 3s", "Galaxy Note 5", "Galaxy Note 8", "Galaxy S8+"};
    public static int[] pprices = new int[] { 460, 400, 140, 840, 950, 820 };

    public GameObject AC, TV, table1, table2, table3, lamp1, lamp2, plant1, plant2, plant3, micro, fridge;
    public GameObject employee1, employee2, employee3;
    public static bool[] stuff;
    public int[] price = new int[] { 3000, 2500, 500, 1500, 3000, 1000, 2000, 500, 500, 500, 1500, 5000};

    public string[] news = new string[]
    {
        "Presiden Amerika berpidato hanya dengan kaos di panggung... Orang banyak menjadi terkejut...",
        "Dengan bantuan teknologi CRISPR, ilmuan dapat menyembuhkan penyakit genetika...",
        "Rilis tugas besar PBD merancang aplikasi dengan teknologi VR...",
        "Tahukah bahwa Anda dapat menjadi orang kaya dengan 3 cara ini...",
        "Perusahaan lain sedang menyebar promo... Popularitas Mobile Inc menjadi menurun...",
        "Gosipnya Mobile Inc sering melakukan penipuan...",
        "Hari libur nasional! Orang-orang akan banyak melakukan shopping...",
        "Orang banyak senang dengan produk Mobile Inc..."
    };

    public const int INITIAL_TIME = 150;

    public AudioSource[] audioSources;

    public string port = "\\\\.\\COM10";
    public int baudrate = 9600;
    public int weatherValue = -1;
    private SerialPort stream;
    
    void Awake ()
    {
        DontDestroyOnLoad(transform.gameObject);
        StartGame();
        InitTags();
    }

	// Use this for initialization
	void Start () {
        audioSources = GetComponents<AudioSource>();
        InvokeRepeating("PlaySoundKeyboard", 0.0f, 7.5f);
        InvokeRepeating("PlaySoundPhone", 0.0f, 11.5f);
        InvokeRepeating("PlaySoundPrinter", 0.0f, 17.5f);
        InvokeRepeating("UpdateTime", 0.0f, 1.0f);
        InvokeRepeating("UpdatePopularity", 0.0f, 5.0f);
        InvokeRepeating("IncreaseMoney1", 0.0f, 5.0f);
        InvokeRepeating("IncreaseMoney2", 0.0f, 4.7f);
        InvokeRepeating("IncreaseMoney3", 0.0f, 4.4f);
        InvokeRepeating("DecreaseMoney", 0.0f, 5.1f);
        InvokeRepeating("GenerateOrder", 0.0f, 10.0f);
        //SetActiveInventory();
        stream = new SerialPort(port, baudrate);
        stream.ReadTimeout =1000;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHappiness();
        //GenerateOrder();
        //WinLose();
        if (stream.IsOpen)
        {
            try
            {
                weatherValue = stream.ReadByte();
            } catch (Exception e)
            {
                weatherValue = -1;
            }
        } else
        {
            weatherValue = -1;
            try
            {
                stream.Open();
            }
            catch (Exception e)
            {
                weatherValue = -1;
            }
        }
    }
    
    void UpdateTime()
    {
        time++;
        time %= 600;
        if (time == INITIAL_TIME) // every 06:00 AM
        {
            day++;
        }
    }

    // Start a New Game
    private void StartGame()
    {
        if (PlayerPrefs.GetString("Start Mode", "Start") == "Start")
        {
            day = 1;
            time = INITIAL_TIME;
            happiness = 40;
            money = 5000;
            popularity = 1;
            trust = 0;
            stuff = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

            Globals.mi5Quantity = 0;
            Globals.mimaxQuantity = 0;
            Globals.redmiQuantity = 0;
            Globals.galaxynote5Quantity = 0;
            Globals.galaxynote8Quantity = 0;
            Globals.galaxys8Quantity = 0;
        }
        else // if (PlayerPrefs.GetString("Start Mode", "Start") == "Load")
        {
            LoadGame();
        }

        InitialSave();
        SetActiveInventory();
    }

    private void InitialSave()
    {
        string query = "UPDATE gamestate SET day = " + Game.day.ToString()
                        + ", time = " + Game.time.ToString() + ", money = " + Game.money.ToString()
                        + ", popularity = " + Game.popularity.ToString() + ", trust = " + Game.trust.ToString()
                        + ", happiness = " + Game.happiness.ToString() + " WHERE player_id = " + Globals.player_id.ToString();

        SQLiteDatabase.UpdateQuery(query);

        for (int cnt = 0; cnt < 15; cnt++)
        {
            //Debug.Log("Stuff[" + cnt + "] = " + Game.stuff[cnt].ToString());
            if (Game.stuff[cnt]) Inventory.PurchaseItem(cnt);
        }

        saveStorage();
    }

    private void saveStorage()
    {
        for (int cnt = 1; cnt <= 6; cnt++)
        {
            int quantity;

            if (cnt == 1) quantity = Globals.mi5Quantity;
            else if (cnt == 2) quantity = Globals.mimaxQuantity;
            else if (cnt == 3) quantity = Globals.redmiQuantity;
            else if (cnt == 4) quantity = Globals.galaxynote5Quantity;
            else if (cnt == 5) quantity = Globals.galaxynote8Quantity;
            else quantity = Globals.galaxys8Quantity;

            string query = "UPDATE storage SET quantity = " + quantity + " WHERE player_id = " + Globals.player_id.ToString() + " AND phone_id = " + cnt.ToString();
            SQLiteDatabase.UpdateQuery(query);
        }
    }

    private void InitTags()
    {
        AC.tag = "AC";
        TV.tag = "TV";
        table1.tag = "table1";
        table2.tag = "table2";
        table3.tag = "table3";
        employee1.tag = "employee1";
        employee2.tag = "employee2";
        employee3.tag = "employee3";
        lamp1.tag = "lamp1";
        lamp2.tag = "lamp2";
    }

    // Load Game
    private void LoadGame()
    {
        string query = "SELECT * FROM gamestate WHERE player_id = " + Globals.player_id.ToString();
        SQLiteDataReader reader = null;

        try {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                reader.Read();
                day = Convert.ToInt32(reader["day"]);
                time = Convert.ToInt32(reader["time"]);
                money = Convert.ToInt32(reader["money"]);
                happiness = Convert.ToInt32(reader["happiness"]);
                popularity = Convert.ToInt32(reader["popularity"]);
                trust = Convert.ToInt32(reader["trust"]);

                stuff = new bool[15];
                Inventory.GetPurchasableItems(stuff);
                getStorage();
            }
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }

    public void getStorage()
    {
        string query = "SELECT quantity FROM storage WHERE player_id = " + Globals.player_id.ToString();

        try
        {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            Globals.mi5Quantity = Convert.ToInt32(reader["quantity"]);
            reader.Read();
            Globals.mimaxQuantity = Convert.ToInt32(reader["quantity"]);
            reader.Read();
            Globals.redmiQuantity = Convert.ToInt32(reader["quantity"]);
            reader.Read();
            Globals.galaxynote5Quantity = Convert.ToInt32(reader["quantity"]);
            reader.Read();
            Globals.galaxynote8Quantity = Convert.ToInt32(reader["quantity"]);
            reader.Read();
            Globals.galaxys8Quantity = Convert.ToInt32(reader["quantity"]);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void PauseGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void SetActiveInventory()
    {
        AC.SetActive(stuff[0]);
        TV.SetActive(stuff[1]);
        table1.SetActive(stuff[2]);
        table2.SetActive(stuff[3]);
        table3.SetActive(stuff[4]);
        lamp1.SetActive(stuff[5]);
        lamp2.SetActive(stuff[6]);
        plant1.SetActive(stuff[7]);
        plant2.SetActive(stuff[8]);
        plant3.SetActive(stuff[9]);
        micro.SetActive(stuff[10]);
        fridge.SetActive(stuff[11]);
        employee1.SetActive(stuff[12]);
        employee2.SetActive(stuff[13]);
        employee3.SetActive(stuff[14]);
    }

    public void UpdateHappiness()
    {
        int count = 0;
        for(int i=0; i<12; i++)
        {
            if (stuff[i]) ++count;
        }

        if (!stuff[12] && !stuff[13] && !stuff[14])
        {
            happiness = 100;
        }
        else
        {
            if (stuff[12] && stuff[13] && stuff[14])
            {
                happiness = (count / 3) * 27;
            }
            else if ((stuff[12] && stuff[13]) || (stuff[12] && stuff[14]) || (stuff[13] && stuff[14]))
            {
                happiness = ((count / 2) * 110) / 6;
            }
            else
            {
                happiness = count * 20;
            }
        }
        if (happiness > 100)
            happiness = 100;
    }

    public void UpdatePopularity()
    {
        int count = 0;
        for(int i=12; i<=14; i++)
        {
            if (stuff[i]) ++count;
        }

        System.Random rnd = new System.Random();
        if (popularity < 9600)
        {
            popularity += (count * rnd.Next(1, 3)) + trust;
        }
    }

    public void IncreaseMoney1()
    {
        if (stuff[12]) money += 75 + happiness * 2;
    }

    public void IncreaseMoney2()
    {
        if (stuff[13]) money += 100 + happiness * 2;
    }

    public void IncreaseMoney3()
    {
        if (stuff[14]) money += 155 + happiness * 2;
    }

    public void DecreaseMoney()
    {
        int count = 0;
        for (int i = 0; i < 12; i++)
        {
            if (stuff[i]) ++count;
        }
        money -= count * 30;

        count = 0;
        for (int i = 12; i <= 14; i++)
        {
            if (stuff[i]) ++count;
        }
        money -= count * 100;
    }

    public int UpdateNews()
    {
        System.Random rnd = new System.Random();
        int idx = rnd.Next(0,15);
        //string n = news[idx % 8];

        if (idx == 4)
        {
            popularity -= 10;
            money -= 1000;
        }
        else if (idx == 5)
        {
            trust -= 4;
            popularity -= 10;
        }
        else if (idx == 6)
        {
            money += 2222;
        }
        else if (idx == 7)
        {
            trust += 2;
            popularity += 12;
        }

        return idx;
    }

    public async void GenerateOrder()
    {
        System.Random rnd = new System.Random();

        // Check order from server

        var values = new Dictionary<string, string>
        {
            { "player", Globals.player_name }
        };

        var data = new FormUrlEncodedContent(values);
        var response = await Globals.client.PostAsync("http://mobileinc.herokuapp.com/api/manage/order/get", data);
        var orders_json = await response.Content.ReadAsStringAsync();

        JArray orders = (JArray) JObject.Parse(orders_json).GetValue("order");

        if(orders.HasValues)
        {
            Debug.Log("Receiving order from server...");
            Debug.Log("Money = " + money);

            foreach (JObject order in orders)
            {
                foreach (JProperty property in order.Properties())
                {
                    string type = property.Name;
                    JArray details_json = (JArray) property.Value;
                    int[] details = details_json.Values<int>().ToArray();

                    int quantity = details[0];
                    int subtotal = details[1];
                    int timeNeeded = rnd.Next(60, 120);
                    money += subtotal;

                    int order_id = rnd.Next(1, 10000);
                    string query = "INSERT INTO orders VALUES (" + order_id.ToString() + ", " + Globals.player_id.ToString()
                            + ", '" + type + "', " + quantity.ToString() + ", " + subtotal.ToString()
                            + ", " + ((day - 1) * 600 + time + timeNeeded).ToString() + ")";

                    SQLiteDatabase.UpdateQuery(query);
                }
            }

            Debug.Log("Sending confirmation...");
            Debug.Log("Money = " + money);

            values = new Dictionary<string, string> {};
            data = new FormUrlEncodedContent(values);

            response = await Globals.client.PostAsync("http://mobileinc.herokuapp.com/api/manage/confirmation", data);
        }
        else
        {
            // If there is no order, consider to generate it
            
            int num;

            if (trust <= 0 && popularity <= 0)
            {
                num = rnd.Next(1, 14);
            }
            else if (trust <= 0)
            {
                num = rnd.Next(popularity);
            }
            else if (popularity <= 0)
            {
                num = rnd.Next(trust * 10);
            }
            else
            {
                num = rnd.Next(popularity + trust * 10);
            }

            bool yes = false;

            for (int i = 10; i < 60 + trust; i++)
            {
                yes = (num % i == 0);
                if (yes)
                {
                    break;
                }
            }

            Debug.Log("GenerateOrder = " + yes);

            if (yes)
            {
                int productId = rnd.Next(0, 5);
                int productQuantity = rnd.Next(1, 5 + (trust >= 0 ? trust : 0));
                int timeNeeded = rnd.Next(60, 120);
                int price = pprices[productId] * productQuantity;

                if (Globals.use_promo != 0)
                {
                    Globals.use_promo--;
                    Debug.Log("Use promo, " + Globals.use_promo + " left");
                    price = (int)(price * 0.9);
                }

                money += price;
                string type;

                if (productId == 0) type = "Mi 5";
                else if (productId == 1) type = "Mi Max";
                else if (productId == 2) type = "Redmi 3s";
                else if (productId == 3) type = "Galaxy Note 5";
                else if (productId == 4) type = "Galaxy Note 8";
                else type = "Galaxy S8+";

                int order_id = rnd.Next(1, 10000);
                string query = "INSERT INTO orders VALUES (" + order_id.ToString() + ", " + Globals.player_id.ToString()
                        + ", '" + type + "', " + productQuantity.ToString() + ", " + price.ToString()
                        + ", " + ((day - 1) * 600 + time + timeNeeded).ToString() + ")";

                SQLiteDatabase.UpdateQuery(query);
            }
        }
    }

    public void PlaySoundPhone()
    {
        System.Random rnd = new System.Random();
        int num = rnd.Next(0, 3);
        if (num < 2)
        {
            audioSources[1].Play();
        }
    }

    public void PlaySoundKeyboard()
    {
        System.Random rnd = new System.Random();
        int num = rnd.Next(0, 3);
        if (num < 2)
        {
            audioSources[2].Play();
        }
    }

    public void PlaySoundPrinter()
    {
        System.Random rnd = new System.Random();
        int num = rnd.Next(0, 3);
        if (num < 2)
        {
            audioSources[0].Play();
        }
    }
}
