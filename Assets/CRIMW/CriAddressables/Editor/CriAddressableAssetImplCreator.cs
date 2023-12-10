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
using UnityEditor.AddressableAssets;
using UnityEditor;
using System.IO;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[CriDisplayName("Addressables (Remote)")]
	public class CriAddressableAssetImplCreator : ICriAssetImplCreator
	{
		public string Description =>
@"Available for assets that are delivered via the Addressables system.
When the assets are loaded, their respective data files are downloaded from a remote location.
Please use Addressables(Local) if you want the Addressables System to manage the assets to be included in your build.";

		public ICriAssetImpl CreateAssetImpl(AssetImportContext ctx)
		{
			if(AddressableAssetSettingsDefaultObject.Settings == null)
				throw new System.Exception($"[CRIWARE] AddressableAssetSettingsDefaultObject.Settings is null.\nCreate Addresasbles Settings and reimport the CRI Asset ({ctx.assetPath})");
			var anchor = CriRemoteAddressableGroupGenerator.CreateAnchor(ctx.assetPath);
			var fileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(anchor)).ToLowerInvariant();
			var impl = new CriAddressableAssetImpl(fileName, anchor, AssetDatabase.AssetPathToGUID(ctx.assetPath));
			return impl;
		}
	}
}

#endif

/** @} */
