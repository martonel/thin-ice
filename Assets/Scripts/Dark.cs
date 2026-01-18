using UnityEngine;

public class Dark : MonoBehaviour
{

    public PlayerSink playerSink;

    public void SetPlayerSink(PlayerSink sink)
    {
        playerSink = sink;
    }

    public void SinkUp()
    {
        GameObject[] roots = GameObject.FindGameObjectsWithTag("IceRoot");
        for (int i = 0; i < roots.Length; i++)
        {
            Destroy(roots[i].gameObject);
        }



        playerSink.InvokeEvents();



    }


}
