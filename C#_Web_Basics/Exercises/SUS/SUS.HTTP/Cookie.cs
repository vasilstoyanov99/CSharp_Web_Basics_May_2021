namespace SUS.HTTP
{
    public class Cookie
    {
        public Cookie(string data)
        {
            var split = data.Split('=', 2);
            this.Name = split[0];
            this.Value = split[1];
        }

        public Cookie(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{this.Name}={this.Value}";
        }
    }
}
