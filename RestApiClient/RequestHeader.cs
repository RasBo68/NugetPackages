namespace RGuide.Fundamental.WebApiInteraction.Models
{
    public class HttpRequestHeaderSimplified
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public HttpRequestHeaderSimplified(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}