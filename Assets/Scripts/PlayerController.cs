using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [Tooltip("Player movement in meters/seconds")]
        [SerializeField] private float m_positionSpeed; //position speed in meters/seconds
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Transform m_cameraTransform;

    [Header("Ball Interactions")]
    [SerializeField] Transform m_ballPointReference;
    [SerializeField] Transform m_ballTransform;
    [SerializeField] Rigidbody m_ballRigidBody;
    [SerializeField] float m_shootingForce;
    [SerializeField] float m_retrievalForce;
    private bool m_isShot = false;

    private void Awake()
    {
        // lock and hide cursor when in play mode
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Lock mouse to the center and hide until pressing ESC
    private void Update()
    {
        MovePlayer();
        RotatePlayer();
        LaunchBall();
    }



    // Receive key codes to move position
    private void MovePlayer()
    {
        //change position speed to meters/seconds
        float m_posFinalSpeed = m_positionSpeed * Time.deltaTime;
        
        // move objects depending on which key is being pressed
        if (Input.GetKey(KeyCode.W)) // move forward
        {
            m_playerTransform.Translate(Vector3.forward * m_posFinalSpeed);
        }

        if (Input.GetKey(KeyCode.S)) // move backwards
        {
            m_playerTransform.Translate(Vector3.back * m_posFinalSpeed);
        }
        if (Input.GetKey(KeyCode.A)) // move to the right
        {
            m_playerTransform.Translate(Vector3.left * m_posFinalSpeed);
        }
        if (Input.GetKey(KeyCode.D)) // move to the left
        {
            m_playerTransform.Translate(Vector3.right * m_posFinalSpeed);
        }

    }


    // Rotate the player ONLY on the Y axis as the mouse moves left or right
    private void RotatePlayer()
    {
        // calculate new side to side rotation angle
        float newYRotation = m_playerTransform.rotation.eulerAngles.y + m_rotationSpeed * Input.GetAxis("Mouse X");

        // apply side to side rotation to prefab
        m_playerTransform.rotation = Quaternion.Euler(0, newYRotation, 0);


        //// calculate new camera orbit rotation angle
        float newXRotation = Mathf.Clamp(m_cameraTransform.localRotation.eulerAngles.x + m_rotationSpeed * Input.GetAxis("Mouse Y"), 271f, 327f);

        // apply new rotation
        m_cameraTransform.localRotation = Quaternion.Euler(new Vector3(newXRotation, 180, 180));

    }

    // Impulse the ball forward from the player when within 2 meters - otherwise, apply small force to ball to get it closely to player
    private void LaunchBall()
    {

        // Hit ball only if not in shot mode
        if (Input.GetKeyDown(KeyCode.Space) && !m_isShot)
        {
            m_ballRigidBody.AddForce(transform.forward * m_shootingForce, ForceMode.Impulse);
            m_isShot = true;
        }
        
        // guard clause, if ball is closer than 2 meters, its not in active shooting mode
        if (Vector3.Distance(m_ballTransform.position, m_ballPointReference.position) < 2)
        {
            m_isShot = false;
            return;
        }
       
        // help ball get closer to player if further than 2 meters
        m_ballRigidBody.AddForce(Vector3.Normalize(m_ballPointReference.position - m_ballTransform.position) * m_retrievalForce, ForceMode.Force);
        m_isShot = true;

    }







}
