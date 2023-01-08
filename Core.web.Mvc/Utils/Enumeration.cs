using System;
using System.ComponentModel;

namespace Core.web.Mvc.Utils
{
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
    public enum WellKnownUserRoles
    {
        [Description("Super Administrator")]
        SuperAdministrator = 1,
        [Description("Administrator")]
        Administrator = 2,
        [Description("Accountant")]
        APIAccount = 3,
        [Description("Human Resource")]
        HumanResource = 4,
        [Description("Standard User")]
        StandardUser = 5,
    }
    public enum DLRStatus
    {
        [Description("UnKnown")]
        UnKnown,
        [Description("Failed")]
        Failed,
        [Description("Pending")]
        Pending,
        [Description("Delivered")]
        Delivered,
        [Description("Delivered To Network")]
        DeliveredToNetwork,
        [Description("Delivered To Terminal")]
        DeliveredToTerminal,
        [Description("Not Applicable")]
        NotApplicable,
        [Description("Submitted")]
        Submitted,
        [Description("Message Waiting")]
        MessageWaiting,
        [Description("Delivery Impossible")]
        DeliveryImpossible,
        [Description("Delivery Uncertain")]
        DeliveryUncertain,
        [Description("Delivery Notification Not Supported")]
        DeliveryNotificationNotSupported,
    }
    public enum ServiceOrigin
    {
        [Description("Web MVC")]
        WebMVC = 1,
        [Description("Web API")]
        WebAPI = 2,
        [Description("Windows Service")]
        WindowsService = 3
    }
}
