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

using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace CriWare.Assets
{
	internal static class CriRemoteAddressableGroupGenerator
	{
		[InitializeOnLoadMethod]
		static void RegisterCallback() =>
			CriAddressablesEditor.OnAfterImportCriAsset += DestroyUnusedAnchors;

		static CriAddressableGroup _group = null;
		static CriAddressableGroup Group => _group ?? (_group = new CriAddressableGroup("CriData", "CriPackedAssetsTemplate", CriAddressablesSetting.Instance.Remote));

		static string CriDataPath { get
			{
				if (!Directory.Exists(CriAddressablesSetting.Instance.AnchorFolderPath))
					Directory.CreateDirectory(CriAddressablesSetting.Instance.AnchorFolderPath);
				return CriAddressablesSetting.Instance.AnchorFolderPath;
			} }

		public static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;

		public static void DeployData(string originalPath, AddressableAssetEntry entry)
		{
			var dataRootDir = CriAddressablesSetting.Instance.Remote.buildPath.GetValue(Settings);
			var dataPath = Path.Combine(dataRootDir, $"{Group.Name.ToLowerInvariant()}_assets_{entry.address.ToLowerInvariant()}");

			CriAddressablesEditor.DeployData(originalPath, dataPath);

			// Delete bundles built by addressables
			var builtBundlePath = $"{dataPath}.bundle";
			if (File.Exists(builtBundlePath))
				File.Delete(builtBundlePath);
		}

		public static CriAddressablesAnchor CreateAnchor(string assetPath)
		{
			var anchorPath = Path.Combine(CriDataPath, $"{Path.GetFileName(assetPath).ToLowerInvariant()}={new FileInfo(assetPath).Length.ToString("x")}.asset");

			var anchor = AssetDatabase.LoadAssetAtPath<CriAddressablesAnchor>(anchorPath);
			if(anchor == null)
			{
				anchor = ScriptableObject.CreateInstance<CriAddressablesAnchor>();
				var md5 = MD5.Create();
				var hash = md5.ComputeHash(File.ReadAllBytes(assetPath));
				md5.Clear();
				anchor.hash = BitConverter.ToString(hash).ToLower().Replace("-", "");
				AssetDatabase.CreateAsset(anchor, anchorPath);
				AssetDatabase.SaveAssets();
			}

			if (Settings != null)
			{
				var path = AssetDatabase.GetAssetPath(anchor);
				var name = anchor.name;
				EditorApplication.delayCall += () =>
				{
					var entry = Settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), Group.AddressableGroup, true);
					entry.address = $"{CriAddressablesSetting.Instance.deployFolderPath}/{name}";
				};
			}

			return anchor;
		}

		public static void DestoroyAnchor(CriAddressablesAnchor anchor)
		{
			if (Settings?.profileSettings == null) return;

			var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(anchor));

			// Delete non-asset data if exist
			var dataRootDir = CriAddressablesSetting.Instance.Remote.buildPath.GetValue(Settings);
			var dataPath = Path.Combine(dataRootDir, $"{Group.Name.ToLowerInvariant()}_assets_{Settings.FindAssetEntry(guid)?.address?.ToLowerInvariant()}");
			if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
				Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
			if (File.Exists(dataPath))
				File.Delete(dataPath);

			Settings.RemoveAssetEntry(guid);

			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(anchor));
			AssetDatabase.SaveAssets();

			// can not delete group while importing asset
			EditorApplication.delayCall += () =>
			{
				if (Group.AddressableGroup.entries.Count <= 0)
					Group.ClearGroup();
			};
		}

		public static void DestroyUnusedAnchors()
		{
			var criAddressableAssets = AssetDatabase.FindAssets($"t:{nameof(CriAssetBase)}").Select(guid => AssetDatabase.LoadAssetAtPath<CriAssetBase>(AssetDatabase.GUIDToAssetPath(guid))).Where(asset => !(asset is ICriReferenceAsset) && (asset.Implementation is CriAddressableAssetImpl));
			var anchors = AssetDatabase.FindAssets($"t:{nameof(CriAddressablesAnchor)}").Select(guid => AssetDatabase.LoadAssetAtPath<CriAddressablesAnchor>(AssetDatabase.GUIDToAssetPath(guid))).ToList();

			if (criAddressableAssets.Count() >= anchors.Count) return;

			foreach (var asset in criAddressableAssets)
				anchors.Remove((asset.Implementation as CriAddressableAssetImpl).anchor);

			foreach (var anchor in anchors)
			{
				if (string.IsNullOrEmpty(anchor.hash)) continue;
				DestoroyAnchor(anchor);
			}
		}

		internal static void ValidateGroup()
		{
			if (Group.AddressableGroup == null) return;

			var schema = Group.AddressableGroup.GetSchema<UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema>();
			if (schema == null)
			{
				Debug.LogWarning("[CRIWARE] CriData Addressable Group does not have BundledAssetGroupSchema");
				return;
			}

			if (schema.AssetBundleProviderType.ToString() != nameof(CriResourceProvider))
				Debug.LogWarning("[CRIWARE] CriData Addressable Group have to use CriResourceProvider as AssetBundleProvider");
			if (schema.BundleMode != UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundlePackingMode.PackSeparately)
				Debug.LogWarning("[CRIWARE] CriData Addressable Group have to use PackSeparately BundleMode");
			if (schema.BundleNaming != UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundleNamingStyle.NoHash)
				Debug.LogWarning("[CRIWARE] CriData Addressable Group have to use NoHash BundleNaming");
		}
	}
}

#endif

/** @} */
