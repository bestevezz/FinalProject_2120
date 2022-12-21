using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cloning : MonoBehaviour
{
    [SerializeField]
    private InputAction rMouseClick;
    [SerializeField]
    private InputAction mMouseClick;
    public GameObject clone;
    private GameObject newInstance;
    public List<GameObject> clones = new List<GameObject>();
    //public int numCloned = 0;

    private void OnEnable()
    {
        mMouseClick.Enable();
        rMouseClick.Enable();
        rMouseClick.performed += Multiply;
        mMouseClick.performed += Divide;
    }

    private void OnDisable()
    {
        rMouseClick.performed -= Multiply;
        mMouseClick.performed -= Divide;
        rMouseClick.Disable();
        mMouseClick.Disable();
    }

    private void Multiply(InputAction.CallbackContext context)
    {
        float instX = clone.transform.position.x;
        float instY = clone.transform.position.y;
        newInstance = Instantiate(clone, transform.position + new Vector3(instX, instY+2f, 0), Quaternion.identity);
        newInstance.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 0f, 1f, 0.4f, 1f);
        clones.Add(newInstance);
        //numCloned++;
    }

    private void Divide(InputAction.CallbackContext context)
    {
        //Code to destory them all at once
        //for(int i = 0; i < numCloned; i++)
        //{
        //    Destroy(clones[i]);

        //}
        //clones.Clear();
        //numCloned = 0;
        
        if(context.performed)
        {

            Debug.Log("Clone remove");
            if (clones.Count>0)
            {


                GameObject tmp = clones[0];
                clones.Remove(clones[0]);
                //tmp.SetActive(false);
                Destroy(tmp);

            }


        }



    }
}
