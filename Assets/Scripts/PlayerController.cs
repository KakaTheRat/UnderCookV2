using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Camera playerCamera;
    private Vector2 lookInput;
    private float pitch = 0f;

    [Header("Raycast Interactable Items Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI")]
    [SerializeField] InterractionCanvas interactionMenuManager;


    private InteractableObjects[] interactableObjects;
    private Animator animator;
    private GameObject underWiewItem = null;
    private List<GameObject> holdingItem = new() {null, null};
    private bool controller = false;
    private bool mouseUnlock = false;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        interactableObjects = FindObjectsOfType<InteractableObjects>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        controller = context.control.device is Gamepad;
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnMouseUnlock(InputAction.CallbackContext context){
        if(context.started){
            mouseUnlock = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if(context.canceled){
            mouseUnlock = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            DisableAllOutlinesAndHideMenu();
        }
    }

    void FixedUpdate(){
        HandleMovement();
        if(!mouseUnlock) HandleCamera();
        else HandleHover();
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        move = transform.TransformDirection(move);
        Vector3 targetVelocity = move * moveSpeed;

        if(!rb.isKinematic){
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        }

        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        animator.SetFloat("Speed", currentSpeed);
    }

    private void HandleCamera()
    {
        float multipy = 0.2f;
        if(controller){multipy = 2f;Debug.Log("Controller");}
        if(!rb.isKinematic){
            float yaw = lookInput.x * mouseSensitivity * multipy;
            transform.Rotate(Vector3.up * yaw);
        }

        pitch -= lookInput.y * mouseSensitivity * multipy;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }



    private void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance,interactableLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green);
            InteractableObjects item = hit.collider.GetComponent<InteractableObjects>();
            if (item != null && item.gameObject != underWiewItem)
            {
                item.ActivateOutline(true);
                item.UpdateAndShowInteractionMenu(interactionMenuManager);
                underWiewItem = hit.collider.gameObject;
                DisableAllOutlinesAndHideMenu(item);
                return;
            }
        }
    }

    public GameObject GetHoldingItem(int handID){
        return holdingItem[handID];
    }

    private void DisableAllOutlinesAndHideMenu(InteractableObjects Ignored = null)
    {
       foreach (var outinteractableObject in interactableObjects)
       {    
            if(Ignored != outinteractableObject){
                outinteractableObject.ActivateOutline(false);
            }
       }
       if(Ignored == null){
            underWiewItem = null;
            interactionMenuManager.SetMenuActive(false);
       }
    }

    public void HoldItem(GameObject itemToHold, int handId){
        holdingItem[handId] = itemToHold;
        Quaternion previousRotation = itemToHold.transform.rotation;
        Debug.Log($"HoldingPlaceHolder{handId}");
        holdingItem[handId].transform.SetParent(GameObject.FindGameObjectWithTag($"HoldingPlaceHolder{handId}").transform);
        holdingItem[handId].transform.localPosition =  new Vector3(0f, 0f,0f);
        holdingItem[handId].transform.localRotation = previousRotation;
    }

    // public void OnInteract(InputAction.CallbackContext context){
    //     if(context.phase == InputActionPhase.Performed){
    //         if(underWiewItem == null) return;
    //         InteractableObjects item = underWiewItem.GetComponent<InteractableObjects>();
    //         if((item.GetCanBeInteracted(0) || item.GetCanBeInteracted(1))  && (item is Mirror || item is Placard)){
    //             item.Interact(0);
    //         } 
    //     }
    // }

    public void DestroyHoldingItem(int handId){
        Destroy(holdingItem[handId]);
        ReleaseItem(handId);
    }

    public void ReleaseItem(int handId){
        holdingItem[handId] = null;
        animator.SetBool("Holding", false);
    }

    public void Static(bool _static){
        if(_static){
            rb.isKinematic = true;
            return;
        }
        rb.isKinematic = false;
    }

    public void PauseTime(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed){
            if(Time.timeScale == 0f){
                Time.timeScale = 1f;
                return;
            }
            Time.timeScale = 0f;
        }
    }

    public void ToggleCutAnim(bool _bool){
        if(_bool){
            animator.SetTrigger("CutStart");
            return;
        }
        animator.SetTrigger("CutEnd");
    }

    public void Emote(){
        string[] triggerNames = {"Hello", "Chockbar"};
        animator.SetTrigger(triggerNames[Random.Range(0,triggerNames.Length)]);
    }
}
