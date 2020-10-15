using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Personal_Budget_planner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tbApply_Click(object sender, EventArgs e)
        {
            string error;
            List<string> inputs1 = new List<string>();//a list (part of generic collections) used here
            bool answer =IsAllchecked(inputs1,out error);//passes the list to IsAllchecked(). out is assign a value to error
            lbErrors.Text=error;

            //loop only if input is not accurate
            while(answer== false)
            {
                //clear all string in the array
                inputs1.Clear();
                MessageBox.Show(error);
                tbMonthlyIn.Focus();
                answer = IsAllchecked(inputs1, out error);
                lbErrors.Text = error;
            }
            
            //from here on, all string in textbooks will be converted to either int or double
            //this is because all strings have been checked 

            //Like so!!!
            double MonthlyIncome = ConvertToDouble(tbMonthlyIn.Text);

            bool isrental;
            double thetotal;
            //this checks the status of the radio bottons. 
            if (rbCarBuying.Checked)
            {
                isrental = false;//obviously right??
                thetotal = TotalCalculations(isrental);//this is necessary as it will affect the total
                double Housing = calculateHousing(isrental);
                double aThird = MonthlyIncome / 3;//remember the monthly income was converted to double at the start after the while loop
                if (Housing > aThird)//this occures only if the Housing amount is a greater than a third of the overall monthly income
                {
                    MessageBox.Show("The monthly home load repayment is too high", "Home loan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                isrental = true;
                thetotal = TotalCalculations(isrental);
            }
            
            
            MessageBox.Show("Your Net monthly income before deductions is :"+thetotal);
        }

        private void Listed(List<string> inputs)
        {
            //it adds to the string List declared in Main()
            inputs.Add(tbMonthlyIn.Text);
            inputs.Add(tbMonthlyTax.Text);
            inputs.Add(tbGroceries.Text);
            inputs.Add(tbWater.Text);
            inputs.Add(tbTravelCosts.Text);
            inputs.Add(tbCell.Text);
            inputs.Add(tbOther.Text);
            if (rbRenting.Checked)
            {
                inputs.Add(tbHouseRent.Text);

            }
            else
            {
                inputs.Add(tbHousePrice.Text);
                inputs.Add(tbHouseDepo.Text);
                inputs.Add(tbHouseIntRate.Text);
            }
            if (rbCarBuying.Checked)
            {
                inputs.Add(tbCarPrice.Text);
                inputs.Add(tbCarDepo.Text);
                inputs.Add(tbCarInt.Text);
                inputs.Add(tbCarPremium.Text);
            }
        }

        private double TotalCalculations(bool a)
        {
            double thetotal;
            double theIncome = ConvertToDouble(tbMonthlyIn.Text);

            thetotal = theIncome - everythingElse(a);// the bool recieved is passed to EverythingElse()

            return thetotal;
        }

        private double everythingElse(bool a)
        {
            double elseTotal;
            double taxs = ConvertToDouble(tbMonthlyTax.Text);
            double carTotal= calculateCar();
            double HousingTotal = calculateHousing(a);
            double expensTotal = calculateExpenditure();
            elseTotal = carTotal + HousingTotal + expensTotal+taxs;
            return elseTotal;
        }

        private double calculateCar()
        {
            if (rbCarBuying.Checked)
            {
                double deposit = ConvertToDouble(tbCarDepo.Text);
                double InitialPrice = ConvertToDouble(tbCarInt.Text);
                double Price = InitialPrice - deposit;
                int n = 5*12;
                double interest = ConvertToDouble(tbCarInt.Text);
                double intPerMon = interest / 12;
                double pre = ConvertToDouble(tbCarPremium.Text);
                double monthly = ((Price * intPerMon) / Math.Pow(1 - (1 - intPerMon), -n)+pre);
                return monthly;
            }
            else
            {
                return 00.00;
            }
        }

        private double calculateHousing(bool isrental)
        {
            if (isrental==false)
            {
                double deposit =ConvertToDouble(tbHouseDepo.Text);
                double InitialPrice = ConvertToDouble(tbHousePrice.Text);
                double Price = InitialPrice-deposit;
                int n= ConvertToInt(tbHouseMonths.Text);
                double interest = ConvertToDouble(tbHouseIntRate.Text);
                double intPerMon = interest / 12;

                double monthly = (Price * intPerMon) / Math.Pow(1 - (1 - intPerMon), -n);

                
                return monthly;
            }
            else
            {
                double rent = ConvertToDouble(tbHouseRent.Text);
                return rent;
            }
        }
        
        private double calculateExpenditure()
        {
            double total;
            double gros = ConvertToDouble(tbGroceries.Text);
            double water = ConvertToDouble(tbWater.Text);
            double trans = ConvertToDouble(tbTravelCosts.Text);
            double cell = ConvertToDouble(tbCell.Text);
            double other = ConvertToDouble(tbOther.Text);
            total = (gros + water + trans + cell + other);
            return total;

        }

        private void ClearAll()
        {
            tbMonthlyIn.Clear();
            tbMonthlyTax.Clear();
            tbGroceries.Clear();
            tbWater.Clear();
            tbTravelCosts.Clear();
            tbCell.Clear();
            tbOther.Clear();
            tbCarModel.Clear();
            tbCarPrice.Clear();
            tbCarDepo.Clear();
            tbCarInt.Clear();
            tbCarPremium.Clear();
            tbHouseRent.Clear();
            tbHousePrice.Clear();
            tbHouseDepo.Clear();
            tbHouseIntRate.Clear();
            tbHouseMonths.Clear();
            tbMonthlyIn.Focus();
        }

        private bool IsAllchecked(List<string> a,out string y)
        {
            y = null;
            bool isallchecked = false;
            bool incorrect =false;
            //created a list of inputs 
            
            Listed(a);

            foreach (var i in a)
            {
                isallchecked=Ischecked(i, out y);
                if (isallchecked == false)
                {
                    incorrect = true;
                }
            }
            if (incorrect == true)
            {
                isallchecked = false;
            }
            return isallchecked;
        }

        private bool Ischecked(string s, out string e)
        {
            bool check;
            bool a = IsNumeric(s);
            if(a== true)
            {
                check = true;
                e = "No error found";
            }
            else
            {
                check = false;
                e = "Invalid entry. Please try again";
            }
            return check;
        }

        private bool IsNumeric(string s)
        {
            double test;
            bool isNum = false;
            if(Double.TryParse(s,out test))
            {
                isNum = true;
            }
            return isNum;
        }
        private double ConvertToDouble(string s)
        {
            double answer = Double.Parse(s);
            return answer;
        }
        private int ConvertToInt(string s)
        {
            int answer = int.Parse(s);
            return answer;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void rbCarBuying_Click(object sender, EventArgs e)
        {
            tbCarModel.Enabled=true;
            tbCarPrice.Enabled = true;
            tbCarDepo.Enabled = true;
            tbCarInt.Enabled = true;
            tbCarPremium.Enabled = true;
            tbCarModel.Focus();
        }

        private void rbRenting_CheckedChanged(object sender, EventArgs e)
        {
            tbHousePrice.Enabled = false;
            tbHouseDepo.Enabled = false;
            tbHouseIntRate.Enabled = false;
            tbHouseMonths.Enabled = false;
            tbHouseRent.Enabled = true;
            tbHouseRent.Focus();
        }

        private void rbPropBuying_CheckedChanged(object sender, EventArgs e)
        {
            tbHouseRent.Enabled = false;
            tbHousePrice.Enabled = true;
            tbHouseDepo.Enabled = true;
            tbHouseIntRate.Enabled = true;
            tbHouseMonths.Enabled = true;
            tbHousePrice.Focus();
        }
    }
}
