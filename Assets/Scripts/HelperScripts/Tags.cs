using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags
{
    public static string WALL = "Wall";
    public static string BLUEFRUIT = "BlueFruit";
    public static string REDFRUIT = "RedFruit";
    public static string BOMB = "Bomb";
    public static string TAIL = "Tail";
}//tags

public class Metrics
{
    public static float NODE = 0.18f;
}//metrics

public enum PlayerDirection
{
    LEFT = 0,
    UP = 1,
    RIGHT = 2,
    DOWN = 3,
    COUNT =4
}//PlayerDirection

public static class Constants
{
    public const string SCORE = "Score : ";
    public const string STREAK = "Streak : ";
    public const string BESTSCORE = "Best Score : ";
}

[Serializable]
public class Fruit
{
    public GameObject FruitObj;
    public int FruitPoint;
}

