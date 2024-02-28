using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/* Tutotial
https://www.youtube.com/watch?v=zZDiC0aOXDY&t=775s&ab_channel=samyam
*/

public class ClickToMove : MonoBehaviour
{
    // the field is serialized so you can bind via the edittor but on awake, I just set
    // to the right button mannually (in awake())
    [SerializeField] private InputAction mouseClickAction;
    private Camera mainCamera;
    [SerializeField] private float playerSpeed = 10f;
    private Vector3 targetPosition;
    private Coroutine coroutine;


    private void Awake() {
        mainCamera = Camera.main;
        mouseClickAction.AddBinding("<Mouse>/rightButton");
    }

    private void OnEnable()
    {   
        /* 1) when the script is enabled, enable the inputAction
           2) when the inpputAction is pperformed (clicked - due to binding), subscribe the Move function
        */
        mouseClickAction.Enable();
        mouseClickAction.performed += Move;
    }

    private void OnDisable() {
        mouseClickAction.performed -= Move;
        mouseClickAction.Disable();
    }

    private void Move(InputAction.CallbackContext context){
        /* 1) need to translate a point click to a position on the screen - do this with a ray from the camera
           2) 
        */
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        // The method from the video
        // if (Physics.Raycast(ray, hitInfo: out RaycastHit hit) && hit.collider) : you shouldnt need the 
        // && hit.collider, because it is already in an if statement
        if (Physics.Raycast(ray, hitInfo: out RaycastHit hit))
        {
            /* used StopAllCoroutines in stead of the following in 
                Private Coroutine coroutine (decleration)
                if (coroutine != null) StopCoroutine(coroutine)
            */
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));

            Debug.Log(hit.point);
            // for the Gizmo
            targetPosition = hit.point;
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        // for getting offset from the player object to the floor - otherwise when you click the gameobject
        // will go into the floor since the raycast hit goes to 0,0,0, i.e., the plan
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;

        // target will be the point hit - if the distance of the player gameObject's (with this script attached)
        // position is beyond threshold of being really close, then movetowards
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            transform.position = destination;
            // yield return null means the coroutine will wait until the next frame (e.g., instead of some # of seconds)
            yield return null;
        }
    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.2f);
    }

}
