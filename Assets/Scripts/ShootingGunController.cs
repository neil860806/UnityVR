using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.VR;
public class ShootingGunController : MonoBehaviour
{
    public AudioSource audioSourse;
    public VRInput vrInput; //VRStandardAssets.Utils 內部物件
    public ParticleSystem flareParticle;
    public LineRenderer gunFlare;
    public Transform gunEnd;
    public float defaultLineLength = 70f;
    public float gunFlareVisibleSeconds = 0.07f;

    private void OnEnable()
    {
        vrInput.OnDown += HandleDown;
    }

    private void OnDisable()
    {
        vrInput.OnDown -= HandleDown;
    }

    private void HandleDown()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        audioSourse.Play();
        float lineLength = defaultLineLength;
        //TODO 判斷有無射到東西
        flareParticle.Play();
        gunFlare.enabled = true;
        yield return StartCoroutine (MoveLineRenderer(lineLength));
        gunFlare.enabled = false;
    }

    private IEnumerator MoveLineRenderer(float lineLength)
    {
        float timer = 0f;
        while (timer < gunFlareVisibleSeconds)
        {
            gunFlare.SetPosition(0, gunEnd.position);
            gunFlare.SetPosition(1, gunEnd.position + gunEnd.forward * lineLength);
            yield return null;
            timer += Time.deltaTime;
        }
    }

}
