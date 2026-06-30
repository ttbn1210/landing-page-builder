namespace TrackQR.Web.Models;

public enum CampaignStatus
{
    Draft,
    Active,
    Paused,
    Completed,
    Archived
}

public enum QRCodeStatus
{
    Active,
    Disabled,
    Expired
}

public enum LandingPageStatus
{
    Draft,
    Published,
    Archived
}

public enum DeviceType
{
    Unknown,
    Mobile,
    Tablet,
    Desktop
}

public enum VisitorSource
{
    QRScan,
    Direct,
    ManualLink
}

public enum EventType
{
    PageView,
    ButtonClick,
    FormOpen,
    FormSubmit,
    CallClick,
    Scroll50,
    Scroll100
}

public enum LeadStatus
{
    New,
    Contacted,
    Interested,
    Appointment,
    Converted,
    Lost
}

public enum TemplateType
{
    SimpleHero,
    LeadForm,
    BookingPage,
    ProductPage
}
