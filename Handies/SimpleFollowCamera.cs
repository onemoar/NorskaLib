using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SimpleFollowCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    [Space]
    public bool smoothing = false;
    [EnableIf("smoothing")]
    public float moveSmoothing = 60;
    [EnableIf("smoothing")]
    public float turnSmoothing = 90;

    #region MonoBehaviour
    void Awake()
    {
        
    }
    
    void OnDestroy()
    {
        
    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    void FixedUpdate()
    {
        if (target == null)
            return;

        transform.position = smoothing
            ? Vector3.Lerp(
                transform.position,
                target.position + offset,
                Time.deltaTime * moveSmoothing)
            : target.position + offset;
    }
}
