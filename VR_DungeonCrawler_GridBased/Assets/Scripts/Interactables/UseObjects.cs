using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class UseObjects : MonoBehaviour
{
    PlayerController player;
    Interactable leftItem;
    Interactable rightItem;

    //controls
    private Controls playerInput;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        leftItem = player.focusLeft;
        rightItem = player.focusRight;
    }
    private void Awake()
    {
        playerInput = new Controls();
       
    }
    
    private void OnEnable()
    {
        //Enable new input system
        playerInput.Player.UseObjectLeft.performed += UseLeft;
        playerInput.Player.UseObjectLeft.Enable();

        playerInput.Player.UseObjectRight.performed += UseRight;
        playerInput.Player.UseObjectRight.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.UseObjectLeft.Disable();
        playerInput.Player.UseObjectRight.Disable();
    }

    void UseLeft(InputAction.CallbackContext button)
    {
        leftItem = player.focusLeft;
        if (leftItem != null)
        {
            leftItem.Use();
        }

    }

    void UseRight(InputAction.CallbackContext button)
    {
        rightItem = player.focusRight;
        if (rightItem != null)
        {
            rightItem.Use();
        }
    }
}
