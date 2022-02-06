using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{
    // Fade out screen between scenes
    // Disable animation

    [SerializeField] ParticleSystem deathVFX;
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] GameObject playerShipCollider;
    [SerializeField] GameObject playerShipMesh;

    [SerializeField] int playerHealth;

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Player collided with a friendly.");
                break;
            case "Enemy":
                Debug.Log($"Player collided with enemy {other.transform.name}");
                ProcessPlayerDamage(1);
                break;
            case "EnemyWeapon":
                Debug.Log($"Player was hit by {other.transform.name}");
                ProcessPlayerDamage(other.GetComponent<EnemyAttack>().bulletDamage);
                break;
            default:
                Debug.Log($"Player collided with {other.transform.name}.");
                break;
        }
    }

    public void ProcessPlayerDamage(int damage)
    {
        playerHealth -= damage;

        if (playerHealth >= 1)
            ChangePlayerHealthbar(damage);
            
        else if (playerHealth < 1)
            StartCrashSequence();

    }

    private void ChangePlayerHealthbar(int damage)
    {
        Debug.Log($"Player health is now {playerHealth}!");
    }
    void StartCrashSequence()
    {
        deathVFX.Play();
        GetComponent<PlayerControls>().enabled = false;
        playerShipCollider.SetActive(false);
        playerShipMesh.SetActive(false);
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
