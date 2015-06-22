﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworking : MonoBehaviour
{
    Animator animator;
    WeaponManager weaponManager;

    public static List<Transform> AvailableSpawns;

    void Awake()
    {
        animator = GetComponent<Animator>();
        weaponManager = GetComponentInChildren<WeaponManager>();

        StartCoroutine(ChooseSpawn());
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = transform.position;

        if (stream.isWriting)
        {
            syncPosition = transform.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            transform.position = syncPosition;
        }
    }

    IEnumerator ChooseSpawn()
    {
        while (AvailableSpawns.Count == 0)
            yield return new WaitForSeconds(.1f);

        Transform spawn = null;
        lock (AvailableSpawns)
        {
            int i = Random.Range(0, AvailableSpawns.Count);
            spawn = AvailableSpawns[i];
            AvailableSpawns.RemoveAt(i);
        }

        transform.position = spawn.position;
        transform.rotation = spawn.rotation;

        yield return new WaitForSeconds(5);

        while (Vector3.Distance(transform.position, spawn.position) < 5)
            yield return new WaitForSeconds(.1f);

        lock (AvailableSpawns)
            AvailableSpawns.Add(spawn);
    }

    [RPC]
    void PlayAnimation(string anim)
    {
        animator.Play(anim);
    }

    [RPC]
    void SwitchToWeapon(int n)
    {
        weaponManager.SwitchToWeapon(n);
    }

    [RPC]
    void UseActive()
    {
        weaponManager.UseActive();
    }

    [RPC]
    void ToggleSilencer()
    {
        weaponManager.ActiveGun.ToggleSilencer();
    }

    [RPC]
    void Reload()
    {
        weaponManager.ActiveGun.Reload();
    }
}
