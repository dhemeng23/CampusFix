namespace campusfix.Models;

public class Issue
{
    public int Id {get; set; }
    public string Title {get; set; }="";
    public string Location {get; set;}="";
    public string Description { get; set;}="";
    public string Status {get; set; }="Pending";
}