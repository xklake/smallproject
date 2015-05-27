using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO; 
using Excel = Microsoft.Office.Interop.Excel;



namespace Product_Importer
{
    public partial class Form1 : Form
    {
        ArrayList inputRows = new ArrayList();

        ArrayList skuList = new ArrayList();

        ArrayList allImages = new ArrayList();

        ArrayList outPut = new ArrayList();

        string url = "http://7xiosd.com1.z0.glb.clouddn.com/"; 

        //"ID","分类","名称","Sku","库存","重量","市场价","商城价","简述","内容","缩略图","图片","关键字","描述","类型","品牌","产地","发货地",缩略图集,图片集

        string rowtemp = ",\"{cat}\",\"{name}\",\"{sku}\",\"{stock}\",\"{weight}\",\"{fullprice}\",\"{discountprice}\",\"{brief}\",\"{content}\",\"{smallimage}\",\"{largeimage}\",\"{keywords}\",\"{seodesc}\",\"17\",\"英国\",\"英国\",\"{brand}\",\"{smallimageset}\",\"{largeimageset}\"";


        string head = "ID,分类,名称,Sku,库存,重量,市场价,商城价,简述,内容,缩略图,图片,关键字,描述,类型,品牌,产地,发货地,缩略图集,图片集"; 

        public Form1()
        {
            InitializeComponent();
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


        private void processTaobao()
        {
            allImages.Sort();

            foreach (Object row in inputRows)
            {
                //rowtemp = ",\"{cat}\",\"{name}\",\"{sku}\",\"{stock}\",\"{weight}\",\"{fullprice}\",\"{discountprice}\",\"{brief}\",{content},\"{smallimage}\",\"{largeimage}\",\"{keywords}\",\"{seodesc}\",\"17\",\"英国\",\"英国\"，\"{brand}\"，\"{smallimageset}\",\"{largeimageset}\"";
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

                    rowtemp = rowtemp.Replace("{brief}", a[3].ToString().Trim());

                    rowtemp = rowtemp.Replace("{brand}", a[8].ToString().Trim());

                    rowtemp = rowtemp.Replace("{fullprice}", a[6].ToString().Trim());

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
                        if (img.IndexOf(sku + "-L-") != -1)
                        {
                            largeimageset += url + img + "|";

                            if (largeimage.Length == 0)
                            {
                                largeimage = url + img;
                            }
                        }

                        else if (img.IndexOf(sku + "-S-") != -1)
                        {
                            smallimageset += url + img + "|";

                            if (smallimage.Length == 0)
                            {
                                smallimage = url + img;
                            }
                        }
                        else if (img.IndexOf(sku) != -1)
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
                rowtemp = ",\"{cat}\",\"{name}\",\"{sku}\",\"{stock}\",\"{weight}\",\"{fullprice}\",\"{discountprice}\",\"{brief}\",\"{content}\",\"{smallimage}\",\"{largeimage}\",\"{keywords}\",\"{seodesc}\",\"17\",\"英国\",\"英国\",\"{brand}\",\"{smallimageset}\",\"{largeimageset}\"";

                ArrayList a = (ArrayList)row;

                string sku = a[0].ToString();

                string cat = a[a.Count - 1].ToString();

                string [] categories = cat.Split('|');


                {
                    rowtemp = rowtemp.Replace("{sku}", a[0].ToString().Trim());

                    rowtemp = rowtemp.Replace("{name}", a[2].ToString().Trim());

                    rowtemp = rowtemp.Replace("{weight}", a[4].ToString().Trim());

                    rowtemp = rowtemp.Replace("{stock}", a[5].ToString().Trim());

                    rowtemp = rowtemp.Replace("{brief}", a[3].ToString().Trim());

                    rowtemp = rowtemp.Replace("{brand}", a[8].ToString().Trim());

                    rowtemp = rowtemp.Replace("{fullprice}", a[6].ToString().Trim());

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
                        if(img.IndexOf(sku + "-L-") != -1)
                        {
                            largeimageset += url + img + "|";

                            if (largeimage.Length == 0)
                            {
                                largeimage = url + img; 
                            }
                        }

                        else if(img.IndexOf(sku + "-S-") != -1)
                        {
                            smallimageset += url + img + "|";

                            if (smallimage.Length == 0)
                            {
                                smallimage = url + img; 
                            }
                        }
                        else if(img.IndexOf(sku) != -1)
                        {
                            detailimage += "<img src=\"\"" + url + img + "\"\"></img>|"; 
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
                  StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

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



    }
}
