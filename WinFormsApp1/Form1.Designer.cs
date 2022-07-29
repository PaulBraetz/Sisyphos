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
                return _result + Random.Shared.NextDouble();
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


            var referenceSeries = new FunctionSeries()
            {
                Color = OxyColor.FromRgb(255, 0, 0)
            };

            var stepSize = 50;

            var targets = new[] { 0, 50, 100, 50 };

            for (int j = 0; j < targets.Length; j++)
            {
                referenceSeries.Points.Add(new DataPoint(j * stepSize, targets[j]));
                referenceSeries.Points.Add(new DataPoint((j + 1) * stepSize, targets[j]));
            }

            plotView.Model.Series.Add(referenceSeries);

            var controlled = new Controlled();

            var controller = new Controller(controlled)
            {
                P = 1.1,
                I = 0.1,
                D = 0.01
            };

            var functionSeries = new FunctionSeries()
            {
                Color = OxyColor.FromRgb(0, 200, 0)
            };

            for (int j = 0; j < targets.Length; j++)
            {
                var target = targets[j];
                controlled.Target = target;

                for (int k = 0; k < stepSize; k++)
                {
                    controller.Correct();
                    functionSeries.Points.Add(new DataPoint((j * stepSize) + k, Math.Clamp(controlled.GetResult(), 0.5 * target, 1.5 * target)));
                }
            }

            plotView.Model.Series.Add(functionSeries);

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