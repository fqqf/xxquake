using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    InputAction switching;

    public TextMeshProUGUI ammoInfoText;
    
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        Gun currentGun = FindObjectOfType<Gun>();
        ammoInfoText.text = currentGun.currentAmmo + "/" + currentGun.magazineSize;
        float scrollValue = switching.ReadValue<Vector2>().y;

        int previousSelected = selectedWeapon;
        
        if (scrollValue > 0) 
        {
            selectedWeapon++;
            if (selectedWeapon == 3)
                selectedWeapon = 0;
        } else if (scrollValue < 0)
        {
            selectedWeapon--;
            if (selectedWeapon == -1)
                selectedWeapon = 2;
        }
        
        
        if (previousSelected!=selectedWeapon) SelectWeapon();
    }

    private void SelectWeapon()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
        transform.GetChild((selectedWeapon)).gameObject.SetActive(true);

        switching = new InputAction("Scroll", binding: "<mouse>/scroll");
        switching.AddBinding("<Gamepad>/Dpad");
        switching.Enable();
    }
}
