using UnityEngine;
using UnityEngine.AI;

public class EnimyControllor : MonoBehaviour
{
    public string Name { get; }
    public float Health { get; set; }
    public Transform Player;
    public NavMeshAgent _navMeshAgent;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            //Debug.Log("Có player");
            FollowPlayer();
        }
    }
    public void FollowPlayer()
    {
        //_navMeshAgent.SetDestination(Player.position);
        _navMeshAgent.destination = Player.transform.position;
    }
}
