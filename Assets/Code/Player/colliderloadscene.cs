using UnityEngine;
using UnityEngine.SceneManagement; // Added this to make scene loading easier

public class colliderloadscene : MonoBehaviour
{
    //on trigger of collider load scene
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player enter collider load scene
        if (collision.CompareTag("Player"))
        {
            //load scene "8"
            SceneManager.LoadScene("1-2");
        }
    }
}
