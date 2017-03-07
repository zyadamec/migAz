namespace MigAz.Azure.Arm
{
    public class Probe
    {
        public string name;
        public Probe_Properties properties;

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Probe))
                return false;

            return ((Probe)obj).name == this.name;
        }
    }
}
