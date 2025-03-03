namespace Versioning
{
    /// <summary>
    /// Reprenents a version of any kind.
    /// Structure: <c>"Major.Minor.Patch"</c>
    /// </summary>
    public readonly struct Version
    {
        private readonly int? _Major, _Minor, _Patch;
        public int Major => _Major ?? 0;
        public int Minor => _Minor ?? 0;
        public int Patch => _Patch ?? 0;

        public Version(int Major) : this()
        => _Major = Major;
        public Version(int Major, int Minor) : this(Major)
        => _Minor = Minor;
        public Version(int Major, int Minor, int Patch) : this(Major, Minor)
        => _Patch = Patch;

        public static bool operator ==(Version V1, Version V2)
        {
            if (V1.Major == V2.Major && V1.Minor == V2.Minor && V1.Patch == V2.Patch) return true;
            else return false;
        }
        public static bool operator !=(Version V1, Version V2) => !(V1 == V2);

        public static bool operator >(Version V1, Version V2)
        {
            if ((V1.Major > V2.Major)
            || (V1.Major == V2.Major && V1.Minor > V2.Minor)
            || (V1.Major == V2.Major && V1.Minor == V2.Minor && V1.Patch > V2.Patch)) return true;
            else return false;
        }
        public static bool operator <(Version V1, Version V2)
        {
            if ((V1.Major < V2.Major)
            || (V1.Major == V2.Major && V1.Minor < V2.Minor)
            || (V1.Major == V2.Major && V1.Minor == V2.Minor && V1.Patch < V2.Patch)) return true;
            else return false;
        }

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode()
        {
            string S = ToString();
            return S.GetHashCode();
        }

        public override string ToString() => $"{Major}.{Minor}.{Patch}";
    }

    public static class VersionExtensions
    {
        /// <summary>
        /// Converts a string to a Version
        /// </summary>
        /// <param name="sVersion"> Version as a string </param>
        /// <returns></returns>
        public static Version ToVersion(this string sVersion)
        {
            string[] Chars = sVersion.Split('.', '-', ' ');
            return new Version(int.Parse(Chars[0]), int.Parse(Chars[1]), int.Parse(Chars[2]));
        }
    }
}
