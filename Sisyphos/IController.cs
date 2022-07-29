namespace Sisyphos
{
    public interface IController<TControlled>
        where TControlled : IControlled
    {
        Double P { get; set; }
        Double I { get; set; }
        Double D { get; set; }

        TControlled Controlled { get; }
        
        void Correct();
    }
}