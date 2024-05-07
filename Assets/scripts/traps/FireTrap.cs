using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("FireTrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;
    private bool triggered;
    private bool active;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        print(anim);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {

            if (!triggered) {

                StartCoroutine(ActivateFireTrap());


            }


        }
    }

    private IEnumerator ActivateFireTrap() {

        print("fire trap is working");
        triggered = true;
        spriteRend.color = Color.red;
        
        yield return new WaitForSeconds(activationDelay);
        active = true;

        spriteRend.color = Color.white;
        anim.SetBool("activated", true);
        
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);


    }

}
