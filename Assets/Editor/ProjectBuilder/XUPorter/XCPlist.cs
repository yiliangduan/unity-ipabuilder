using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.XCodeEditor 
{
	public class XCPlist
	{
		string plistPath;
		bool plistModified;

		// URLTypes constant --- plist
		const string BundleUrlTypes = "CFBundleURLTypes";
		const string BundleTypeRole = "CFBundleTypeRole";
		const string BundleUrlName = "CFBundleURLName";
		const string BundleUrlSchemes = "CFBundleURLSchemes";
		
		
		public const string CFBundleDisplayName = "CFBundleDisplayName";
		public const string CFBundleShortVersionString = "CFBundleShortVersionString";
		public const string CFBundleVersion = "CFBundleVersion";

		// URLTypes constant --- projmods
		const string PlistUrlType = "urltype";
		const string PlistRole = "role";
		const string PlistEditor = "Editor";
		const string PlistName = "name";
		const string PlistSchemes = "schemes";

		const string PlistDicType = "dicType";
		const string PlistDicKey = "key";
		const string PlistDicValue = "value";

		public XCPlist(string plistPath)
		{
			this.plistPath = plistPath;
		}

		public void Process(Hashtable plist)
		{
			if (null == plist) return;

			Dictionary<string, object> dict = (Dictionary<string, object>)PlistCS.Plist.readPlist(plistPath);
			foreach( DictionaryEntry entry in plist)
			{
				this.AddPlistItems((string)entry.Key, entry.Value, dict);
			}

			if (plistModified)
			{
				PlistCS.Plist.writeXml(dict, plistPath);
			}
		}

		// http://stackoverflow.com/questions/20618809/hashtable-to-dictionary
		public static Dictionary<K,V> HashtableToDictionary<K,V> (Hashtable table)
		{
			Dictionary<K,V> dict = new Dictionary<K,V>();
			foreach(DictionaryEntry kvp in table)
				dict.Add((K)kvp.Key, (V)kvp.Value);
			return dict;
		}

		void processDicTypes (ArrayList list,  Dictionary<string, object> dict)
		{
			foreach(Hashtable table in list)
			{
				dict[(string)table[PlistDicKey]] = table[PlistDicValue];

				plistModified = true;
			}
		}

		public void AddPlistItems(string key, object value, Dictionary<string, object> dict)
		{
			if (key.CompareTo(PlistUrlType) == 0)
			{
				processUrlTypes((ArrayList)value, dict);
			}
			else if (key.CompareTo(PlistDicType) == 0)
			{
				processDicTypes((ArrayList)value, dict);
			}
			else
			{
				dict[key] = HashtableToDictionary<string, object>((Hashtable)value);
				plistModified = true;
			}
		}
		
		public void OverWriteItems(Dictionary<string, object> dict)
		{
			Dictionary<string, object> plistDict = (Dictionary<string, object>)PlistCS.Plist.readPlist(plistPath);

			foreach (KeyValuePair<string, object> entry in dict)
			{
				if (plistDict.ContainsKey(entry.Key))
				{
					plistDict[entry.Key] = entry.Value;
				}
			}

			PlistCS.Plist.writeXml(plistDict, plistPath);
		}

		private void processUrlTypes(ArrayList urltypes, Dictionary<string, object> dict)
		{
			List<object> bundleUrlTypes;
			if (dict.ContainsKey(BundleUrlTypes))
			{
				bundleUrlTypes = (List<object>)dict[BundleUrlTypes];
			}
			else
			{
				bundleUrlTypes = new List<object>();
			}
			foreach(Hashtable table in urltypes)
			{
				string role = (string)table[PlistRole];
				if (string.IsNullOrEmpty(role))
				{
					role = PlistEditor;
				}
				string name = (string)table[PlistName];
				ArrayList shcemes = (ArrayList)table[PlistSchemes];
				
				// new schemes
				List<object> urlTypeSchemes = new List<object>();
				foreach(string s in shcemes)
				{
					urlTypeSchemes.Add(s);
				}
				
				Dictionary<string, object> urlTypeDict = this.findUrlTypeByName(bundleUrlTypes, name);
				if (urlTypeDict == null)
				{
					urlTypeDict = new Dictionary<string, object>();
					urlTypeDict[BundleTypeRole] = role;
					urlTypeDict[BundleUrlName] = name;
					urlTypeDict[BundleUrlSchemes] = urlTypeSchemes;
					bundleUrlTypes.Add(urlTypeDict);
				}
				else
				{
					urlTypeDict[BundleTypeRole] = role;
					urlTypeDict[BundleUrlSchemes] = urlTypeSchemes;
				}
				plistModified = true;
			}
			dict[BundleUrlTypes] = bundleUrlTypes;
		}
		
		private Dictionary<string, object> findUrlTypeByName(List<object> bundleUrlTypes, string name)
		{
			if ((bundleUrlTypes == null) || (bundleUrlTypes.Count == 0))
				return null;
			
			foreach(Dictionary<string, object> dict in bundleUrlTypes)
			{
				string _n = (string)dict[BundleUrlName];
				if (string.Compare(_n, name) == 0)
				{
					return dict;
				}
			}
			return null;
		}
	}
}
