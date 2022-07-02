using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TextField = TMPro.TextMeshProUGUI;

namespace NorskaLib.UI
{
	public class ValueText : MonoBehaviour
	{
		[SerializeField] TextField label;
		public string textFormat;

		[Space]
		[SerializeField] AnimationCurve curve;

		#region MonoBehaviour

		void OnDisable()
		{
			Break();
		}

		#endregion

		/// <summary>
		/// Sets the label text using provided format instantly.
		/// </summary>
		public void DisplayImmediate(int value)
		{
			Break();

			SetText(value);
		}

		private Coroutine countAnimatioRoutine;
		/// <summary>
		/// Sets the label text using provided format over given time.
		/// </summary>
		public void DisplayAnimated(int from, int to, float duration = 0.3f, float delay = 0.0f)
		{
			IEnumerator Routine()
			{
				var t = 0f;

				yield return delay > 0
					? new WaitForSeconds(delay)
					: null;

				while (t < duration)
				{
					t += Time.deltaTime;

					var factor = curve.Evaluate(t / duration);
					var value = Mathf.RoundToInt(Mathf.Lerp(from, to, factor));

					SetText(value);

					yield return null;
				}

				SetText(to);

				countAnimatioRoutine = null;
			}

			Break();

			countAnimatioRoutine = StartCoroutine(Routine());
		}

		private void SetText(int value)
		{
			label.text = string.IsNullOrEmpty(textFormat)
				? value.ToString()
				: string.Format(textFormat, value);
		}

		/// <summary>
		/// Stops the animation.
		/// </summary>
		public void Break()
		{
			if (countAnimatioRoutine != null)
				StopCoroutine(countAnimatioRoutine);
		}
	}
}