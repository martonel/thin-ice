using UnityEngine;
using UnityEngine.Events;

public class PlayerSink : MonoBehaviour
{

    public UnityEvent sinkEvents;
    public Animator darkAnim;
    public GameObject otherPlayer;

    public void DarkAnimPlay()
    {
        if (darkAnim != null)
        {
            darkAnim.Play("upsideDownFadeInAndOut");
            darkAnim.gameObject.GetComponent<Dark>().SetPlayerSink(this);
        }
        if (otherPlayer != null)
        {
            otherPlayer.transform.position = this.transform.position;
        }
    }
    public void InvokeEvents()
    {
        sinkEvents.Invoke();
    }


    public void SpriteSwich()
    {
        //todo after the fall the sprites switches
        GameObject[] spriteSwitcherObjects = GameObject.FindGameObjectsWithTag("UpsideDown");
        foreach (GameObject spriteSwitcherObject in spriteSwitcherObjects)
        {
            UpsideDownSpriteSwitcher udSpriteSwitcher = spriteSwitcherObject.GetComponent<UpsideDownSpriteSwitcher>();
            if (udSpriteSwitcher != null)
            {
                udSpriteSwitcher.SwitchSide();
            }
            UpsideDownSwitcher2 upsideDownSwitcher2 = spriteSwitcherObject.GetComponent<UpsideDownSwitcher2>();
            if(upsideDownSwitcher2 != null)
            {
                upsideDownSwitcher2.TurnSide();
            }
        }
    }
}
