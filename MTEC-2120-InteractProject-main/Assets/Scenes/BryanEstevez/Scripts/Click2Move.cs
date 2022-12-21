using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Click2Move : MonoBehaviour
{
    [SerializeField]
    private InputAction lMouseClick;
    private Camera mainCamera;
    private Coroutine coroutine;
    [SerializeField]
    private float playerSpeed = 10f;
    private Vector3 targetPosition;
    private Rigidbody rb;
    [SerializeField]
    private float rotationSpeed = 3f;
    private int groundLayer;

    private void Awake() {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void OnEnable() {
        lMouseClick.Enable();
        lMouseClick.performed += Move;
    }

    private void OnDisable() {
        lMouseClick.performed -= Move;
        lMouseClick.Disable();
    }

    private void Move(InputAction.CallbackContext context) {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer) == 0);
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            targetPosition = hit.point;
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target) {
        float playerDistanceToFloor = transform.position.y - targetPosition.y;
        target.y += playerDistanceToFloor;
        while (Vector3.Distance(transform.position, target) > 1f)
        { //Ignores collisions
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            Vector3 direction = target - transform.position;
            //transform.position = destination;

            rb.velocity = direction.normalized * playerSpeed;
            transform.rotation =   Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 
                rotationSpeed * Time.deltaTime);
            
            yield return null;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(targetPosition, 1);
    }
}
