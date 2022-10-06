using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using STD;

namespace STD
{
	public class Util
	{
		struct TexInfo
		{
			public TexInfo(Texture t, string p)
			{
				tex = t;
				path = p;
			}

			public Texture tex;
			public string path;
		}

		static List<TexInfo> listTI = new List<TexInfo>();

		public static Texture createTexture(string path)
		{
			int i, j = listTI.Count;
			for(i=0; i<j; i++)
			{
				if (listTI[i].path == path)
					return listTI[i].tex;
			}

			Texture tex = Resources.Load<Texture>(path);

			TexInfo ti = new TexInfo(tex, path);
			listTI.Add(ti);

			return tex;
		}

		public static iTexture createITexture(string path)
		{
			return new iTexture(createTexture(path));
		}

		public static void cleanTexture()
		{
			//int i, j = listTI.Count;
			//for (i = 0; i < j; i++)
			//{
			//
			//}
			listTI.Clear();
		}
	}

}
