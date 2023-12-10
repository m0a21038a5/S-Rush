/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ADDRESSABLES_INTEGRATION
 * @{
 */

#if CRI_USE_ADDRESSABLES

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using System.Linq;
using UnityEditor.AddressableAssets;
using System.IO;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace CriWare.Assets
{
	internal static class CriAddressablesEditor
	{
		internal static event System.Action OnAfterImportCriAsset;

		static bool shouldDestroyUnusedAnchors = false;
		[InitializeOnLoadMethod]
		static void RegisterCallback()
		{
			CriAssetImporter.OnCreateImpl += path => {
				shouldDestroyUnusedAnchors = true;
				EditorApplication.delayCall += () => {
					if (!shouldDestroyUnusedAnchors) return;
					OnAfterImportCriAsset?.Invoke();
					shouldDestroyUnusedAnchors = false;
				};
			};
		}

		public static void DeployData(string srcPath, string dstPath)
		{
			if (!Directory.Exists(Path.GetDirectoryName(dstPath)))
				Directory.CreateDirectory(Path.GetDirectoryName(dstPath));
			if (File.Exists(dstPath))
				File.Delete(dstPath);
			File.Copy(srcPath, dstPath);
			// remote read-only attribute
			var att = File.GetAttributes(dstPath);
			if ((att & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				File.SetAttributes(dstPath, att & ~FileAttributes.ReadOnly);
		}

		public static IEnumerable<CriAssetBase> CollectDependentAssets<T>() where T : ICriAssetImpl =>
			EditorUtility.CollectDependencies(
				AddressableAssetSettingsDefaultObject.Settings.groups
				.SelectMany(group => group.entries)
				.SelectMany(entry =>
				{
					var ret = new List<AddressableAssetEntry>();
					entry.GatherAllAssets(ret, true, true, true);
					return ret;
				})
				.Select(entry => entry.TargetAsset).ToArray()
			)
			.Where(obj => obj is CriAssetBase)
			.Select(obj => obj as CriAssetBase)
			.Where(obj => !(obj is ICriReferenceAsset) && (obj.Implementation is T));
	}

	internal class CriAddressableGroup
	{
		string groupName;
		string templateName;
		CriAddressablesPathPair pathPair;

		public CriAddressableGroup(string name, string temp, CriAddressablesPathPair pathPair)
		{
			if(Settings == null)
			{
				UnityEngine.Debug.LogError("[CRIWARE] Create Addressables Settings before using CRI Addressables.");
				return;
			}
			groupName = name;
			templateName = temp;
			this.pathPair = pathPair;
		}

		public string Name => groupName;

		AddressableAssetGroupTemplate _groupTemplate = null;
		AddressableAssetGroupTemplate GroupTemplate
		{
			get
			{
				if (_groupTemplate == null)
					_groupTemplate = AssetDatabase.LoadAssetAtPath<AddressableAssetGroupTemplate>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:AddressableAssetGroupTemplate {templateName}").FirstOrDefault()));
				return _groupTemplate;
			}
		}

		AddressableAssetGroup _criAddressableGroup;
		public AddressableAssetGroup AddressableGroup
		{
			get
			{
				if (_criAddressableGroup == null)
					for (int i = Settings.groups.Count - 1; i >= 0; i--)
						if (Settings.groups[i].name.Contains(groupName))
							_criAddressableGroup = Settings.groups[i];
				if (_criAddressableGroup == null)
					_criAddressableGroup = Settings.CreateGroup(groupName, false, true, true, GroupTemplate.SchemaObjects);
				_criAddressableGroup.GetSchema<BundledAssetGroupSchema>().BuildPath.SetVariableById(Settings, pathPair.buildPath.Id);
				_criAddressableGroup.GetSchema<BundledAssetGroupSchema>().LoadPath.SetVariableById(Settings, pathPair.loadPath.Id);
				return _criAddressableGroup;
			}
		}

		static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;

		public void ClearGroup()
		{
			for (int i = Settings.groups.Count - 1; i >= 0; i--)
				if (Settings.groups[i].name.Contains(groupName))
					Settings.RemoveGroup(Settings.groups[i]);
			AssetDatabase.SaveAssets();
			_criAddressableGroup = null;
		}
	}
}

#endif

/** @} */
