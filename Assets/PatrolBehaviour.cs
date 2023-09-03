using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : StateMachineBehaviour
{
    float timer;

    List<Transform> waypoints = new List<Transform>();
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    NavMeshAgent agent;
    Transform currentWaypoint;

    Transform player;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform wayPointsObject = GameObject.FindGameObjectWithTag("Waypoints").transform;
        foreach (Transform t in wayPointsObject)
        {
            waypoints.Add(t);
        }

        agent = animator.GetComponent<NavMeshAgent>(); // Получение дитя-компонента NavMeshAgent 
        currentWaypoint = waypoints[Random.Range(0, waypoints.Count)];
        agent.SetDestination(currentWaypoint.position);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    float chaseRange = 10;
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance-2 <= agent.stoppingDistance)
        {
            waypoints.Remove(currentWaypoint);
            currentWaypoint = waypoints[Random.Range(0, waypoints.Count)];
            agent.SetDestination(currentWaypoint.position);
        }

        timer += Time.deltaTime;
        if (timer > 40)
            animator.SetBool("is_patrolling", false);

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < chaseRange)
        {
            animator.SetBool("is_chasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
