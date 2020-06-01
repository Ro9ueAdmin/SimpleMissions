using ExampleMissions.Missions;
using GTA;

using SimpleMissions;
using System;
using System.Collections.Generic;

namespace ExampleMissions
{
    public class Main : Script
    {
        internal static List<Blip> drawnBlips = new List<Blip>();
        internal static List<Vehicle> spawnedVehicles = new List<Vehicle>();

        public Main()
        {
            Aborted += OnAbort;

            Func.RegisterMission(typeof(CarRobbery));
        }

        private void OnAbort(object sender, EventArgs e)
        {
            foreach (var Blip in drawnBlips) Blip.Remove();
            foreach (var Vehicle in spawnedVehicles) Vehicle.MarkAsNoLongerNeeded();
        }
    }
}
