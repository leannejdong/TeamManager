using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamManager
{
    public class TeamDetails
    {
        public string TeamName { get; set; } = string.Empty;
        public string PrimaryContact { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        private int _competitionPoints;
        private string v1;
        private string v2;
        private string v3;
        private string v4;
        private int v5;

        public TeamDetails() { }

        public TeamDetails(string teamName, string primaryContact, string contactPhone, string contactEmail, int competitionPoints)
        {
            TeamName = teamName;
            PrimaryContact = primaryContact;
            ContactPhone = contactPhone;
            ContactEmail = contactEmail;
            _competitionPoints = competitionPoints;

        }

        public TeamDetails(string v) { }

        public TeamDetails(string v, string v1, string v2, string v3, string v4, int v5) : this(v)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
        }

        public int CompetitionPoints
        {
            get { return _competitionPoints; }
            set
            {
                if (value >= 0)
                {
                    _competitionPoints = value;
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(value), "Competition Points cannot be negative.");
            }
        }
    }
}
