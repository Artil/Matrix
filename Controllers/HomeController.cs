using Matrix.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Matrix.Controllers
{
    public class HomeController : Controller
    {
        Random rand = new Random();
        private static matrix array = new matrix();

        public ActionResult Index()
        {
            int w = 3, h = 3;
            gener(array, w, h);
                return View(array);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Nvalue(int N)
        {
            gener(array, N, N);

            return View("Index",array);
        }

        public ActionResult Create()
        {
            try
            {
                array.Data = null;
                using (var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory+"1.csv", false, Encoding.Default))
                {
                    for (int i = 0; i < array.Data.Length; i++)
                    {
                        string semicolon = ";";
                        for (int j = 0; j < array.Data.Length; j++)
                        {
                            if (j == array.Data.Length - 1) // отображение элементов матрицей в csv файле
                                semicolon = "\n";
                            sw.Write(array.Data[i][j] + semicolon);
                        }
                    }
                }
                Read();
            } catch (Exception e) { }

            return View("Index", array);
        }

        public ActionResult Read()
        {
            try
            {
                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "1.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadToEnd();
                        var values = line.Split(new Char[] { ';', '\n' });
                        int count=0;
                        array.Data = new int[Convert.ToInt32(Math.Sqrt(values.Length))][];//в квадратической матрице количество строк
                        for (int i = 0; i < array.Data.Length; i++)                       //равно корню общего количества элементов
                        {
                            array.Data[i] = new int[Convert.ToInt32(Math.Sqrt(values.Length))];
                            for (int j = 0; j < array.Data.Length; j++)
                            {
                                array.Data[i][j] = Convert.ToInt32(values[count]);
                                count++;
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View("Index", array);
        }

        public ActionResult Swap() //переворачиваем матрицу используя XOR
        {
            for (int i = 0; i < array.Data.Length; i++) 
            {
                for (int j = i; j < array.Data.Length; j++)
                {
                    if (i != j)
                    {
                        array.Data[i][j] ^= array.Data[j][i];
                        array.Data[j][i] ^= array.Data[i][j];
                        array.Data[i][j] ^= array.Data[j][i];
                    }
                }
            }

            for (int i = 0; i < array.Data.Length / 2; i++)
            {
                for (int j = 0; j < array.Data.Length; j++)
                {
                    array.Data[j][i] ^= array.Data[j][array.Data.Length - 1 - i];
                    array.Data[j][array.Data.Length - 1 - i] ^= array.Data[j][i];
                    array.Data[j][i] ^= array.Data[j][array.Data.Length - 1 - i];
                }
            }

            return View("Index", array);
        }

        public void gener(matrix array, int w, int h)
        {
            array.Data = null;
            array.Data = new int[w][];
            for (int i = 0; i < w; i++)
            {
                array.Data[i] = new int[h];
                for (int j = 0; j < w; j++)
                    array.Data[i][j] = rand.Next(100);
            }
        }

    }
}