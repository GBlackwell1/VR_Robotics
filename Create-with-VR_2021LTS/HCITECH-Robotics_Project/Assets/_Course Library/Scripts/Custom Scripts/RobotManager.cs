using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    private float speedModifier = 1f;
    public void ResetSpeedModifier()
    {
        speedModifier = 1f;
    }
    public void LowerSpeedModifier()
    {
        speedModifier = 0.1f;
    }
    public float GetSpeedModifier()
    {
        return speedModifier;
    }
}