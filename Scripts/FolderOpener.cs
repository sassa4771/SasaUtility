using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SasaUtility
{
	public class iOSFolderOpener : MonoBehaviour
	{
		public void onClickOpen()
		{
			string path = Application.persistentDataPath + "/Image";
			ShowInExplorer(path);
		}

#if UNITY_EDITOR_OSX || (UNITY_STANDALONE_OSX && !UNITY_EDITOR)
		private static string PhotosScheme { get { return "photos://"; } }
		private static string FileScheme { get { return "file://"; } }
#elif UNITY_IOS && !UNITY_EDITOR
		private static string PhotosScheme { get { return "photos-redirect://"; } }
		private static string FileScheme { get { return "shareddocuments://"; } }
#endif

		private static string URLEscapePathByPercentEncoding(string path)
		{
			return path.Replace("%", "%25")     // Escape percents first so escaped character's percents aren't themselves escaped
					   .Replace(" ", "%20")
					   .Replace("\"", "%22")
					   .Replace("#", "%23")
					   .Replace(";", "%3B")
					   .Replace("<", "%3C")
					   .Replace(">", "%3E")
					   .Replace("?", "%3F")
					   .Replace("[", "%5B")
					   .Replace("\\", "%5C")
					   .Replace("]", "%5D")
					   .Replace("^", "%5E")
					   .Replace("`", "%60")
					   .Replace("{", "%7B")
					   .Replace("|", "%7C")
					   .Replace("}", "%7D");
		}

		public static bool ShowInExplorer(string itemPath)
		{
			bool result = false;
#if UNITY_EDITOR_OSX || (!UNITY_EDITOR && (UNITY_STANDALONE_OSX || UNITY_IOS))
			string url;
			if (itemPath.StartsWith("avpmc-photolibrary://"))
			{
				// TODO: Find out how to open photos to a specific album/item
				url = PhotosScheme;
			}
			else
			{
				url = FileScheme + URLEscapePathByPercentEncoding(Directory.GetParent(itemPath).FullName);
			}
			UnityEngine.Debug.LogFormat("ShowInExplorer - opening URL: {0}", url);
			UnityEngine.Application.OpenURL(url);
			result = true;
#else
#if !UNITY_WEBPLAYER
			itemPath = Path.GetFullPath(itemPath.Replace(@"/", @"\"));   // explorer doesn't like front slashes
			if (File.Exists(itemPath))
			{
#if UNITY_EDITOR_WIN || (!UNITY_EDITOR && UNITY_STANDALONE_WIN)
				Process.Start("explorer.exe", "/select," + itemPath);
#else

#endif
				result = true;
			}
			else if (Directory.Exists(itemPath))
			{
				// NOTE: We use OpenURL() instead of the explorer process so that it opens explorer inside the folder
				UnityEngine.Application.OpenURL(itemPath);
				result = true;
			}

#endif
#endif // UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

			return result;
		}
	}
}