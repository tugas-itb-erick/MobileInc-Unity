using System;
using System.Data.SQLite;
using UnityEngine;

public static class Inventory
{
    /*
    public static string[] purchasable = new string[] { "AC", "TV", "table1", "table2", "table3", "lamp1", "lamp2",
                                                        "plant1", "plant2", "plant3", "micro", "fridge",
                                                        "employee1", "employee2", "employee3" };
    */
    public static void GetPurchasableItems(bool[] result)
    {
        int mask = 0;

        string query = "SELECT inventory FROM gamestate WHERE player_id = " + Globals.player_id.ToString();

        try {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            int inventory = Convert.ToInt32(reader["inventory"]);

            for (int cnt = 0; cnt < 15; cnt++)
            {
                if (cnt == 0) mask++;
                else mask = mask << 1;

                if (Convert.ToBoolean(inventory & mask)) result[cnt] = true;
                else result[cnt] = false;
            }
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }

    public static void PurchaseItem(int idx)
    {
        string query;

        query = "SELECT inventory FROM gamestate WHERE player_id = " + Globals.player_id.ToString();

        try {
            SQLiteCommand command = new SQLiteCommand(query, SQLiteDatabase.connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            int inventory = Convert.ToInt32(reader["inventory"]);
            int new_inventory = inventory | (1 << idx);

            query = "UPDATE gamestate SET inventory = " + new_inventory.ToString() + " WHERE player_id = " + Globals.player_id.ToString();
            SQLiteDatabase.UpdateQuery(query);
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }
}