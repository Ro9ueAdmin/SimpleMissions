using SimpleMissions;
using SimpleMissions.Attributes;

using GTA;
using GTA.Math;

namespace ExampleMissions.Missions
{
    /// <summary>
    /// The basic information on the mission. You can learn more about this in the API docs.
    /// </summary>
    [MissionInfo("Car Robbery", "exMisCarRob", 485.979f, -1311.222f, 29.249f, MissionType.Default, 2500, "None", Characters.Franklin)]
    public class CarRobbery : Mission
    {
        /// <summary>
        /// All of the variables the mission uses. These MUST be static if you intend to modify them at runtime.
        /// </summary>
        static int ticks = 0;
        static MissionState state = MissionState.GoToCar;
        static Vehicle voltic;
        static Blip volticBlip;
        static Blip mansionBlip;
        static Blip hayesBlip;

        /// <summary>
        /// The mission setup. This spawns things like vehicles, blips, etc
        /// </summary>
        public override void Start()
        {
            UI.Notify("Car Robbery started!");
            voltic = World.CreateVehicle(new Model("voltic"), new Vector3(-926.974f, 11.819f, 47.719f), 214.167f);
            Main.spawnedVehicles.Add(voltic);
            volticBlip = voltic.AddBlip();
            volticBlip.Name = "Voltic";
            volticBlip.Color = BlipColor.Blue;

            mansionBlip = World.CreateBlip(new Vector3(-926.974f, 11.819f, 47.719f));
            mansionBlip.Color = BlipColor.Yellow;
            mansionBlip.Name = "Mansion";
            mansionBlip.ShowRoute = true;
            Main.drawnBlips.Add(mansionBlip);

            Main.drawnBlips.Add(volticBlip);
        }

        /// <summary>
        /// Called once per frame, this is where all of the actual mission logic is
        /// </summary>
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
                    if(mansionBlip != null)
                    {
                        Main.drawnBlips.Remove(mansionBlip);
                        mansionBlip.Remove();
                        mansionBlip = null;
                    }
                    if (Game.Player.Character.CurrentVehicle == voltic) state = MissionState.GoToHayes;
                    break;

                case MissionState.GoToHayes:
                    if(volticBlip.Alpha != 0) volticBlip.Alpha = 0;
                    if (hayesBlip == null)
                    {
                        hayesBlip = World.CreateBlip(new Vector3(487.549f, -1313.981f, 28.585f));
                        hayesBlip.Color = BlipColor.Yellow;
                        hayesBlip.Name = "Hayes Autos";
                        hayesBlip.ShowRoute = true;
                        Main.drawnBlips.Add(hayesBlip);
                    }
                    if(hayesBlip.Alpha == 0) hayesBlip.Alpha = 255;
                    UI.ShowSubtitle("Bring the Voltic to ~y~Hayes Autos");
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

        /// <summary>
        /// Cleans up after the mission by removing blips and marking the vehicle as not needed
        /// </summary>
        public override void End()
        {
            Main.drawnBlips.Remove(hayesBlip);
            Main.drawnBlips.Remove(volticBlip);
            volticBlip.Remove();
            hayesBlip.Remove();
            Main.spawnedVehicles.Remove(voltic);
            voltic.LockStatus = VehicleLockStatus.CannotBeTriedToEnter;
            voltic.MarkAsNoLongerNeeded();
        }

        /// <summary>
        /// The state that the mission is in, this is just an easy and simple way to manage objectives
        /// </summary>
        private enum MissionState
        {
            GoToCar, StealCar, ReturnToCar, GoToHayes
        }
    }
}
