using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public float speed = 7.0f;
    public static float speedIncrease = 0f;


    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * (speed + speedIncrease) * Time.deltaTime, Space.World);
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
}
