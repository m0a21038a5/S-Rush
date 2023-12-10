using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEffect : MonoBehaviour
{
    public static BeamEffect Beaminstance;
    //ビーム
    public float Timer = 0.0f;
    public float EffectDestroyTime = 0.2f;
    public float Range = 100.0f;
    private Ray ShotRay;
    private RaycastHit ShotHit;
    private ParticleSystem beamParticle;
    private LineRenderer lineRenderer;
    public void Awake()
    {
        if (Beaminstance == null)
        {
           Beaminstance = this;
        }
    }
    void Start()
    {
        //パーティクルシステムの取得
        beamParticle = GetComponent<ParticleSystem>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        
    }
    public void BeamShot()
    {
        Timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            shot();
        }
        if (Timer >= EffectDestroyTime)
        {
            disableEffect();
        }
    }
    private void shot()
    {
        Timer = 0f;
        beamParticle.Stop();
        beamParticle.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        ShotRay.origin = transform.position;
        ShotRay.direction = transform.forward;

        int layerMask = 0;
        if (Physics.Raycast(ShotRay, out ShotHit, Range, layerMask))
        {
            // hit 
        }
        lineRenderer.SetPosition(1, ShotRay.origin + ShotRay.direction * Range);
    }
    private void disableEffect()
    {
        beamParticle.Stop();
        lineRenderer.enabled = false;
    }
}
