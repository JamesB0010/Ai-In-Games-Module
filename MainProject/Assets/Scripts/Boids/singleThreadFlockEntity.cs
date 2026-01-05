using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class singleThreadFlockEntity : MonoBehaviour
{
private float speed = 1f;
    public float Speed => this.speed;

    private Transform player;

    public Transform Player
    {
        set => this.player = value;
    }

    [SerializeField] private float rotSpeed = 3f;

    private float neighbourDistance = 10f;

    private bool turning = true;

    private void Start()
    {
        this.speed = Random.Range(0.5f, 2f);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= singleThreadGlobalFlock.AirSize)
        {
            this.turning = true;
        }
        else if(transform.position.y <= 10f)
        {
            this.turning = true;
        }
        else
        {
            turning = false;
        }

        if (this.turning)
        {
            Vector3 direction = player.transform.position + new Vector3(0, Random.Range(10f, singleThreadGlobalFlock.AirSize)) -transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                this.rotSpeed * Time.deltaTime);
            this.speed = Random.Range(0.5f, 3f);
        }
        if (Random.Range(0, 5) < 1) //20% do steering forces on each flock entity
        {
            //good stuff happens!!!!
            singleThreadFlockEntity[] birds = singleThreadGlobalFlock.FlockEntities;

            Vector3 vCentre = this.transform.position;
            Vector3 vAvoid = this.transform.position;


            float gSpeed = 0.5f;
            Vector3 goalPos = singleThreadGlobalFlock.instance.GoalPos;

            float dist;
            int groupSize = 0;

            foreach (singleThreadFlockEntity bird in birds)
            {
                if (bird != this)
                {
                    dist = Vector3.Distance(bird.transform.position, this.transform.position);
                    if (dist <= this.neighbourDistance)
                    {
                        vCentre += bird.transform.position;
                        groupSize++;
                        if (dist < 6f)
                        {
                            vAvoid = vAvoid + (this.transform.position - bird.transform.position); //separation
                        }
                        gSpeed = gSpeed + bird.speed;
                    }
                }
            }

            if (groupSize >= 0)
            {
                vCentre = vCentre / groupSize + (goalPos - this.transform.position); //cohesion
                this.speed = (gSpeed / groupSize) + Random.Range(-0.1f, 0.1f);
                if (this.speed > 5f)
                {
                    this.speed = Random.Range(0.5f, 3f);
                }

                Vector3 direction = (vCentre + vAvoid).normalized; // alignment
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotSpeed * Time.deltaTime);
            }
        }
        transform.Translate(0,0,Time.deltaTime * this.speed);
    }
}
