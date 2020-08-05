using Code.EditorTools;
using Code.EditorTools.Attributes;
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
		private Vector2 _scrollPos;

		private readonly Dictionary<string, bool> _enginesDisplayStateIndex = new Dictionary<string, bool>();

		[MenuItem("Svelto/Svelto.ECS/Engines")]
		private static void ShowWindow()
		{
			var window = GetWindow<SveltoEcsEnginesEditorWindow>();
			window.titleContent = new GUIContent("Svelto Ecs Engines");
			window.Show();
		}

		private void OnGUI()
		{
			if (ObservableEnginesSingletonContext.CompositionRoot is null)
			{
				DisplaySveltoEcsEnginesEditorWindowHolder();
			}
			else
			{
				DisplaySveltoEcsEnginesEditorWindowContent();
			}
		}

		private void DisplaySveltoEcsEnginesEditorWindowHolder()
		{
			GUILayout.Label ("To see list of observable engines, press Play button!", EditorStyles.boldLabel);
		}

		private void DisplaySveltoEcsEnginesEditorWindowContent()
		{
			var assembly = Assembly.GetAssembly(ObservableEnginesSingletonContext.CompositionRoot.GetType());
			var observableEngines = assembly.GetTypes()
				.Where(x => x.ContainsCustomAttribute(typeof(ObservableEngineAttribute)))
				.ToArray();
			
			GUILayout.Label ("List of observable engines:", EditorStyles.boldLabel);

			EditorGUILayout.BeginVertical();
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			
			for (var i = 0; i < observableEngines.Length; i++)
			{
				var observabilityAttributes = observableEngines[i]
					.GetCustomAttributes<ObservableEngineAttribute>()
					.ToArray();

				EditorGUILayout.BeginHorizontal();
				var engineExpanded = _enginesDisplayStateIndex.TryGetValue(observableEngines[i].Name, out var value) && value;
				
				_enginesDisplayStateIndex[observableEngines[i].Name] = EditorGUILayout.BeginFoldoutHeaderGroup(engineExpanded, $"{i + 1}. {observableEngines[i].Name}");

				if (engineExpanded)
				{
					EditorGUILayout.BeginVertical();
					for (var j = 0; j < observabilityAttributes.Length; j++)
					{
						DisplayComponents(observabilityAttributes[j]);
						DisplayEntityCount(observabilityAttributes[j]);
						GUILayout.Space(15);
					}
					EditorGUILayout.EndVertical();	
				}

				EditorGUILayout.EndFoldoutHeaderGroup();
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

			var entitiesDb = GetEntitiesDbFromEnginesRoot(ObservableEnginesSingletonContext.EnginesRoot);

			for (var i = 0; i < attribute.GroupNameList.Count; i++)
			{
				var result = methodRef.Invoke(entitiesDb, new[] { GetEcsGroupByName(attribute.GroupNameList[i], attribute.EcsGroupsType) });
				var count = constructedClass.GetProperty("count")?.GetValue(result);

				totalCount += (uint)(count ?? 0);
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

		private EntitiesDB GetEntitiesDbFromEnginesRoot(EnginesRoot enginesRoot)
		{
			return (EntitiesDB) enginesRoot.GetType()
				.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
				.First(x => x.Name == "_entitiesDB")
				.GetValue(enginesRoot);
		}
	}
}
