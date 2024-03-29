using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyDataSO enemyData;

    public PatrolRouteManager myPatrolRoute;
    
    private float _moveSpeed;

    private int _maxHealthPoints;

    private GameObject _enemyMesh;

    public float FollowDistance => _followDistance;
    public float _followDistance; //x = distancia minima pro inimigo seguir o jogador

    public float ReturnDistance => _returnDistance;
    public float _returnDistance; //z = distancia maxima pro inimigo seguir o jogador

    public float AttackDistance => _attackDistance;
    public float _attackDistance; //y = distancia minima pro inimigo atacar o jogador

    public float GiveUpDistance => _giveUpDistance;
    public float _giveUpDistance; //w = distancia maxima pro inimigo atacar o jogador

    private int _currentHealthPoints;

    private float _currentMoveSpeed;

    private Animator _enemyFSM;

    private NavMeshAgent _navMeshAgent;

    private SphereCollider _sphereCollider;

    private Transform _playerTransform;

    private Transform _currentPatrolPoint;

    private Transform _nextPatrolPoint;

    private int _currentPatrolIndex;

    private MeshRenderer _meshRenderer;

    public Color readyColor;
    public Color attackColor;
    public Color cooldownColor;

    public float attackCooldown;

    private float _currentCooldownTime;

    public bool CanAttack => _currentCooldownTime <= 0;

    private void Awake()
    {
        _moveSpeed = enemyData.moveSpeed;
        _maxHealthPoints = enemyData.maxHealthPoints;

        //_enemyMesh = Instantiate(enemyData.enemyMesh, transform);

        _followDistance = enemyData.followDistance;
        _returnDistance = enemyData.returnDistance;
        _attackDistance = enemyData.attackDistance;
        _giveUpDistance = enemyData.giveUpDistance;

        _currentHealthPoints = _maxHealthPoints;
        _currentMoveSpeed = _moveSpeed;

        _enemyFSM = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _sphereCollider = GetComponent<SphereCollider>();
        _meshRenderer = GetComponent<MeshRenderer>();


    }

    // Start is called before the first frame update
    void Start()
    {
        _currentPatrolIndex = 0;
        _currentPatrolPoint = myPatrolRoute.patrolRoutePoints[_currentPatrolIndex];
    }

    // Update is called once per frame
    void Update()
    {
        DistanceToPlayer();
        
        if (_enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Return"))
        {
            _enemyFSM.SetFloat("ReturnDistance",
                Vector3.Distance(transform.position, _currentPatrolPoint.position));
        }

        if (_enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (_currentCooldownTime > 0) _currentCooldownTime -= Time.deltaTime;
            else if (_meshRenderer.material.color != readyColor) _meshRenderer.material.color = readyColor;
        }
    }

    public void SetSphereRadius(float value)
    {
        _sphereCollider.radius = value;
    }

    public void SetDestinationToPlayer()
    {
        //transform.position += (_playerTransform.position - transform.position).normalized * _moveSpeed * Time.deltaTime;
        if(_playerTransform!=null)
            _navMeshAgent.SetDestination(_playerTransform.position);
    }

    public void SetDestinationToPatrol()
    {
        _navMeshAgent.SetDestination(_currentPatrolPoint.position);
    }

    public void ResetPlayerTransform()
    {
        _playerTransform = null;
    }

    public bool CheckPatrolPointReached()
    {
        return Vector3.Distance(transform.position, _currentPatrolPoint.position) < 1f;
    }

    public void UpdatePatrolPoint()
    {
        _currentPatrolIndex++;

        if (_currentPatrolIndex >= myPatrolRoute.patrolRoutePoints.Count)
        {
            _currentPatrolIndex = 0;
        }

        _currentPatrolPoint = myPatrolRoute.patrolRoutePoints[_currentPatrolIndex];
    }

    private IEnumerator DoAttack()
    {
        _meshRenderer.material.color = attackColor;
        if(_playerTransform != null)
            _playerTransform.GetComponent<PlayerController>().TakeDamage(10, transform.position);
        _currentCooldownTime = attackCooldown + 1f;
        yield return new WaitForSeconds(1f);
        _meshRenderer.material.color = cooldownColor;
        
    }

    public void Attack()
    {
        StartCoroutine("DoAttack");
    }

    public void DistanceToPlayer()
    {
        if (_enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
            _enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Follow"))
        {
            if (_playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, _playerTransform.position);
                if (_enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Follow"))
                {
                    if(distance < _attackDistance) _enemyFSM.SetTrigger("EnterAttack");
                }

                if (_enemyFSM.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    if(distance > _giveUpDistance) _enemyFSM.SetTrigger("LeaveAttack");
                }
            }
            
        }
        
        
    }

    public void LeaveAttackState()
    {
        _meshRenderer.material.color = readyColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTransform = other.transform;
            _enemyFSM.SetTrigger("OnPlayerEntered");
            Debug.Log("Jogador entrou na area");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _enemyFSM.SetTrigger("OnPlayerExited");
            Debug.Log("Jogador saiu da area");
        }
    }
}
