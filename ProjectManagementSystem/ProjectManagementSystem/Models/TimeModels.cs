using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models {
    public class TimeCreateModel {
        public IEnumerable<Requirement> Requirements { get; set; }
        public IEnumerable<Phase> Phases { get; set; }
    }
}