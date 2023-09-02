using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Scope : MonoBehaviour
{
    public Animator animator;
    public GameObject scope_overlay;
    bool is_scoped;
    public Camera fpsCam;
    InputAction scope;
    // Start is called before the first frame update
    void Start()
    {
        scope = new InputAction("Scope", binding: "<mouse>/rightButton");
        scope.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Gun gun = FindObjectOfType<Gun>();

        if (gun.isReloading || gun.currentAmmo == 0)
        {
            OnUnscoped();
        }
        else
        {
            if (scope.triggered)
            {
                is_scoped = !is_scoped;
                if (is_scoped)
                {
                    StartCoroutine(OnScoped());
                }
                else
                {
                    OnUnscoped();
                }
            }
        }
    }

    IEnumerator OnScoped()
    {
        animator.SetBool("is_scoped", is_scoped);
        yield return new WaitForSeconds(0.25f);
        scope_overlay.SetActive(true);
        // в 00000000 бит 1 сдвигается на 6 влево, итого, выходит 0010000, а потом инвертируем, и делаем И.
        // тем самым, создавая маску 1101111, которая уберет шестой слой
        fpsCam.cullingMask = fpsCam.cullingMask & ~(1 << 6);
        fpsCam.fieldOfView = 30;
    }

    void OnUnscoped()
    {
        fpsCam.fieldOfView = 60;
        scope_overlay.SetActive(false);
        animator.SetBool("is_scoped", false);
        is_scoped = false;
        // тут же все так же, только без инвертирования, 
        // и проводится операция ИЛИ, тем самым, возвращая слой, но не трогая другие
        fpsCam.cullingMask = fpsCam.cullingMask | (1 << 6);
    }
}
