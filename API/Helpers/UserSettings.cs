using System;
using API.Enums;

namespace API.Helpers
{
    // TODO: move the constants into config. Maybe use user settings factory, which spits out default setup and then lets to adjust the values
    public class UserSettings : PaginationSettings
    {
        public string CurrentUsername { get; set; }

        public string Gender { get; set; }

        // TODO: Move the age into the settings
        public int MinAge { get; set; } = 18;
        
        // TODO: Move the age into the settings
        public int MaxAge { get; set; } = 200;

        public MembersSortOptionsEnum OrderBy { get; set; } = MembersSortOptionsEnum.None;
    }
}
