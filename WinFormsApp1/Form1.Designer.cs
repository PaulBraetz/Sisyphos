using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using Sisyphos;

namespace WinFormsApp1
{
	partial class Form1
	{
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


			var referenceSeries = new FunctionSeries()
			{
				Color = OxyColor.FromRgb(255, 0, 0)
			};

			var targets = new[] { 0, 50, 100, 50 };

			for (int j = 0; j < targets.Length; j++)
			{
				referenceSeries.Points.Add(new DataPoint(j * 100, targets[j]));
				referenceSeries.Points.Add(new DataPoint((j + 1) * 100, targets[j]));
			}

			plotView.Model.Series.Add(referenceSeries);

			var plots = 2d;

			for (Double p = 0; p < 1; p += 1d / plots)
			{
				for (Double i = 0; i < 1; i += 1d / plots)
				{
					for (Double d = 0; d < 1; d += 1d / plots)
					{
						var controller = new Controller()
						{
							P = p,
							I = i,
							D = d
						};

						var functionSeries = new FunctionSeries()
						{
							Color = OxyColor.FromRgb((Byte)(200 * p + 20), (Byte)(200 * i + 20), (Byte)(200 * d + 20))
						};

						for (int j = 0; j < targets.Length; j++)
						{
							referenceSeries.Points.Add(new DataPoint(j * 100, targets[j]));
							referenceSeries.Points.Add(new DataPoint((j + 1) * 100, targets[j]));

							controller.Target = targets[j];

							for (int k = 0; k < 100; k++)
							{
								functionSeries.Points.Add(new DataPoint((j * 100) + k, Math.Clamp(controller.Calculate(), -1 * targets[j], targets[j])));
							}
						}

						plotView.Model.Series.Add(functionSeries);
					}
				}
			}

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