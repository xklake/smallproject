using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; 
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO; 
using Excel = Microsoft.Office.Interop.Excel;
using System.Net;
using System.Threading; 


namespace Product_Importer
{
    public partial class Form1 : Form
    {
        ArrayList inputRows = new ArrayList();

        ArrayList skuList = new ArrayList();

        ArrayList allImages = new ArrayList();

        ArrayList outPut = new ArrayList();

        string url = "http://7xiosd.com1.z0.glb.clouddn.com/";

        string oneline = "================================================================================";
        string error = "error"; 

        //"ID","分类","名称","Sku","库存","重量","市场价","商城价","简述","内容","缩略图","图片","关键字","描述","类型","品牌","产地","发货地",缩略图集,图片集

        string rowdef = ",\"{cat}\",\"{name}\",\"{enname}\",\"{sku}\",\"{stock}\",\"{weight}\",\"{fullprice}\",\"{discountprice}\",\"{brief}\",\"{intro}\",\"{content}\",\"{smallimage}\",\"{largeimage}\",\"{keywords}\",\"{seodesc}\",\"17\",\"{brand}\",\"{madein}\",\"{sendfrom}\",\"{smallimageset}\",\"{largeimageset}\",\"{extendcat}\"";
        string rowtemp = ",\"{cat}\",\"{name}\",\"{enname}\",\"{sku}\",\"{stock}\",\"{weight}\",\"{fullprice}\",\"{discountprice}\",\"{brief}\",\"{intro}\",\"{content}\",\"{smallimage}\",\"{largeimage}\",\"{keywords}\",\"{seodesc}\",\"17\",\"{brand}\",\"{madein}\",\"{sendfrom}\",\"{brand}\",\"{smallimageset}\",\"{largeimageset}\", \"{extendcat}\"";

        //string head = "ID,分类,名称,Sku,库存,重量,市场价,商城价,简述,内容,缩略图,图片,关键字,描述,类型,品牌,产地,发货地,缩略图集,图片集"; 
        string head = "ID,分类,名称,英文名称,Sku,库存,重量,市场价,商城价,简述,介绍,内容,缩略图,图片,关键字,描述,类型,品牌,产地,发货地,缩略图集,图片集,扩展分类"; 

        public Form1()
        {
            InitializeComponent();

            //FileStream log = new FileStream("log.txt", FileMode.OpenOrCreate);
           // Console.SetOut(new StreamWriter(log)); 
        }

        private void sourceFile_MouseDown(object sender, MouseEventArgs e)
        {
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                sourceFile.Text = openFileDialog1.FileName; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Application.Exit();

            inputRows.Clear();
            skuList.Clear();
            allImages.Clear();
            outPut.Clear();


            readSourceFile();

            readImgfromDirectory();

            processTaobao();

            writeCSV();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inputRows.Clear();
            skuList.Clear();
            allImages.Clear();
            outPut.Clear(); 


            readSourceFile();

            readImgfromDirectory();

            process();

            writeCSV();

        }

        private void DownloadImage(string url, string location)
        {
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            try
            {
                System.Uri httpUrl = new System.Uri(url.Trim());
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                req.UserAgent = "XXXXX";
                req.Accept = "XXXXXX";
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());
                img = new Bitmap(res.GetResponseStream());//获取图片流                 
                img.Save(location);                

            }
            catch (Exception ex)
            {
                string aa = ex.Message;

                if (res != null)
                {
                    res.Close(); 
                }
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
        }

        private string DownloadWebPage(string url)
        {
            string testurl = "http://detail.tmall.hk/hk/item.htm?spm=a220m.1000858.1000725.1.2wQFHl&id=40957877409&skuId=67793583877&areaId=430100&cat_id=2&rn=1e5c9ffe3e68d072bee93050d477eb37&standard=1&user_id=2113658227&is_b=1";
            
            WebRequest request = null; 
            WebResponse response = null;
            StreamReader reader = null;
            string webContent = error;  


            try
            {
                Console.WriteLine(oneline);
                Console.WriteLine("Download this link   " + testurl);

                request = WebRequest.Create(testurl);
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                webContent = reader.ReadToEnd();
                Console.WriteLine("Download finished");
                Console.WriteLine(oneline);           

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(oneline);
                Console.WriteLine("Download Failed on this link   " + testurl);
                Console.WriteLine("Error Message: " + ex.Message);

                Console.WriteLine(oneline); 
                

                reader.Close();
                reader.Dispose();
                response.Close();
            }

            return webContent; 
        }


        private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser web = (WebBrowser)sender;
           

            Console.WriteLine(web.StatusText.ToString()); 

            //HtmlElement description = web.Document.GetElementById("description");

            //web.Document.Encoding = "gbk"; 

            System.IO.StreamReader getReader = new System.IO.StreamReader(web.DocumentStream, System.Text.Encoding.GetEncoding("gb2312"));
            string gethtml = getReader.ReadToEnd();


            if (web.DocumentText != null && gethtml.IndexOf("描述加载中") == -1)
            {
                int i = 0; 

            }

        }


        private string DownloadWebPageNew(string url)
        {
            string testurl = "http://detail.tmall.hk/hk/item.htm?spm=a220m.1000858.1000725.1.2wQFHl&id=40957877409&skuId=67793583877&areaId=430100&cat_id=2&rn=1e5c9ffe3e68d072bee93050d477eb37&standard=1&user_id=2113658227&is_b=1";
            testurl = "http://detail.tmall.hk/hk/item.htm?spm=a1z10.3-b.w4011-8327940127.133.X2zzKQ&id=40530672006&rn=318d384556e79f81624d946e3f8e0085&abbucket=15&skuId=76657856556";
            testurl = "http://detail.tmall.com/item.htm?spm=a220m.1000858.1000725.5.c6qI0R&id=36577468222&areaId=430100&cat_id=2&rn=33bd6e14b0267132f08bc7c9c19061aa&user_id=1881479267&is_b=1"; 


            WebBrowser wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true; 
            //wb.Document.DefaultEncoding = Encoding.GetEncoding("gb2312"); 

            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted);

            wb.Navigate(testurl);

           // wb.Document.Body; 
            
            WebRequest request = null;
            WebResponse response = null;
            StreamReader reader = null;
            string webContent = error;


            try
            {
                Console.WriteLine(oneline);
                Console.WriteLine("Download this link   " + testurl);

                request = WebRequest.Create(testurl);
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));

                webContent = reader.ReadToEnd();
                Console.WriteLine("Download finished");
                Console.WriteLine(oneline);

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(oneline);
                Console.WriteLine("Download Failed on this link   " + testurl);
                Console.WriteLine("Error Message: " + ex.Message);

                Console.WriteLine(oneline);


                reader.Close();
                reader.Dispose();
                response.Close();
            }

            return webContent;
        }


        private void processTaobao()
        {
            allImages.Sort();

            foreach (Object row in inputRows)
            {
                rowtemp = ",\"{cat}\",\"{name}\",\"{enname}\",\"{sku}\",{content}";

                ArrayList a = (ArrayList)row;

                string sku = a[0].ToString();

                string cat = a[a.Count - 1].ToString();

                string[] categories = cat.Split('|');


                {
                    rowtemp = rowtemp.Replace("{sku}", a[0].ToString().Trim());

                    rowtemp = rowtemp.Replace("{enname}", a[1].ToString().Trim());

                    rowtemp = rowtemp.Replace("{name}", a[2].ToString().Trim());

                    rowtemp = rowtemp.Replace("{weight}", a[4].ToString().Trim());

                    rowtemp = rowtemp.Replace("{stock}", a[5].ToString().Trim());

                    rowtemp = rowtemp.Replace("{brief}", a[9].ToString().Trim());

                    rowtemp = rowtemp.Replace("{brand}", a[8].ToString().Trim());

                    rowtemp = rowtemp.Replace("{intro}", a[3].ToString().Trim());

                    rowtemp = rowtemp.Replace("{madein}", a[10].ToString().Trim());

                    rowtemp = rowtemp.Replace("{sendfrom}", a[11].ToString().Trim());


                    rowtemp = rowtemp.Replace("{fullprice}", a[6].ToString().Trim());

                    rowtemp = rowtemp.Replace("{extendcat}", a[12].ToString().Trim());

                    rowtemp = rowtemp.Replace("{discountprice}", a[7].ToString().Trim());


                    //ok, let's find out all the images 
                    string detailimage = "";
                    string smallimage = "";
                    string largeimage = "";
                    string smallimageset = "";
                    string largeimageset = "";


                    foreach (string img in allImages)
                    {
                        //large images
                        if (img.ToUpper().IndexOf(sku.ToUpper() + "-L-") != -1)
                        {
                            largeimageset += url + img + "|";

                            if (largeimage.Length == 0)
                            {
                                largeimage = url + img;
                            }
                        }

                        else if (img.ToUpper().IndexOf(sku + "-S-") != -1)
                        {
                            smallimageset += url + img + "|";

                            if (smallimage.Length == 0)
                            {
                                smallimage = url + img;
                            }
                        }
                        else if (img.ToUpper().IndexOf(sku + "-P-") != -1)
                        {
                            continue;
                        }
                        else if (img.ToUpper().IndexOf(sku) != -1)
                        {
                            detailimage += "<img src=\"" + url + img + "\"></img>";
                        }
                    }


                    rowtemp = rowtemp.Replace("{smallimage}", smallimage);
                    rowtemp = rowtemp.Replace("{largeimage}", largeimage);
                    rowtemp = rowtemp.Replace("{smallimageset}", smallimageset);
                    rowtemp = rowtemp.Replace("{largeimageset}", largeimageset);
                    rowtemp = rowtemp.Replace("{content}", detailimage);
                }


                foreach (string str in categories)
                {
                    outPut.Add(rowtemp.Replace("{cat}", str));
                }
            }

        }




        private void process()
        {
            allImages.Sort(); 

            foreach(Object row in inputRows)
            {
                rowtemp = rowdef; 

                ArrayList a = (ArrayList)row;

                string sku = a[0].ToString();

                string cat = a[a.Count - 1].ToString();

                string [] categories = cat.Split('|');


                {
                    rowtemp = rowtemp.Replace("{sku}", a[0].ToString().Trim());

                    rowtemp = rowtemp.Replace("{enname}", a[1].ToString().Trim());

                    rowtemp = rowtemp.Replace("{name}", a[2].ToString().Trim());

                    rowtemp = rowtemp.Replace("{weight}", a[4].ToString().Trim());

                    rowtemp = rowtemp.Replace("{stock}", a[5].ToString().Trim());

                    rowtemp = rowtemp.Replace("{brief}", a[9].ToString().Trim());

                    rowtemp = rowtemp.Replace("{brand}", a[8].ToString().Trim());

                    rowtemp = rowtemp.Replace("{intro}", a[3].ToString().Trim());


                    rowtemp = rowtemp.Replace("{madein}", a[10].ToString().Trim());

                    rowtemp = rowtemp.Replace("{sendfrom}", a[11].ToString().Trim());

                    rowtemp = rowtemp.Replace("{fullprice}", a[6].ToString().Trim());

                    rowtemp = rowtemp.Replace("{extendcat}", a[12].ToString().Trim());


                    rowtemp = rowtemp.Replace("{discountprice}", a[7].ToString().Trim());


                    //ok, let's find out all the images 
                    string detailimage = "";
                    string smallimage = "";
                    string largeimage = ""; 
                    string smallimageset = "";
                    string largeimageset = ""; 


                    foreach(string img in allImages)
                    {
                        //large images
                        if(img.ToUpper().IndexOf(sku.ToUpper() + "-L-") != -1)
                        {
                            largeimageset += url + img + "|";

                            if (largeimage.Length == 0)
                            {
                                largeimage = url + img; 
                            }
                        }

                        else if (img.ToUpper().IndexOf(sku.ToUpper() + "-S-") != -1)
                        {
                            smallimageset += url + img + "|";

                            if (smallimage.Length == 0)
                            {
                                smallimage = url + img; 
                            }
                        }
                        else if (img.ToUpper().IndexOf(sku.ToUpper() + "-P-") != -1)
                        {
                            continue;
                        }
                        else if (img.ToUpper().IndexOf(sku.ToUpper()) != -1)
                        {
                            detailimage += "<img src=\"\"" + url + img + "\"\"></img>"; 
                        }
                    }


                    rowtemp = rowtemp.Replace("{smallimage}", smallimage);
                    rowtemp = rowtemp.Replace("{largeimage}", largeimage);
                    rowtemp = rowtemp.Replace("{smallimageset}", smallimageset);
                    rowtemp = rowtemp.Replace("{largeimageset}", largeimageset);
                    rowtemp = rowtemp.Replace("{content}", detailimage);
                }


                foreach(string str in categories)
                {
                    outPut.Add(rowtemp.Replace("{cat}", str));
                }
            }

        }


        private void writeCSV()
        {

            FileInfo fi = new FileInfo(txtOutput.Text.Trim()); 

      
          if(!fi.Exists)
          {
              fi.Directory.Create();
          }

          try {
                  FileStream  fs = new FileStream(txtOutput.Text.Trim() + "\\" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                  StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                    sw.WriteLine(head);

                    foreach(string row in outPut)
                    {
                        sw.WriteLine(row); 
                    }

                  sw.Close();
                  fs.Close();
              }
            catch(Exception x)
          {
              
              MessageBox.Show("写入文件失败， 失败的原因是"+ x.Message.ToString()); 
          }

        }

        private void readImgfromDirectory()
        {
            if(imgFold.Text.Length ==0)
            {
                return; 
            }

            ProcessDirectory(imgFold.Text);  
        }


        private void ProcessDirectory(string dir)
        {
            string[] files = Directory.GetFiles(dir);

            foreach (string str in files)
            {
                int k = str.LastIndexOf("\\"); 

                allImages.Add(str.Substring(k + 1)); 
            }

            string[] subdirectoryEntries = Directory.GetDirectories(dir);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }


        private void imgFold_MouseDown(object sender, MouseEventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                imgFold.Text = folderBrowserDialog1.SelectedPath; 
            }
        }

        
        public bool readSourceFile()
        {
            if(sourceFile.Text.Length == 0)
            { 
                return false; 
            }


            // load excel application 

            ReadExcelFile(); 

            return true; 
        }




        private void ReadExcelFile()
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            Object cellvalue;
            int rCnt = 0;
            int cCnt = 0;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(sourceFile.Text, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;


            //clear old data
            inputRows.Clear(); 


            // start from second line
            for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
            {
                ArrayList row = new ArrayList(); 
                for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                {
                    cellvalue = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;


                    if (cellvalue != null)
                    {
                        row.Add(cellvalue.ToString());
                        Console.WriteLine(cellvalue.ToString());

                        if (cCnt == 1)
                        {
                            skuList.Add(cellvalue.ToString().Trim());
                        }
                    }
                }

                if(row.Count >0)
                { 
                    inputRows.Add(row);
                }
            }


            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }


        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://detail.tmall.hk/hk/item.htm?spm=a220m.1000858.1000725.1.2wQFHl&id=40957877409&skuId=67793583877&areaId=430100&cat_id=2&rn=1e5c9ffe3e68d072bee93050d477eb37&standard=1&user_id=2113658227&is_b=1"); 


            string content  = DownloadWebPageNew(""); 

            if (content.IndexOf(".tmall.")!= -1)
            {
                //use rules for tmall

                ParseTMALL(content); 

            }
            else if(content.IndexOf(".taobao.") != -1)
            {
                ParseTaobao(content); 
            }
            else 
            {

            }

        }

        private void ParseTMALL(string content)
        {

            WriteStartMark();
            Console.WriteLine("Begin to parse the data");

            ProductDetails product = new ProductDetails();

//            content = content.Replace("\"", "#"); 

            //解析名称
            int loc1 = content.IndexOf("tb-detail-hd");
            int loc2 = content.IndexOf("h1", loc1);
            int loc3 = content.IndexOf(">", loc2);

            int loc4 = content.IndexOf("<", loc3);

            string name = content.Substring(loc3 + 1, loc4 - loc3 - 1).Trim();
            name = name.Replace("\n\t", "");

            Console.WriteLine("产品名称是:  " + name);

            product.name = name; 


            //parse price
            /*int loc5 = content.IndexOf("<span class=#tm-price#>", loc4);
            int loc6 = content.IndexOf(">", loc5);
            int loc7 = content.IndexOf("<", loc6);

            string sfullprice = content.Substring(loc6 + 1, loc7 - loc6);

            decimal fullprice = 0; 

            try 
            {
                fullprice = Decimal.Parse(sfullprice); 
            }
            catch(Exception x)
            {
                Console.WriteLine("原价解析错误" + x.Message); 
            }

            Console.WriteLine("产品原价是:  " + sfullprice);

            product.fullprice = fullprice; 
             */

            //parse smallimage set
            int loc5 = content.IndexOf("J_UlThumb", loc4);
            int end = content.IndexOf("J_EditItem", loc5); 

            string imgset = content.Substring(loc5 + 8, end - loc5 - 8); 

            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            MatchCollection matches = regImg.Matches(imgset);

            ArrayList imgs = new ArrayList(); 

            foreach(Match mt in matches)
            {
                imgs.Add("http://" + mt.Groups["imgUrl"].Value.Substring(2));

                Console.WriteLine(mt.Groups["imgUrl"].Value.Substring(2)); 
            }

            int sq = 0;

            if (!Directory.Exists(txtOutput.Text))
            {
                Directory.CreateDirectory(txtOutput.Text); 
            }

           
            string parentPath = txtOutput.Text + "\\" + product.name; 


            if(!Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath); 
            }

            if (!Directory.Exists(parentPath + "\\small"))
            {
                Directory.CreateDirectory(parentPath +"\\small");
            }

            if (!Directory.Exists(parentPath + "\\mobile"))
            {
                Directory.CreateDirectory(parentPath + "\\mobile");
            }

            if (!Directory.Exists(parentPath + "\\large"))
            {
                Directory.CreateDirectory(parentPath + "\\large");
            }


            foreach(string url in imgs)
            {
                sq ++; 
                string small = url.Replace("60x60", "400x400");

                string ext = url.Substring(url.Length - 4, 4);
                string filename = sq.ToString() + ext;


                DownloadImage(small, parentPath + "\\small\\" + filename);

                string mobile = url.Replace("60x60", "720x720");
                DownloadImage(mobile, parentPath + "\\mobile\\" + filename);

                string large = url.Replace("60x60", "760x760");
                DownloadImage(large, parentPath + "\\large\\" + filename); 
            }

            //parse largeimage set
             



            WriteCloseMark(); 
        }

        private void WriteStartMark()
        {
            Console.WriteLine("");
            Console.WriteLine(oneline);
        }

        private void WriteCloseMark()
        {
            Console.WriteLine(oneline);
            Console.WriteLine("");
        }
        
        private void ParseTaobao(string content)
        {
            WriteStartMark(); 
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Console.Out.Close(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HtmlElement el = webBrowser1.Document.GetElementById("mainwrap"); 
            if(el != null)
            {
                int i = 0; 
            }

            webBrowser1.ShowSaveAsDialog();

//            System.IO.StreamReader getReader = new System.IO.StreamReader(webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("gb2312"));

//            string gethtml = getReader.ReadToEnd();

        }

    }


    class ProductDetails
    {
        public string SUK;

        public string URL;

        public string category;

        public string name;

        public int stock;

        public int weight;

        public decimal fullprice;

        public decimal discountprice;

        public string brief;

        public string intro;

        public string content;

        public string smallimage;

        public string largeimage;

        public string keyword;

        public string seodesc;

        public int producttype; 

        public string brand; 

        public string madein; 

        public string sendfrom;

        public string smallimageset;

        public string largetimageset;

        public string extendcategories; 

    }
}
