﻿using System.Xml;
using ArcherLoaderMod.Layers;
using Microsoft.Xna.Framework;
using Monocle;

namespace ArcherLoaderMod.Source.Layers.PortraitLayers
{
    public class PortraitLayerParser
    {
        public static void Parse(ArcherCustomData data, XmlElement xml)
        {
            if (xml.HasChild("PortraitLayer"))
            {
                var info = ParseLayer(xml["PortraitLayer"]);
                data.PortraitLayerInfos ??= new(1);
                data.PortraitLayerInfos.Add(info);
            }

            if (!xml.HasChild("PortraitLayers")) return;

            var portraitLayersXml = xml["PortraitLayers"];
            foreach (var o in portraitLayersXml)
            {
                if (o is not XmlElement {Name: "Layer"} layerXml) continue;
                var info = ParseLayer(layerXml);
                data.PortraitLayerInfos ??= new();
                data.PortraitLayerInfos.Add(info);
            }
        }

        private static PortraitLayerInfo ParseLayer(XmlElement xml)
        {
            if (FortEntrance.Settings.DisableLayers)
                return null;

            var attachToText = xml.ChildText("AttachTo", null);
                
            var portraitLayerInfo = new PortraitLayerInfo
            {
               
                AttachTo = attachToText == "join" || attachToText == "Join"
                    || attachToText == "joined" || attachToText == "Joined"? PortraitLayersAttachType.Join : PortraitLayersAttachType.NotJoin,

                Sprite = xml.ChildText("Sprite"),
                Position = xml.ChildPosition("Position", Vector2.Zero),
                Color = Calc.HexToColor(xml.ChildText("Color", "FFFFFF")),
                // ColorSwitch = xml.ChildInt("ColorSwitch", 0),
                // ColorSwitchLoop = xml.ChildBool("ColorSwitchLoop", false),
                IsRainbowColor = xml.ChildBool("IsRainbowColor", false),
                ToScale = xml.ChildBool("ToScale", true),
            };

            return portraitLayerInfo;
        }
    }
}