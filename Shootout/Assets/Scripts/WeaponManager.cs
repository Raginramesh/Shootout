using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public bool autoAim;
    public bool autoShoot;

    public Transform projectileStartPos;
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;


    private LineRenderer projectilePath;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private float nextFire;

    public PlayerInputHandler m_InputHandler;
    public Camera cam;

    void Start()
    {
        projectilePath = this.GetComponent<LineRenderer>();
        m_InputHandler = this.GetComponent<PlayerInputHandler>();
        //cam = this.GetComponent<Camera>();
    }

    
    void Update()
    {
        //if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        if (m_InputHandler.currentTouchStatus == PlayerInputHandler.TouchStatus.Tap && Time.time > nextFire)
        {
            Debug.Log("Fire");
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());

            // Create a vector at the center of our camera's viewport
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            projectilePath.SetPosition(0, projectileStartPos.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                projectilePath.SetPosition(1, hit.point);

                // Get a reference to a health script attached to the collider we hit
                //ShootableBox health = hit.collider.GetComponent<ShootableBox>();

                // If there was a health script attached
                //if (health != null)
                //{
                //    // Call the damage function of that script, passing in our gunDamage variable
                //    health.Damage(gunDamage);
                //}

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
                else
                {
                    // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                    projectilePath.SetPosition(1, rayOrigin + (cam.transform.forward * weaponRange));
                }
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        projectilePath.enabled = true;
        yield return shotDuration;
        projectilePath.enabled = false;
    }
}
