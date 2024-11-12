using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{   
    public GameObject bulletPrefab;
    public Transform gunBarrel;
    public Transform armTransform;
    public float aimSpeed = 1f;
    public float bulletSpeed;
    public float zoomFOV = 30f;
    public float zoomSpeed = 10f;
    
    private float defaultFOV;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            defaultFOV = mainCamera.fieldOfView;
        }
        else
        {
            Debug.LogError("Main Camera not found! Please ensure a camera in the scene is tagged as MainCamera.");
        }
    }

    void Update()
    {
        if (mainCamera == null || gunBarrel == null || armTransform == null || bulletPrefab == null)
            return; // Exit if any essential component is missing

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000);
        }
        
        // Aim the arm towards the target point only on specific axes
        Vector3 direction = targetPoint - armTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate to the target direction using Slerp
        armTransform.rotation = Quaternion.Slerp(
            armTransform.rotation, 
            targetRotation, 
            Time.deltaTime * Mathf.Clamp(aimSpeed, 0.1f, 5f)
        );

        // Check for left mouse button click to shoot
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 shootDirection = (targetPoint - gunBarrel.position).normalized;
                rb.velocity = shootDirection * bulletSpeed;
            }
            else
            {
                Debug.LogError("Bullet prefab is missing a Rigidbody component!");
            }
        }
        
        // Zoom functionality
        if (Input.GetMouseButton(1))
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, defaultFOV, Time.deltaTime * zoomSpeed);
        }
    }
}
