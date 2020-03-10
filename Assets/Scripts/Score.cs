using Player;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;

    private void OnTriggerEnter(Collider other)
    {
        var pc = other.GetComponent<PlayerControl>();
        if (pc == null) return;
        pc.AddHealth(score);
        Destroy(gameObject);
    }
}