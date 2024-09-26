using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKActions : MonoBehaviour
{
    [SerializeField] private KeyCode _actionButton;
    
    [SerializeField] private Transform _leftHandTarget, _rightHandTarget;
    [SerializeField] private Vector3 _leftHandPosition = Vector3.zero,_rightHandPosition = Vector3.zero;
    [SerializeField] private Quaternion _leftHandRotation = Quaternion.identity,_rightHandRotation = Quaternion.identity;
    [SerializeField] private TwoBoneIKConstraint _leftHandIK, _rightHandIK;
    [SerializeField] private TwistChainConstraint _twistChainConstraint;
    [SerializeField] private Transform _tipTarget;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Vector3 _endRotation;
    private bool isIKOn;
    
    
    
    [Header("Raycast")]
    [SerializeField] private Transform _overlapSphere;
    [SerializeField] private float _radius = 0.4f;
    
    private InteractableObject _currentInteractableObject;

    private static float InterpolationRate = 10f;
    private float _interpolatedWeightR = 0f;
    private float _interpolatedWeightL = 0f;
    private float _interpolatedWeightB = 0f;


    private void Update()
    {
        if (Input.GetKey(_actionButton))
        {
            TryGrapInteractableObject();
        }

        if (Input.GetKeyUp(_actionButton))
        {
            TurnOffIK();
            isIKOn = false;
        }

        if (isIKOn)
        {
            _interpolatedWeightB = Mathf.Lerp(_interpolatedWeightB, 1, Time.deltaTime * InterpolationRate);
        }
        else
        {
            _interpolatedWeightB = Mathf.Lerp(_interpolatedWeightB, 0, Time.deltaTime * InterpolationRate);
        }

        if (_leftHandPosition != Vector3.zero)
        {
            _interpolatedWeightL = Mathf.Lerp(_interpolatedWeightL, 1, Time.deltaTime * InterpolationRate);
            _leftHandTarget.position = _leftHandPosition;
            _leftHandTarget.rotation = _leftHandRotation;
        }
        else
        {
            _interpolatedWeightL = Mathf.Lerp(_interpolatedWeightL, 0, Time.deltaTime * InterpolationRate);
        }
        if (_rightHandPosition != Vector3.zero)
        {
            _interpolatedWeightR = Mathf.Lerp(_interpolatedWeightR, 1, Time.deltaTime * InterpolationRate);
            _rightHandTarget.position = _rightHandPosition;
            _rightHandTarget.rotation = _rightHandRotation;
        }
        else
        {
            _interpolatedWeightR = Mathf.Lerp(_interpolatedWeightR, 0, Time.deltaTime * InterpolationRate);
        }
        _leftHandIK.weight = _interpolatedWeightL;
        _rightHandIK.weight = _interpolatedWeightR;
        _twistChainConstraint.weight = _interpolatedWeightB;
    }

    private void TryGrapInteractableObject()
    {
        Collider[] colliders = Physics.OverlapSphere(_overlapSphere.position, _radius,LayerMask.GetMask("InteractableObject"));
        if(colliders.Length <= 0)
            TurnOffIK();
        else
        {
            isIKOn = true;
        }
        
        foreach (Collider collider in colliders)
        {
            InteractableObject interactableObject = collider.GetComponent<InteractableObject>();
                _currentInteractableObject = interactableObject;
                if (_currentInteractableObject.leftHandTransform != null)
                {
                    _leftHandPosition = _currentInteractableObject.leftHandTransform.position;
                    _leftHandRotation = _currentInteractableObject.leftHandTransform.rotation;
                }

                if (_currentInteractableObject.rightHandTransform != null)
                {
                    _rightHandPosition = _currentInteractableObject.rightHandTransform.position;
                    _rightHandRotation = _currentInteractableObject.rightHandTransform.rotation;
                }

                if (collider.GetComponent<Lever>() != null)
                {
                    Lever lever = collider.GetComponent<Lever>();
                    _twistChainConstraint.weight = _interpolatedWeightB;
                    lever.ChangeLeverState(Input.GetAxis("Mouse Y") / 5);
                    _tipTarget.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation),
                        Quaternion.Euler(_endRotation), lever.leverState);
                }

        }
    }

    private void TurnOffIK()
    {
        _currentInteractableObject = null;
        _leftHandPosition = Vector3.zero;
        _rightHandPosition = Vector3.zero;
        _leftHandRotation = Quaternion.identity;
        _rightHandRotation = Quaternion.identity;
    }
}
