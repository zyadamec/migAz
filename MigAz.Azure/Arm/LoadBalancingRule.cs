namespace MigAz.Azure.Arm
{
    public class LoadBalancingRule
    {
        public string name;
        public LoadBalancingRule_Properties properties;

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(LoadBalancingRule))
                return false;

            return ((LoadBalancingRule)obj).name == this.name;
        }
    }
}
