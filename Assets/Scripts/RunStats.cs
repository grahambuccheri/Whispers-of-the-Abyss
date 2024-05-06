using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/RunStats")]
public class RunStats : ScriptableObject
{
    public int score;

    public int credits;

    public int currentDay;
    
    // Call this on start game!
    public void Reset()
    {
        score = 0;
        credits = 0;
        currentDay = 1;
    }

    public void PickupItem(int itemScore, int worth)
    {
        score += itemScore;
        credits += worth;
    }

    // attempts to purchase item with price. returns false if should fail (would put user in debt)
    public bool Purchase(int price)
    {
        if (credits - price < 0)
        {
            return false;
        }
        credits -= price;
        return true;
    }
}
