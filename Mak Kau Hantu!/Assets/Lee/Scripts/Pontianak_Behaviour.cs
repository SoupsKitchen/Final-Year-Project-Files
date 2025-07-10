using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pontianak_Behaviour : MonoBehaviour
{
    //Stat Params
    public float moveSpeed;
    public float turnSpeed;
    public float accelSpeed;
    
    //Stalking Params
    private float minDistance, maxDistance;
    private float _interest;

    //Detection Params
    public float visionAngle;
    private float _halfCone;
    private float _cosFOV;
    
    private Transform _lastSeen;
    private NavMeshAgent _agent;
    private Transform _player;






    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _halfCone = visionAngle / 2 * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Look()
    {
        _cosFOV = Mathf.Cos(_halfCone);
        Vector3 toPlayer = (_player.position - transform.position).normalized;
        float dp = Vector3.Dot(transform.forward, toPlayer);

        if (dp > _cosFOV)
        {
            _lastSeen = _player;
            BeginChase();
            //Player is in the cone of vision
        }

    }

    public void BeginChase()
    {
        if (_lastSeen != null)
        {
            //Chase Player;
        }
    }
}
