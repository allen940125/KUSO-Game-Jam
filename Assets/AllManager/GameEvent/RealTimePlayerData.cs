using UnityEngine;
using Gamemanager;
using System.ComponentModel;

public class RealTimePlayerData
{
    public int NowLevel = 0;
    public float PlayerDrunkennessValue = 0;
    
    public int PlayerCurWineBottle = 0;
    public float PlayerCurWineBottleRemainingAlcohol = 0;

    public int ExcitementValue;
    public int SuspicionValue;
        
    public bool CanPlayerMove = true;

    public bool IsListeningYajuuSenpai = false;
}
