using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler1 : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent crawlerAgent;
    private Animator crawlerAnim;
    private GameManager gameManager;

    private Vector3 position1 = new Vector3(-14.81f, -14.9f, -17.8f);
    private Vector3 position2 = new Vector3(-14.1f, -14.9f, 6.05f);
    private Vector3 position3 = new Vector3(24.9f, -14.9f, 6f);

    private Vector3 destination;

    private bool waiting;
    private float distance2Run = 20;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        crawlerAgent = GetComponent<NavMeshAgent>();
        crawlerAnim = GetComponent<Animator>();
        waiting = true;


        destination = SetWaitingPosition();
        transform.position = destination;
        crawlerAgent.Warp(transform.position);
        crawlerAnim.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        //se comprueba si el juego est? en pausa
        if (gameManager.pause)
        {
            crawlerAgent.isStopped = true;    //el NPC se detiene
            crawlerAnim.Play("Idle");         //animaci?n a reproducir
        }
        else
        {
            //se comprueba si el jugador est? en rango
            if (Vector3.Distance(transform.position, player.transform.position) < distance2Run)
            {
                //se comprueba si el destino es alcanzable
                if (crawlerAgent.pathStatus == NavMeshPathStatus.PathPartial & waiting)
                {
                    TryToOpenDoor(gameManager.keyCount);
                }

                waiting = false;                //el NPC deja de estar en espera
                crawlerAgent.isStopped = false;   //el NPC est? en movimiento

                //el jugador es el objetivo del NPC
                crawlerAgent.SetDestination(new Vector3(player.transform.position.x,
                    this.transform.position.y, player.transform.position.z));

                crawlerAnim.Play("Walk");         //animaci?n a reproducir
            }
            else
            {
                //se determina 1 vez una posici?n aleatoria
                if (!waiting)
                {
                    crawlerAgent.isStopped = false;
                    destination = SetWaitingPosition();
                    crawlerAnim.Play("Walk");
                    waiting = true;
                }

                crawlerAgent.SetDestination(destination);

                //se comprueba si la posici?n es alcanzable
                if (crawlerAgent.pathStatus == NavMeshPathStatus.PathPartial)
                    destination = SetWaitingPosition(); //se define nueva posici?n

                //se comprueba si el NPC ha alcanzado el objetivo
                if (Vector3.Distance(destination, transform.position) < 0.5 &&
                    crawlerAgent.remainingDistance != Mathf.Infinity &&
                    crawlerAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                    crawlerAgent.remainingDistance == 0)
                {
                    crawlerAgent.isStopped = true;       //el NPC se detiene
                    crawlerAnim.Play("Idle");            //animaci?n a reproducir
                }
            }
        }
    }

    Vector3 SetWaitingPosition()
    {
        int n = Random.Range(1, 4);

        switch(n) {
            case 1:
                return position1;
            case 2:
                return position2;
            default:
                return position3;

        }
    }



    public GameObject FindClosestDoor()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Door");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void TryToOpenDoor(int difficulty)
    {
        GameObject door = FindClosestDoor();
        int r;
        switch (difficulty)
        {
            case 0:
                break;
            case 1:
                r = Random.Range(0, 3);
                if (r == 1)
                {
                    door.transform.Find("Door_Wood").GetComponent<Door>().openDoor();
                }
                break;
            case 2:
                r = Random.Range(0, 2);
                if (r == 1)
                {
                    door.transform.Find("Door_Wood").GetComponent<Door>().openDoor();
                }
                break;
            case 3:
                door.transform.Find("Door_Wood").GetComponent<Door>().openDoor();
                break;
        }
    }
}
