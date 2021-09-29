namespace Alten.API.Models;

public class Reservation
{
    private DateTime accomodationStart;
    private DateTime accomodationEnd;

    public int Id { get; set; }

    public string GuestName { get; set; }

    public DateTime AccomodationStart
    {
        get => accomodationStart;
        set => accomodationStart = value.Date;
    }

    public DateTime AccomodationEnd
    {
        get => accomodationEnd;
        set => accomodationEnd = value.Date.AddDays(1).AddSeconds(-1);
    }
}
