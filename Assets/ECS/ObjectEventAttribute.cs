using System;

namespace ECS
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ObjectEventAttribute: Attribute
	{
	}
}