using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTargets : MonoBehaviour
{

    NavMeshAgent m_navAgent;
    public Animator animator;
    public bool randomTargets = true;
    public GameObject[] targets;
    public float distanceToChangeTarget = 1f;
    public float waitBeforeChangeTarget = 1f;

    [SerializeField]float dist;
    int i;
    float time;

    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();
        RandomTarget();
    }

    private void Update()
    {
        dist = Vector3.Distance(targets[i].transform.position, gameObject.transform.position);

        if (dist < distanceToChangeTarget)
        {
            time += Time.deltaTime;
            if (time >= waitBeforeChangeTarget)
            {
                if (randomTargets)
                {
                    RandomTarget();
                    SetTrigger();
                }
                else
                {
                    NextTarget();
                    SetTrigger();
                }
                    

            }
        }
    }

    private void SetTrigger()
    {
        animator.SetTrigger("Walk");
    }

    private void RandomTarget()
    {
        i = Random.Range(0, targets.Length);
        SetDestination();
    }
    private void NextTarget()
    {
        i++;

        if (i == targets.Length)
        {
            i = 0;
        }
        SetDestination();
    }
    private void SetDestination()
    {
        m_navAgent.SetDestination(targets[i].transform.position);
        time = 0f;
    }

    

}
