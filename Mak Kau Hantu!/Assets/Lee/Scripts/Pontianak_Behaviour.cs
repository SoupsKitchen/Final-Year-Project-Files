using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Pontianak_Behaviour : MonoBehaviour
{
    //Time Table related Params
    public List<Transform> patrolSpots;
    public Transform patrolCenter;
    private int _patrolIdx = 0;

    //Animations Params
    public Animator anim;

    //Detection Params
    public float visionAngle;
    private float _halfCone;
    private bool _seesPlayer = false;

    //Mood Params
    private float _anger
    {
        get => _anger;
        set => _anger = Mathf.Clamp(value, 0f, 100f);
    }
    public float _interest
    {
        get => _interest;
        set => _interest = Mathf.Clamp(value, 0f, 100f);
    }

    //Movement Params
    public float maxMoveSpeed, turningSpeed, accelSpeed;
    public Transform targetLocation;
    public Transform player;
    public NavMeshAgent agent;

    //State Params
    private StateMachine _stateMachine;
    private IState patrolState, idleState, stalkState, chaseState;
    private List<IState> _allStates;

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new StateMachine();
        patrolState = new State_Patrol(this);
        idleState = new State_Idle(this);
        stalkState = new State_Stalk(this);
        chaseState = new State_Chase(this);

        _allStates = new List<IState> { patrolState, idleState, stalkState, chaseState };

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _halfCone = visionAngle / 2 * Mathf.Deg2Rad;
        StartCoroutine(Look());
    }

    void Update()
    {
        _stateMachine.Update();
    }

    public IEnumerator Look()
    {
        while (true)
        {
            float _cosFOV = Mathf.Cos(_halfCone);
            Vector3 toPlayer = (player.position - transform.position).normalized;
            float dp = Vector3.Dot(transform.forward, toPlayer);
            RaycastHit hit;

            if (dp > _cosFOV && Physics.Raycast(transform.position, player.position - transform.position, out hit, 100f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _seesPlayer = true;
                    targetLocation = player;

                    if (_anger >= 80f)
                    {
                        Debug.Log("Anger high enough, enter Chase!");
                        _stateMachine.ChangeState(chaseState);
                    }
                    else
                    {
                        _interest += 5f; // Increase interest gradually
                        Debug.Log("Anger too low. Gaining interest... Interest: " + _interest);
                        _stateMachine.ChangeState(stalkState);
                    }
                }
            }
            else
            {
                _seesPlayer = false;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void IncreaseAnger(float amount)
    {
        _anger += amount;
        Debug.Log("Anger Increased: " + _anger);
    }
    public void UpdatePatrol()
    {
        patrolCenter = patrolSpots[_patrolIdx];
        agent.SetDestination(patrolSpots[_patrolIdx].position);
        _stateMachine.ChangeState(patrolState);
        _patrolIdx = (_patrolIdx + 1) % patrolSpots.Count;
        Debug.Log("Next Patrol Index: " + _patrolIdx);
    }
    public IEnumerator RandomizeState()
    {
        while (true)
        {
            int idx = Random.Range(0, _allStates.Count);
            _stateMachine.ChangeState(_allStates[idx]);
            yield return new WaitForSeconds(5f);
        }

    }
    
    public IEnumerator ChooseRandomPoint()
    {
        while (true)
        {
            Vector3 rawPoint = Vector3.zero;
            NavMeshHit hit = new NavMeshHit(); // ✅ Initialized here
            bool foundValidPoint = false;
            int attempts = 0;

            // Try up to 10 times to find a valid point
            while (!foundValidPoint && attempts < 10)
            {
                Vector3 randomOffset = Random.insideUnitSphere * 25f;
                randomOffset.y = 0;
                rawPoint = patrolCenter.position + randomOffset;

                if (NavMesh.SamplePosition(rawPoint, out hit, 25f, NavMesh.AllAreas))
                {
                    foundValidPoint = true;
                }
                else
                {
                    attempts++;
                    yield return null; // wait a frame before trying again
                }
            }

            if (foundValidPoint)
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid patrol point after 10 attempts.");
            }

            yield return new WaitForSeconds(5f);
        }
    }

}
