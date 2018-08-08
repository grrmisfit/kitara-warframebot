using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordbot.Modules
{
    class Warframe
    {
        public class Id
        {
            public string __invalid_name__$oid { get; set; }
    }

    public class Message
    {
        public string LanguageCode { get; set; }
        public string Message { get; set; }
    }

    public class Date2
    {
        public string __invalid_name__$numberLong { get; set; }
}

public class Date
{
    public Date2 __invalid_name__$date { get; set; }
}

public class Event
{
    public Id _id { get; set; }
    public List<Message> Messages { get; set; }
    public string Prop { get; set; }
    public Date Date { get; set; }
    public string ImageUrl { get; set; }
    public bool Priority { get; set; }
    public bool MobileOnly { get; set; }
}

public class Id2
{
    public string __invalid_name__$oid { get; set; }
}

public class Date3
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation
{
    public Date3 __invalid_name__$date { get; set; }
}

public class Date4
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Expiry
{
    public Date4 __invalid_name__$date { get; set; }
}

public class RewardInterim
{
    public int credits { get; set; }
    public List<string> items { get; set; }
}

public class RewardInterim2
{
    public int credits { get; set; }
    public List<string> items { get; set; }
}

public class Reward
{
    public int credits { get; set; }
    public List<string> items { get; set; }
}

public class BonusReward
{
    public int credits { get; set; }
    public List<string> items { get; set; }
}

public class Goal
{
    public Id2 _id { get; set; }
    public Activation Activation { get; set; }
    public Expiry Expiry { get; set; }
    public int Count { get; set; }
    public int Goal { get; set; }
    public int GoalInterim { get; set; }
    public int GoalInterim2 { get; set; }
    public int BonusGoal { get; set; }
    public int Success { get; set; }
    public bool Personal { get; set; }
    public bool Bounty { get; set; }
    public bool ClampNodeScores { get; set; }
    public string Node { get; set; }
    public List<string> ConcurrentMissionKeyNames { get; set; }
    public List<int> ConcurrentNodeReqs { get; set; }
    public List<string> ConcurrentNodes { get; set; }
    public string MissionKeyName { get; set; }
    public string Faction { get; set; }
    public string Desc { get; set; }
    public string Icon { get; set; }
    public string Tag { get; set; }
    public RewardInterim RewardInterim { get; set; }
    public RewardInterim2 RewardInterim2 { get; set; }
    public Reward Reward { get; set; }
    public BonusReward BonusReward { get; set; }
}

public class Id3
{
    public string __invalid_name__$oid { get; set; }
}

public class Date5
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation2
{
    public Date5 __invalid_name__$date { get; set; }
}

public class Date6
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Expiry2
{
    public Date6 __invalid_name__$date { get; set; }
}

public class CountedItem
{
    public string ItemType { get; set; }
    public int ItemCount { get; set; }
}

public class MissionReward
{
    public int credits { get; set; }
    public List<string> items { get; set; }
    public List<CountedItem> countedItems { get; set; }
}

public class MissionInfo
{
    public string missionType { get; set; }
    public string faction { get; set; }
    public string location { get; set; }
    public string levelOverride { get; set; }
    public string enemySpec { get; set; }
    public int minEnemyLevel { get; set; }
    public int maxEnemyLevel { get; set; }
    public double difficulty { get; set; }
    public int seed { get; set; }
    public int maxWaveNum { get; set; }
    public bool archwingRequired { get; set; }
    public MissionReward missionReward { get; set; }
    public string extraEnemySpec { get; set; }
}

public class Alert
{
    public Id3 _id { get; set; }
    public Activation2 Activation { get; set; }
    public Expiry2 Expiry { get; set; }
    public MissionInfo MissionInfo { get; set; }
}

public class Id4
{
    public string __invalid_name__$oid { get; set; }
}

public class Date7
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation3
{
    public Date7 __invalid_name__$date { get; set; }
}

public class Date8
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Expiry3
{
    public Date8 __invalid_name__$date { get; set; }
}

public class Variant
{
    public string missionType { get; set; }
    public string modifierType { get; set; }
    public string node { get; set; }
    public string tileset { get; set; }
}

public class Sorty
{
    public Id4 _id { get; set; }
    public Activation3 Activation { get; set; }
    public Expiry3 Expiry { get; set; }
    public string Boss { get; set; }
    public string Reward { get; set; }
    public List<object> ExtraDrops { get; set; }
    public int Seed { get; set; }
    public List<Variant> Variants { get; set; }
    public bool Twitter { get; set; }
}

public class Id5
{
    public string __invalid_name__$oid { get; set; }
}

public class Date9
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation4
{
    public Date9 __invalid_name__$date { get; set; }
}

public class Date10
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Expiry4
{
    public Date10 __invalid_name__$date { get; set; }
}

public class Job
{
    public string jobType { get; set; }
    public string rewards { get; set; }
    public int masteryReq { get; set; }
    public int minEnemyLevel { get; set; }
    public int maxEnemyLevel { get; set; }
    public List<int> xpAmounts { get; set; }
}

public class SyndicateMission
{
    public Id5 _id { get; set; }
    public Activation4 Activation { get; set; }
    public Expiry4 Expiry { get; set; }
    public string Tag { get; set; }
    public int Seed { get; set; }
    public List<object> Nodes { get; set; }
    public List<Job> Jobs { get; set; }
}

public class Id6
{
    public string __invalid_name__$oid { get; set; }
}

public class Date11
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation5
{
    public Date11 __invalid_name__$date { get; set; }
}

public class Date12
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Expiry5
{
    public Date12 __invalid_name__$date { get; set; }
}

public class ActiveMission
{
    public Id6 _id { get; set; }
    public int Region { get; set; }
    public int Seed { get; set; }
    public Activation5 Activation { get; set; }
    public Expiry5 Expiry { get; set; }
    public string Node { get; set; }
    public string MissionType { get; set; }
    public string Modifier { get; set; }
}

public class Date13
{
    public string __invalid_name__$numberLong { get; set; }
}

public class StartDate
{
    public Date13 __invalid_name__$date { get; set; }
}

public class Date14
{
    public string __invalid_name__$numberLong { get; set; }
}

public class EndDate
{
    public Date14 __invalid_name__$date { get; set; }
}

public class FlashSale
{
    public string TypeName { get; set; }
    public StartDate StartDate { get; set; }
    public EndDate EndDate { get; set; }
    public bool Featured { get; set; }
    public bool Popular { get; set; }
    public bool ShowInMarket { get; set; }
    public int BannerIndex { get; set; }
    public int Discount { get; set; }
    public int RegularOverride { get; set; }
    public int PremiumOverride { get; set; }
    public int BogoBuy { get; set; }
    public int BogoGet { get; set; }
}

public class Id7
{
    public string __invalid_name__$oid { get; set; }
}

public class AttackerMissionInfo
{
    public int seed { get; set; }
    public string faction { get; set; }
}

public class CountedItem2
{
    public string ItemType { get; set; }
    public int ItemCount { get; set; }
}

public class DefenderReward
{
    public List<CountedItem2> countedItems { get; set; }
}

public class DefenderMissionInfo
{
    public int seed { get; set; }
    public string faction { get; set; }
    public List<object> missionReward { get; set; }
}

public class Date15
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation6
{
    public Date15 __invalid_name__$date { get; set; }
}

public class Invasion
{
    public Id7 _id { get; set; }
    public string Faction { get; set; }
    public string Node { get; set; }
    public int Count { get; set; }
    public int Goal { get; set; }
    public string LocTag { get; set; }
    public bool Completed { get; set; }
    public object AttackerReward { get; set; }
    public AttackerMissionInfo AttackerMissionInfo { get; set; }
    public DefenderReward DefenderReward { get; set; }
    public DefenderMissionInfo DefenderMissionInfo { get; set; }
    public Activation6 Activation { get; set; }
}

public class Id8
{
    public string __invalid_name__$oid { get; set; }
}

public class NodeOverride
{
    public Id8 _id { get; set; }
    public string Node { get; set; }
    public bool Hide { get; set; }
    public int? Seed { get; set; }
}

public class Id9
{
    public string __invalid_name__$oid { get; set; }
}

public class Id10
{
    public string __invalid_name__$oid { get; set; }
}

public class DefenderInfo
{
    public bool IsAlliance { get; set; }
    public Id10 Id { get; set; }
    public string Name { get; set; }
    public string MOTD { get; set; }
    public string DeployerName { get; set; }
    public string DeployerClan { get; set; }
}

public class BadlandNode
{
    public Id9 _id { get; set; }
    public DefenderInfo DefenderInfo { get; set; }
    public string Node { get; set; }
}

public class Id11
{
    public string __invalid_name__$oid { get; set; }
}

public class Date16
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation7
{
    public Date16 __invalid_name__$date { get; set; }
}

public class Date17
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Expiry6
{
    public Date17 __invalid_name__$date { get; set; }
}

public class Manifest
{
    public string ItemType { get; set; }
    public int PrimePrice { get; set; }
    public int RegularPrice { get; set; }
}

public class VoidTrader
{
    public Id11 _id { get; set; }
    public Activation7 Activation { get; set; }
    public Expiry6 Expiry { get; set; }
    public string Character { get; set; }
    public string Node { get; set; }
    public List<Manifest> Manifest { get; set; }
}

public class PrimeAccessAvailability
{
    public string State { get; set; }
}

public class PrimeVaultAvailability
{
    public string State { get; set; }
}

public class Date18
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Activation8
{
    public Date18 __invalid_name__$date { get; set; }
}

public class Date19
{
    public string __invalid_name__$numberLong { get; set; }
}

public class Expiry7
{
    public Date19 __invalid_name__$date { get; set; }
}

public class DailyDeal
{
    public string StoreItem { get; set; }
    public Activation8 Activation { get; set; }
    public Expiry7 Expiry { get; set; }
    public int Discount { get; set; }
    public int OriginalPrice { get; set; }
    public int SalePrice { get; set; }
    public int AmountTotal { get; set; }
    public int AmountSold { get; set; }
}

public class LibraryInfo
{
    public string LastCompletedTargetType { get; set; }
}

public class Id12
{
    public string __invalid_name__$oid { get; set; }
}

public class Date20
{
    public string __invalid_name__$numberLong { get; set; }
}

public class StartDate2
{
    public Date20 __invalid_name__$date { get; set; }
}

public class Date21
{
    public string __invalid_name__$numberLong { get; set; }
}

public class EndDate2
{
    public Date21 __invalid_name__$date { get; set; }
}

public class Param
{
    public string n { get; set; }
    public int v { get; set; }
}

public class PVPChallengeInstance
{
    public Id12 _id { get; set; }
    public string challengeTypeRefID { get; set; }
    public StartDate2 startDate { get; set; }
    public EndDate2 endDate { get; set; }
    public List<Param> @params { get; set; }
    public bool isGenerated { get; set; }
    public string PVPMode { get; set; }
    public List<object> subChallenges { get; set; }
    public string Category { get; set; }
}

public class RootObject
{
    public string WorldSeed { get; set; }
    public int Version { get; set; }
    public string MobileVersion { get; set; }
    public string BuildLabel { get; set; }
    public int Time { get; set; }
    public int Date { get; set; }
    public List<Event> Events { get; set; }
    public List<Goal> Goals { get; set; }
    public List<Alert> Alerts { get; set; }
    public List<Sorty> Sorties { get; set; }
    public List<SyndicateMission> SyndicateMissions { get; set; }
    public List<ActiveMission> ActiveMissions { get; set; }
    public List<object> GlobalUpgrades { get; set; }
    public List<FlashSale> FlashSales { get; set; }
    public List<Invasion> Invasions { get; set; }
    public List<object> HubEvents { get; set; }
    public List<NodeOverride> NodeOverrides { get; set; }
    public List<BadlandNode> BadlandNodes { get; set; }
    public List<VoidTrader> VoidTraders { get; set; }
    public PrimeAccessAvailability PrimeAccessAvailability { get; set; }
    public List<PrimeVaultAvailability> PrimeVaultAvailabilities { get; set; }
    public List<DailyDeal> DailyDeals { get; set; }
    public LibraryInfo LibraryInfo { get; set; }
    public List<PVPChallengeInstance> PVPChallengeInstances { get; set; }
    public List<object> PersistentEnemies { get; set; }
    public List<object> PVPAlternativeModes { get; set; }
    public List<object> PVPActiveTournaments { get; set; }
    public List<double> ProjectPct { get; set; }
    public List<object> ConstructionProjects { get; set; }
    public List<object> TwitchPromos { get; set; }
}
    }
}
