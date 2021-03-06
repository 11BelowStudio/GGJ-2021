﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Utilities.fader
{
	
	
	public class FaderScript : MonoBehaviour
	{

		//texture that will overlay the screen.
		//public Texture2D fadeOutTexture;
		
		public static FaderScript theFader;

		
		public Texture2D black;
		

		public Texture2D beige;

		public enum FadeType
        {
			BLACK,
			BEIGE
        }

		private bool fading;

		private FadeType LAST_FADE = FadeType.BLACK;

		//fading speed. 
		public float fadeSpeed = 0.8f;

		//the texture's alpha value (0 to 1).
		private float alpha = 1.0f;

		//order in the draw's hierarchy (highest priority).
		private int drawDepth = -1000;

		//fade direction: in = -1 (towards texture not visible), 
		//				  out = 1 (towards texture visible).
		private int fadeDir = -1;
		
		//singleton time
		void Awake(){
			Debug.Log("awake");
			if (theFader == null)
			{
				DontDestroyOnLoad(gameObject);
				theFader = this;
				SceneManager.sceneLoaded += OnLevelLoaded;
			}
			else if (theFader != this)
			{
				Destroy(gameObject);
			}
		}

		void OnGUI()
		{
			if (fading)
			{
				//Modify the alpha value gradually and 
				// use deltaTime to talk in seconds.
				alpha = Mathf.Clamp01(alpha += fadeDir * fadeSpeed * Time.deltaTime);
			}
			//Set colour of our texture. Keep the color the same and 
			// change the alpha channel.
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = drawDepth;
			Rect dimension = new Rect(0, 0, Screen.width, Screen.height);
			//GUI.DrawTexture(dimension, fadeOutTexture);

			Texture2D fadeTexture = black;
			switch (LAST_FADE)
			{
				case FadeType.BLACK:
					fadeTexture = black;
					break;
				case FadeType.BEIGE:
					fadeTexture = beige;
					break;
			}

			GUI.DrawTexture(dimension, fadeTexture);

		}

		
		private float BeginFade(int direction, FadeType whatFade)
        {
			LAST_FADE = whatFade;
			return BeginFade(direction);
        }

		private float BeginFade(int direction)
		{
			fadeDir = direction;
			if (fadeDir == 1)
            {
				alpha = 0f;
            }
            else
            {
				alpha = 1f;
            }
			fading = true;
			return 1.0f / fadeSpeed;
		}

		public void FadeAndQuit(FadeType texture = FadeType.BLACK, bool slowMo = true)
		{
			StartCoroutine(DoTheFadingAndTheQuitting(texture, slowMo));
		}

        
		//callbacked when a scene is loaded
        void OnLevelLoaded(Scene scene, LoadSceneMode mode)
		{
			Debug.Log("loaded");
			Time.timeScale = 1;
			BeginFade(-1);
		}

		public void ChangeLevel(int swapToIndex, FadeType texture = FadeType.BLACK)
        {
			StartCoroutine(DoTheFadingBetweenTheLevels(swapToIndex, texture));
        }

		private IEnumerator DoTheFadingBetweenTheLevels(int swapToIndex, FadeType texture = FadeType.BLACK)
        {
			yield return new WaitForSeconds(BeginFade(1, texture));
			SceneManager.LoadScene(swapToIndex);
			//yield break;
        }


		private IEnumerator DoTheFadingAndTheQuitting(FadeType texture = FadeType.BLACK, bool slowMo = true)
		{
			if (slowMo)
			{
				//slowmotion
				fadeSpeed *= 0.1f;
			}

			//does the fading
			yield return new WaitForSeconds(BeginFade(1, texture));
			//and also the quitting
			#if UNITY_WEBGL || UNITY_EDITOR
			if (slowMo)
			{
				fadeSpeed *= 10; //undo slowmo if it was enabled
			}
			SceneManager.LoadScene(0);
			#else
			Time.timeScale = 0;
			Application.Quit(0);
			#endif
		}
		
	}


}
