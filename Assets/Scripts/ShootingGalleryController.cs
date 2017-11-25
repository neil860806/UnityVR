﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;
using UnityEngine.UI; //namespace 命名空間
public class ShootingGalleryController : MonoBehaviour //Class 類別
{
    public UIController uiController; //Field 欄位
    public Reticle reticle;
    public SelectionRadial selectionRadial;
    public SelectionSlider selectionSlider;

    public Image timeBar;
    public float gameDuration = 30f;
    public float endDelay = 1.5f;

    public bool IsPlaying //Property 屬性
    {
        private set; //只有自己能改
        get; //外部能讀取
    }
    
    private IEnumerator Start() //Method 方法
    {
        SessionData.SetGameType(SessionData.GameType.SHOOTER180);
        while (true) {
            Debug.Log("Start StartPhase");
            yield return StartCoroutine(StartPhase());
            Debug.Log("Start PlayPhase");
            yield return StartCoroutine(PlayPhase());
            yield return StartCoroutine(EndPhase());
            Debug.Log("Start CompletePhase");
        }
    }

    private IEnumerator StartPhase()
    {
        yield return StartCoroutine(uiController.ShowIntroUI());
        reticle.Show();
        selectionRadial.Hide();
        yield return StartCoroutine(selectionSlider.WaitForBarToFill());
        yield return StartCoroutine(uiController.HideIntroUI());
    }

    private IEnumerator PlayPhase()
    {
        yield return StartCoroutine(uiController.ShowPlayerUI());
        IsPlaying = true;
        reticle.Show();
        SessionData.Restart();

        float gameTimer = gameDuration;
        while (gameTimer > 0f)
        {
            yield return null;
            gameTimer -= Time.deltaTime;
            timeBar.fillAmount = gameTimer / gameDuration;
        }
        yield return StartCoroutine(uiController.HidePlayerUI());
        IsPlaying = false;
        
    }

    private IEnumerator EndPhase()
    {
        reticle.Hide();
        yield return StartCoroutine(uiController.ShowOutroUI());
        yield return new WaitForSeconds(endDelay);
        yield return StartCoroutine(selectionRadial.WaitForSelectionRadialToFill());
        yield return StartCoroutine(uiController.HideOutroUI());
    }

}
