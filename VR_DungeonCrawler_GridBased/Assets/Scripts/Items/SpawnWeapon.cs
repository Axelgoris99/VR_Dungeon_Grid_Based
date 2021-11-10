using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{
    private Controls playerInput;
    HandsEquipment spawn;
    public PlayerController player;
    void Awake()
    {
        playerInput = new Controls();
        spawn = GetComponent<HandsEquipment>();
    }
    private void OnEnable()
    {
        playerInput.Player.InteractLeft.Enable();
        playerInput.Player.InteractRight.Enable();
    }
    private void OnDisable()
    {
        playerInput.Player.InteractLeft.Disable();
        playerInput.Player.InteractRight.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerInput.Player.InteractLeft.triggered || playerInput.Player.InteractRight.triggered)
        {
            if (other.CompareTag("LeftController"))
            {
                spawn.SpawnWeaponInHand(ref player.focusLeft, ref player.leftController, ref player.interactingLeft);
            }
            if (other.CompareTag("RightController"))
            {
                spawn.SpawnWeaponInHand(ref player.focusRight, ref player.rightController, ref player.interactingRight);
            }
        }
    }

}
