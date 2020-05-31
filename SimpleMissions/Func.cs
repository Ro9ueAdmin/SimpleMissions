using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GTA;

using SimpleMissions.Attributes;

namespace SimpleMissions
{
    public class Func
    {
        /// <summary>
        /// Registers a mission so it can be seen and used in-game
        /// </summary>
        /// <param name="mission"></param>
        public static void RegisterMission(Type mission)
        {
            if (mission.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(MissionInfo)) == null)
                throw new Exception("Mission must have the MissionInfo attribute!");
            Mission.missions.Add(mission);
        }

        /// <summary>
        /// Starts a mission
        /// </summary>
        /// <param name="mission">The mission to start</param>
        public static void StartMission(Type mission)
        {
            Game.FadeScreenOut(1000);
            GTA.Script.Wait(1000);
            foreach(Blip blip in Main.blips)
            {
                blip.Remove();
            }

            var constructor = mission.GetConstructor(Type.EmptyTypes);
            var typeClass = constructor.Invoke(new object[] { });
            var mis = (Mission)typeClass;

            Mission.currentMission = mis;
            Mission.missionType = mission;
            Mission.isOnMission = true;
            mis.Start();
            GTA.Script.Wait(1000);
            Game.FadeScreenIn(1000);
        }

        /// <summary>
        /// Returns a MissionInfo object based on the inputted mission's attributes
        /// </summary>
        /// <param name="mission">The mission to get info on</param>
        internal static MissionInfo GetMissionInfo(Type mission)
        {
            MissionInfo info = (MissionInfo)mission.GetCustomAttribute(typeof(MissionInfo));
            return info;
        }
    }
}
