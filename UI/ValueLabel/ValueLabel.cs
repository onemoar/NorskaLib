using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Label = TMPro.TextMeshProUGUI;

namespace NorskaLib.UI
{
	public class ValueLabel : MonoBehaviour
	{
		[SerializeField] Label label;
		public Label Label => label;
		public string textFormat;

		[Space]
		[SerializeField] AnimationCurve curve;

		private System.Action routineCallback;
		private Coroutine animationRoutine;

		#region MonoBehaviour

		void OnDisable()
		{
			Break();
		}

		#endregion

		private void SetText(int value)
		{
			label.text = string.IsNullOrEmpty(textFormat)
				? value.ToString()
				: string.Format(textFormat, value);
		}

		/// <summary>
		/// Sets the label text using provided format instantly.
		/// </summary>
		public void DisplayImmediate(int value)
		{
			Break();

			SetText(value);
		}
		/// <summary>
		/// Sets the label text using provided format over given time.
		/// </summary>
		public void DisplayAnimated(int from, int to, float duration = 0.3f, float delay = 0.0f, System.Action callback = null)
		{
			IEnumerator Routine()
			{
				var time = 0f;

				if (delay > 0)
					while(time < delay)
                    {
						time += Time.deltaTime;
						yield return null;
					}


				time = 0f;
				while (time < duration)
				{
					time += Time.deltaTime;

					var factor = curve.Evaluate(time / duration);
					var value = Mathf.RoundToInt(Mathf.Lerp(from, to, factor));

					SetText(value);

					yield return null;
				}

				SetText(to);

				animationRoutine = null;
				routineCallback?.Invoke();
			}

			Break();

			routineCallback = callback;
			animationRoutine = StartCoroutine(Routine());
		}

		/// <summary>
		/// Stops the animation instantly without completing it or invoking final callback.
		/// </summary>
		public void Break()
		{
			if (animationRoutine != null)
				StopCoroutine(animationRoutine);

			routineCallback = null;
		}
	}
}