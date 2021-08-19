using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KDCtrlLib.KDControls
{
	/// <summary>
	/// Interaction logic for KDProgressBar.xaml
	/// </summary>
	public partial class KDProgressBar : UserControl
	{
		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(KDProgressBar));
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(KDProgressBar));
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(KDProgressBar));
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(KDProgressBar));

		public double Value
		{
			get
			{
				return (double)GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}
		public double Maximum
		{
			get
			{
				return (double)GetValue(MaximumProperty);
			}
			set
			{
				SetValue(MaximumProperty, value);
			}
		}
		public double Minimum
		{
			get
			{
				return (double)GetValue(MinimumProperty);
			}
			set
			{
				SetValue(MinimumProperty, value);
			}
		}
		public Orientation Orientation
		{
			get
			{
				return (Orientation)GetValue(OrientationProperty);
			}
			set
			{
				SetValue(OrientationProperty, value);
			}
		}
		public KDProgressBar()
		{
			InitializeComponent();
			Orientation = Orientation.Vertical;
		}
	}
}
