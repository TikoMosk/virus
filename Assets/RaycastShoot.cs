using UnityEngine;
using System.Collections;

public class RaycastShoot : MonoBehaviour
{
    public GameObject projectile;
    public int gunDamage = 1;                                            
    public float fireRate = 0.25f;                                        
    public float weaponRange = 100f;                                        
    public float hitForce = 100f;                                        
    public Transform gunEnd;                                            

    private Camera fpsCam;                                               
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private WaitForSeconds destroyBullet = new WaitForSeconds(1);
    private LineRenderer laserLine;                                        
    private float nextFire;
    private GameObject bullet;
    private ParticleSystem muzzleFlashParticle;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();
    }


    void Update()
    {
        if ((Input.GetButtonDown("Fire1") && Time.time > nextFire) || (Input.touchCount > 0 && Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            bullet = Instantiate(projectile, gunEnd.position, Quaternion.identity) as GameObject;
            bullet.transform.localRotation = Quaternion.Euler(0, 90, 180);
            Collider bulletCollider = bullet.GetComponent<BoxCollider>();
            StartCoroutine(DestroyBullet());

            if (Physics.Raycast(rayOrigin, gunEnd.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                ShootableVirus health = hit.collider.GetComponent<ShootableVirus>();

                if (health != null)
                {
                    health.Damage(gunDamage);
                    Debug.LogError("Health = " + health);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(hit.normal * hitForce);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (gunEnd.transform.forward * weaponRange));
            }
        }

        if (muzzleFlashParticle == null)
        {
            GameObject child;
            for(int i =0;i < gameObject.transform.childCount;i++)
            {
                child = gameObject.transform.GetChild(i).gameObject;
                if(child.name.Contains("MuzzleFlash01"))
                {
                    muzzleFlashParticle = child.GetComponent<ParticleSystem>();
                    muzzleFlashParticle.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        muzzleFlashParticle.Play();
        laserLine.enabled = false;

    }

    private IEnumerator DestroyBullet()
    {
        bullet.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5000);
        yield return destroyBullet;
        Destroy(bullet);
    }
}