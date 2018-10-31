using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour {

    public UIManager UM;
    public Slider musicSlider;

    public void OnPauseClick()
    {
        Debug.Log("OnPauseClick");
        UM.GM.PauseGame();
        UM.pausePanel.SetActive(true);

        UM.pausePanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(OnSaveClick);
        UM.pausePanel.GetComponentsInChildren<Button>()[1].onClick.AddListener(OnLoadClick);
        UM.pausePanel.GetComponentsInChildren<Button>()[2].onClick.AddListener(OnContinueClick);
        UM.pausePanel.GetComponentsInChildren<Button>()[3].onClick.AddListener(OnQuitClick);

        // Music Slider
        musicSlider = UM.pausePanel.GetComponentInChildren<Slider>();
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderOnChange(); });
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        musicSlider.value = AudioListener.volume;
    }

    private void OnContinueClick()
    {
        Debug.Log("OnContinueClick");
        UM.GM.ContinueGame();
        UM.pausePanel.SetActive(false);
    }

    private void MusicSliderOnChange()
    {
        float volume = musicSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void OnSaveClick()
    {
        Debug.Log("OnSaveClick");

        string query = "UPDATE gamestate SET day = " + Game.day.ToString()
                        + ", time = " + Game.time.ToString() + ", money = " + Game.money.ToString()
                        + ", popularity = " + Game.popularity.ToString() + ", trust = " + Game.trust.ToString()
                        + ", happiness = " + Game.happiness.ToString() + " WHERE player_id = " + Globals.player_id.ToString();

        SQLiteDatabase.UpdateQuery(query);

        for(int cnt=0; cnt < 15; cnt ++)
        {
            Debug.Log("Stuff[" + cnt + "] = " + Game.stuff[cnt].ToString());
            if (Game.stuff[cnt]) Inventory.PurchaseItem(cnt);
        }

        saveStorage();
    }

    public void saveStorage()
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

    private void OnLoadClick()
    {
        Debug.Log("OnLoadClick");
        
    }

    private void OnQuitClick()
    {
        Debug.Log("OnQuitClick");
        //Globals.database.CloseConnection(); // close database connection
        Application.Quit(); // quit app
    }
}
