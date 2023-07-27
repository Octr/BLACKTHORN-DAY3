using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public ParticleSystem lowHealthParticle;
    public float lowHealthThreshold = 30f;

    public float flipForce = 1000f;

    private Rigidbody rb;
    private bool isFlipped = false;

    private MovementController movementController;

    public BustedScreen deathPanel;
    public Vector3 targetPanelPosition;

    public float forceMagnitude = 10f;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        if (currentHealth <= 0f && !isFlipped)
        {
            FlipObject();
        }
        else if (currentHealth <= lowHealthThreshold)
        {
            PlayLowHealthParticle();
        }
        else
        {
            lowHealthParticle.Stop();
        }
    }

    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    private void PlayLowHealthParticle()
    {
        if (lowHealthParticle != null && !lowHealthParticle.isPlaying)
        {
            lowHealthParticle.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Police"))
        {
            ModifyHealth(-10);

            PoliceAi policeAI = collision.gameObject.GetComponent<PoliceAi>();
            policeAI.agent.SetDestination(collision.gameObject.transform.position *-0.01f); // Set the destination to self to stop moving forward?
            policeAI.isWaiting = true;
            policeAI.StartCoroutine("Cooldown");

            //Knockback
            Vector3 oppositeForceDirection = -collision.contacts[0].normal;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(oppositeForceDirection * forceMagnitude, ForceMode.Impulse);
        }

    }

    private void FlipObject()
    {
        isFlipped = true;

        Quaternion targetRotation = Quaternion.Euler(180f, transform.rotation.eulerAngles.y, 0f);

        float flipDuration = 1.0f; 
        StartCoroutine(RotateObjectSmoothly(targetRotation, flipDuration));

        if (movementController != null)
        {
            movementController.enabled = false;
        }

        //if (deathPanel != null)
        //{
            //deathPanel.targetPosition = targetPanelPosition;
            //deathPanel.gameObject.SendMessage("SlideOnScreenSmoothly");
            BustedScreen.Instance.DoThisPlease(); // I was having a bad day today
        //}

    }

    private IEnumerator RotateObjectSmoothly(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
