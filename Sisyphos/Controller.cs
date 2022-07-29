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
        public Double T { get; set; } = 1;
        public Double CorrectionMax { get; set; } = Double.MaxValue;
        public Double CorrectionMin { get; set; } = Double.MinValue;

        public TControlled Controlled { get; }

        private Double _integral;
        private Double _error;
        private Double _previousError;

        public virtual void Correct()
        {
            var target = Controlled.GetTarget();
            var result = Controlled.GetResult();

            _error = target - result;

            _integral += _error;

            var derivative = _error - _previousError;

            var correction = P * _error + (I * T) * _integral + (D / T) * derivative;

            _previousError = _error;

            correction = Math.Clamp(correction, CorrectionMin, CorrectionMax);

            Controlled.Apply(correction);
        }
    }
    public class Controller : Controller<IControlled>
    {
        public Controller(IControlled controlled) : base(controlled)
        {
        }
    }
    public class TimedController<TControlled> : Controller<TControlled>
        where TControlled : IControlled
    {
        public TimedController(TControlled controlled, TimeSpan correctionInterval) : base(controlled)
        {
            T = correctionInterval.Milliseconds;
        }

        private DateTimeOffset lastCorrection = DateTimeOffset.MinValue;

        public override void Correct()
        {
            Correct(DateTimeOffset.UtcNow);
        }
        public void Correct(DateTimeOffset t)
        {
            var deltaT = (t - lastCorrection).Milliseconds;
            if (deltaT > T)
            {
                lastCorrection = t;
                base.Correct();
            }
        }
    }

    public class TimedController : TimedController<IControlled>
    {
        public TimedController(IControlled controlled, TimeSpan correctionInterval) : base(controlled, correctionInterval)
        {
        }
    }
}