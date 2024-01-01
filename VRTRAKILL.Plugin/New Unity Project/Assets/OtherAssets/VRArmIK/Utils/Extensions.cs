using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace VRArmIK
{

	public static class LinqExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
				action(item);
		}
	}

	public static class UtilsExtensions
	{
		public static string F(this string self, params object[] objects)
		{
			return string.Format(self, objects);
		}
	}

	public static class TransformExtensions
	{
		public static void SetParentForReal(this Transform self, Transform parent)
		{
			self.SetParent(parent);
			self.localScale = Vector3.one;
		}
	}

	public static class MonoBehaviourExtensions
	{
		public static T GetOrAddComponent<T>(this Component self) where T : Component
		{
			T component = self.GetComponent<T>();
			return component != null ? component : self.gameObject.AddComponent<T>();
		}

		public static T GetOrAddComponentInChildren<T>(this MonoBehaviour self) where T : MonoBehaviour
		{
			T component = self.GetComponentInChildren<T>();
			return component != null ? component : self.gameObject.AddComponent<T>();
		}
	}


	public static class LayerMaskExtensions
	{
		public static LayerMask Create(params string[] layerNames)
		{
			return NamesToMask(layerNames);
		}

		public static LayerMask Create(params int[] layerNumbers)
		{
			return LayerNumbersToMask(layerNumbers);
		}

		public static LayerMask NamesToMask(params string[] layerNames)
		{
			LayerMask ret = (LayerMask)0;
			foreach (var name in layerNames)
			{
				ret |= (1 << LayerMask.NameToLayer(name));
			}

			return ret;
		}

		public static LayerMask LayerNumbersToMask(params int[] layerNumbers)
		{
			LayerMask ret = (LayerMask)0;
			foreach (var layer in layerNumbers)
			{
				ret |= (1 << layer);
			}
			return ret;
		}

		public static LayerMask Inverse(this LayerMask original)
		{
			return ~original;
		}

		public static LayerMask AddToMask(this LayerMask original, params string[] layerNames)
		{
			return original | NamesToMask(layerNames);
		}

		public static LayerMask RemoveFromMask(this LayerMask original, params string[] layerNames)
		{
			return original & ~NamesToMask(layerNames);
		}

		public static string[] MaskToNames(this LayerMask original)
		{
			var output = new List<string>();

			for (int i = 0; i < 32; ++i)
			{
				int shifted = 1 << i;
				if ((original & shifted) == shifted)
				{
					string layerName = LayerMask.LayerToName(i);
					if (!string.IsNullOrEmpty(layerName))
					{
						output.Add(layerName);
					}
				}
			}
			return output.ToArray();
		}

		public static string MaskToString(this LayerMask original)
		{
			return MaskToString(original, ", ");
		}

		public static string MaskToString(this LayerMask original, string delimiter)
		{
			return string.Join(delimiter, MaskToNames(original));
		}
	}

	public static class VectorExtensions
	{
		public static Vector3 toVector3(this Vector2 self)
		{
			return new Vector3(self.x, self.y);
		}

		public static Vector2 xy(this Vector3 self)
		{
			return new Vector2(self.x, self.y);
		}

		public static Vector2 xz(this Vector3 self)
		{
			return new Vector2(self.x, self.z);
		}

		public static Vector2 yz(this Vector3 self)
		{
			return new Vector2(self.y, self.y);
		}
	}

	public static class ListExtensions
	{
		public static T random<T>(this List<T> self) => self[(int)(self.Count * Random.value)];
		public static T next<T>(this List<T> self, int currentIndex, bool loop = false)
		{
			currentIndex++;
			if (!loop && currentIndex >= self.Count)
			{
				throw new IndexOutOfRangeException();
			}

			currentIndex %= self.Count;
			return self[currentIndex];
		}

		public static T next<T>(this List<T> self, T current, bool loop = false)
		{
			return self.next(self.IndexOf(current), loop);
		}
	}


	public static class RandomExtensions
	{
		public static Vector2 insideUnitCircle(this System.Random self)
		{
			float radius = (float)self.NextDouble();
			float angle = (float)self.NextDouble() * Mathf.PI * 2;

			return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
		}

		public static float range(this System.Random self, float min, float max)
		{
			return (float)(self.NextDouble() * (max - min) + min);
		}

		public static Vector3 insideUnitCube(this System.Random self)
		{
			return new Vector3(self.range(-1f, 1f), self.range(-1f, 1f), self.range(-1f, 1f));
		}
	}

	public static class EnumExtenstions
	{
		public class Enum<T> where T : struct, IConvertible
		{
			public static int Count
			{
				get
				{
					if (!typeof(T).IsEnum)
						throw new ArgumentException("T must be an enumerated type");

					return Enum.GetNames(typeof(T)).Length;
				}
			}
		}

		public static T Next<T>(this T src) where T : struct
		{
			if (!typeof(T).IsEnum) throw new ArgumentException($"Argumnent {typeof(T).FullName} is not an Enum");

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf(Arr, src) + 1;
			return Arr.Length == j ? Arr[0] : Arr[j];
		}

		public static bool IsLast<T>(this T src) where T : struct
		{
			if (!typeof(T).IsEnum) throw new ArgumentException($"Argumnent {typeof(T).FullName} is not an Enum");

			T[] Arr = (T[])Enum.GetValues(src.GetType());
			int j = Array.IndexOf(Arr, src);
			return Arr.Length == j + 1;
		}
	}

	public static class UIExtensions
	{
		public static void setNormalButtonColor(this Button self, Color color)
		{

			var buttonColors = self.colors;
			buttonColors.normalColor = color;
			self.colors = buttonColors;
		}

		public static IEnumerator fadeIn(this CanvasGroup self, float duration)
		{
			float timePassed = 0.0f;

			while (timePassed < duration)
			{
				self.alpha = timePassed / duration;
				timePassed += Time.unscaledDeltaTime;
				yield return null;
			}

			self.alpha = 1f;
		}

		public static IEnumerator fadeOut(this CanvasGroup self, float duration)
		{
			float timePassed = 0.0f;

			while (timePassed < duration)
			{
				self.alpha = 1f - timePassed / duration;
				timePassed += Time.unscaledDeltaTime;
				yield return null;
			}

			self.alpha = 0f;
		}
	}

	public static class IENumerableExtensions
	{
		public static T random<T>(this IEnumerable<T> self)
		{
			if (self.Count() == 0)
			{
				throw new IndexOutOfRangeException();
			}
			return self.ElementAt(Random.Range(0, self.Count()));
		}
	}

	public static class FloatExtensions
	{
		public static float toSignedEulerAngle(this float self)
		{
			float result = self.toPositiveEulerAngle();
			if (result > 180f)
				result = result - 360f;
			return result;
		}

		public static float toPositiveEulerAngle(this float self)
		{
			float result = (self % 360f + 360f) % 360f;
			return result;
		}
	}
}