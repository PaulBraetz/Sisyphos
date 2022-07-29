namespace Sisyphos
{
    public class Controller<TControlled> : IController<TControlled>
        where TControlled : IControlled
    {
        public Controller(TControlled controlled)
        {
            Controlled = controlled;
        }

        public Double P { get; set; }
        public Double I { get; set; }
        public Double D { get; set; }

        public TControlled Controlled { get; }

        private Double _integral;
        private Double _error;
        private Double _previousError;

        public void Correct()
        {
            var target = Controlled.GetTarget();
            var result = Controlled.GetResult();

            _error = target - result;
            _integral += _error;
            var derivative = _error - _previousError;
            var correction = P * _error + I * _integral + D * derivative;
            _previousError = _error;

            Controlled.Apply(correction);
        }
    }
    public class Controller : Controller<IControlled>
    {
        public Controller(IControlled controlled) : base(controlled)
        {
        }
    }
}