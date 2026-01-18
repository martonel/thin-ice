using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PolygonCollider2D))]
public class WaterFallTrigger : MonoBehaviour
{

    private PolygonCollider2D waterCollider;
    private bool triggered;

    private void Awake()
    {
        waterCollider = GetComponent<PolygonCollider2D>();
        waterCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        if (IsColliderFullyInside(other, waterCollider))
        {
            triggered = true;
            //onPlayerFallIntoWater.Invoke();
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            PlayerSlideMovement playerSlideMovement = playerObj.GetComponent<PlayerSlideMovement>();
            if (playerSlideMovement != null)
            {
                playerSlideMovement.isEnd();
            }
            SpriteSwitcher[] spriteSwitcher = playerObj.GetComponentsInChildren<SpriteSwitcher>();
            if (spriteSwitcher != null)
            {
                spriteSwitcher[0].IsEnd();
                spriteSwitcher[1].IsEnd();
            }
            Animator anim = playerObj.GetComponent<Animator>();
            if (anim != null)
            {

                Debug.Log("vége jól");
                anim.Play("playerSink");

                //todo ide kell a fordítás
            }
            else
            {
                Debug.Log("vége rosszul");
            }

                //Time.timeScale = 0f;
        }
    }

    bool IsColliderFullyInside(Collider2D target, PolygonCollider2D container)
    {
        Bounds b = target.bounds;

        Vector2[] testPoints =
        {
            new Vector2(b.min.x, b.min.y),
            new Vector2(b.min.x, b.max.y),
            new Vector2(b.max.x, b.min.y),
            new Vector2(b.max.x, b.max.y)
        };

        foreach (var p in testPoints)
        {
            if (!container.OverlapPoint(p))
                return false;
        }

        return true;
    }


}
