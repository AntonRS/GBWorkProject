using System;
using UnityEngine;

namespace Game.CommandUI
{
	[DisallowMultipleComponent]
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		private static object _lock = new object();
		private static bool _applicationIsQuitting;

		public static T Instance
		{
			get 
			{
				if (_applicationIsQuitting)
					return null;

				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = (T)FindObjectOfType(typeof(T));

						if (_instance == null) {
							GameObject singleton = new GameObject ();
							singleton.name = String.Format ("{0} {1}", "(singleton) ", typeof(T));
							DontDestroyOnLoad (singleton);

							_instance = singleton.AddComponent<T> ();
						}
					}

					return _instance;
				}
			}
		}

		public void OnDestroy()
		{
			_applicationIsQuitting = true;
		}
	}
}

