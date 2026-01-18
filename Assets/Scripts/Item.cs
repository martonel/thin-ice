using UnityEngine;

public class Item : MonoBehaviour
{
    private LevelManager levelManager;
    public int itemScore = 5;

    public string itemType = "simpleIceCream";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("LevelManager");
        if(obj != null)
        {
            levelManager = obj.GetComponent<LevelManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {




            levelManager.AddScore(itemScore);
            Destroy(this.gameObject);
        }
    }



}
