using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Overgrown
{
	namespace Utils
	{
		public class Singleton<T> : MonoBehaviour where T : Component
		{
			private static T _instance;
			public static T Instance
			{
				get
				{
					if (_instance == null)
					{
						_instance = FindObjectOfType<T>();
						if (_instance == null)
						{
							GameObject obj = new GameObject();
							obj.name = typeof(T).Name;
							_instance = obj.AddComponent<T>();
						}
					}
					return _instance;
				}
			}

			protected virtual void Awake()
			{
				if (_instance == null)
				{
					_instance = this as T;
					if (transform.parent == null) DontDestroyOnLoad(this.gameObject);
				}
				else
				{
					Destroy(this.gameObject);
				}
			}
		}

		public class NetworkSingleton<T> : NetworkBehaviour where T : Component
		{
			private static T _instance;
			public static T Instance
			{
				get
				{
					if (_instance == null)
					{
						_instance = FindObjectOfType<T>();
						if (_instance == null)
						{
							GameObject obj = new GameObject();
							obj.name = typeof(T).Name;
							_instance = obj.AddComponent<T>();
						}
					}
					return _instance;
				}
			}

			protected virtual void Awake()
			{
				if (_instance == null)
				{
					_instance = this as T;
					if (transform.parent == null) DontDestroyOnLoad(this.gameObject);
				}
				else
				{
					Destroy(this.gameObject);
				}
			}
		}
	}
}