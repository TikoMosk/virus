using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableVirus : MonoBehaviour
{
    public int currentHealth = 1;
    private ParticleSystem fireBall;
    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            if (fireBall == null)
            {
                GameObject child;
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    child = gameObject.transform.GetChild(i).gameObject;
                    if (child.name.Contains("BigExplosion"))
                    {
                        fireBall = child.GetComponent<ParticleSystem>();
                        fireBall.gameObject.SetActive(true);
                        break;
                    }
                }
            }
            StartCoroutine(KillTheVirus());
        }
    }

    private IEnumerator KillTheVirus()
    {
        fireBall.Play();
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
