using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRank
{
    private int _playerID;
    private float _distance;

    public PlayerRank(int ID, float distance)
    {
        this._playerID = ID;
        this._distance = distance;
    }
}
