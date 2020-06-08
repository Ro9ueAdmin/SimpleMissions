using GTA;
using GTA.Math;
using SimpleMissions.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SimpleMissions
{
    internal class Main : Script
    {
        internal static List<Blip> blips = new List<Blip>();
        internal static string modDir = "scripts\\Simple Missions";
        internal static string Saves = modDir + "\\Saves";

        public Main()
        {
            if (!Directory.Exists(modDir)) Directory.CreateDirectory(modDir);
            if (!Directory.Exists(Saves)) Directory.CreateDirectory(Saves);
            SaveManager.LoadSave();

            Tick += OnTick;
            Aborted += OnAbort;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Game.Player.IsDead && Mission.isOnMission)
            {
                Mission.currentMission.Fail("The player died.");
            }

           
            // Tick missions and draw blips if necessary
            foreach (var mission in Mission.missions)
            {
                MissionInfo info = Func.GetMissionInfo(mission);
                if (!SaveManager.save.completedMissions.Contains(info.id))
                {
                    bool allMissionsCompleted = false;
                    var preReq = info.preRequisites.Split(' ');
                    int counter = 0;
                    if (info.preRequisites != "None")
                    {
                        if (info.preRequisites.Contains(" "))
                        {
                            foreach (string missionID in preReq)
                            {
                                if (SaveManager.save.completedMissions.Contains(missionID))
                                {
                                    if (counter == preReq.Length)
                                    {
                                        allMissionsCompleted = true;
                                    }
                                    else UI.Notify($"{counter} {preReq.Length}");
                                }
                                else break;
                                counter++;
                            }
                        }
                        else if (SaveManager.save.completedMissions.Contains(info.preRequisites))
                        {
                            allMissionsCompleted = true;
                        }
                    }
                    else allMissionsCompleted = true;

                    if (allMissionsCompleted)
                    {
                        if (Mission.isOnMission && Mission.missionType == mission)
                        {
                            Mission.currentMission.Tick();
                        }

                        // BLIP MANAGEMENT BEGIN

                        // Create blips
                        if (!Mission.isOnMission && blips.FirstOrDefault(x => x.Position == new GTA.Math.Vector3(info.xPos, info.yPos, info.zPos)) == null)
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
                                        UI.ShowHelpMessage($"Press ~INPUT_CONTEXT~ to start {info.displayName}", 1, true);
                                        if (Game.IsControlJustPressed(2, GTA.Control.Context))
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
                                        UI.ShowHelpMessage($"Press ~INPUT_CONTEXT~ to start {info.displayName}", 1, true);
                                        if (Game.IsControlJustPressed(2, GTA.Control.Context))
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
                                        UI.ShowHelpMessage($"Press ~INPUT_CONTEXT~ to start {info.displayName}", 1, true);
                                        if (Game.IsControlJustPressed(2, GTA.Control.Context))
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
