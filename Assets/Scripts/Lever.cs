using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{
    private Animator _animator;
    public float leverState;
    [SerializeField] private Animator _doorAnimator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ChangeLeverState(float changeAmount)
    {
        if (leverState + changeAmount > 1)
            leverState = 1;
        else
            leverState += changeAmount;

        if (leverState + changeAmount < 0)
            leverState = 0;
        else
            leverState += changeAmount;
        
        _animator.SetFloat("LeverState",leverState);
        
        if (leverState >= 0.9f)
        {
            _doorAnimator.SetBool("isDoorOpen",true);
        }
        else if (leverState <= 0.1f)
        {
            _doorAnimator.SetBool("isDoorOpen",false);
        }
    }
    
    
    
}
