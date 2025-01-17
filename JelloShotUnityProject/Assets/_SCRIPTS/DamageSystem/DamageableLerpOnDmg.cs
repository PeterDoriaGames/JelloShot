﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DamageableLerpOnDmg : DamageableBase
{
    #region VARIABLES
    private SizeLerper _SizeLerper = null;
    private ColorLerper _ColorLerper = null;
    [SerializeField]
    private bool _IsSizeLerper;
    public bool isSizeLerper { get { return _IsSizeLerper; } set { _IsSizeLerper = value; } }
    [SerializeField]
    private bool _IsColorLerper;
    public bool isColorLerper { get { return _IsColorLerper; } set { _IsColorLerper = value; } } 
    public float maxHealthPoints { get { return maxHealth; } set { maxHealth = value; } }
    public float currenthealthPoints { get { return currentHealth; } set { currentHealth = value; } }
    private float _WaitTime
    {
        get { return waitTime; }
        set
        {
            if (value >= 0)
                waitTime = value;
        }
    }
    #endregion

    #region UNITY CALLBACKS
    void OnEnable()
    {
        if (GetComponent<SizeLerper>() != null)
            _SizeLerper = GetComponent<SizeLerper>();
        if (GetComponent<ColorLerper>() != null)
            _ColorLerper = GetComponent<ColorLerper>();

        maxHealth = maxHealthPoints;
        currentHealth = maxHealthPoints;
        if (_SizeLerper != null)
        {
            _WaitTime = _SizeLerper.lerpTime;
        }
        if (_ColorLerper != null)
        {
            _WaitTime = _ColorLerper.lerpTime;
        }
    }

    void OnDisable()
    {
        _SizeLerper = null;
    }
    #endregion

    public override void OnTakeDmg()
    {
        base.OnTakeDmg();
        if (_SizeLerper != null)
            _SizeLerper.StartLerp(currentHealth, maxHealth);
        if (_ColorLerper != null)
            _ColorLerper.StartLerp(currentHealth, maxHealth);
    }

    public override void OnHeal()
    {
        if (_SizeLerper != null)
            _SizeLerper.StartLerp(currentHealth, healedHPValue);
        if (_ColorLerper != null)
            _ColorLerper.StartLerp(currentHealth, healedHPValue);
        base.OnHeal();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DamageableLerpOnDmg))]
public class LerperAdder : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        DamageableLerpOnDmg _DamageableLerpScript = (DamageableLerpOnDmg)target;

        if (_DamageableLerpScript.isSizeLerper == true && _DamageableLerpScript.gameObject.GetComponent<SizeLerper>() == null)
        {
            _DamageableLerpScript.gameObject.AddComponent<SizeLerper>();
        }
        else if (_DamageableLerpScript.isSizeLerper == false && _DamageableLerpScript.gameObject.GetComponent<SizeLerper>() != null)
        {
            DestroyImmediate(_DamageableLerpScript.gameObject.GetComponent<SizeLerper>(), true);
        }

        if (_DamageableLerpScript.isColorLerper == true && _DamageableLerpScript.gameObject.GetComponent<ColorLerper>() == null)
        {
            _DamageableLerpScript.gameObject.AddComponent<ColorLerper>();
        }
        else if (_DamageableLerpScript.isColorLerper == false && _DamageableLerpScript.gameObject.GetComponent<ColorLerper>() != null)
        {
            DestroyImmediate(_DamageableLerpScript.gameObject.GetComponent<ColorLerper>(), true);
        }

        if (_DamageableLerpScript.gameObject.GetComponent<ColorLerper>() == null && _DamageableLerpScript.gameObject.GetComponent<SizeLerper>() == null)
        {
            Debug.Log("ERROR ERROR: DamageableLerpOnDmg has nothing to lerp!");
        }
    }
}
#endif