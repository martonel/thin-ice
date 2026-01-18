using UnityEngine;

public class AddHearth : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerHearth>().AddHealth();
                Destroy(this.gameObject);
            }
        }
    }
}
