using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsEquipment : MonoBehaviour
{
    public EquipmentManager equipped;
    public Equipment equip;
    public MeshFilter currentMesh;
    public MeshRenderer currentRenderer;
    public PlayerController player;
    public EquipmentSlot slot;

    public SpawnWeapon spawn;
    private void Start()
    {
        equipped.onEquipmentChanged += SwapWeapons;

        equip = EquipmentManager.instance.currentEquipment[(int)slot-1];
        currentMesh = GetComponent<MeshFilter>();
        currentRenderer = GetComponent<MeshRenderer>();
        spawn = GetComponent<SpawnWeapon>();
    }

    void SwapWeapons(Equipment newItem, Equipment oldItem)
    {
        if (equip != newItem && newItem.equipSlot == slot && newItem != null)
        {
            equip = newItem;
            currentMesh.mesh = newItem.prefabObject.GetComponent<MeshFilter>().sharedMesh;
            currentRenderer.materials = newItem.prefabObject.GetComponent<MeshRenderer>().sharedMaterials;

        }
    }


    public void SpawnWeaponInHand(ref Interactable focus, ref Transform controller, ref bool interacting)
    {
        if (equip != null)
        {
            GameObject weapon = Instantiate(equip.prefabObject);
            player.SetFocus(ref focus, equip.prefabObject.GetComponent<WeaponPickup>(), controller);
            interacting = true;
            spawn.enabled = false;
        }
    }

    public void ActivateSpawnWeapon()
    {
        spawn.enabled = true;
    }
}
