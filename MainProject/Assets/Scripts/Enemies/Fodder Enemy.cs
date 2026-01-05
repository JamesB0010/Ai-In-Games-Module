using UnityEngine;

public class FodderEnemy : EnemyBase
{
    Transform playerTransform;

    [SerializeField]
    private float speed;

    private void Start()
    {
        this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 direction = (playerTransform.position - transform.position);

        direction.y = 0;

        Vector3 velocity = direction * this.speed * Time.deltaTime;

        transform.position += velocity;

        transform.forward = direction;
    }
}
