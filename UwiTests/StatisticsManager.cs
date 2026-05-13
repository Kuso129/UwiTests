using UwiTests.Model;

namespace UwiTests
{
    public class StatisticsState
    {
        public int UserId { get; set; }
        public Statistics Statistics { get; set; } = new Statistics();
    }

    public class StatisticsManager
    {
        public List<StatisticsState> Stats { get; private set; }

        public StatisticsManager() 
        {
            Stats = new List<StatisticsState>();
        }

        private StatisticsState FindStateId(int id)
        {
            foreach (var i in Stats)
            {
                if (i.UserId == id)
                    return i;
            }

            return null;
        }

        public bool SetStatistics(int id, Statistics stats)
        {
            if (!DataBase.Instance.IsHaveUserID(id))
                return false;

            var stat = FindStateId(id);
            if (stat == null)
            {
                StatisticsState state = new StatisticsState();
                state.UserId = id;
                state.Statistics = stats;
                Stats.Add(state);
            }
            else
            {
                stat.Statistics = stats;
            }

            return true;
        }

        public Statistics GetStatistics(int id)
        {
            var stat = FindStateId(id);
            if (stat == null)
                return null;

            return stat.Statistics;
        }

    }
}
