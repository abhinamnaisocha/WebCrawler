using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WebCrawlerForm
{
    public partial class Form1 : Form
    {
        string contents = "";
        List<string> linklist = new List<string>();
        int start = 0;

        public Form1()
        {
            InitializeComponent();
        }


        private void fetchBTN_Click(object sender, EventArgs e)
        {
            WebRequest httpReq;
            string contents = "";
            List<string> linklist = new List<string>();
            int start = 0;


            if (urlTB.Text != string.Empty || urlTB.Text != "" && checkBox1.Checked == false)
            {
                try
                {

                    httpReq = HttpWebRequest.Create(urlTB.Text.ToString());
                    WebResponse response = httpReq.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        contents = reader.ReadToEnd();

                    }

                    do
                    {
                        listBox1.Items.Add(FindLink(contents, ref start));
                    } while (start < contents.Length);

                }
                catch (Exception ex)
                {

                }
            }

            else if (urlTB.Text != string.Empty || urlTB.Text != "" && checkBox1.Checked == true)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlTB.Text);
                WebProxy myproxy = new WebProxy(tbSvAdd.Text + ":" + tbPort, true);
                myproxy.BypassProxyOnLocal = false;
                request.Proxy = myproxy;
                request.Method = "GET";
                request.UserAgent = "Crawler";
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    contents = reader.ReadToEnd();
                }

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (tbSvAdd.Enabled == false && tbPort.Enabled == false)
            {
                tbSvAdd.Enabled = true;
                tbPort.Enabled = true;
            }
            else
            {
                tbSvAdd.Enabled = false;
                tbPort.Enabled = false;
            }
        }


        static string FindLink(string htmlstr, ref int startloc)
        {
            int i;
            int start, end;
            string uri = null;
            i = htmlstr.IndexOf("href=\"http", startloc,
            StringComparison.OrdinalIgnoreCase);
            if (i != -1)
            {
                start = htmlstr.IndexOf('"', i) + 1;
                end = htmlstr.IndexOf('"', start);
                uri = htmlstr.Substring(start, end - start);
                startloc = end;
            }
            return uri;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            WebRequest httpReq;
            urlTB.Text = listBox1.SelectedItem.ToString();
            try
            {

                httpReq = HttpWebRequest.Create(urlTB.Text.ToString());
                WebResponse response = httpReq.GetResponse();
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    contents = reader.ReadToEnd();

                }

                do
                {
                  listBox1.Items.Add(FindLink(contents, ref start));
                } while (start < contents.Length);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
