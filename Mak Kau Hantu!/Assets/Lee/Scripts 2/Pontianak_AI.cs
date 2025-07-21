using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Pontianak_AI : MonoBehaviour
{

    //NavMeshAgent Params
    private NavMeshAgent _agent;
    private GameObject _player;
    private Vector3 _targetLocation;

    //Sight Params
    public Transform eyes;
    public bool seesPlayer()
    {
        Vector3 dirToPlayer = (_player.transform.position - eyes.position).normalized;
        float distanceToPlayer = Vector3.Distance(eyes.position, _player.transform.position);
        RaycastHit hit;

        // Check within radius
        if (distanceToPlayer < _viewRad)
        {
            // Check within view angle
            float angleToPlayer = Vector3.Angle(eyes.forward, dirToPlayer);
            if (angleToPlayer < _viewAng / 2f)
            {
                // Optional: Raycast to check line of sight
                if (Physics.Raycast(eyes.position, dirToPlayer, out hit, distanceToPlayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }
    [SerializeField] private float _viewRad;
    [Range(0, 360)] private float _viewAng = 90f;
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = _player.transform.position - eyes.position;
        Debug.DrawRay(eyes.position, toPlayer, Color.red);
        if (seesPlayer())
        {
            Debug.Log("Pontianak sees the player!");
            _targetLocation = _player.transform.position;
        }
        else
        {
            Debug.Log("Pontianak doesn't see the player!");
        }
        _agent.SetDestination(_targetLocation);
    }

    IEnumerator ChooseRandomSpot()
    {
        bool validLocation = false;
        int attempts = 0;
        NavMeshHit hit;
        while (!validLocation && attempts <= 10)
        {
            Vector3 newTargetLocation = transform.position + Random.onUnitSphere * 25f;
            if (NavMesh.SamplePosition(newTargetLocation, out hit, 25f, NavMesh.AllAreas))
            {
                _targetLocation = newTargetLocation;
                validLocation = true;
                yield return null;
            }
            else
            {
                attempts++;
                yield return new WaitForSeconds(1f);
            }
        }

        if (!validLocation)
        {
            Debug.LogWarning("The Pontianak could not find a suitable location!");
        }
    }
}
