using System;
using CoreLogic.Common;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XNode;

namespace CoreLogic.Graph
{
	[CreateAssetMenu][Serializable]
	public class ComponentNodeGraph : NodeGraph
	{
		[HideInInspector]public GameObject gameObject;
		[HideInInspector]public Actor actor;
		
		[Button("Refresh")]
		private void DefaultSizedButton()
		{
			nodes.Clear();
			var path = AssetDatabase.GetAssetPath(this);
			var assets = AssetDatabase.LoadAllAssetsAtPath(path);
			foreach (var obj in assets)
			{
				if (AssetDatabase.LoadMainAssetAtPath(path) == obj)
				{
					Debug.Log($"[{path}] Found main asset: {obj.name}");
				}
				else
				{
					Debug.Log($"[{path}] Found asset in graph: {obj.name}");
					nodes.Add(obj as Node);
				}
				
			}
		}

		private void OnValidate()
		{
			
		}
	}
}