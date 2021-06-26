using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public Transform forwardPointer;
    public LayerMask layerMaskWallDetection;
    public LayerMask layerMaskPlayerDetection;
    public GameObject bulletPrefab;



    public float moveSpeed = 5f;
    public float distance = 10f;
    public int enemyHealth = 2;

    public float detectionRadius = 20f;
    [Range(0,360)]
    public float detectionAngle = 45f;

    private bool hasFoundPlayer = false;

    public Vector3 DirectionFromAngle(float eulerAngle, bool isGlobal)
    {
        if(!isGlobal)
        {
            eulerAngle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(eulerAngle * Mathf.Deg2Rad), 0, Mathf.Cos(eulerAngle * Mathf.Deg2Rad));
    }


    void PlayerDetection()
    {
        Collider[] targetsFound = Physics.OverlapSphere(transform.position, detectionRadius, layerMaskPlayerDetection);
        
        if(targetsFound.Length != 0)
        {
            Transform target = targetsFound[0].transform; //since there is always only one player in a scene, this will check if the player is found
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            if (angle < detectionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, layerMaskWallDetection))
                {
                    Instantiate(bulletPrefab, forwardPointer.position, Quaternion.Euler(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z + angle));
                }

            }
        }
    }

    

    IEnumerator Detector()
    {
        while(true)
        {
            PlayerDetection();
            yield return new WaitForSeconds(1f);
        }
    }
    

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true); //starts walking
        animator.SetFloat("speedMultiplier", 1f); 
        StartCoroutine(Detector());
    }

    private void Update()
    {
        
        if (!hasFoundPlayer && enemyHealth > 0)
        {
            Ray ray = new Ray(forwardPointer.position, transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
            Physics.Raycast(ray, out RaycastHit hit, distance, layerMaskWallDetection);
            if (hit.collider != null)
            {
                if (hit.collider.name == "Map")
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 45, transform.eulerAngles.z);
                }
            }
            Vector3 moveVector = transform.forward * moveSpeed * Time.deltaTime;
            controller.Move(moveVector);
        }

        if (enemyHealth <= 0)
        {
            controller.Move(Vector3.zero);
            animator.SetTrigger("killedTrigger");
            Destroy(gameObject, 2);
        }

    }
}
