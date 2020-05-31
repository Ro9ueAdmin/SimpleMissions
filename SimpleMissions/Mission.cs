using GTA;
using SimpleMissions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleMissions
{
    public abstract class Mission
    {
        internal static List<Type> missions = new List<Type>();
        internal static bool isOnMission = false;
        internal static Mission currentMission = null;
        internal static Type missionType = null;

        /// <summary>
        /// This is called when a mission is started by the user. Typically this would be used to set up the mission by spawning things like peds and vehicles
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// This is called on every tick even from ScriptHookVDotNet
        /// </summary>
        public abstract void Tick();
        /// <summary>
        /// This is called as soon as the mission is ended. Typically this would be used for cleanup such as removing peds and vehicles
        /// </summary>
        public abstract void End();

        /// <summary>
        /// Stops a mission
        /// </summary>
        public void Stop()
        {
            currentMission.End();

            MissionInfo info = Func.GetMissionInfo(missionType);
            isOnMission = false;
            Game.Player.Money += info.pay;
            currentMission = null;
            missionType = null;
        }
    }

    /// <summary>
    /// The type of mission, this determines what kind of blip and mission passed screen the mission has
    /// </summary>
    public enum MissionType
    {
        Default, Stranger, Heist, HeistSetup
    }
}
