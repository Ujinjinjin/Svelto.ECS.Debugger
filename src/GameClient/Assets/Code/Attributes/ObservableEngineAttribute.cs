using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ObservableEngineAttribute : Attribute
	{
		public int TypeCount { get; }
		public Type T1 { get; }
		public Type T2 { get; }
		public Type T3 { get; }
		public Type T4 { get; }
		public Type EcsGroupsType { get; }
		public IList<string> GroupNameList { get; }
		
		public ObservableEngineAttribute(Type t1, Type ecsGroupsType, params string[] groupNameList)
		{
			TypeCount = 1;
			T1 = t1;
			EcsGroupsType = ecsGroupsType;
			GroupNameList = groupNameList
				.Select(x => x.Split('.').Last())
				.ToArray();
		}
		
		public ObservableEngineAttribute(Type t1, Type t2, Type ecsGroupsType, params string[] groupNameList)
		{
			TypeCount = 2;
			T1 = t1;
			T2 = t2;
			EcsGroupsType = ecsGroupsType;
			GroupNameList = groupNameList
				.Select(x => x.Split('.').Last())
				.ToArray();
		}
		
		public ObservableEngineAttribute(Type t1, Type t2, Type t3, Type ecsGroupsType, params string[] groupNameList)
		{
			TypeCount = 1;
			T1 = t1;
			T2 = t2;
			T3 = t3;
			EcsGroupsType = ecsGroupsType;
			GroupNameList = groupNameList
				.Select(x => x.Split('.').Last())
				.ToArray();
		}
		
		public ObservableEngineAttribute(Type t1, Type t2, Type t3, Type t4, Type ecsGroupsType, params string[] groupNameList)
		{
			TypeCount = 1;
			T1 = t1;
			T2 = t2;
			T3 = t3;
			T4 = t4;
			EcsGroupsType = ecsGroupsType;
			GroupNameList = groupNameList
				.Select(x => x.Split('.').Last())
				.ToArray();
		}
	}
}
