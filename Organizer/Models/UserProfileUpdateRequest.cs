using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class UserProfileUpdateRequest
    {
        // this will NOT come up in the body of the request. It must be pulled from the cookie in the controller
        public int Id { get; set; }

        public BasicUserInfo User { get; set; }

        // user-type-specific fields
        // Only ONE of these may be non-null!
        public AthleteUpdateRequest Athlete { get; set; }
        public CoachUpdateRequest Coach { get; set; }
        public AdvocateUpdateRequest Advocate { get; set; }
        public AthleticDirectorUpdateRequest AthleticDirector { get; set; }

        public class BasicUserInfo
        {
            // basic info
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int? UserTypeId { get; set; }
            public string AvatarUrl { get; set; }
            public bool? SubscribeToNewsLetter { get; set; }
        }

        public class AthleteUpdateRequest
        {
            public DateTime? DOB { get; set; }
            public string BirthPlace { get; set; }
            public int? SchoolId { get; set; }
            public int? SportsLevelId { get; set; }
            public int? EducationLevelId { get; set; }
            public int? ClassYearId { get; set; }
            public int? GraduationYear { get; set; }
            public string ShortBio { get; set; }
        }

        public class CoachUpdateRequest
        {
            public string Title { get; set; }
            public int? SchoolId { get; set; }
            public bool? IsCurrent { get; set; }
            public string ShortBio { get; set; }
        }

        public class AdvocateUpdateRequest
        {
            public string Title { get; set; }
            public int? SchoolId { get; set; }
            public int? ClubId { get; set; }
            public bool? IsCurrent { get; set; }
            public string ShortBio { get; set; }
        }

        public class AthleticDirectorUpdateRequest
        {
            public string Title { get; set; }
            public int? SchoolId { get; set; }
            public int? ClubId { get; set; }
            public bool? IsCurrent { get; set; }
            public string ShortBio { get; set; }
        }
    }
}
