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
        reset_positions.Add("Base - Pivot", new Vector3(0,0.000999987125f,0.174000025f)); 
        reset_positions.Add("Segment 1 - Pivot", new Vector3(-0.00999999046f,0.000999987125f,0.274000049f));
        reset_positions.Add("Segment 2 - Pivot", new Vector3(0.0185010433f,0.149899304f,0.524781704f));
        reset_positions.Add("Segment 3 - Pivot", new Vector3(0.516099989f,1.71879995f,0.0181999207f));
        reset_positions.Add("Segment 4 - Pivot", new Vector3(-0.00999999139f,-0.0130000114f,0.453200042f));
        reset_positions.Add("Segment 5 - Pivot", new Vector3(0.598300636f,1.44790101f,0.00839972496f));
        // Add rotations to dictionary
        reset_rotations.Add("Base - Pivot", new Quaternion(0, 0, 0, 1));
        reset_rotations.Add("Segment 1 - Pivot", new Quaternion(0, 0, 0, 1));
        reset_rotations.Add("Segment 2 - Pivot", new Quaternion(0, 0, 0, 1));
        reset_rotations.Add("Segment 3 - Pivot", new Quaternion(0.706698656f,0.024020819f,-0.706698656f,0.0240208823f));
        reset_rotations.Add("Segment 4 - Pivot", new Quaternion(0.0990288109f,0,0,0.995084584f));
        reset_rotations.Add("Segment 5 - Pivot", new Quaternion(-0.664092183f,0.242861241f,0.664092183f,0.242861241f));
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