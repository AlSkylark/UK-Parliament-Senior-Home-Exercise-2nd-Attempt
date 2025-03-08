namespace UKParliament.CodeTest.Data;

public class ApiConfiguration
{
    public const string Section = "ApiConfiguration";

    public required string BaseUrl { get; set; }
    public required string ApiPrefix { get; set; }
    public int DefaultLimit { get; set; }
}
