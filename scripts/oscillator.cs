using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3 (10f,10f,10f);
    [Range(0, 1)] [SerializeField] float movementFactor;
    [SerializeField] float period = 2f;
    [SerializeField] Vector3 offset = new Vector3(0f,0f,0f);

    Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        else
        {
            float cycles = Time.time / period;
            const float tau = Mathf.PI * 2;
            float rawSin = Mathf.Sin(cycles * tau);
            movementFactor = rawSin / 2 + 0.5f;
            offset = movementVector * movementFactor;
            transform.position = startPos + offset;
        }
    }
}
