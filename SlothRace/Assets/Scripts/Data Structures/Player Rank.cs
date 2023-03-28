using System;
using UnityEngine;

[Serializable]
public class PlayerRank
{
    private int _playerID;
    private float _distance;

    public PlayerRank(int playerID, float distance)
    {
        this._playerID = playerID;
        this._distance = distance;
    }

    public override string ToString()
    {
        return _playerID.ToString() + " is " + _distance.ToString() + "m away from the destination.";
    }

    public static bool operator ==(PlayerRank p1, PlayerRank p2)
    {
        if (p1 == null)
        {
            Debug.LogError("PlayerRank 1 Not Exist.");
            return false;
        }

        if (p2 == null)
        {
            Debug.LogError("PlayerRank 2 Not Exist.");
            return false;
        }
        return (p1._playerID == p2._playerID);
    }

    public static bool operator !=(PlayerRank p1, PlayerRank p2)
    {
        return !(p1 == p2);
    }

    public static bool operator >(PlayerRank p1, PlayerRank p2)
    {
        if (p1 == null)
        {
            Debug.LogError("PlayerRank 1 Not Exist.");
            return false;
        }

        if (p2 == null)
        {
            Debug.LogError("PlayerRank 2 Not Exist.");
            return false;
        }
        return (p1._distance > p2._distance);
    }

    public static bool operator <(PlayerRank p1, PlayerRank p2)
    {
        if (p1 == null)
        {
            Debug.LogError("PlayerRank 1 Not Exist.");
            return false;
        }

        if (p2 == null)
        {
            Debug.LogError("PlayerRank 2 Not Exist.");
            return false;
        }
        return (p1._distance > p2._distance);
    }

    public static bool operator >=(PlayerRank p1, PlayerRank p2)
    {
        if (p1 == null)
        {
            Debug.LogError("PlayerRank 1 Not Exist.");
            return false;
        }

        if (p2 == null)
        {
            Debug.LogError("PlayerRank 2 Not Exist.");
            return false;
        }
        return (p1._distance >= p2._distance);
    }

    public static bool operator <=(PlayerRank p1, PlayerRank p2)
    {
        if (p1 == null)
        {
            Debug.LogError("PlayerRank 1 Not Exist.");
            return false;
        }

        if (p2 == null)
        {
            Debug.LogError("PlayerRank 2 Not Exist.");
            return false;
        }
        return (p1._distance <= p2._distance);
    }
}
