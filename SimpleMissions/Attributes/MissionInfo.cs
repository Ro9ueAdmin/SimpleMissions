using GTA.Math;
using System;

namespace SimpleMissions.Attributes
{
    /// <summary>
    /// All of the necessary info to set up a mission
    /// </summary>
    public class MissionInfo : Attribute
    {
        public string displayName, id, preRequisites;
        public int pay;
        public float xPos, yPos, zPos;
        public Vector3 startPoint;
        public Characters availableTo;
        public MissionType type;

        /// <summary>
        /// The basic information about a mission
        /// </summary>
        /// <param name="displayName">The name seen in-game</param>
        /// <param name="id">The unique ID of the mission. Typically this will be formatted something like this: [Mission Pack Name][Mission Name]. For example: exampleMissionsCarRobbery</param>
        /// <param name="xPos">The X position of the starting point</param>
        /// <param name="yPos">The Y position of the starting point</param>
        /// <param name="zPos">The Z position of the starting point</param>
        /// <param name="type">The type of mission, this determines things like the blip and end screen</param>
        /// <param name="pay">How much money the player receives for completing the mission</param>
        /// <param name="preRequisites">The mission(s) that must be completed before this mission can be triggered. This MUST be formatted as follows: "[Mission1ID] [Mission2ID]" and so on.</param>
        /// <param name="availableTo">What characters the mission should be available to.</param>
        public MissionInfo(string displayName, string id, float xPos, float yPos, float zPos, MissionType type, int pay = 2500, string preRequisites = "None", Characters availableTo = Characters.All)
        {
            this.displayName = displayName;
            this.id = id;
            this.preRequisites = preRequisites;
            this.pay = pay;
            this.startPoint = new Vector3();
            this.xPos = xPos;
            this.yPos = yPos;
            this.zPos = zPos;
            this.availableTo = availableTo;
            this.type = type;
        }
    }

    public enum Characters
    {
        Michael, Franklin, Trevor, All
    }
}
