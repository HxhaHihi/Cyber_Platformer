using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float Speed = 2f;
    public float Distance = 3f;

    private Vector3 startPosition;

    void Start()
    {
        //начальная точка платформы, куда была поставлена 
        startPosition = transform.position;
    }

    void Update()
    {
        
        float movement = Mathf.Sin(Time.time * Speed) * Distance;

        
        transform.position = startPosition + new Vector3(movement, 0, 0);
    }
}