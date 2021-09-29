namespace Alten.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Reservation
{
    public int Id { get; set; }
    public string GuestName { get; set; }
    public DateTime AccomodationStart { get; set; }
    public DateTime AccomodationEnd { get; set; }
}
