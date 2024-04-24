using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

[SerializeField] private float moveSpeed = 7f;
[SerializeField] public GameInput gameInput;
[SerializeField] private LayerMask countersLayerMask;

[SerializeField] private float playerRaduis = .7f;
[SerializeField] private float playerHeight = 2f;
private bool isWalking;
private Vector3 lastInteractDir;

private void Start(){
 gameInput.OnInteractAction += GameInput_OnInteractAction;
 }

private void GameInput_OnInteractAction(object sender, System.EventArgs e){
     float interactDistance = 2f;

    Vector2 inputVector = gameInput.GetMovementVectorNormalised();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    if(moveDir != Vector3.zero){

        lastInteractDir = moveDir;
    }

   if(Physics.Raycast(transform.position, lastInteractDir ,out RaycastHit raycastHit, interactDistance, countersLayerMask))
   {
    if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
        {
            clearCounter.Interact();
        }

   }    
}
private void Update()
{
    HandleMovement();
    HandleInteractions();
}

public bool IsWalking(){

    return isWalking;
}
private void HandleInteractions(){
    float interactDistance = 2f;

    Vector2 inputVector = gameInput.GetMovementVectorNormalised();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    if(moveDir != Vector3.zero){

        lastInteractDir = moveDir;
    }

   if(Physics.Raycast(transform.position, lastInteractDir ,out RaycastHit raycastHit, interactDistance, countersLayerMask))
   {
    if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
        {
            //clearCounter.Interact();
        }

   }    
   else
   {
        Debug.Log("-");
   }
}
private void HandleMovement(){

    Vector2 inputVector = gameInput.GetMovementVectorNormalised();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
    
    float moveDistance = moveSpeed * Time.deltaTime;


    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDir,moveDistance);
    
    if(!canMove){
            // can move only in X direction
        Vector3 moveDirX = new Vector3( moveDir.x, 0 ,0).normalized;
        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDirX,moveDistance);
    
        if(canMove){

            moveDir = moveDirX;
        } else {
            // Can move only in z direction
            Vector3 moveDirZ = new Vector3(0,0, moveDir.z).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDirZ,moveDistance);
            if(canMove){

                moveDir = moveDirZ;
            } else {
                //Cant Move in any direction
            }
        
        
        }
    
    }
    
    if(canMove)
    {
        transform.position += moveDistance * moveDir;
    }
    isWalking = moveDir != Vector3.zero;
    float rotationSpeed = 10f;
    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
}
}
