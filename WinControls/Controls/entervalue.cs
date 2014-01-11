using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace ICTEAS.WinForms.Controls
{
    public partial class entervalue : Form
    {
        clsWritableGrid grid;
        int column = 0;
        string strstoretext;
        public entervalue(string s,clsWritableGrid grid,int col)
        {
            this.grid = grid;
            this.column = col;
            InitializeComponent();

            if (s == "date")
            {
                dateTimePicker1.Visible = true;
            }
            if (s == "text")
            {
                textBox1.Visible = true;
            }
        }
        public entervalue(string[] s, clsWritableGrid grid,int col)
        {
            this.column = col;
            this.grid = grid;
            InitializeComponent();
            comboBox1.Visible = true;
            comboBox1.DataSource = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Visible)
                {
                    string select = comboBox1.SelectedValue.ToString();
                    for (int i = 0; i < grid.Rows.Count; i++)
                    {
                        grid[i, column] = select;
                    }
                }
                else
                {
                    if (dateTimePicker1.Visible)
                    {
                        string select = dateTimePicker1.Value.ToShortDateString();
                        for (int i = 0; i < grid.Rows.Count; i++)
                        {
                            grid[i, column] = select;
                        }
                    }
                    else
                    {
                        if (textBox1.Visible)
                        {
                            int start=0;
                            if (grid.CheckNumericColumn(column,ref start))
                            {
                                string text = this.strInCommaFormat(textBox1.Text,column);
                                for (int i = 0; i < grid.Rows.Count; i++)
                                {
                                    grid[i, column] = text;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < grid.Rows.Count; i++)
                                {
                                    grid[i, column] = textBox1.Text;
                                }
                            }

                        }
                    }
                }
                this.Close();
                
            }
            catch (Exception ex)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string strInCommaFormat(string strParamTextinComma, int columnindex)
        {
            try
            {
                int realindex = grid.get_actual_colum_index(columnindex);
                strstoretext = strParamTextinComma;
                int noofcommas = 0;
                CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name, true);
                NumberFormatInfo nmfi = new NumberFormatInfo();
                nmfi = ci.NumberFormat;
                ci.ClearCachedData();
                string strTextinComma = strParamTextinComma;
                bool flag = false;
                string integerPart = "";
                string DecimalPart = "";


                int dotIndex = strTextinComma.LastIndexOf(Convert.ToChar(nmfi.NumberDecimalSeparator));
                int columnnumber = 0;
                for (columnnumber = 0; columnnumber < grid.arlNumericColumNum.Count; columnnumber++)
                {
                    if ((int)grid.arlNumericColumNum[columnnumber] == realindex)
                    {
                        break;
                    }
                }
                int DecimalPrecision = (int)grid.arlNumericColFloatSize[columnnumber];
                int CommaPrecision = 3;
                if (dotIndex == -1)
                {

                    dotIndex = strTextinComma.Length;
                    DecimalPart = strTextinComma.Substring(dotIndex);
                    integerPart = strTextinComma.Substring(0, dotIndex);
                    bool test;
                    if (grid.arlNumericColCurrency.Count > columnnumber)
                    {
                        test = (bool)grid.arlNumericColCurrency[columnnumber];

                    }
                    else
                    {
                        test = false;
                    }
                    if (test && (clsTxtBox.TypeEnum)grid.arlNumericColumnType[columnnumber] == clsTxtBox.TypeEnum.Float)
                    {
                        int dpres = 4;
                        string tmp = String.Empty;

                        for (int k = 0; k < dpres; k++)
                            tmp = tmp + "0";

                        DecimalPart = tmp;
                        flag = true;


                    }
                }
                else
                {


                    DecimalPart = strTextinComma.Substring(dotIndex + 1);

                    if (DecimalPrecision < DecimalPart.Length)
                    {
                        DecimalPart = nmfi.NumberDecimalSeparator + DecimalPart;
                        decimal val = Convert.ToDecimal(DecimalPart);
                        int counter = 0;
                        int totalcount = DecimalPart.Length - DecimalPrecision;



                        val = Math.Round(val, DecimalPrecision);
                        DecimalPart = val.ToString();
                        string[] pos = DecimalPart.Split(Convert.ToChar(nmfi.NumberDecimalSeparator));
                        DecimalPart = pos[1];

                    }
                    integerPart = strTextinComma.Substring(0, dotIndex);
                    flag = true;
                }

                integerPart = integerPart.Replace(nmfi.NumberGroupSeparator, "");


                string prefix = "";
                if (integerPart.IndexOf(nmfi.NegativeSign) == 0)
                {
                    integerPart = integerPart.TrimStart(Convert.ToChar(nmfi.NegativeSign));
                    prefix = nmfi.NegativeSign;
                }
                int commasalreadyinintpart = this.find_no_occurences(strParamTextinComma, nmfi.NumberGroupSeparator);
                if (integerPart.Length >= CommaPrecision)
                {
                    int temp = integerPart.Length;
                    for (int k = CommaPrecision; k < temp; k += CommaPrecision)
                    {

                        integerPart = integerPart.Insert(temp - k, nmfi.NumberGroupSeparator);
                        noofcommas++;
                    }
                    noofcommas = noofcommas - commasalreadyinintpart;

                }

                if (flag)
                    strTextinComma = prefix + integerPart + nmfi.NumberDecimalSeparator + DecimalPart;
                else
                    strTextinComma = prefix + integerPart;
             
              
                return strTextinComma;
            }
            catch (Exception ex)
            {
                return strstoretext;
            }
        }

        private int find_no_occurences(string input, string seperator)
        {
            try
            {
                char c = Convert.ToChar(seperator);
                char[] total = input.ToCharArray();
                int count = 0;
                foreach (char m in total)
                {
                    if (c == m)
                    {
                        count++;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
    }
}