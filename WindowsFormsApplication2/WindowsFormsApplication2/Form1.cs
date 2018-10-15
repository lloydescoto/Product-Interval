using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        string[] productsList = { "Hotdog", "Bread", "Egg", "Cheese", "Water" };
        List<Product> products = new List<Product>();
        List<Product> productIntervalList = new List<Product>();
        Dictionary<string, double> productInterval = new Dictionary<string, double>();
        Dictionary<string, double> productAverageQty = new Dictionary<string, double>();
        Dictionary<string, double> sortedInterval = new Dictionary<string, double>();
        Dictionary<string, double> sortedAverage = new Dictionary<string, double>();
        Random rand = new Random();
        int currentYear = 2018;
        int currentMonth = 1;
        int currentDay = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int loop = int.Parse(textBox1.Text);
            for (int z = 0; z < loop; z++)
            {
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                productInterval.Clear();
                productAverageQty.Clear();
                int randNum = rand.Next(0, 5);
                int quantity = rand.Next(1, 6);
                dataGridView1.Rows.Add(productsList[randNum], quantity, label1.Text);
                products.Add(new Product(productsList[randNum], quantity, currentYear, currentMonth, currentDay));
                for (int x = 0; x < productsList.Length; x++)
                {
                    productIntervalList.Clear();
                    productIntervalList = getIntervalList(productsList[x], products);
                    productInterval.Add(productsList[x], calculateInterval(productIntervalList));
                    productAverageQty.Add(productsList[x], calculateAverageQty(productIntervalList));
                }
                sortedInterval = SortedInterval(productInterval);
                sortedAverage = SortedAverageQty(productAverageQty);
                foreach (var prod in sortedInterval)
                {
                    dataGridView2.Rows.Add(prod.Key, prod.Value);
                }
                foreach (var prod in sortedAverage)
                {
                    dataGridView3.Rows.Add(prod.Key, prod.Value);
                }
                label1.Text = updateDate(ref currentYear, ref currentMonth, ref currentDay);
            }
        }
        public Dictionary<string, double> SortedAverageQty(Dictionary<string, double> ProductDictio)
        {
            double[] average = new double[ProductDictio.Count];
            string[] productName = new string[ProductDictio.Count];
            Dictionary<string, double> sortedProduct = new Dictionary<string, double>();
            int index = 0;
            foreach (var prod in ProductDictio)
            {
                average[index] = prod.Value;
                productName[index] = prod.Key;
                index++;
            }
            for (int x = 0; x < ProductDictio.Count; x++)
            {
                int minimum = x;
                for (int y = x; y < ProductDictio.Count; y++)
                {
                    if (average[minimum] < average[y])
                    {
                        minimum = y;
                    }
                }
                double temp = average[x];
                average[x] = average[minimum];
                average[minimum] = temp;
                string tempName = productName[x];
                productName[x] = productName[minimum];
                productName[minimum] = tempName;
            }
            for (int x = 0; x < ProductDictio.Count; x++)
            {
                sortedProduct.Add(productName[x], average[x]);
            }
            return sortedProduct;
        }

        public Dictionary<string, double> SortedInterval(Dictionary<string, double> ProductDictio)
        {
            double[] intervals = new double[ProductDictio.Count];
            string[] productName = new string[ProductDictio.Count];
            Dictionary<string, double> sortedProduct = new Dictionary<string, double>();
            int index = 0;
            foreach(var prod in ProductDictio)
            {
                intervals[index] = prod.Value;
                productName[index] = prod.Key;
                index++;
            }
            for(int x = 0;x < ProductDictio.Count;x++)
            {
                int minimum = x;
                for(int y = x;y < ProductDictio.Count; y++)
                {
                    if(intervals[minimum] > intervals[y])
                    {
                        minimum = y;
                    }
                }
                double temp = intervals[x];
                intervals[x] = intervals[minimum];
                intervals[minimum] = temp;
                string tempName = productName[x];
                productName[x] = productName[minimum];
                productName[minimum] = tempName;
            }
            for(int x = 0; x < ProductDictio.Count; x++)
            {
                sortedProduct.Add(productName[x], intervals[x]);
            }
            return sortedProduct;
        }

        public List<Product> getIntervalList(string Key, List<Product> ProductList)
        {
            List<Product> Product = new List<Product>();
            for(int x = 0;x < ProductList.Count; x++)
            {
                if (Key == ProductList[x].Name)
                    Product.Add(new Product(ProductList[x].Name, ProductList[x].Quantity, ProductList[x].Year, ProductList[x].Month, ProductList[x].Day));
            }
            return Product;
        }
    
        public double calculateInterval(List<Product> ProductList)
        {
            double totalDays = 0;
            double intervalDays = 0;
            int day = 0;
            int month = 0;
            int year = 0;
            if(ProductList.Count != 1)
            {
                for (int x = 1; x < ProductList.Count; x++)
                {
                    day += ProductList[x - 1].Day - ProductList[x].Day;
                    month += (ProductList[x - 1].Month - ProductList[x].Month) * 30;
                    year += (ProductList[x - 1].Year - ProductList[x].Year) * 360;
                    totalDays = year + month + day;
                    intervalDays = Math.Abs(totalDays) / (ProductList.Count - 1);
                }
                return intervalDays;
            }
            else
            {
                return 1;
            }
        }

        public double calculateAverageQty(List<Product> ProductList)
        {
            double averageQty = 0;
            double totalQty = 0;
            for (int x = 0; x < ProductList.Count; x++)
            {
                totalQty += ProductList[x].Quantity;
                averageQty = totalQty / ProductList.Count;
            }
            return averageQty;
        }

        public string updateDate(ref int Year, ref int Month, ref int Day)
        {
            Day++;
            if (Day > 30)
            {
                Month++;
                Day = 1;
            }
            if (Month > 12)
            {
                Year++;
                Month = 1;
            }
            switch(Month)
            {
                case 1:
                    return "January " + Day + ", " + Year;
                case 2:
                    return "February " + Day + ", " + Year;
                case 3:
                    return "March " + Day + ", " + Year;
                case 4:
                    return "April " + Day + ", " + Year;
                case 5:
                    return "May " + Day + ", " + Year;
                case 6:
                    return "June " + Day + ", " + Year;
                case 7:
                    return "July " + Day + ", " + Year;
                case 8:
                    return "August " + Day + ", " + Year;
                case 9:
                    return "September " + Day + ", " + Year;
                case 10:
                    return "October " + Day + ", " + Year;
                case 11:
                    return "November " + Day + ", " + Year;
                case 12:
                    return "December " + Day + ", " + Year;
            }
            return "Unknown";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       Color.Snow,
                                                                       Color.DodgerBlue,
                                                                       360F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            products.Clear();
            productIntervalList.Clear();
            productInterval.Clear();
            productAverageQty.Clear();
            sortedInterval.Clear();
            sortedAverage.Clear();
            currentYear = 2018;
            currentMonth = 1;
            currentDay = 1;
            label1.Text = "January 1, 2018";
            textBox1.Text = "1";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int loop = int.Parse(textBox1.Text);
            }
            catch(System.Exception)
            {
                textBox1.Text = "1";
            }
        }
    }

    public class Product
    {
        public string Name;
        public double Quantity;
        public int Year;
        public int Month;
        public int Day;

        public Product(string Name, double Quantity, int Year, int Month, int Day)
        {
            this.Name = Name;
            this.Quantity = Quantity;
            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
        }
    }
}
