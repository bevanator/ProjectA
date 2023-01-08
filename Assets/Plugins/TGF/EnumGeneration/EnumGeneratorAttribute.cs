using System;

namespace TGF.EnumGeneration
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class EnumGeneratorAttribute : Attribute
	{
		public readonly Type EnumType;
		public readonly string EnumFile;

		public EnumGeneratorAttribute(Type enumType, string enumFile)
		{
			EnumType = enumType;
			EnumFile = enumFile;
		}
	}
}