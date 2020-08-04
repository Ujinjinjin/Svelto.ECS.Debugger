﻿using Code;
using Code.Attributes;
using Code.Infrastructure;
using Svelto.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class SveltoEcsEnginesEditorWindow : EditorWindow
	{
		Vector2 scrollPos;
		
		[MenuItem("Svelto/Svelto.ECS/Engines")]
		private static void ShowWindow()
		{
			var window = GetWindow<SveltoEcsEnginesEditorWindow>();
			window.titleContent = new GUIContent("Svelto Ecs Engines");
			window.Show();
		}

		private void OnGUI()
		{
			var assembly = Assembly.GetAssembly(typeof(SveltoCompositionRoot));
			var observableEngines = assembly.GetTypes()
				.Where(x => x.ContainsCustomAttribute(typeof(ObservableEngineAttribute)))
				.ToArray();
			
			GUILayout.Label ("List of observable engines:", EditorStyles.boldLabel);

			EditorGUILayout.BeginVertical();
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			
			for (var i = 0; i < observableEngines.Length; i++)
			{
				var observabilityAttributes = observableEngines[i]
					.GetCustomAttributes<ObservableEngineAttribute>()
					.ToArray();
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label($"{i + 1}. {observableEngines[i].Name}");
				
				EditorGUILayout.BeginVertical();
				for (var j = 0; j < observabilityAttributes.Length; j++)
				{
					DisplayComponents(observabilityAttributes[j]);

					DisplayEntityCount(observabilityAttributes[j]);
					GUILayout.Label(string.Empty);
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void DisplayComponents(ObservableEngineAttribute attribute)
		{
			GUILayout.Label($"Groups: {string.Join(", ", attribute.GroupNameList)}");
			GUILayout.Label($"Components count: {attribute.TypeCount.ToString()}");
			
			var componentNames = new List<string>();

			if (attribute.TypeCount >= 1)
				componentNames.Add(attribute.T1.Name);
			if (attribute.TypeCount >= 2)
				componentNames.Add(attribute.T2.Name);
			if (attribute.TypeCount >= 3)
				componentNames.Add(attribute.T3.Name);
			if (attribute.TypeCount >= 4)
				componentNames.Add(attribute.T4.Name);

			GUILayout.Label($"Components: {string.Join(", ", componentNames)}");
		}

		private void DisplayEntityCount(ObservableEngineAttribute attribute)
		{
			if (Application.isPlaying == false)
			{
				GUILayout.Label("Entities count: -");
				return;
			}
			
			var queryMethod = typeof(EntitiesDB)
				.GetMethods()
				.Where(x => x.Name == "QueryEntities")
				.First(x => x.GetGenericArguments().Length == attribute.TypeCount);
			
			MethodInfo methodRef;
			Type constructedClass;
			Type genericClass;
			
			switch (attribute.TypeCount)
			{
				case 1:
					methodRef = queryMethod.MakeGenericMethod(attribute.T1);
					genericClass = typeof(EntityCollection<>);
					constructedClass = genericClass.MakeGenericType(attribute.T1);
					break;
				case 2:
					methodRef = queryMethod.MakeGenericMethod(attribute.T1, attribute.T2);
					genericClass = typeof(EntityCollection<,>);
					constructedClass = genericClass.MakeGenericType(attribute.T1, attribute.T2);
					break;
				case 3:
					methodRef = queryMethod.MakeGenericMethod(attribute.T1, attribute.T2, attribute.T3);
					genericClass = typeof(EntityCollection<,,>);
					constructedClass = genericClass.MakeGenericType(attribute.T1, attribute.T2, attribute.T3);
					break;
				case 4:
					methodRef = queryMethod.MakeGenericMethod(attribute.T1, attribute.T2, attribute.T3, attribute.T4);
					genericClass = typeof(EntityCollection<,,,>);
					constructedClass = genericClass.MakeGenericType(attribute.T1, attribute.T2, attribute.T3, attribute.T4);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			uint totalCount = 0;

			for (var i = 0; i < attribute.GroupNameList.Count; i++)
			{
				var result = methodRef.Invoke(CompositionRootHolder.CompositionRoot.EnginesRoot.EntitiesDb, new[] { GetEcsGroupByName(attribute.GroupNameList[i], attribute.EcsGroupsType) });
				var count = constructedClass.GetProperty("count").GetValue(result);

				totalCount += (uint)count;
			}
			
			GUILayout.Label($"Entities count: {totalCount.ToString()}");
		}

		private object GetEcsGroupByName(string groupName, Type ecsGroupsType)
		{
			return ecsGroupsType
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Single(p => p.Name == groupName)
				.GetValue(null);
		}
	}
}
