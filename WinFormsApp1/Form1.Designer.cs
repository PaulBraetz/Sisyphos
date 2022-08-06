using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using Sisyphos;

namespace WinFormsApp1
{
	partial class Form1
	{
		private sealed class Controlled : IControlled
		{
			private Double _result;
			public Double Target { get; set; }

			public void Apply(Double correction)
			{
				_result += correction;
			}

			public Double GetResult()
			{
				return _result + (Random.Shared.NextDouble()/10 - 0.5);
			}

			public Double GetTarget()
			{
				return Target;
			}
		}

		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		protected override void OnLoad(EventArgs e)
		{
			this.Size = new System.Drawing.Size(1000, 1000);

			var plotView = new PlotView()
			{
				Location = new Point(0, 0),
				Size = new Size(850, 850)
			};
			Controls.Add(plotView);

			plotView.Model = new PlotModel()
			{
				Title = "Data"
			};

			var controlled = new Controlled();
			var controller = new TimedController(controlled, TimeSpan.FromMilliseconds(10))
			{
				P = 0.19,
				I = 0.1,
				D = 0.2
			};

			var referenceSeries = new FunctionSeries()
			{
				Color = OxyColor.FromRgb(200, 200, 255)
			};
			var functionSeries = new FunctionSeries()
			{
				Color = OxyColor.FromRgb(0, 200, 0)
			};

			var targets = Convert.ToString(Random.Shared.Next(), 2).Take(5).Select(c => Byte.Parse(c.ToString())).ToArray();

			var steps = 1000d;

			var t = DateTimeOffset.Now;

			for (int j = 0; j < targets.Length; j++)
			{
				var target = targets[j];

				referenceSeries.Points.Add(new DataPoint(j * steps, target));
				referenceSeries.Points.Add(new DataPoint((j + 1) * steps, target));

				controlled.Target = target;

				for (int k = 0; k < steps; k++)
				{
					controller.Correct(t);
					functionSeries.Points.Add(new DataPoint((j * steps) + k, controlled.GetResult()));
					t += TimeSpan.FromMilliseconds(1);
				}
			}

			plotView.Model.Series.Add(functionSeries);
			plotView.Model.Series.Add(referenceSeries);

			base.OnLoad(e);
		}

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Text = "Form1";
		}

		#endregion
	}
}