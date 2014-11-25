using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models {
    public class RiskModel {
        public IEnumerable<Risk> Risks { get; set; }
        public IEnumerable<RiskLevel> Levels { get; set; }
        public IEnumerable<RiskStatus> Statuses { get; set; }
    }
}