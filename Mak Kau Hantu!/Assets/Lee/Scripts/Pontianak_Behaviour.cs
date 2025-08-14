using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;
public class Pontianak_Behaviour : MonoBehaviour
{
    #region NavMeshAgent Params
    public NavMeshAgent agent;
    public GameObject player;
    public Vector3 targetLocation;
    #endregion

    #region States Params
    private StateMachine _stateMachine;
    private IState patrolState, scaredState, stalkState, chaseState, idleState;
    public bool ReachedDestination()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    #endregion

    #region Mood Params
    [Header("Mood Params")]
    [SerializeField] private float _anger;
    public float anger
    {
        get => _anger;
        set
        {
            _anger = Mathf.Clamp(value, 0f, 100f);
        }
    }

    private float _interest;
    public float interest
    {
        get => _interest;
        set
        {
            _interest = Mathf.Clamp(value, 0f, 100f);
        }
    }

    [SerializeField] private float angerDecayRate;
    [SerializeField] private float _interestIncreaseRate;
    [SerializeField] private float _interestDecayRate;
    [SerializeField] private float _stalkThreshold;
    public bool isResting { get; private set; } = false;
    #endregion

    #region Patrolling Params
    [Header("Patrolling Params")]
    private int _patrolIdx = -1;
    private Transform _currentPatrolPoint;
    public List<Transform> patrolPoints;
    #endregion

    #region Sight Params
    [Header("Pontianak Viewing Params")]
    public Transform eyes;
    public Vector3 toPlayer;
    public bool seesPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyes.position, toPlayer, out hit, 100f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    [SerializeField] private float _viewRad;
    [Range(0, 360)] private float _viewAng = 90f;
    #endregion

    #region Unity stock methods
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            StartCoroutine(FindPlayerAgain());
        }
        agent = GetComponent<NavMeshAgent>();

        _stateMachine = new StateMachine();
        patrolState = new State_Patrol(this);
        scaredState = new State_Scared(this);
        stalkState = new State_Stalk(this);
        chaseState = new State_Chase(this);
        idleState = new State_Idle(this);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        toPlayer = player.transform.position - eyes.position;
        Debug.DrawRay(eyes.position, toPlayer, Color.red);
        if (seesPlayer() && !isResting)
        {
            _interest += _interestIncreaseRate;
            if (_anger >= 100f)
            {
                if (!_stateMachine.IsInState(chaseState))
                {
                    _stateMachine.ChangeState(chaseState);
                }
            }
            else
            {
                if (!_stateMachine.IsInState(stalkState) && !_stateMachine.IsInState(chaseState))
                {
                    _stateMachine.ChangeState(stalkState);
                }
            }
            
        }
        else
        {
            _interest -= _interestDecayRate;
        }

        //When the Pontianak is chasing, do this
        if (_stateMachine.IsInState(chaseState))
        {
            anger -= angerDecayRate * Time.deltaTime;

            if (_anger <= 0f)
            {
                StartCoroutine(BeginCooldown(20f));
            }
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            _anger += 80f;
        }

        //Stuff that works per frame
        _stateMachine.Update();
        agent.SetDestination(targetLocation);

    }
    #endregion

    #region Coroutines
    public IEnumerator ChooseRandomSpot()
    {
        //Setting params for the loop
        bool validLocation = false;
        int attempts = 0;

        //Calculating Vectors
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 25f;
        Vector3 randomOffset3D = new Vector3(randomOffset.x, 0f, randomOffset.y);

        //Begin checking for suitable spots
        NavMeshHit hit;
        while (!validLocation && attempts <= 10)
        {
            Vector3 newTargetLocation = transform.position + randomOffset3D;
            if (NavMesh.SamplePosition(newTargetLocation, out hit, 25f, NavMesh.AllAreas))
            {
                targetLocation = newTargetLocation;
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

    public IEnumerator BeginCooldown(float cooldownTimer)
    {
        isResting = true;
        _stateMachine.ChangeState(idleState);
        yield return new WaitForSeconds(cooldownTimer);
        isResting = false;
        _stateMachine.ChangeState(patrolState);
    }

    private IEnumerator FindPlayerAgain()
    {
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
            
        }
        
    }
    #endregion

    #region Public methods
    public void IncreaseAnger(float amount)
    {
        _anger += amount;
        if (seesPlayer() && !_stateMachine.IsInState(chaseState))
        {
            if (_anger >= 100f)
            {
                _stateMachine.ChangeState(chaseState);
            }
            else if (_interest >= _stalkThreshold)
            {
                _stateMachine.ChangeState(stalkState);
            }
        }
        else
        {
            _stateMachine.ChangeState(scaredState);
        }
    }
    public void ProgressToNextSpot()
    {
        _patrolIdx++;
        _currentPatrolPoint = patrolPoints[_patrolIdx];
        _stateMachine.ChangeState(patrolState);
    }
    #endregion
}
