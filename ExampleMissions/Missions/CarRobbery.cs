using SimpleMissions;
using SimpleMissions.Attributes;

using GTA;
using GTA.Math;
using GTA.Native;

namespace ExampleMissions.Missions
{
    [MissionInfo("Car Robbery", "exMisCarRob", 485.979f, -1311.222f, 29.249f, MissionType.Default, 2500, "None", Characters.Franklin)]
    public class CarRobbery : Mission
    {
        static int ticks = 0;
        static MissionState state = MissionState.GoToCar;
        static Vehicle voltic;
        static Blip volticBlip;
        static Blip hayesBlip;

        public override void Start()
        {
            UI.Notify("Car Robbery started!");
            voltic = World.CreateVehicle(new Model("voltic"), new Vector3(-926.974f, 11.819f, 47.719f), 214.167f);
            volticBlip = voltic.AddBlip();
            volticBlip.Name = "Mansion";
            volticBlip.ShowRoute = true;
            volticBlip.Color = BlipColor.Yellow;
            Main.drawnBlips.Add(volticBlip);
        }

        public override void Tick()
        {
            switch (state)
            {
                case MissionState.GoToCar:
                    UI.ShowSubtitle("Go to the ~y~Mansion");
                    if(World.GetDistance(voltic.Position, Game.Player.Character.Position) <= 25)
                        state = MissionState.StealCar;
                    break;

                case MissionState.StealCar:
                    UI.ShowSubtitle("Steal the ~b~Voltic");
                    volticBlip.Name = "Voltic";
                    volticBlip.Color = BlipColor.Blue;
                    if (Game.Player.Character.CurrentVehicle == voltic) state = MissionState.GoToHayes;
                    break;

                case MissionState.GoToHayes:
                    volticBlip.Alpha = 0;
                    hayesBlip.Alpha = 255;
                    if (hayesBlip == null)
                    {
                        hayesBlip = World.CreateBlip(new Vector3(487.549f, -1313.981f, 28.585f));
                        hayesBlip.Color = BlipColor.Yellow;
                        hayesBlip.Name = "Hayes Autos";
                        hayesBlip.ShowRoute = true;
                        Main.drawnBlips.Add(hayesBlip);
                    }
                    UI.ShowSubtitle("Bring the voltic to ~y~Hayes Autos");
                    if (Game.Player.Character.CurrentVehicle != voltic) state = MissionState.ReturnToCar;
                    else if(World.GetDistance(new Vector3(487.549f, -1313.981f, 28.585f), Game.Player.Character.CurrentVehicle.Position) <= 3.5f)
                    {
                        Game.Player.Character.Task.LeaveVehicle(voltic, true);
                        Stop();
                    }
                    break;

                case MissionState.ReturnToCar:
                    volticBlip.Alpha = 255;
                    hayesBlip.Alpha = 0;
                    UI.ShowSubtitle("Get back into the ~b~Voltic");
                    if (Game.Player.Character.CurrentVehicle == voltic) state = MissionState.GoToHayes;
                    break;
            }
        }

        public override void End()
        {
            Main.drawnBlips.Remove(hayesBlip);
            Main.drawnBlips.Remove(volticBlip);
            volticBlip.Remove();
            hayesBlip.Remove();
            voltic.LockStatus = VehicleLockStatus.CannotBeTriedToEnter;
            voltic.MarkAsNoLongerNeeded();
            UI.Notify("Car Robbery Ended!");
        }

        private enum MissionState
        {
            GoToCar, StealCar, ReturnToCar, GoToHayes
        }
    }
}
