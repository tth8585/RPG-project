﻿
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class MMO_Player_movement : MonoBehaviour
{
    //Var definition 
    public bool swimming = false;                    //Can be triggert to slow down the movements (like when u swim) 
    public bool walkingBack = false;
    public string moveStatus = "idle";               //movestatus for animations 

    //Movement speeds 
    private float jumpSpeed = 8.0f;                  //Jumpspeed / Jumpheight 
    private float gravity = 20.0f;                   //Gravity for jump 
    private float runSpeed = 8.0f;                  //Speed when the Character is running 
    private float walkSpeed = 3.0f;                  //Speed when the Character is walking (normal movement) 
    private float rotateSpeed = 250.0f;              //Rotationspeed of the Character 
    private float walkBackMod = 0.65f;               //Speed in Percent for walk backwards and sidewalk 

    //Internal vars to work with 
    private float speedMod = 0.0f;                   //temp Var for Speedcalculation 
    private bool grounded = false;                   //temp var if the character is grounded 
    private Vector3 moveDirection = Vector3.zero;    //move direction of the Character 
    private bool isWalking = false;                  //toggle var between move and run 
    private bool jumping = false;                    //temp var for jumping 
    private bool mouseSideButton = false;            //temp var for mouse side buttons 
    private float pbuffer = 0.0f;                    //Cooldownpuffer for SideButtons 
    private float coolDown = 0.5f;                   //Cooldowntime for SideButtons 
    private CharacterController controller;          //CharacterController for movement 

    bool isDancing = false;
    public bool isDead = false;
    bool isPlayerDragging = false;

    private void Start()
    {
        UIEvent.OnPlayerDragging += CheckDrag;
    }

    private void CheckDrag(bool isDrag)
    {
        isPlayerDragging = isDrag;
    }
    //Every Frame 
    void Update () 
    {
        if (isDead)
        {
            PlayerAnimationController.Instance.SetXDir(0);
            PlayerAnimationController.Instance.SetZDir(0);
        }
        //Set idel animation 
        moveStatus = "idle"; 
        isWalking = true;

        // Hold "Run" to run 
        if (Input.GetAxis("Run") != 0) 
        {
            isWalking = false;  
        }
        // Only allow movement and jumps while grounded 
        if (grounded && !isDead) { 
             
            //movedirection 
            moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0),0,Input.GetAxis("Vertical")); 
             
            //pushbuffer to avoid on/off flipping 
            if(pbuffer>0) 
                pbuffer -=Time.deltaTime; 
            if(pbuffer<0)pbuffer=0; 
                            
            //Automove Sidebuttonmovement 
            if((Input.GetAxis("Toggle Move") !=0) && pbuffer == 0){ 
                pbuffer=coolDown; 
                mouseSideButton = !mouseSideButton; 
            } 
            if(mouseSideButton && ((Input.GetAxis("Vertical") != 0) || Input.GetButton("Jump")) || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
                mouseSideButton = false;             
             
            //L+R MouseButton Movement 
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1) || mouseSideButton) 
                moveDirection.z += 1; 
            if (moveDirection.z > 1)
                moveDirection.z = 1;
                 
            //Strafing move (like Q/E movement     
            moveDirection.x -= Input.GetAxis("Strafing"); 
                 
              // if moving forward and to the side at the same time, compensate for distance 
              if(Input.GetMouseButton(1) && (Input.GetAxis("Horizontal") != 0) && (Input.GetAxis("Vertical") != 0)) { 
                moveDirection *= 0.7f; 
              } 
                               
            //Speedmodification / is moving forward or side/backward 
            speedMod = ((Input.GetAxis("Vertical") < 0) || (Input.GetMouseButton(1) && (Input.GetAxis("Horizontal")) != 0) || Input.GetAxis("Strafing") != 0) ? walkBackMod : 1.0f;
            //Use run or walkspeed 
            moveDirection *= isWalking ? walkSpeed * speedMod : runSpeed * speedMod;
             
            //reduce movement by 70% when swimming is toggled    
            moveDirection*= swimming ? 0.7f : 1;
           

            // Jump! 
            if (Input.GetButtonDown("Jump")){ 
                jumping = true; 
                moveDirection.y = jumpSpeed;
            }

            if (moveDirection != Vector3.zero)
            {
                isDancing = false;
            }

            //movestatus normal movement (for animations)
            if ((moveDirection.x == 0) && (moveDirection.z == 0))
            {
                moveStatus = "idle";
                if (!isDancing && !jumping && !PlayerAnimationController.Instance.isDoingSomething)
                {
                    PlayerAnimationController.Instance.AnimateIdle();
                }
            }
            if (moveDirection.z > 0)
            {
                moveStatus = isWalking ? "walking" : "running";

                if (!jumping && !PlayerAnimationController.Instance.isDoingSomething)
                {
                    if (moveStatus == "walking")
                    {
                        PlayerAnimationController.Instance.AnimateWalk();
                    }
                    else
                    {
                        PlayerAnimationController.Instance.AnimateRun();
                    }
                }
            }

            if (moveDirection.z < 0)
            {
                moveStatus = isWalking ? "backwalking" : "backrunning";

                if (!jumping && !PlayerAnimationController.Instance.isDoingSomething)
                {
                    if (moveStatus == "backwalking")
                    {
                        PlayerAnimationController.Instance.AnimateWalkB();
                    }
                    else
                    {
                        PlayerAnimationController.Instance.AnimateRunB();
                    }
                }
            }

            if (moveDirection.x > 0)
            {
                moveStatus = isWalking ? "sidewalking_r" : "siderunning_r";

                if (!jumping && !PlayerAnimationController.Instance.isDoingSomething)
                {
                    if (moveStatus == "sidewalking_r")
                    {
                        PlayerAnimationController.Instance.AnimateWalkR();
                    }
                    else
                    {
                        PlayerAnimationController.Instance.AnimateRunR();
                    }
                } 
            }

            if (moveDirection.x < 0)
            {
                moveStatus = isWalking ? "sidewalking_l" : "siderunning_l";
                if (!jumping && !PlayerAnimationController.Instance.isDoingSomething)
                {
                    if (moveStatus == "sidewalking_l")
                    {
                        PlayerAnimationController.Instance.AnimateWalkL();
                    }
                    else
                    {
                        PlayerAnimationController.Instance.AnimateRunL();
                    }
                }
            }

            PlayerAnimationController.Instance.SetXDir(Input.GetAxis("Horizontal"));
            PlayerAnimationController.Instance.SetZDir(Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Dance"))
            {
                if (!jumping && !PlayerAnimationController.Instance.isDoingSomething)
                {
                    isDancing = true;
                    PlayerAnimationController.Instance.AnimateDance();
                }
            }

            //movestatus swim movement (for animations)               
            if (swimming){ 
                if((moveDirection.x == 0 ) && (moveDirection.z == 0)) 
                    moveStatus = "swimidle";             
                if(moveDirection.z > 0) 
                    moveStatus = isWalking ? "swim" : "swimfast"; 
                if(moveDirection.z < 0) 
                    moveStatus = isWalking ? "backswim" : "backswimfast";         
                if(moveDirection.x > 0) 
                    moveStatus = isWalking ? "sideswim_r" : "sideswimfast_r"; 
                if(moveDirection.x < 0) 
                    moveStatus = isWalking ? "sidewswim_l" : "sideswimfast_l";     
                if(jumping) 
                    moveStatus = "swimup";                     
            }   
             
            //transform direction 
            moveDirection = transform.TransformDirection(moveDirection);
        }
        // Allow turning at anytime. Keep the character facing in the same direction as the Camera if the right mouse button is down. 
        if (!isPlayerDragging)
        {
            if (Input.GetMouseButton(1) && !isDead)
            {
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            }
            else
            {
                if (!isDead)
                {
                    transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
                }
            }
        }
        

        //Apply gravity 
        moveDirection.y -= gravity * Time.deltaTime;

        if (isDead)
        {
            moveDirection.x = 0;
            moveDirection.z = 0;
        }
         
        //Get CharacterController 
        controller = GetComponent<CharacterController>(); 
        //Move Charactercontroller and check if grounded 
        grounded = ((controller.Move(moveDirection * Time.deltaTime)) & CollisionFlags.Below) != 0; 
         
        //Reset jumping after landing 
        jumping = grounded ? false : jumping;

        //movestatus jump/swimup (for animations)       
        if (jumping) 
        {
            moveStatus = "jump";
            if (!isDead)
            {
                PlayerAnimationController.Instance.AnimateJump();
            }
        }

        if(jumping && swimming) 
            moveStatus = "swimup";

        ////dang le ra la o torng cast
        //if (Input.GetKeyUp(KeyCode.Alpha2))
        //{
        //    PlayerAnimationController.Instance.AnimateCast();
        //}
        if (Input.GetKeyUp(KeyCode.U))
        {
            PlayerAnimationController.Instance.AnimateAttack();
        }
    }
}
