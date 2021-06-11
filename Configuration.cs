using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public abstract class Configuration<C> where C : class, new()
{
    private static C instance;

    const string HideItBobbyConfigName = "HideItBobbyConfig.xml";
    const string HideItConfigName = "HideItConfig.xml";

    public static C Load()
    {
        if (instance == null)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(C));

            try
            {
                if (File.Exists(HideItBobbyConfigName))
                {
                    using (StreamReader streamReader = new StreamReader(HideItBobbyConfigName))
                    {
                        instance = xmlSerializer.Deserialize(streamReader) as C;
                    }
                } else
                {
                    if (File.Exists(HideItConfigName))
                    {
                        using (StreamReader streamReader = new StreamReader(HideItConfigName))
                        {
                            instance = xmlSerializer.Deserialize(streamReader) as C;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Hide it, BOBby!] Configuration:Load -> Exception: " + e.Message);
            }
        }
        return instance ?? (instance = new C());
    }

    public static void Save()
    {
        if (instance == null) return;

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(C));
        XmlSerializerNamespaces noNamespaces = new XmlSerializerNamespaces();
        noNamespaces.Add("", "");

        try
        {
            using (StreamWriter streamWriter = new StreamWriter(HideItBobbyConfigName))
            {
                xmlSerializer.Serialize(streamWriter, instance, noNamespaces);
            }
        }
        catch (Exception e)
        {
            Debug.Log("[Hide it, BOBby!] Configuration:Save -> Exception: " + e.Message);
        }
    }
}
