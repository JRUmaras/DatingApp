using System;

namespace API.Helpers
{
    // TODO: move the constants into config. Maybe use user settings factory, which spits out default setup and then lets to adjust the values
    public class UserSettings
    {
        private const int _maxPageSize = 50;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Min(_maxPageSize, value);
        }
    }
}
