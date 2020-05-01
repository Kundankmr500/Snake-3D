using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerDirection direction;
    [HideInInspector] public float step_Length = 0.2f;
    [HideInInspector] public float movement_Frequency = 0.1f;

    private float counter;
    private bool move;
    
    [SerializeField]
    private GameObject tailPrefab;

    private List<Vector3> delta_Position;
    
    private List<Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;

    private bool create_Node_At_Tail;
    
    // Start is called before the first frame update
    void Awake()
    {
        tr = transform;
        main_Body = GetComponent<Rigidbody>();
        
        InitSnakeNodes();
        InitPlayer();
        
        delta_Position = new List<Vector3>()
        {
            new Vector3(-step_Length,0f),
            new Vector3(0f,step_Length),  
            new Vector3(step_Length,0f),  
            new Vector3(0f,-step_Length)  
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequency();
    }

    private void FixedUpdate()
    {
        if (move && GameHandler.Instance.CanPlayGame)
        {
            move = false;
            Move();
        }
    }

    void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        head_Body = nodes[0];
    }

    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int) PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
    }

    void InitPlayer()
    {
        SetDirectionRandom();     
        switch (direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE * 1.8f, 0f, 0f);
                break;
            
            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 1.8f, 0f, 0f);
                break;
            
            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 1.8f, 0f);
                break;
            
            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 1.8f, 0f);
                break;
        }
    }

    void Move()
    {
        Vector3 dPosition = delta_Position[(int) direction];
        
        Vector3 parentPos = main_Body.position;
        Vector3 prevPosition;

        main_Body.position = main_Body.position + dPosition;
        head_Body.position = head_Body.position + dPosition;
        

        for (int i = 1; i < nodes.Count; i++)
        {
            prevPosition = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPosition;
        }

        if (create_Node_At_Tail)
        {
            create_Node_At_Tail = false;
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position,
                                                                    Quaternion.identity);
            newNode.transform.SetParent(transform,true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency()
    {
        counter += Time.deltaTime;

        if (counter >= movement_Frequency)
        {
            counter = 0f;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        if (dir == PlayerDirection.UP && direction == PlayerDirection.DOWN || 
            dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
            dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT || 
            dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT)
        {
            return;
        }

        direction = dir;
        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag(Tags.BLUEFRUIT))
        {
            ProcessPickUps(target.gameObject, GameHandler.Instance.Fruits[0].FruitPoint);
        }
        if (target.CompareTag(Tags.REDFRUIT))
        {
            ProcessPickUps(target.gameObject, GameHandler.Instance.Fruits[1].FruitPoint);
        }
        if (target.CompareTag(Tags.WALL) || target.CompareTag(Tags.BOMB) || target.CompareTag(Tags.TAIL))
        {
            GameHandler.Instance.GameOver();
        }
    }

    void ProcessPickUps(GameObject target, int fruitPoint)
    {
        target.SetActive(false);
        create_Node_At_Tail = true;
        GameHandler.Instance.CheckStreakValue(target.tag);
        GameHandler.Instance.CalculateScore(fruitPoint);
        AudioManager.Instance.PlayPickUpSound();
    }
}
