using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows;

namespace Macro_Counter
{
	/// <summary>
	/// Interaction logic for AddFoodWindow.xaml
	/// </summary>

	public partial class AddFoodWindow : Window
	{
		public string DialogPicturePath { get; set; }
		private int foodId;
		public AddFoodWindow()
		{
			InitializeComponent();
		}
		public AddFoodWindow(int id)
		{
			foodId = id;
			InitializeComponent();
		}
		public AddFoodWindow(Food food)
		{
			InitializeComponent();

			Food.Id = food.Id;
			Food.FoodName = food.FoodName;
			Food.PortionSize = food.PortionSize;
			Food.Calories = food.Calories;
			Food.Proteins = food.Proteins;
			DialogPicturePath = food.PicPath;

			FoodNameTextBox.Text = food.FoodName;
			PortionSizeTextBox.Text = food.PortionSize.ToString();
			CaloriesTextBox.Text = food.Calories.ToString();
			ProteinsTextBox.Text = food.Proteins.ToString();
			PicturePath.Content = food.PicPath;

		}
		public Food Food = new Food();
		private void Ok_Click(object sender, RoutedEventArgs e)
		{

			//validate entered data
			if (!string.IsNullOrWhiteSpace(FoodNameTextBox.Text) &&
				double.TryParse(PortionSizeTextBox.Text, out double portionSize) &&
				int.TryParse(CaloriesTextBox.Text, out int calories) &&
				int.TryParse(ProteinsTextBox.Text, out int proteins) && !string.IsNullOrEmpty(DialogPicturePath))
			{
				Food.Id = foodId;
				Food.FoodName = FoodNameTextBox.Text;
				Food.PortionSize = portionSize;
				Food.Calories = calories;
				Food.Proteins = proteins;
				Food.PicPath = DialogPicturePath;

				DialogResult = true;
			}
			else
			{
				MessageBox.Show("Invalid input. please try again");
			}
		}
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void PicturePath_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Pictures (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
			ofd.FilterIndex = 1;
			if (ofd.ShowDialog() == true)
			{
				DialogPicturePath = ofd.FileName;
				PicturePath.Content = ofd.FileName;
			}
		}
	}
}
