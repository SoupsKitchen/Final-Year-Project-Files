using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Pontianak_Behaviour : MonoBehaviour
{
    //Detection Params
    public float visionAngle;
    private float _halfCone;
    private bool _seesPlayer = false;
    
    private Transform _lastSeenLocation;
    private Transform _player;
    private NavMeshAgent _agent;
    public List<Transform> importantLocations;

    private StateMachine _stateMachine;
    private IState patrolState, idleState, stalkState, chaseState;

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new StateMachine();
        patrolState = new State_Patrol();
        idleState = new State_Idle();
        stalkState = new State_Stalk();
        chaseState = new State_Chase();
        
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _halfCone = visionAngle / 2 * Mathf.Deg2Rad;
        StartCoroutine(Look());
    }

    // Update is called once per frame
    void Update()
    {
        if (_seesPlayer)
        {
            _agent.SetDestination(_lastSeenLocation.position);
        }

        
        
    }

    public IEnumerator Look()
    {
        while (true)
        {
            float _cosFOV = Mathf.Cos(_halfCone);
            Vector3 toPlayer = (_player.position - transform.position).normalized;
            float dp = Vector3.Dot(transform.forward, toPlayer);

            if (dp > _cosFOV)
            {
                _seesPlayer = true;
                _lastSeenLocation = _player;
                Debug.Log("Player Detected!");
                //Player is in the cone of vision
            }
            else
            {
                _seesPlayer = false;
                Debug.Log("Player Not Found!");
            }
            yield return new WaitForSeconds(0.1f);
        }
        

    }

    public void BeginChase()
    {
        if (_lastSeenLocation != null)
        {
            //Chase Player;
        }
    }
}
