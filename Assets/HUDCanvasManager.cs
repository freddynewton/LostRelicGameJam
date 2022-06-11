using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvasManager : MonoBehaviour
{
    [SerializeField]
    private Slider staminaSlider;

    [SerializeField]
    private RopeController ropeController;

    private void Awake()
    {
        // Garbage code for now;
        if (ropeController == null)
        {
            ropeController = FindObjectOfType<RopeController>();
        }

        staminaSlider.maxValue = ropeController.ropePullStrengthStaminaMax;
        staminaSlider.value = ropeController.ropePullStrengthStaminaMax;
    }

    private void Update()
    {
        staminaSlider.value = ropeController.ropePullStrengthStaminaCurrent;
    }
}
