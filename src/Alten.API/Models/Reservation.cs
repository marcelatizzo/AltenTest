namespace Alten.API.Models;

public class Reservation
{
    public int Id { get; set; }
    public string GuestName { get; set; }
    public DateTime AccomodationStart { get; set; }
    public DateTime AccomodationEnd { get; set; }
}
