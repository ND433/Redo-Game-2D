using UnityEngine;
using UnityEngine.SceneManagement; // Added this to make scene loading easier

public class colliderloadscenecoords : MonoBehaviour
{
    //on trigger of collider load scene
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player enter collider tp to coords
        if (collision.CompareTag("Player"))
        {
            //sent player to coords 34.6029 -33.7788 0
            collision.transform.position = new Vector3(34.6029f, -33.7788f, 0f);
        }
    }
}
