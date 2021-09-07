using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AxleObject 
{
    [SerializeField] public WheelObject[] wheels;
    public bool drive;
    public bool steer;
    public bool brakes;
    public bool antiRoll;

    internal int leftWheel;
    internal int rightWheel;
}
