﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Resources
    //[SerializeField] public 

    public static int Gold = 15;
    public static int Mana = 10;

    // Other things
    public static int MainTowerHP = 50;

    private void Update() {

        if ( MainTowerHP < 1 ) {
            Debug.Log("GAME OVER");
        } else {
            //Debug.LogFormat("Main Tower HP: {0}", MainTowerHP);
        }
    }
}
