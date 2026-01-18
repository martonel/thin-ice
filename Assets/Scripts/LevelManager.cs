using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int enemyNumber;
    public Animator levelEndAnim;
    public int score;
    public TMP_Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject enemiesParent = GameObject.FindGameObjectWithTag("Enemies");

        enemyNumber = enemiesParent.transform.childCount;
    }

    public void subFromEnemyOneNumber()
    {
        enemyNumber--;
        if(enemyNumber == 0)
        {
            StartCoroutine(LevelEnd(0.7f));
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = "SCORE: " + this.score
            ;
    }

    private IEnumerator LevelEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = 0f;
        if (levelEndAnim != null) {
            levelEndAnim.Play("GameOverAnim");
        }
        Debug.Log("level completed!");
    }


}
