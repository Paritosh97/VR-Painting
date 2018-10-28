using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DripEmitterScript : MonoBehaviour
{
    public ParticleSystem particleEmitter;
    public ParticleSystem splatterParticles;

    public ParticleDecalPool splatDecalPool;

    public Gradient particleColorGradient;
    
    public bool debug;

    List<ParticleCollisionEvent> collisionEvents;

    // Use this for initialization
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {        
        ParticlePhysicsExtensions.GetCollisionEvents(particleEmitter, other, collisionEvents);

        for(int i = 0; i < collisionEvents.Count; i++)
        {
            splatDecalPool.ParticleHit(collisionEvents[i], particleColorGradient);
            EmitAtLocation(collisionEvents[i]);
        }
    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatterParticles.transform.position = particleCollisionEvent.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);
        ParticleSystem.MainModule psMain = splatterParticles.main;
        psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));

        splatterParticles.Emit(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || debug)
        {
            ParticleSystem.MainModule psMain = particleEmitter.main;
            psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));
            particleEmitter.Emit(1);
        }
    }
}
