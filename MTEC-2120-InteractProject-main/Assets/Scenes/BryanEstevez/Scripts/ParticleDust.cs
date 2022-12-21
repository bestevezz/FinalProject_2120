using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParticleDust : MonoBehaviour
{
    [SerializeField]
    private InputAction jumpEffect;
    [SerializeField]
    private GameObject dust;

    private void Awake()
    {
        dust.SetActive(false);
    }


    private void OnEnable()
    {
        jumpEffect.Enable();
        jumpEffect.performed += kickUpDust;
    }

    private void OnDisable()
    {
        jumpEffect.performed -= kickUpDust;
        jumpEffect.Disable();
        
    }

    private void kickUpDust(InputAction.CallbackContext context)
    {
        dust.SetActive(true);
        Debug.Log("particles ACTIVATE");
        

    }
}
