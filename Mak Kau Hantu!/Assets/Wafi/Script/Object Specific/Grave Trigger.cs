using UnityEngine;

public class GraveTrigger : MonoBehaviour
{
    public int requiredItems = 5; // How many objects must be deposited
    private int currentItems = 0;

    public GameObject endScreenUI; // Assign in Inspector (optional)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            currentItems++;
            Debug.Log("Item deposited. Total: " + currentItems);

            // Optionally destroy or disable the object
            Destroy(other.gameObject);

            /*if (currentItems >= requiredItems)
            {
                EndGame();
            }*/
        }
    }

    /*void EndGame()
    {
        Debug.Log("All items deposited. Game over.");
        if (endScreenUI != null)
            endScreenUI.SetActive(true);

        // Freeze time or disable player movement
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    } */
        }