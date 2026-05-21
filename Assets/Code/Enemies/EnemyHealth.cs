using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //enemy hp
    public float health = 30f;
    //if enemy has died
    private bool isDead = false;

    void Start()
    {
        //if (ScoreManager.instance != null)
        //{
        //    //show dead and alive ones text at start
        //    ScoreManager.remainingEnemies = ScoreManager.remainingEnemies + 1;
        //    ScoreManager.instance.UpdateUI();
        //}
    }
    //take damage from player,s pew pews
    public void TakeDamage(float amount)
    {
        health = health - amount;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }
    //dieeeee text basicly just updater of current beings dead or alive
    void Die()
    {
        isDead = true;

        //ScoreManager.killedEnemies = ScoreManager.killedEnemies + 1;
        //ScoreManager.remainingEnemies = ScoreManager.remainingEnemies - 1;

        //ScoreManager.instance.UpdateUI();

        Destroy(gameObject);
    }
}