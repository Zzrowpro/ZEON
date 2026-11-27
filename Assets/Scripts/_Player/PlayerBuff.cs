using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class ProfileBuff : MonoBehaviour
{
    PlayerController player;
    RocketProfiles baseProfile;
    Coroutine running;

    void Awake() => player = GetComponent<PlayerController>();

    public void Apply(RocketProfiles buff, float duration)
    {
        if (running != null) StopCoroutine(running);        // refresh the timer
        else baseProfile = player.rocketProfiles;           // only remember when no buff is active

        running = StartCoroutine(Run(buff, duration));
    }

    IEnumerator Run(RocketProfiles buff, float duration)
    {
        player.SetProfile(buff);
        yield return new WaitForSeconds(duration);
        player.SetProfile(baseProfile);
        running = null;
    }
}