using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed = 100f;
    [SerializeField] private float m_Range = 5f;
    [SerializeField] private GameObject m_PointA, m_PointB;
    [SerializeField] private Transform m_Player;

    private Rigidbody2D m_Rigidbody2D;
    private bool mustPatrol = true;
    private bool isMovingForward = true;
    private bool isMovingBack = false;
    private bool isFocused = false;
    private float Distance;


    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        IsInRange();
        if(transform.position.x >= m_PointA.transform.position.x && transform.position.x <= m_PointB.transform.position.x)
        {        
            Patrol();         
            if(isFocused)
            {
                Chase();
            }
        }
        else if(transform.position.x < m_PointA.transform.position.x)
        {
            isFocused = false;
            if(isMovingBack)
            {
                Flip();
                Patrol();         
            }
            else
            {
                Patrol();       
            }
        }
        else if(transform.position.x > m_PointB.transform.position.x)
        {
            isFocused = false;
            if(isMovingForward)
            {
                Flip();
                Patrol();         
            }
            else
            {
                Patrol();
            }
        }  
    }

    void Patrol()
    {
        if(isMovingForward && !isFocused)
        {
            if(Mathf.Abs(transform.position.x - m_PointB.transform.position.x) < 0.1)
            {
                Flip();
            }
        }
        else if(isMovingBack && !isFocused)
        {
            if(Mathf.Abs(transform.position.x - m_PointA.transform.position.x) < 0.1)
            {
                Flip();
            }
        }  
        if(mustPatrol)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Speed * Time.fixedDeltaTime, m_Rigidbody2D.velocity.y);
        }
        else
        {
            m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        }
        
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        m_Speed *= -1;
        if(m_Speed < 0)
        {
            isMovingForward = false;
            isMovingBack = true;
        }
        else if(m_Speed > 0)
        {
            isMovingForward = true;
            isMovingBack = false;
        }
        mustPatrol = true;
    }

    void IsInRange()
    {
        Distance = Vector2.Distance(transform.position, m_Player.position);
        if(Distance <= m_Range)
        {
            isFocused = true;
        }
        else 
        {
            isFocused = false;
        }
    }

    void Chase()
    {
        if(m_Player.position.x > transform.position.x && transform.localScale.x < 0
        || m_Player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }
        if(Vector2.Distance(transform.position,m_Player.position) <= 1.5)
        {
            mustPatrol = false;
        }
        else
        {
            mustPatrol = true;
        }
    }

}
