using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace Macro_Counter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ObservableCollection<Food> Foods { get; private set; }

		private readonly string filePath = "output.json";
		public MainWindow()
		{
			InitializeComponent();
			PopulateFoodList();
			DataContext = this;
		}



		/// <summary>
		/// Add 1 item to local json file.
		/// </summary>
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			AddFoodWindow addFoodWindow = new AddFoodWindow(FoodIndex);
			if (addFoodWindow.ShowDialog() == true)
			{
				List<Food> foods = GetJsonList();
				if (foods == null)
				{
					foods = new List<Food>();
				}
				foods.Add(addFoodWindow.Food);

				string updatedJson = JsonConvert.SerializeObject(foods, Formatting.Indented);

				File.WriteAllText(filePath, updatedJson);

				Foods.Clear();
				foreach (var food in foods)
				{
					Foods.Add(food);
				}
				amountOfItems.Content = Foods.Count().ToString();

			}
		}
		public int FoodIndex = 0;

		private void PopulateFoodList()
		{

			try
			{
				Foods = new ObservableCollection<Food>(GetJsonList());
			}
			catch (ArgumentNullException)
			{
				Foods = new ObservableCollection<Food>();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				FoodIndex = Foods.Count() > 0 ? Foods.Max(f => f.Id) + 1 : 1;
				amountOfItems.Content = Foods.Count();
			}
		}

		private List<Food> GetJsonList()
		{
			if (File.Exists(filePath))
			{
				string JsonData = File.ReadAllText(filePath);
				return JsonConvert.DeserializeObject<List<Food>>(JsonData);
			}
			return new List<Food>();

		}

		private void Remove_Item(Object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement element && element.DataContext is Food food)
			{
				Foods.Remove(food);
				UpdateJsonFile();
				amountOfItems.Content = Foods.Count();
			}
		}

		private void Edit_Item(Object sender, RoutedEventArgs e)
		{

			if (sender is FrameworkElement element && element.DataContext is Food food)
			{
				AddFoodWindow editFoodWindow = new AddFoodWindow(food);
				if (editFoodWindow.ShowDialog() == true)
				{
					food.FoodName = editFoodWindow.Food.FoodName;
					food.PortionSize = editFoodWindow.Food.PortionSize;
					food.Calories = editFoodWindow.Food.Calories;
					food.Proteins = editFoodWindow.Food.Proteins;
					food.PicPath = editFoodWindow.Food.PicPath;

					UpdateJsonFile();
					UpdateUI();
				}
			}
		}


		private void Increase_Counter_Click(Object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement element && element.DataContext is Food food)
			{
				// calories
				int oldTotalCalories = Int32.Parse(totalCalories.Content.ToString());
				oldTotalCalories += food.Calories;
				totalCalories.Content = oldTotalCalories;

				// protein
				int oldTotalProtein = Int32.Parse(totalProtein.Content.ToString());
				oldTotalProtein += food.Proteins;
				totalProtein.Content = oldTotalProtein;
			}
		}

		private void Decrease_Counter_Click(Object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement element && element.DataContext is Food food)
			{

				int oldTotalCalories = Int32.Parse(totalCalories.Content.ToString());
				int oldTotalProtein = Int32.Parse(totalProtein.Content.ToString());
				if (oldTotalCalories - food.Calories >= 0 && oldTotalProtein - food.Proteins >= 0)
				{
					oldTotalCalories -= food.Calories;
					oldTotalProtein -= food.Proteins;
				} else
				{
					oldTotalCalories = 0;
					oldTotalProtein= 0;
				}
				totalCalories.Content = oldTotalCalories;
				totalProtein.Content = oldTotalProtein;

			}
		}

		private void Clear_List_Click(Object sender, RoutedEventArgs e)
		{
			if (sender is FrameworkElement element)
			{
				totalCalories.Content = "0";
				totalProtein.Content = "0";
			}
		}


		private void UpdateJsonFile()
		{
			string updatedJson = JsonConvert.SerializeObject(Foods, Formatting.Indented);
			File.WriteAllText(filePath, updatedJson);
		}
		private void UpdateUI()
		{
			List<Food> foods = GetJsonList();
			Foods.Clear();
			foreach (var food in foods)
			{
				Foods.Add(food);

			}
		}
	}
}


