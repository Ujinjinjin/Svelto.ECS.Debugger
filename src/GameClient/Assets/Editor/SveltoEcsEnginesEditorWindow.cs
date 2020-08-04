using Code;
using Code.Attributes;
using Code.ECS;
using UnityEditor;
using UnityEngine;
using Code.Infrastructure;
using Svelto.ECS;
using System.Linq;
using System.Reflection;

namespace Editor
{
	public class SveltoEcsEnginesEditorWindow : EditorWindow
	{
		[MenuItem("Svelto/Svelto.ECS/Engines")]
		private static void ShowWindow()
		{
			var window = GetWindow<SveltoEcsEnginesEditorWindow>();
			window.titleContent = new GUIContent("Svelto Ecs Engines");
			window.Show();
		}

		private void OnGUI()
		{
			if (!Application.isPlaying)
				return;
			
			var assembly = Assembly.GetAssembly(typeof(SveltoCompositionRoot));
			var observableEngines = assembly.GetTypes()
				.Where(x => x.ContainsCustomAttribute(typeof(ObservableEngine)))
				.ToArray();
			
			GUILayout.Label ("List of observable engines:", EditorStyles.boldLabel);

			for (var i = 0; i < observableEngines.Length; i++)
			{
				var observabilityAttributes = observableEngines[i].GetCustomAttributes<ObservableEngine>()
					.ToArray();
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(observableEngines[i].Name);
				
				EditorGUILayout.BeginVertical();
				for (var j = 0; j < observabilityAttributes.Length; j++)
				{
					EditorGUILayout.BeginHorizontal();
					
					GUILayout.Label($"Querying components: {observabilityAttributes[j].TypeCount.ToString()}");
					GUILayout.Label($"Group: {observabilityAttributes[j].GroupName}");
					GUILayout.Label($"Component 1: {observabilityAttributes[j].T1.Name}");
					GUILayout.Label($"Component 2: {observabilityAttributes[j].T2.Name}");

					EditorGUILayout.EndHorizontal();

					var queryMethod = typeof(EntitiesDB)
						.GetMethods()
						.Where(x => x.Name == "QueryEntities")
						.First(x => x.GetGenericArguments().Length == observabilityAttributes[j].TypeCount);
					var methodRef = queryMethod.MakeGenericMethod(observabilityAttributes[j].T1, observabilityAttributes[j].T2);

					var genericClass = typeof(EntityCollection<,>);
					var constructedClass = genericClass.MakeGenericType(observabilityAttributes[j].T1, observabilityAttributes[j].T2);
					
					var result = methodRef.Invoke(CompositionRootHolder.CompositionRoot.EnginesRoot.EntitiesDb, new[] {(object)EcsGroups.GridGroup});
					var count = constructedClass.GetProperty("count").GetValue(result);
					GUILayout.Label($"Found: {count}");
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			
			// foreach (var engine in CompositionRootHolder.CompositionRoot.EnginesRoot.EnginesSet)
			// {
			// 	GUILayout.Label (engine.TypeName());
			// }
			
		}
		
		private struct DummyEntityComponent : IEntityComponent
		{
			
		}
	}
}
