namespace Sisyphos
{
    public interface IControlled
    {
        public Double GetTarget();
        public Double GetResult();
        public void Apply(Double correction);
    }
}