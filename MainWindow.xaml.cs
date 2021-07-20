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
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace optionsanalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public struct OptionsData
    {
        public string col1 { set; get; }
        public string col2 { set; get; }
        public string col3 { set; get; }
        public string col4 { set; get; }
        public string col5 { set; get; }
        public string col6 { set; get; }
        public string col7 { set; get; }
        public string col8 { set; get; }
        public string col9 { set; get; }
        public string col10 { set; get; }
        public string col11 { set; get; }

    }


    public partial class MainWindow : Window
    {
        private static string symquote;
        static List<string> expdate = new List<string>();
        static List<string> allcalls = new List<string>();
        static List<string> allputs = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            optiontype.Items.Add("calls");
            optiontype.Items.Add("puts");

            optiontype.SelectedIndex = 0;
            optiondate.SelectedIndex = 0;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (symb.Text.Length > 0)
            {
                datagrid1.Items.Clear();

                //DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(1603411200);
                //DateTimeOffset dateTimeOffset2 = DateTimeOffset.FromUnixTimeSeconds(1601571384);

                //MessageBox.Show(dateTimeOffset.ToString("MM-dd-yyyy"));
                //Console.WriteLine(dateTimeOffset2.ToString("MM-dd-yyyy hh:mm:ss tt"));
                List<string> coname = new List<string>();
                List<string> ltdate = new List<string>();
                List<string> strike = new List<string>();
                List<string> ltpr = new List<string>();
                List<string> bid = new List<string>();
                List<string> ask = new List<string>();
                List<string> chg = new List<string>();
                List<string> pchg = new List<string>();
                List<string> vol = new List<string>();
                List<string> oi = new List<string>();
                List<string> iv = new List<string>();

                Task<int> datatask = DownloadYfinanceoptions(symb.Text, optiondate.SelectedIndex, optiontype.SelectedIndex);
                int x = await datatask;

                col1.Binding = new Binding("col1");
                col2.Binding = new Binding("col2");
                col3.Binding = new Binding("col3");
                col4.Binding = new Binding("col4");
                col5.Binding = new Binding("col5");
                col6.Binding = new Binding("col6");
                col7.Binding = new Binding("col7");
                col8.Binding = new Binding("col8");
                col9.Binding = new Binding("col9");
                col10.Binding = new Binding("col10");
                col11.Binding = new Binding("col11");

                //datagrid1.Items.Add(new OptionsData { col1 = "a", col2 = "b" });

                if (optiontype.SelectedIndex == 0)
                {
                    if (allcalls.Count > 0)
                    {
                        int csize = 0;
                        int vsize = 0;

                        for (int y = 0; y < allcalls.Count; y++)
                        {
                            /*
                            if (allcalls[y].ToLower().Contains("contractsymbol"))
                            {
                                string[] cname = allcalls[y].Split(':');

                                //contractname.Items.Add(cname[1].Trim('"'));
                                datagrid1.Items.Add(new OptionsData { col1 = cname[1].Trim().Trim('"') });
                            }*/

                            //MessageBox.Show(allcalls[y].ToLower());

                            if (allcalls[y].ToLower().Contains("contractsymbol"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                coname.Add(cname[1].Trim().Trim('"'));

                                if (csize != 0)
                                {
                                    if (csize != vsize)
                                    {
                                        vol.Add("-");
                                        vsize++;
                                    }

                                }

                                csize++;
                            }

                            if (allcalls[y].ToLower().Contains("lasttradedate"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cname[1].Trim().Trim('"')));
                                //Console.WriteLine(dateTimeOffset2.ToString("MM-dd-yyyy hh:mm:ss tt"));

                                ltdate.Add(dateTimeOffset.ToString("MM-dd-yyyy hh:mm:ss tt"));
                            }

                            if (allcalls[y].ToLower().Contains("strike"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                strike.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allcalls[y].ToLower().Contains("lastprice"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                ltpr.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allcalls[y].ToLower().Contains("bid"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                bid.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allcalls[y].ToLower().Contains("ask"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                ask.Add(cname[1].Trim().Trim('"'));
                            }

                            if ((allcalls[y].ToLower().Contains("change")) & (!(allcalls[y].ToLower().Contains("percentchange"))))
                            {
                                string[] cname = allcalls[y].Split(':');
                                double temp = Convert.ToDouble(cname[1].Trim().Trim('"'));
                                temp = Math.Round(temp, 2);
                                chg.Add(temp.ToString());
                            }

                            if (allcalls[y].ToLower().Contains("percentchange"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                double temp = Convert.ToDouble(cname[1].Trim().Trim('"'));
                                temp = Math.Round(temp, 2);
                                pchg.Add(temp.ToString() + "%");
                            }

                            if (allcalls[y].ToLower().Contains("volume"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                vol.Add(cname[1].Trim().Trim('"'));
                                vsize++;
                            }

                            if (allcalls[y].ToLower().Contains("openinterest"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                oi.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allcalls[y].ToLower().Contains("impliedvolatility"))
                            {
                                string[] cname = allcalls[y].Split(':');
                                double temp = Convert.ToDouble(cname[1].Trim().Trim('"'));
                                temp = Math.Round(temp * 100, 2);
                                iv.Add(temp.ToString() + "%"); ;

                            }


                        }

                    }
                }


                if (optiontype.SelectedIndex == 1)
                {
                    if (allputs.Count > 0)
                    {
                        int csize = 0;
                        int vsize = 0;

                        for (int y = 0; y < allputs.Count; y++)
                        {
                            /*
                            if (allcalls[y].ToLower().Contains("contractsymbol"))
                            {
                                string[] cname = allcalls[y].Split(':');

                                //contractname.Items.Add(cname[1].Trim('"'));
                                datagrid1.Items.Add(new OptionsData { col1 = cname[1].Trim().Trim('"') });
                            }*/

                            //MessageBox.Show(allcalls[y].ToLower());

                            if (allputs[y].ToLower().Contains("contractsymbol"))
                            {
                                string[] cname = allputs[y].Split(':');
                                coname.Add(cname[1].Trim().Trim('"'));

                                if (csize != 0)
                                {
                                    if (csize != vsize)
                                    {
                                        vol.Add("-");
                                        vsize++;
                                    }

                                }

                                csize++;
                            }

                            if (allputs[y].ToLower().Contains("lasttradedate"))
                            {
                                string[] cname = allputs[y].Split(':');
                                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cname[1].Trim().Trim('"')));
                                //Console.WriteLine(dateTimeOffset2.ToString("MM-dd-yyyy hh:mm:ss tt"));

                                ltdate.Add(dateTimeOffset.ToString("MM-dd-yyyy hh:mm:ss tt"));
                            }

                            if (allputs[y].ToLower().Contains("strike"))
                            {
                                string[] cname = allputs[y].Split(':');
                                strike.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allputs[y].ToLower().Contains("lastprice"))
                            {
                                string[] cname = allputs[y].Split(':');
                                ltpr.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allputs[y].ToLower().Contains("bid"))
                            {
                                string[] cname = allputs[y].Split(':');
                                bid.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allputs[y].ToLower().Contains("ask"))
                            {
                                string[] cname = allputs[y].Split(':');
                                ask.Add(cname[1].Trim().Trim('"'));
                            }

                            if ((allputs[y].ToLower().Contains("change")) & (!(allputs[y].ToLower().Contains("percentchange"))))
                            {
                                string[] cname = allputs[y].Split(':');
                                double temp = Convert.ToDouble(cname[1].Trim().Trim('"'));
                                temp = Math.Round(temp, 2);
                                chg.Add(temp.ToString());
                            }

                            if (allputs[y].ToLower().Contains("percentchange"))
                            {
                                string[] cname = allputs[y].Split(':');
                                double temp = Convert.ToDouble(cname[1].Trim().Trim('"'));
                                temp = Math.Round(temp, 2);
                                pchg.Add(temp.ToString() + "%");
                            }

                            if (allputs[y].ToLower().Contains("volume"))
                            {
                                string[] cname = allputs[y].Split(':');
                                vol.Add(cname[1].Trim().Trim('"'));
                                vsize++;
                            }

                            if (allputs[y].ToLower().Contains("openinterest"))
                            {
                                string[] cname = allputs[y].Split(':');
                                oi.Add(cname[1].Trim().Trim('"'));
                            }

                            if (allputs[y].ToLower().Contains("impliedvolatility"))
                            {
                                string[] cname = allputs[y].Split(':');
                                double temp = Convert.ToDouble(cname[1].Trim().Trim('"'));
                                temp = Math.Round(temp * 100, 2);
                                iv.Add(temp.ToString() + "%"); ;

                            }


                        }

                    }
                }



                if (coname.Count > 0)
                {

                    for (int y = 0; y < coname.Count; y++)
                    {

                        datagrid1.Items.Add(new OptionsData { col1 = coname[y].Trim().Trim('"'), col2 = ltdate[y].Trim().Trim('"'), col3 = strike[y].Trim().Trim('"'), col4 = ltpr[y].Trim().Trim('"'), col5 = bid[y].Trim().Trim('"'), col6 = ask[y].Trim().Trim('"'), col7 = chg[y].Trim().Trim('"'), col8 = pchg[y].Trim().Trim('"'), col9 = vol[y].Trim().Trim('"'), col10 = oi[y].Trim().Trim('"'), col11 = iv[y].Trim().Trim('"') });


                    }

                }

                /*

                uxDGV.Rows.Clear();
                int rowCount = dataGridView1.Rows.Count;
                for (int n = 0; n < rowCount; n++)
                {
                    if (dataGridView1.Rows[0].IsNewRow == false)
                        dataGridView1.Rows.RemoveAt(0);
                }
                */

                //fill listbox
                /*
                for (int y = 0; y < allcalls.Count; y++)
                {
                    if (allcalls[y].ToLower().Contains("contractsymbol"))
                    {
                        string[] cname = allcalls[y].Split(':');

                        contractname.Items.Add(cname[1].Trim('"'));
                    }
                }
                */
            }

            else
            {
                MessageBox.Show("Enter symbol");
            }



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txt1.Text = (Convert.ToDouble(stcost.Text) - Convert.ToDouble(premcol.Text)).ToString();
            txt2.Text = (((Convert.ToDouble(premcol.Text) * 100) * Convert.ToDouble(cont.Text)) - ((Convert.ToDouble(stcost.Text) - Convert.ToDouble(stprice.Text)) * 100 * Convert.ToDouble(cont.Text))).ToString();
            txt3.Text = (((Convert.ToDouble(premcol.Text) * 100) * Convert.ToDouble(cont.Text)) - ((Convert.ToDouble(stcost.Text) - Convert.ToDouble(strike.Text)) * 100 * Convert.ToDouble(cont.Text))).ToString();

        }



        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (symb.Text.Length > 0)
            {

                //DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(1603411200);
                //DateTimeOffset dateTimeOffset2 = DateTimeOffset.FromUnixTimeSeconds(1601571384);

                //MessageBox.Show(dateTimeOffset.ToString("MM-dd-yyyy"));
                //Console.WriteLine(dateTimeOffset2.ToString("MM-dd-yyyy hh:mm:ss tt"));
              

                Task<int> datatask = DownloadYfinanceoptions(symb.Text, optiondate.SelectedIndex, optiontype.SelectedIndex);
                int x = await datatask;

                stprice.Text = symquote;

                if (expdate.Count > 0)
                {
                    for (int y = 0; y < expdate.Count; y++)
                    {
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expdate[y]));
                        optiondate.Items.Add(dateTimeOffset.ToString("MM-dd-yyyy"));
                    }
                }

             
                //datagrid1.Items.Add(new OptionsData { col1 = "a", col2 = "b" });             

                /*

                uxDGV.Rows.Clear();
                int rowCount = dataGridView1.Rows.Count;
                for (int n = 0; n < rowCount; n++)
                {
                    if (dataGridView1.Rows[0].IsNewRow == false)
                        dataGridView1.Rows.RemoveAt(0);
                }
                */

                //fill listbox
                /*
                for (int y = 0; y < allcalls.Count; y++)
                {
                    if (allcalls[y].ToLower().Contains("contractsymbol"))
                    {
                        string[] cname = allcalls[y].Split(':');

                        contractname.Items.Add(cname[1].Trim('"'));
                    }
                }
                */
            }

            else
            {
                MessageBox.Show("Enter symbol");
            }


        }


        //static async Task<int> DownloadYfinanceoptions(string sym)
        static async Task<int> DownloadYfinanceoptions(string sym, int dateind, int optind)
        {
            allcalls.Clear();
            allputs.Clear();


            using (var httpClient = new HttpClient())
            {
                //https://query2.finance.yahoo.com/v7/finance/options/bac?date=1674172800

                string date = "";

                if (expdate.Count == 0)
                {
                    date = "";
                }

                else
                {
                    date = expdate[dateind];
                }

                //MessageBox.Show(date);


                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://query2.finance.yahoo.com/v7/finance/options/"+ sym +"?date="+date))
                {
                    

                    //request.Headers.TryAddWithoutValidation("Authorization", "Bearer 7166e0ce6769bab5ed3aae64663fe15e-bca23d48c2ceda4f9907d4e404059b69");

                    var response = await httpClient.SendAsync(request);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic stuff4 = JObject.Parse(responseBody);
                    //var test = stuff4.optionchain.result.underlyingSymbol;

                    //MessageBox.Show(stuff4.optionChain.result.ToString());
                    string emptytest = stuff4.optionChain.result.ToString();

                    if (emptytest != "[]")
                    {
                        //datagrid1.Rows.Clear();

                        string c = stuff4.optionChain.result[0].expirationDates[0].ToString();
                        //string d = stuff4.optionChain.result[0].options[0].calls[0].contractSymbol.ToString();
                        //string f = stuff4.optionChain.result[0].options[0].calls[0].ToString();
                        //List<string> expdate = new List<string>();
                        //List<string> allcalls = new List<string>();
                        //List<string> allputs = new List<string>();
                        List<string> strikes = new List<string>();
                        string miniopt = stuff4.optionChain.result[0].hasMiniOptions.ToString();
                        //JToken test = stuff4.optionChain.result[0].options[0].calls[0];

                        string exdate = stuff4.optionChain.result[0].options[0].expirationDate.ToString();
                        string miniopt2 = stuff4.optionChain.result[0].options[0].hasMiniOptions.ToString();
                        JToken test2 = stuff4.optionChain.result[0].options[0].calls;
                        JToken test4 = stuff4.optionChain.result[0].options[0].puts;
                        JToken test5 = stuff4.optionChain.result[0].strikes;
                        JToken test6 = stuff4.optionChain.result[0].quote;





                        foreach (JToken item in test6)
                        {
                            if (item.ToString().ToLower().Contains("regularmarketprice"))
                            {
                                //MessageBox.Show(item.ToString());
                                string[] price = item.ToString().Split(':');
                                symquote = price[1].Trim();

                            }
                        }


                        for (int x = 0; x < test5.Count(); x++)
                        {
                            //MessageBox.Show(test5[x].ToString());
                            strikes.Add(test5[x].ToString());
                        }


                  
                        for (int x = 0; x < stuff4.optionChain.result[0].expirationDates.Count; x++)
                        {
                            c = stuff4.optionChain.result[0].expirationDates[x].ToString();
                            expdate.Add(c);
                            //MessageBox.Show(c);
                        }


                        //calls
                        for (int x = 0; x < test2.Count(); x++)
                        {

                            foreach (JToken item in test2[x])
                            {

                                //MessageBox.Show(item.ToString());
                                allcalls.Add(item.ToString());
                            }
                        }

                        //puts
                        for (int x = 0; x < test4.Count(); x++)
                        {
                            foreach (JToken item in test4[x])
                            {
                                //MessageBox.Show(item.ToString());
                                allputs.Add(item.ToString());
                            }
                        }


                    }

                    else
                    {
                        MessageBox.Show("Invalid symbol");
                    }                 

                }

            }
            return 1;
        }

      
    }
}
