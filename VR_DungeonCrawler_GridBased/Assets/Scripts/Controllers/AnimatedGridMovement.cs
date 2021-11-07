/**************************************************************
 * The AnimatedGridMovement script performs "Dungeon Master"/ * 
 * "Legend of Grimrock" style WSADEQ movement in your Unity3D *
 * game.                                                      *
 *                                                            *
 * Written by: Lutz Grosshennig, 06/27/2020                   *
 * MIT Licence                                                *
 * ************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class AnimatedGridMovement : MonoBehaviour
{
    [SerializeField]
    private LayerMask wallLayerMask;

    private const float LeftHand = -90.0f;
    private const float RightHand = +90.0f;

    [SerializeField] private float gridSize = 4.0f;
    public float GridSize
    {
        get { return gridSize; }
    }

    [SerializeField] private float movementSpeed = 1.0f;

    private Vector3 moveTowardsPosition;
    private Quaternion rotateFromDirection;
    private Quaternion rotateTowardsDirection;

    private Controls playerInput;
    private InputAction move;

    [SerializeField] private Transform mainCam;
    private Vector3[] axis = new Vector3[4];

    private void Awake()
    {
        axis[0] = Vector3.forward;
        axis[1] = -Vector3.forward;
        axis[2] = Vector3.right;
        axis[3] = -Vector3.right;


        playerInput = new Controls();
    }

    private void OnEnable()
    {
        //move = playerInput.Player.Move;
        //move.Enable();
        playerInput.Player.Move.performed += MoveAndRotate;
        playerInput.Player.Move.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Move.Disable();
    }
    void Start()
    {
        moveTowardsPosition = transform.position;
        rotateTowardsDirection = transform.rotation;
        rotateFromDirection = transform.rotation;
    }

    public void MoveAndRotate(InputAction.CallbackContext value)
    {
        var val = value.ReadValue<Vector2>();
        if (val.y > 0.5f)
        {
            MoveForward();
        }
        else if (val.y < -0.5f)
        {
            MoveBackward();
        }
        else if (val.x < -0.5f)
        {
            TurnLeft();
        }
        else if (val.x > 0.5f)
        {
            TurnRight();
        }
    }

    private void FixedUpdate()
    {
        if (IsStationary())
        {
            //Vector2 moveAndRotate = move.ReadValue<Vector2>();
           
            //if (moveAndRotate == new Vector2(0f, 1f))
            //{
            //    MoveForward();
            //}
            //else if (moveAndRotate == new Vector2(0f, -1f))
            //{
            //    MoveBackward();
            //}
            //else if (moveAndRotate == new Vector2(1f, 0f))
            //{
            //    TurnLeft();
            //}
            //else if (moveAndRotate == new Vector2(-1f, 0f))
            //{
            //    TurnRight();
            //}
            //else if (Input.GetKey(KeyCode.A))
            //{
            //    StrafeLeft();
            //}
            //else if (Input.GetKey(KeyCode.E))
            //{
            //    StrafeRight();
            //}
        }
    }
    void Update()
    {
        if (IsMoving())
        {
            var step = Time.deltaTime * gridSize * movementSpeed;
            //AnimateMovement(step);
            transform.position = moveTowardsPosition;
        }
        if (IsRotating())
        {
            //AnimateRotation();
            transform.rotation = rotateTowardsDirection;
        }
    }

    //private void AnimateRotation()
    //{
    //    rotationTime += Time.deltaTime;
    //    transform.rotation = Quaternion.Slerp(rotateFromDirection, rotateTowardsDirection, rotationTime * rotationSpeed);
    //    CompensateRotationRoundingErrors();
    //}

    //private void AnimateMovement(float step)
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, moveTowardsPosition, step);
    //}

    private void CompensateRotationRoundingErrors()
    {
        // Bear in mind that floating point numbers are inaccurate by design. 
        // The == operator performs a fuzy compare which means that we are only approximatly near the target rotation.
        // We may not entirely reached the rotateTowardsViewAngle or we may have slightly overshot it already (both within the margin of error).
        if (transform.rotation == rotateTowardsDirection)
        {
            // To compensate rounding errors we explictly set the transform to our desired rotation.
            transform.rotation = rotateTowardsDirection;
        }
    }

    private void MoveForward()
    {
        CollisonCheckedMovement(CalculateForwardPosition());
    }

    private void MoveBackward()
    {
        CollisonCheckedMovement(-CalculateForwardPosition());
    }

    //private void StrafeRight()
    //{
    //    CollisonCheckedMovement(CalculateStrafePosition());
    //}

    //private void StrafeLeft()
    //{
    //    CollisonCheckedMovement(-CalculateStrafePosition());
    //}

    private void CollisonCheckedMovement(Vector3 movementDirection)
    {
        Vector3 targetPosition = moveTowardsPosition + movementDirection;
        
        if (CollisionDetection(targetPosition))
        {
            moveTowardsPosition = targetPosition;
        }
    }

    /// <summary>
    /// The collision detection method
    /// </summary>
    /// <returns></returns>
    private bool CollisionDetection(Vector3 target)
    {
        bool canMove = true;
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), target - transform.position, out hit, gridSize, wallLayerMask)){
            canMove = false;
        }
        return canMove;
    }

    private void TurnRight()
    {
        TurnEulerDegrees(RightHand);
    }

    private void TurnLeft()
    {
        TurnEulerDegrees(LeftHand);
    }

    private void TurnEulerDegrees(in float eulerDirectionDelta)
    {
        rotateFromDirection = transform.rotation;
        rotateTowardsDirection *= Quaternion.Euler(0.0f, eulerDirectionDelta, 0.0f);
    }

    private bool IsStationary()
    {
        return !(IsMoving() || IsRotating());
    }

    private bool IsMoving()
    {
        return transform.position != moveTowardsPosition;
    }

    private bool IsRotating()
    {
        return transform.rotation != rotateTowardsDirection;
    }

    private Vector3 CalculateForwardPosition()
    {
        Vector3 forwardPosition = transform.forward;
        float angle = float.MaxValue;
        for(int i =0; i<4; i++)
        {
            float newAngle = Mathf.Abs(Vector3.Angle(mainCam.forward, axis[i]));
            if (newAngle < angle)
            {
                forwardPosition = axis[i];
                angle = newAngle;
            }
        }
        return forwardPosition * gridSize;
    }

    //private Vector3 CalculateStrafePosition()
    //{
    //    return transform.right * gridSize;
    //}
}
