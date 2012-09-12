namespace PacketFragmenter.Classes
{
    internal class Fragment
    {
        public bool MoreFragments { get; set; }

        public int Offset { get; set; }

        public int Length { get; set; }

        public Fragment(Fragment fragment)
        {
            MoreFragments = fragment.MoreFragments;
            Offset = fragment.Offset;
            Length = fragment.Length;
        }

        public Fragment(int length, bool moreFragments, int offset)
        {
            Length = length;
            MoreFragments = moreFragments;
            Offset = offset;
        }

        public override string ToString()
        {
            return string.Format("[OFFSET: {0} | LENGTH: {1} | MF: {2}]", Offset, Length, MoreFragments);
        }
    }
}