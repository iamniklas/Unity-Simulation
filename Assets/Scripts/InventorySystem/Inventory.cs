using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Inventarliste (Beinhaltet Bücher und die Schlüsselkarte(n))
    List<InventoryItem> mItems = new List<InventoryItem>();

    //Gegenstand des Typs InventoryItem zur Liste hinzufügen
    public void AddItem(InventoryItem _item)
    {
        mItems.Add(_item);
    }

    //Generische Methode zur Überprüfung auf Gegenstandsbesitz
    public bool HasItemOfType<T>()
    {
        //Iteration über Itemliste
        for (int i = 0; i < mItems.Count; i++)
        {
            //Ist das aktuelle Element vom Typ T
            if (mItems[i].GetType() == typeof(T))
            {
                //Spieler besitzt Gegenstand des Typs T
                return true;
            }
        }
        //Spieler besitzt Gegenstand des Typs T nicht
        return false;
    }
}