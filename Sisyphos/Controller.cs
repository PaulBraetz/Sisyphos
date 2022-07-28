namespace Sisyphos
{
	public sealed class Controller : IController
	{
		public Double Target { get; set; }

		public Double P { get; set; }

		public Double I { get; set; }

		public Double D { get; set; }

		public Double Result { get; set; }

		private Double _integral;
		private Double _error;
		private Double _previousError;

		public void Calculate()
		{
			_error = Target - Result;

			_integral = _error + _integral;

			Result = Result + P * _error + I * _integral + D * (_error - _previousError);

			_previousError = _error;
		}
	}
	public sealed class Controller<TControlled> : IController
		where TControlled : IControlled
	{
		public Controller(TControlled controlled)
		{
			_controlled = controlled;
			_controller = new Controller()
			{
				Target = controlled.Target,
				Result = controlled.Result,
			};
		}

		private TControlled _controlled;
		private IController _controller;

		public Double D { get => _controller.D; set => _controller.D = value; }
		public Double I { get => _controller.I; set => _controller.I = value; }
		public Double P { get => _controller.P; set => _controller.P = value; }
		public Double Result
		{
			get => _controlled.Result;
			set
			{
				_controller.Result = value;
				_controlled.Result = value;
			}
		}
		public Double Target
		{
			get => _controlled.Target;
			set
			{
				_controller.Target = value;
				_controlled.Target = value;
			}
		}

		public void Calculate()
		{
			_controller.Calculate();
		}
	}
}