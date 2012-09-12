namespace PacketFragmenter.Classes
{
    internal class Router
    {
        public int MTU { get; private set; }

        public string Name { get; set; }

        public Router()
        {
            MTU = 1492;
        }

        public Router(string name, int mtu)
        {
            Name = name;
            MTU = mtu;
        }

        public override string ToString()
        {
            return string.Format("[Name : {0}, MTU : {1}]", Name, MTU);
        }
    }
}