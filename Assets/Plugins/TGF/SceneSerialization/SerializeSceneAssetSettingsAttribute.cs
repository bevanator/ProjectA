using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.TGF.SceneSerialization
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class SerializeSceneAssetSettingsAttribute : Attribute
	{
		public bool ShowOpenButton = false;
		public bool ShowRefreshButton = false;
		public bool ShowPath = false;
		public bool ShowBuildIndex = false;
		public string Paths;
	}
}