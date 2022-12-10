using System.ComponentModel;

namespace Domain.Common
{
    public enum Roles
    {
        Root = 1,
        Admin,
        Webapi,
        Regular
    }
    public enum RecordStatus
    {
        [Description("New")]
        New = 0,
        [Description("Edited")]
        Edited = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("IsQueued")]
        IsQueued = 3,
        [Description("Rejected")]
        Rejected = 4,
        [Description("Posted")]
        Posted = 5,
        [Description("IsProcessed")]
        IsProcessed = 6,
        [Description("Failed")]
        Failed = 7,
        [Description("Extracted")]
        Extracted = 8,
        [Description("Text Alert Generated")]
        TextAlertGenerated = 9,
        [Description("Disabled")]
        DisAbled = 10,
    }
}
