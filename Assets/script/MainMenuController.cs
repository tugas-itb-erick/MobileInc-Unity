using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    // Buttons in Main UI
    public Button startButton;
    public Button loadButton;
    public Button settingsButton;
    public Button creditButton;
    public Button quitButton;
    public Button backButton;

    // UIs
    public GameObject startUI;
    public GameObject loadUI;
    public GameObject settingsUI;
    public GameObject creditUI;

    // Settings
    public Slider musicSlider;
    public Button theme1, theme2;

    public InputField usernameField;

    // Load
    public Dropdown dropdown;
    public List<string> listPlayer;

    // Image
    public Texture front1;
    public Texture front2;

    // Use this for initialization
    void Start()
    {
        // Buttons are initialized in Unity

        front1 = (Texture)Resources.Load("front1");
        front2 = (Texture)Resources.Load("front2");

        // Connect to database
        SQLiteDatabase.OpenConnection();

        // Button Listeners
        startButton.onClick.AddListener(StartOnClick);
        listPlayer = GetAllPlayer();
        if (listPlayer.Count == 0)
        {
            loadButton.interactable = false;
        }
        else
        {
            dropdown.AddOptions(listPlayer);
            loadButton.interactable = true;
        }

        loadButton.onClick.AddListener(LoadOnClick);
        settingsButton.onClick.AddListener(SettingsOnClick);
        creditButton.onClick.AddListener(CreditOnClick);
        quitButton.onClick.AddListener(QuitOnClick);
        backButton.onClick.AddListener(BackOnClick);
        backButton.gameObject.SetActive(false);
        BackOnClick();
        startUI.GetComponentInChildren<Button>().onClick.AddListener(StartGame);
        loadUI.GetComponentInChildren<Button>().onClick.AddListener(LoadGame);

        // Music Slider
        musicSlider = settingsUI.GetComponentInChildren<Slider>();
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderOnChange(); });
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);

        // Theme
        theme1 = settingsUI.GetComponentsInChildren<Button>()[0];
        theme2 = settingsUI.GetComponentsInChildren<Button>()[1];
        theme1.onClick.AddListener(Theme1OnClick);
        theme2.onClick.AddListener(Theme2OnClick);

        // Input Field
        usernameField.onValueChanged.AddListener(delegate { CheckPlayerNameValid(); });

        CheckPlayerNameValid();
        listPlayer = GetAllPlayer();
        dropdown.AddOptions(listPlayer);
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Theme", 1) == 1)
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = front1;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = front2;
        }
    }

    private void StartOnClick()
    {
        // Set Visibility
        HideMainButtons();
        startUI.SetActive(true);
    }

    private void LoadOnClick()
    {
        // Set Visibility
        HideMainButtons();
        loadUI.SetActive(true);
        //listPlayer = GetAllPlayer();
    }

    private void SettingsOnClick()
    {
        // Set Visibility
        HideMainButtons();
        settingsUI.SetActive(true);

        // Audio
        musicSlider.value = AudioListener.volume;
    }

    private void CreditOnClick()
    {
        // Set Visibility
        HideMainButtons();
        creditUI.SetActive(true);
    }

    private void QuitOnClick()
    {
        //UnityEditor.EditorApplication.isPlaying = false; // quit editor
        SQLiteDatabase.CloseConnection(); // close database connection
        Application.Quit(); // quit app
    }

    private void BackOnClick()
    {
        startButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        creditButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);

        startUI.SetActive(false);
        loadUI.SetActive(false);
        settingsUI.SetActive(false);
        creditUI.SetActive(false);
    }

    private void HideMainButtons()
    {
        startButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        creditButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        CreateNewPlayer(usernameField.text.ToString());
        PlayerPrefs.SetString("Start Mode", "Start");
        SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
    }

    private void MusicSliderOnChange()
    {
        float volume = musicSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void Theme1OnClick()
    {
        PlayerPrefs.SetInt("Theme", 1);
    }

    private void Theme2OnClick()
    {
        PlayerPrefs.SetInt("Theme", 2);
    }

    /* Player */
    private void CheckPlayerNameValid()
    {
        bool check = false;
        string username = usernameField.text.ToString();
        if (username == "" || username == null)
        {
            startUI.GetComponentInChildren<Button>().interactable = false;
            return;
        }
        string query = "SELECT name FROM player WHERE name = '" + username + "'";

        try {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            check = !(reader.HasRows);
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }

        startUI.GetComponentInChildren<Button>().interactable = check;
    }

    private void CreateNewPlayer(string name)
    {
        System.Random random = new System.Random();
        int player_id = random.Next(1, 10000);
        string query = "INSERT INTO player VALUES (" + player_id.ToString() + ", '" + name + "')";

        SQLiteDatabase.UpdateQuery(query);
        Globals.player_id = player_id;
        Globals.player_name = name;

        query = "INSERT INTO gamestate VALUES (" + player_id.ToString() + ", 0, 0, 0, 0, 0, 0, 0)";
        SQLiteDatabase.UpdateQuery(query);

        for (int cnt = 1; cnt <= 6; cnt++)
        {
            query = "INSERT INTO storage VALUES (" + (Globals.player_id * 10 + cnt).ToString() + ", " + player_id.ToString()
                    + ", " + cnt.ToString() + ", 0)";
            SQLiteDatabase.UpdateQuery(query);
        }
    }

    private List<string> GetAllPlayer()
    {
        List<string> players = new List<string>();

        string query = "SELECT name FROM player";

        try {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                players.Add(reader["name"].ToString());
            }
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }

        return players;
    }

    public void LoadGame()
    {
        PlayerPrefs.SetString("Start Mode", "Load");
        Globals.player_name = listPlayer[dropdown.value];

        string query = "SELECT id FROM player WHERE name = '" + Globals.player_name + "'";

        try
        {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            Globals.player_id = Convert.ToInt32(reader["id"]);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }

        Debug.Log(Globals.player_name);
        Debug.Log(Globals.player_id);
        SceneManager.LoadScene("Main Game", LoadSceneMode.Single);
    }
}
