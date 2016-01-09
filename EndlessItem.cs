// Endless Reach
// version 2.4.1  -  November 2014
// Soverance Studios
// www.soverance.com

using UnityEngine;
using System.Collections;

// Serializable classes allow for multiple instances to be instantiated simultaneously.

[System.Serializable] 
public class EndlessItem : ScriptableObject 
{
    public int _cost = 0;
    public string _name = "";
    public string _description = "";
}

[System.Serializable] 
public class Weapon : EndlessItem
{
    public int _Damage = 5;
}

[System.Serializable] 
public class Armor : EndlessItem
{
    public int _Defense = 0;
}
