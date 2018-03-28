// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2018 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Hazelnut
// Contributors:    

using System.Collections.Generic;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game.Entity;
using System;

namespace DaggerfallWorkshop.Game.Guilds
{
    public class ThievesGuild : Guild
    {
        #region Constants

        public const string InitiationQuestName = "O0A0AL00";

        protected const int WelcomeMsgId = 5225;    // Not used AFAIK
        protected const int PromotionMsgId = 5235;
        protected const int PromotionFenceId = 5226;
        protected const int PromotionSpymasterId = 5227;
        protected const int PromotionMap1Id = 5228;
        protected const int PromotionMap2Id = 5229;
        protected const int BribesJudgeId = 550;

        private const int factionId = 42;

        #endregion

        #region Properties & Data

        static string[] rankTitles = new string[] {
                "Apprentice", "Journeyman", "Filcher", "Crook", "Robber", "Bandit", "Thief", "Ringleader", "Mastermind", "Master Thief"
        };

        static List<DFCareer.Skills> guildSkills = new List<DFCareer.Skills>() {
                DFCareer.Skills.Backstabbing,
                DFCareer.Skills.Climbing,
                DFCareer.Skills.Lockpicking,
                DFCareer.Skills.Pickpocket,
                DFCareer.Skills.ShortBlade,
                DFCareer.Skills.Stealth,
                DFCareer.Skills.Streetwise
            };

        static List<DFCareer.Skills> trainingSkills = new List<DFCareer.Skills>() {
                DFCareer.Skills.Backstabbing,
                DFCareer.Skills.BluntWeapon,
                DFCareer.Skills.Climbing,
                DFCareer.Skills.Dodging,
                DFCareer.Skills.Jumping,
                DFCareer.Skills.Lockpicking,
                DFCareer.Skills.Pickpocket,
                DFCareer.Skills.ShortBlade,
                DFCareer.Skills.Stealth,
                DFCareer.Skills.Streetwise,
                DFCareer.Skills.Swimming
            };

        public override string[] RankTitles { get { return rankTitles; } }

        public override List<DFCareer.Skills> GuildSkills { get { return guildSkills; } }

        public override List<DFCareer.Skills> TrainingSkills { get { return trainingSkills; } }

        #endregion

        #region Guild Membership and Faction

        public ThievesGuild()
        {
            // Register for location entry events so can auto discover guild houses.
            PlayerGPS.OnEnterLocationRect += PlayerGPS_OnEnterLocationRect;
        }

        public static int FactionId { get { return factionId; } }

        public override int GetFactionId()
        {
            return factionId;
        }

        #endregion

        #region Guild Ranks

        public override TextFile.Token[] TokensPromotion(int newRank)
        {
            return DaggerfallUnity.Instance.TextProvider.GetRandomTokens(GetPromotionMsgId(newRank));
        }

        private int GetPromotionMsgId(int rank)
        {
            switch (rank)
            {
                case 2:
                    return PromotionFenceId;
                case 4:
                    return PromotionSpymasterId;
                case 6:
                    return PromotionMap1Id;
                case 8:
                    return PromotionMap2Id;
                default:
                    return PromotionMsgId;
            }
        }

        #endregion

        #region Benefits

        // TESTING ONLY - REMOVE!
        public override bool CanRest()
        {
            return IsMember();
        }

        public override bool HallAccessAnytime()
        {
            return IsMember();
        }

        #endregion

        #region Service Access:

        public override bool CanAccessService(GuildServices service)
        {
            switch (service)
            {
                case GuildServices.Training:
                    return IsMember();
                case GuildServices.Quests:
                    return true;
                case GuildServices.BuyMagicItems:
                    return (rank >= 2);
                case GuildServices.Spymaster:
                    return (rank >= 4);

                default:
                    return false;
            }
        }

        #endregion

        #region Joining

        public override TextFile.Token[] TokensIneligible(PlayerEntity playerEntity)
        {
            throw new NotImplementedException();
        }
        public override TextFile.Token[] TokensEligible(PlayerEntity playerEntity)
        {
            throw new NotImplementedException();
        }
        public override TextFile.Token[] TokensWelcome()
        {
            return DaggerfallUnity.Instance.TextProvider.GetRSCTokens(WelcomeMsgId);
        }

        #endregion

        #region Event handlers

        private void PlayerGPS_OnEnterLocationRect(DFLocation location)
        {
            BuildingDirectory buildingDirectory = GameManager.Instance.StreamingWorld.GetCurrentBuildingDirectory();
            if (buildingDirectory)
                foreach (BuildingSummary building in buildingDirectory.GetBuildingsOfFaction(factionId))
                    GameManager.Instance.PlayerGPS.DiscoverBuilding(building.buildingKey, GetGuildName());
        }

        #endregion
    }
}