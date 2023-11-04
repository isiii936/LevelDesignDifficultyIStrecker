using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SpatialAudio : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0f, 1f)] private float maxPan = 1f;
    [SerializeField, Range(0f, 1f)] private float volumeBehindWall = 0.5f;
    private float stereoPan;
    private float baseVolume;
    private float basePitch;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;
    public Bomb bomb;

    [Header("Spatial Audio Values")]
    private float minimumDistance;
    private float maximumDistance;
    public float maxDistanceMultiplier = 1f;
    [SerializeField] private AnimationCurve volumeDropoff;

    private float distanceToPlayer;


    private Vector3 direction;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        baseVolume = audioSource.volume * bomb.bombSettings[GameController.instance.currentLevel].TickVolume;
        basePitch = audioSource.pitch;
        minimumDistance = bomb.bombSettings[GameController.instance.currentLevel].BombRadius;
        maximumDistance = bomb.bombSettings[GameController.instance.currentLevel].BombRadius * maxDistanceMultiplier;
    }

    private void Update()
    {
        //float playerInSightVolumeMultiplier = PlayerInSight();
        direction = (Camera.main.transform.position - transform.position);
        distanceToPlayer = (Camera.main.transform.position - transform.position).magnitude;

        #region Pan
        //Returns the value for the stereo pan based on the player Head position relative to the Object
        stereoPan = (Vector3.Dot(Camera.main.transform.right, direction.normalized) * -1f);
        stereoPan = Mathf.Clamp(stereoPan, -1, 1) * maxPan;

        audioSource.panStereo = stereoPan;
        #endregion

        #region Volume
        float normalizedDistance = Mathf.Clamp01((distanceToPlayer - minimumDistance) / (maximumDistance - minimumDistance));
        float volumeMultiplier = volumeDropoff.Evaluate(normalizedDistance);
        audioSource.volume = baseVolume * volumeMultiplier;
        #endregion
    }

}
