/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif
using System.IO;

namespace CriWare.Assets
{
	[ScriptedImporter(1, "acb", 2)]
	class CriAtomAcbAssetImporter : CriAssetImporter
	{
		/* deprecated */
		[SerializeField]
		internal CriAtomAwbAsset awb = null;

		[SerializeField]
		internal AcbAssetInfo assetInfo;

		[System.Serializable]
		internal struct AcbAssetInfo
		{
			public CriAtomAwbAsset awb;
		}

		public override void OnImportAsset(AssetImportContext ctx)
		{
			/* for compatibility */
			assetInfo.awb = assetInfo.awb ?? awb;

			var main = ScriptableObject.CreateInstance<CriAtomAcbAsset>();
			main.implementation = CreateAssetImpl(ctx);
			main.awb = assetInfo.awb;
			ctx.AddObjectToAsset("main", main);
			ctx.SetMainObject(main);
		}

		public override bool IsAssetImplCompatible => true;
	}
}

/** @} */
