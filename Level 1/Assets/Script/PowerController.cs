using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerController : MonoBehaviour {

    // If the 
    public enum Status { ON, OFF };

    // Status of the power cable
    public Status PowerStatus { get; set; }
    public Status DebugStatus;
    public Status DefaulStatus;

    public TilemapRenderer PowerOn;
    public TilemapRenderer PowerOff;

    private void Awake()
    {
        PowerStatus = DefaulStatus;
        DebugStatus = PowerStatus;
    }

    private void FixedUpdate()
    {
        if (PowerStatus == Status.OFF)
        {
            PowerOff.enabled = true;
            PowerOn.enabled = false;
        }
        else
        {
            PowerOff.enabled = false;
            PowerOn.enabled = true;
        }
    }
}
