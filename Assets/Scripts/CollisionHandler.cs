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

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Enemy":
                StartCrashSequence();
                break;
            default:
                Debug.Log("Somehow, this is neither friendly nor an enemy.");
                break;
        }
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
