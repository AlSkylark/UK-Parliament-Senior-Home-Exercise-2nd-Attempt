namespace UKParliament.CodeTest.Data.HATEOAS;

public class Link(string rel, string href, string[] actions)
{
    public string Rel { get; set; } = rel;
    public string Href { get; set; } = href;
    public string[] Actions { get; set; } = actions;

    public static Link GenerateLink(string rel, string href, params string[] actions)
    {
        return new Link(rel, href, actions);
    }
}
