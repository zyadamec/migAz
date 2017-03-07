namespace MigAz.Azure.Arm
{
    public class InboundNatRule
    {
        public string name;
        public InboundNatRule_Properties properties;

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(InboundNatRule))
                return false;

            return ((InboundNatRule)obj).name == this.name;
        }
    }
}
