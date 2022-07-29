namespace Sisyphos
{
    public interface IController<TControlled>
        where TControlled : IControlled
    {
        public Double P { get; set; }
        public Double I { get; set; }
        public Double D { get; set; }
        public Double CorrectionMax { get; set; }
        public Double CorrectionMin { get; set; }

        TControlled Controlled { get; }
        
        void Correct();
    }
}