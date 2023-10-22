using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public Dictionary<string, Vector3> reset_positions = new Dictionary<string, Vector3>();
    public Dictionary<string, Quaternion> reset_rotations = new Dictionary<string, Quaternion>();
    void Start()
    {
        // Add positions to dictionary
        reset_positions.Add("Base - Pivot", new Vector3(0f, 0.0009999871f, 0.174f));
        reset_positions.Add("Segment 1 - Pivot", new Vector3(-0.00999999f, 0.0009999871f, 0.274f));
        reset_positions.Add("Segment 2 - Pivot", new Vector3(0.01999998f, 0.1729f, 0.6472001f));
        reset_positions.Add("Segment 3 - Pivot", new Vector3(0.5471001f, 1.5861f, 0.01090002f));
        reset_positions.Add("Segment 4 - Pivot", new Vector3(-0.009999991f, -0.01300001f, 0.4532f));
        reset_positions.Add("Segment 5 - Pivot", new Vector3(0.5983006f, 1.447901f, 0.008399725f));
        // Add rotations to dictionary
        reset_rotations.Add("Base - Pivot", new Quaternion(0, 0, 0, 1));
        reset_rotations.Add("Segment 1 - Pivot", new Quaternion(0, 0, 0, 1));
        reset_rotations.Add("Segment 2 - Pivot", new Quaternion(0, 0, 0, 1));
        reset_rotations.Add("Segment 3 - Pivot", new Quaternion(-0.669509113f, 0.227502942f, 0.669509113f, 0.227502942f));
        reset_rotations.Add("Segment 4 - Pivot", new Quaternion(0.0990288109f, 0, 0, 0.995084584f));
        reset_rotations.Add("Segment 5 - Pivot", new Quaternion(-0.664092183f, 0.242861241f, 0.664092183f, 0.242861241f));
    }
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