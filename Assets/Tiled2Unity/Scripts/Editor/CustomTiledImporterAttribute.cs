using System;

namespace Tiled2Unity
{
	[AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	class CustomTiledImporterAttribute : Attribute
	{
		public int Order
		{
			get; set;
		}
	}
}
