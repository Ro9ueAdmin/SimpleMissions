using GTA;
using GTA.Math;
using SimpleMissions.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMissions
{
    internal class Main : Script
    {
        internal static List<Blip> blips = new List<Blip>();

        public Main()
        {
            Tick += OnTick;
            Aborted += OnAbort;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Game.Player.IsDead && Mission.isOnMission)
            {
                Mission.isOnMission = false;
                Mission.currentMission = null;
                Mission.missionType = null;
            }


            // Tick missions and draw blips if necessary
            foreach (var mission in Mission.missions)
            {
                var constructor = mission.GetConstructor(Type.EmptyTypes);
                var typeClass = constructor.Invoke(new object[] { });
                var mis = (Mission)typeClass;

                if (Mission.isOnMission && Mission.missionType == mission)
                { 
                    mis.Tick();
                }

                // BLIP MANAGEMENT BEGIN

                // Create blips
                MissionInfo info = Func.GetMissionInfo(mission);
                if(!Mission.isOnMission && blips.FirstOrDefault(x => x.Position == new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos)) == null)
                {
                    var newBlip = World.CreateBlip(new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos));
                    newBlip.Name = "Mission";
                    newBlip.Color = BlipColor.White;

                    // Set character specific blip information
                    switch (info.availableTo)
                    {
                        case Characters.Franklin:
                            newBlip.Sprite = BlipSprite.Franklin;
                            newBlip.Color = BlipColor.Franklin;
                            newBlip.Name = "Franklin";
                            break;
                        case Characters.Michael:
                            newBlip.Sprite = BlipSprite.Michael;
                            newBlip.Color = BlipColor.Michael;
                            newBlip.Name = "Michael";
                            break;
                        case Characters.Trevor:
                            newBlip.Sprite = BlipSprite.Trevor;
                            newBlip.Color = BlipColor.Trevor;
                            newBlip.Name = "Trevor";
                            break;
                    }

                    blips.Add(newBlip);
                }

                // Change the blip according to each character and display start messages
                if (!Mission.isOnMission)
                {
                    var blip = blips.FirstOrDefault(x => x.Position == new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos));
                    if (Game.Player.Character.Model == new Model("player_zero")) // Michael
                    {
                        if (info.availableTo == Characters.All && blip.Sprite != BlipSprite.Michael && blip.Color != BlipColor.Michael)
                        {
                            blip.Sprite = BlipSprite.Michael;
                            blip.Color = BlipColor.Michael;
                        }
                        else if (info.type == MissionType.Heist && blip.Sprite != BlipSprite.Heist)
                        {
                            blip.Sprite = BlipSprite.Heist;
                            blip.Color = BlipColor.Michael;
                        }
                        else if (info.type == MissionType.HeistSetup && blip.Sprite != BlipSprite.HeistSetup)
                        {
                            blip.Sprite = BlipSprite.HeistSetup;
                            blip.Color = BlipColor.Michael;
                        }
                        else if (info.type == MissionType.Stranger && blip.Sprite != BlipSprite.StrangersAndFreaks)
                        {
                            blip.Sprite = BlipSprite.StrangersAndFreaks;
                            blip.Color = BlipColor.Michael;
                        }

                        if (info.availableTo == Characters.All || info.availableTo == Characters.Michael)
                        {
                            if (!Mission.isOnMission) World.DrawMarker(MarkerType.VerticalCylinder, new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 1), color: Color.Yellow);
                            if (World.GetDistance(new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Game.Player.Character.Position) <= 2f && !Mission.isOnMission)
                            {
                                UI.ShowHelpMessage($"Press ~y~E~w~ to start {info.displayName}", 1, true);
                                if (Game.IsKeyPressed(Keys.E))
                                {
                                    Func.StartMission(mission);
                                }
                            }
                        }
                        else
                        {
                            if (World.GetDistance(new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Game.Player.Character.Position) <= 5f && !Mission.isOnMission)
                            {
                                UI.ShowHelpMessage($"Return as {info.availableTo} to start {info.displayName}", 1, true);
                            }
                        }
                    }
                    else if (Game.Player.Character.Model == new Model("player_one")) // Franklin
                    {
                        if (info.availableTo == Characters.All && blip.Sprite != BlipSprite.Franklin && blip.Color != BlipColor.Franklin)
                        {
                            blip.Sprite = BlipSprite.Franklin;
                            blip.Color = BlipColor.Franklin;
                        }
                        else if (info.type == MissionType.Heist && blip.Sprite != BlipSprite.Heist)
                        {
                            blip.Sprite = BlipSprite.Heist;
                            blip.Color = BlipColor.Franklin;
                        }
                        else if (info.type == MissionType.HeistSetup && blip.Sprite != BlipSprite.HeistSetup)
                        {
                            blip.Sprite = BlipSprite.HeistSetup;
                            blip.Color = BlipColor.Franklin;
                        }
                        else if (info.type == MissionType.Stranger && blip.Sprite != BlipSprite.StrangersAndFreaks)
                        {
                            blip.Sprite = BlipSprite.StrangersAndFreaks;
                            blip.Color = BlipColor.Franklin;
                        }

                        if (info.availableTo == Characters.All || info.availableTo == Characters.Franklin)
                        {
                            if (!Mission.isOnMission) World.DrawMarker(MarkerType.VerticalCylinder, new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 1), color: Color.Yellow);
                            if (World.GetDistance(new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Game.Player.Character.Position) <= 2f && !Mission.isOnMission)
                            {
                                UI.ShowHelpMessage($"Press ~y~E~w~ to start {info.displayName}", 1, true);
                                if (Game.IsKeyPressed(Keys.E))
                                {
                                    Func.StartMission(mission);
                                }
                            }
                        }
                        else
                        {
                            if (World.GetDistance(new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Game.Player.Character.Position) <= 5f && !Mission.isOnMission)
                            {
                                UI.ShowHelpMessage($"Return as {info.availableTo} to start {info.displayName}", 1, true);
                            }
                        }
                    }
                    else if (Game.Player.Character.Model == new Model("player_two")) // Trevor
                    {
                        if (info.availableTo == Characters.All && blip.Sprite != BlipSprite.Trevor && blip.Color != BlipColor.Trevor)
                        {
                            blip.Sprite = BlipSprite.Trevor;
                            blip.Color = BlipColor.Trevor;
                        }
                        else if (info.type == MissionType.Heist && blip.Sprite != BlipSprite.Heist)
                        {
                            blip.Sprite = BlipSprite.Heist;
                            blip.Color = BlipColor.Trevor;
                        }
                        else if (info.type == MissionType.HeistSetup && blip.Sprite != BlipSprite.Heist)
                        {
                            blip.Sprite = BlipSprite.HeistSetup;
                            blip.Color = BlipColor.Trevor;
                        }
                        else if (info.type == MissionType.Stranger && blip.Sprite != BlipSprite.Heist)
                        {
                            blip.Sprite = BlipSprite.StrangersAndFreaks;
                            blip.Color = BlipColor.Trevor;
                        }

                        if (info.availableTo == Characters.All || info.availableTo == Characters.Trevor)
                        {
                            if (!Mission.isOnMission) World.DrawMarker(MarkerType.VerticalCylinder, new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Vector3.Zero, Vector3.Zero, new Vector3(2, 2, 1), color: Color.Yellow);
                            if (World.GetDistance(new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Game.Player.Character.Position) <= 2f && !Mission.isOnMission)
                            {
                                UI.ShowHelpMessage($"Press ~y~E~w~ to start {info.displayName}", 1, true);
                                if (Game.IsKeyPressed(Keys.E))
                                {
                                    Func.StartMission(mission);
                                }
                            }
                        }
                        else
                        {
                            if (World.GetDistance(new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos - 1.5f), Game.Player.Character.Position) <= 5f && !Mission.isOnMission)
                            {
                                UI.ShowHelpMessage($"Return as {info.availableTo} to start {info.displayName}", 1, true);
                            }
                        }
                    }
                }
                // BLIP MANAGEMENT END
            }
        }

        private void OnAbort(object sender, EventArgs e)
        {
            foreach (Blip blip in blips) blip.Remove();
        }
    }
}
