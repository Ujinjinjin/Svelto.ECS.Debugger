using Svelto.ECS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ObservableEngine : Attribute
	{
		public int TypeCount { get; }
		public Type T1 { get; }
		public Type T2 { get; }
		public Type T3 { get; }
		public Type T4 { get; }
		public string GroupName { get; }
		
		public ObservableEngine(Type t1, string groupName)
		{
			TypeCount = 1;
			T1 = t1;
			GroupName = groupName;
		}
		
		public ObservableEngine(Type t1, Type t2, string groupName)
		{
			TypeCount = 2;
			T1 = t1;
			T2 = t2;
			GroupName = groupName;
		}
		
		public ObservableEngine(Type t1, Type t2, Type t3, string groupName)
		{
			TypeCount = 1;
			T1 = t1;
			T2 = t2;
			T3 = t3;
			GroupName = groupName;
		}
		
		public ObservableEngine(Type t1, Type t2, Type t3, Type t4, string groupName)
		{
			TypeCount = 1;
			T1 = t1;
			T2 = t2;
			T3 = t3;
			T4 = t4;
			GroupName = groupName;
		}
	}
}
