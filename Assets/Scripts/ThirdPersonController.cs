using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThirdPersonController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public Transform hips;
    public Transform cam;
    public Transform shootLocation;


    public TMP_Text healthDisplay;
    public GameManager gameManager;

    public GameObject laserPrefab;
    
    [Range(0, 1)]
    public float inputDeadZone = 0;
    public float playerSpeed = 5;

    float g = -9.81f;
    public float gMultiplier = 0.5f;
    public int playerHealth = 10;
    Vector3 gravity;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        UpdateHealth();

    }
    private void Update()
    {
        if(playerHealth > 0)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 mover = new Vector3(horizontal, 0, vertical).normalized;
            if (controller.isGrounded && gravity.y < 0)
            {
                gravity.y = -2f;
            }
            if (mover.magnitude >= 0.1)
            {
                float targetAngle = Mathf.Atan2(mover.x, mover.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);

                //Debug.Log(horizontal);
                //if (controller.isGrounded)
                //{
                Vector3 moveVector = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * playerSpeed * Time.deltaTime;
                controller.Move(moveVector);
            }

            if (Input.GetButtonDown("Fire1")) Fire();

            gravity.y += g * gMultiplier;
            controller.Move(gravity * Time.deltaTime);


            if (vertical != 0)
            {
                hips.localEulerAngles = new Vector3(hips.localEulerAngles.x, Mathf.Rad2Deg * Mathf.Atan(horizontal / vertical), hips.localEulerAngles.z);
            }
            else
            {
                hips.localEulerAngles = new Vector3(hips.localEulerAngles.x, Mathf.Rad2Deg * Mathf.Asin(horizontal), hips.localEulerAngles.z);
            }

            //}
            if (vertical > inputDeadZone || horizontal > inputDeadZone)
            {
                animator.SetFloat("speedMultiplier", 1);
                animator.SetBool("isWalking", true);
            }
            else if (vertical < (inputDeadZone * -1) || horizontal < (inputDeadZone * -1))
            {
                animator.SetFloat("speedMultiplier", -1);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        

        if(playerHealth <= 0)
        {
            healthDisplay.enabled = false;
            controller.Move(Vector3.zero);
            animator.SetTrigger("killedTrigger");
            gameManager.PlayerIsDead();
        }
    }

    public void UpdateHealth()
    {
        healthDisplay.text = "+ " + playerHealth;
    }

    private void Fire()
    {
        Instantiate(laserPrefab, shootLocation.position, Quaternion.Euler(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z));
    }
}
