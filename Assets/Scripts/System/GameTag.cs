using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class GameTag
{
    #region property
    public static string Player => _player;
    public static string Obstacle => _obstacle;
    public static string Item => _item;
    #endregion

    #region private
    private static string _player = "Player";
    private static string _obstacle = "Obstacle";
    private static string _item = "Item";
    #endregion
}
