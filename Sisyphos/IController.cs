namespace Sisyphos
{
	public interface IController
	{
		Double D { get; set; }
		Double I { get; set; }
		Double P { get; set; }
		Double Result { get; set; }
		Double Target { get; set; }

		void Calculate();
	}
}