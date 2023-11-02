using Microsoft.VisualBasic.FileIO;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace App_SamplingDist_C    // max e min del relative trovato, dividi hai le classi

{
    public partial class Form1 : Form
    {






        EditableRectangle r1;
        EditableRectangle r2;
        EditableRectangle r3;
        EditableRectangle r4;
        EditableRectangle r5;
        EditableRectangle r6;
        Bitmap b;
        Graphics g;

        float lunghezza_tratto;
        float altezza_tratto;
        float lunghezza_tratto_r2;
        float altezza_tratto_r2;
        float lunghezza_tratto_r3;
        float altezza_tratto_r3;
        float lunghezza_tratto_r4;
        float altezza_tratto_r4;
        float lunghezza_tratto_r5;
        float altezza_tratto_r5;
        float lunghezza_tratto_r6;
        float altezza_tratto_r6;



        List<Pen> pens = new List<Pen>();
        List<Pen> pens_g = new List<Pen>();
        List<List<int>> listOfLists = new List<List<int>>();
        List<List<int>> listOflist_of_attacks = new List<List<int>>();

        List<List<int>> list_c = new List<List<int>>();
        List<List<int>> list_d = new List<List<int>>();



        int linee = 10;
        int sample = 100;



        int n_linee = 10;
        int n_sample = 100;
        float prob = 0.5f;



        int sample_size = 7;
        int num_samples = 50;

        Random rand = new Random();


        SortedDictionary<Interval, int> dist_mean;
        SortedDictionary<Interval, int> dist_variance;

        public Form1()
        {


            InitializeComponent();

            initialize_graphics(); // OK
        }

        public void create_pens()
        {

            pens.Clear();
            pens_g.Clear();
            for (int i = 0; i < linee; i++)
            {
                int red = rand.Next(256); // 0-255
                int green = rand.Next(256); // 0-255
                int blue = rand.Next(256); // 0-255


                Color randomColor = Color.FromArgb(red, green, blue);
                Pen peng = new Pen(randomColor, 1);

                pens.Add(peng);
                //Console.WriteLine("aggiunta penna");
            }
            for (int i = 0; i < linee; i++)
            {
                int red = rand.Next(256); // 0-255
                int green = rand.Next(256); // 0-255
                int blue = rand.Next(256); // 0-255


                Color randomColor = Color.FromArgb(red, green, blue);
                Pen peng = new Pen(randomColor, 1);

                pens_g.Add(peng);
                //Console.WriteLine("aggiunta penna");
            }

        }

        public void initialize_stat()
        {

            listOfLists.Clear();
            listOflist_of_attacks.Clear();
            for (int i = 0; i < linee; i++)
            {
                List<int> a = new List<int>();

                for (int j = 0; j < sample; j++)
                {

                    float randomFloat = (float)rand.NextDouble();
                    //Console.WriteLine($"valore usci vs prob == {randomFloat} {prob}");
                    if (randomFloat < prob)                               // fai scegliere prob
                    {
                        a.Add(+1);

                    }
                    else
                    {
                        a.Add(-1);


                    }
                }
                listOfLists.Add(a);
            }

            //           SECONDO GRAFICO



            for (int i = 0; i < linee; i++)                      //  vari sistemi
            {
                List<int> a = new List<int>();

                for (int j = 0; j < sample; j++)             // quanti giorni consideriamo
                {

                    float randomFloat1 = (float)rand.NextDouble();
                    //Console.WriteLine($"valore usci vs prob == {randomFloat1} {prob}");
                    if (randomFloat1 < prob)                                   // scegli prob
                    {
                        a.Add(+1);

                    }
                    else
                    {
                        a.Add(-1);


                    }
                }
                listOflist_of_attacks.Add(a);
            }



        }

        private void initialize_graphics()
        {
            //timer1.Start();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            // coordinate iniziali                                              // this passa il form
            r1 = new EditableRectangle(20, 20, pictureBox1.Width / 4, pictureBox1.Height / 4, pictureBox1, this);
            r2 = new EditableRectangle(40 + (pictureBox1.Width / 4), 20, pictureBox1.Width / 4, pictureBox1.Height / 4, pictureBox1, this);
            r3 = new EditableRectangle(20, (pictureBox1.Height / 4) + 20, pictureBox1.Width / 4, pictureBox1.Height / 4, pictureBox1, this);
            r4 = new EditableRectangle(40 + (pictureBox1.Width / 4), (pictureBox1.Height / 5) + 40, pictureBox1.Width / 4, pictureBox1.Height / 4, pictureBox1, this);

            r5 = new EditableRectangle(20, 20, pictureBox1.Width / 4, pictureBox1.Height / 4, pictureBox1, this);
            r6 = new EditableRectangle(20 + (pictureBox1.Width / 2), 20, pictureBox1.Width / 4, pictureBox1.Height / 4, pictureBox1, this);
            /*Console.WriteLine(r1.r.Top.ToString()); // 20
            Console.WriteLine(r1.r.Bottom.ToString());  //466
            Console.WriteLine(r1.r.Left.ToString());// 20
            Console.WriteLine(r1.r.Right.ToString());// 900*/
        }




        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            n_linee = trackBar1.Value;
            string labeltext = "Sample size -" + n_linee.ToString();
            label1.Text = labeltext;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            n_sample = trackBar2.Value;
            string labeltext = "Number of samples -" + n_sample.ToString();
            label2.Text = labeltext;
        }

        private void button1_Click(object sender, EventArgs e)
        {



            timer1.Stop();



            sample = n_sample;
            linee = n_linee;
            if (sample > 200)
            {
                MessageBox.Show("Please choose a lower value.", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            prob = (float)(numericUpDown3.Value);
            //Console.WriteLine("probabilità ==");
            //Console.WriteLine(prob);
            initialize_stat();
            create_pens();


            timer1.Start();



        }








        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            draw_rectangles();


            linechart();                                // per primo grafico
            line_chart_num_att();
            line_chart_rel();
            line_chart_norm();
            pictureBox1.Image = b;
        }

        public void line_chart_norm()    //REPLICAUGUALE A REL            // qUI                        // occhia l'altezza
        {
            Point centro_sempre = new Point(r4.r.Left, ((r4.r.Bottom - r4.r.Top) / 2) + r4.r.Top);
            Point fine = new Point(r4.r.Right, ((r4.r.Bottom - r4.r.Top) / 2) + r4.r.Top);

            Pen pen = new Pen(Color.Black, 3);
            //g.DrawLine(Pens.Black, centro_sempre, fine);



            lunghezza_tratto_r4 = (r4.r.Right - r4.r.Left) / sample;
            altezza_tratto_r4 = (r4.r.Bottom - r4.r.Top) / sample;
            Console.WriteLine($"ALTEZZA TRATTO R4 == {altezza_tratto_r4}");

            List<float> min_max = new List<float>();
            List<float> min_max_med = new List<float>();


            for (int j = 0; j < listOfLists.Count; j++)
            {

                //int j = 0;

                min_max.Add(0.0f);
                min_max_med.Add(0.0f);

                //PointF p0 = new PointF(r4.r.Left, ((r4.r.Bottom - r4.r.Top) / 2) + r4.r.Top);   // centro
                PointF p0 = new PointF(r4.r.Left, r4.r.Bottom);   // centro

                Pen new_p = pens_g[j];
                PointF next;

                int tot = 0;
                int tot_succ = 0;




                for (int i = 0; i < listOflist_of_attacks[j].Count; i++)
                {




                    if (listOflist_of_attacks[j][i] > 0)
                    {

                        tot++;
                        tot_succ++;
                        float y_comp = (float)(tot_succ / (Math.Sqrt((double)tot)));                 // questo è di quanto si alza il tratto

                        float p1x = (float)p0.X + lunghezza_tratto_r4;        // lunghezza tratto proporzionale a n_succ/tot_attacks    ??
                        float p1y = (float)p0.Y - y_comp * 0.1f;                  // in teoria non si sposta più di unacosa costante

                        next = new PointF(p1x, p1y);
                        tot_succ = tot_succ + 1;
                        g.DrawLine(new_p, p0, next);
                        p0 = next;


                    }
                    else                // non so se alzarmi sempre di tot_succ/tot o lasciare la y di prima, se non lascio quello di prima viene basso
                    {

                        tot++;
                        float y_comp1 = (float)(tot_succ / (Math.Sqrt((double)tot)));                 // questo è di quanto si alza il tratto



                        float p1x = (float)p0.X + (float)(lunghezza_tratto_r4);

                        float p1y_m = (float)p0.Y - y_comp1 * 0.1f;

                        next = new PointF(p1x, p1y_m);

                        g.DrawLine(new_p, p0, next);
                        p0 = next;



                    }
                    float alta1 = p0.Y;
                    if (i == sample / 2)
                    {
                        min_max_med[j] = alta1;
                    }

                }
                float alta = p0.Y;
                min_max[j] = alta;




            }
            int delta = 5;
            float m = min_max.Max();
            float min = min_max.Min();
            int q_n = linee / delta;
            float q = (min_max.Max() - min_max.Min()) / q_n;
            Console.WriteLine($"il massimo è {m} il minimo è {min}");
            Console.WriteLine($"la spaziatura è da {q}");

            int intervalli = min_max.Count / delta;


            compute_insto_norm(min_max, intervalli, q, 0);

            float m1 = min_max_med.Max();
            float min1 = min_max_med.Min();
            int q_n1 = linee / delta;
            float q1 = (min_max_med.Max() - min_max_med.Min()) / q_n1;
            Console.WriteLine($"il massimo è {m1} il minimo è {min1}");
            Console.WriteLine($"la spaziatura è da {q1}");
            int intervalli1 = min_max_med.Count / delta;
            foreach (int number in min_max_med)
            {
                Console.WriteLine("valore in lista ==");
                Console.WriteLine(number);
            }

            compute_insto_norm(min_max_med, intervalli1, q1, 1);
        }

        public void compute_insto_norm(List<float> val, int inter, float spaziatura, int f)
        {
            Rectangle r_insto;
            int x_s;
            if (f != 0)
            {
                x_s = (int)(r4.r.Left + (sample / 2) * lunghezza_tratto_r4);     //  la x è questa

            }
            else
            {
                x_s = (int)(r4.r.Left + (sample) * lunghezza_tratto_r4);     //  la x è questa

            }
            int y_s = r4.r.Bottom;
            int y_f = r4.r.Top;
            int base_rett = (y_s - y_f) / linee;


            PointF p0 = new PointF(x_s, y_s);
            PointF p1 = new PointF(x_s, y_f);
            g.DrawLine(Pens.Magenta, p0, p1);


            Console.WriteLine($"date linee {linee} faro {inter} rettangoli");



            //quanto sono alti? --> quantivalori ci cadono


            for (int j = 0; j < inter; j++)
            {
                int quanti_ne_cadono = 0;

                for (int i = 0; i < val.Count; i++)
                {
                    float min = val.Min() + (spaziatura * j);
                    float m = val.Min() + (spaziatura * (j + 1));
                    if (val[i] > min && val[i] < m)
                    {
                        Console.WriteLine($"intervallo {min} a {m}");
                        quanti_ne_cadono++;     // quanti di successo
                    }
                }


                //grafica
                int l = (int)(quanti_ne_cadono * lunghezza_tratto_r4);
                if (l >= 0)
                {
                    r_insto = new Rectangle(x_s, y_s - (base_rett) * (j + 1), l, base_rett);
                    //Console.WriteLine($"index == {index}");
                    g.DrawRectangle(Pens.Black, r_insto);
                    g.FillRectangle(Brushes.Red, r_insto);



                }

            }
        }



        public void line_chart_rel()              // FUNGE
        {
            Point centro_sempre = new Point(r3.r.Left, ((r3.r.Bottom - r3.r.Top) / 2) + r3.r.Top);
            Point fine = new Point(r3.r.Right, ((r3.r.Bottom - r3.r.Top) / 2) + r3.r.Top);

            Pen pen = new Pen(Color.Black, 3);
            //g.DrawLine(Pens.Black, centro_sempre, fine);
            List<float> min_max = new List<float>();
            List<float> min_max_med = new List<float>();



            lunghezza_tratto_r3 = (r3.r.Right - r3.r.Left) / sample;
            altezza_tratto_r3 = (r3.r.Bottom - r3.r.Top) / sample;
            Console.WriteLine($"ALTEZZA TRATTO R3 == {altezza_tratto_r3}");


            for (int j = 0; j < listOfLists.Count; j++)
            {

                //int j = 0;

                min_max.Add(0.0f);
                min_max_med.Add(0.0f);
                PointF p0 = new PointF(r3.r.Left, r3.r.Bottom);   // centro

                Pen new_p = pens_g[j];
                PointF next;

                int tot = 0;
                int tot_succ = 0;



                for (int i = 0; i < listOflist_of_attacks[j].Count; i++)
                {




                    if (listOflist_of_attacks[j][i] > 0)
                    {

                        tot++;
                        tot_succ++;
                        float y_comp = (tot_succ / tot);                 // questo è di quanto si alza il tratto -------------------------------------------

                        float p1x = (float)p0.X + lunghezza_tratto_r3;        // lunghezza tratto proporzionale a n_succ/tot_attacks    ??
                        float p1y = (float)p0.Y - y_comp;                  // in teoria non si sposta più di unacosa costante

                        next = new PointF(p1x, p1y);
                        tot_succ = tot_succ + 1;
                        g.DrawLine(new_p, p0, next);
                        p0 = next;


                    }
                    else                // non so se alzarmi sempre di tot_succ/tot o lasciare la y di prima, se non lascio quello di prima viene basso
                    {

                        tot++;
                        float y_comp1 = (tot_succ / tot);                                       // questo è di quanto si alza il tratto



                        float p1x = (float)p0.X + (float)(lunghezza_tratto_r3);

                        float p1y_m = (float)p0.Y - y_comp1;

                        next = new PointF(p1x, p1y_m);

                        g.DrawLine(new_p, p0, next);
                        p0 = next;



                    }
                    float alta = p0.Y;
                    min_max[j] = alta;
                    if (i == sample / 2)
                    {
                        min_max_med[j] = alta;
                    }

                }


            }

            int delta = 5;
            float m = min_max.Max();
            float min = min_max.Min();
            int q_n = linee / delta;
            float q = (min_max.Max() - min_max.Min()) / q_n;
            Console.WriteLine($"il massimo è {m} il minimo è {min}");
            Console.WriteLine($"la spaziatura è da {q}");

            int intervalli = min_max.Count / delta;


            compute_instogram_relative(min_max, intervalli, q, 0);

            float m1 = min_max_med.Max();
            float min1 = min_max_med.Min();
            int q_n1 = linee / delta;
            float q1 = (min_max_med.Max() - min_max_med.Min()) / q_n1;
            Console.WriteLine($"il massimo è {m1} il minimo è {min1}");
            Console.WriteLine($"la spaziatura è da {q1}");
            int intervalli1 = min_max_med.Count / delta;
            foreach (int number in min_max_med)
            {
                Console.WriteLine("valore in lista ==");
                Console.WriteLine(number);
            }

            compute_instogram_relative(min_max_med, intervalli1, q1, 1);
        }

        public void compute_instogram_relative(List<float> val, int inter, float spaziatura, int f)
        {

            int x_s;
            Rectangle r_insto;
            if (f != 0)
            {
                x_s = (int)(r3.r.Left + (sample / 2) * lunghezza_tratto_r3);     //  la x è questa

            }
            else
            {
                x_s = (int)(r3.r.Left + (sample) * lunghezza_tratto_r3);     //  la x è questa

            }
            int y_s = r3.r.Bottom;
            int y_f = r3.r.Top;
            int base_rett = (y_s - y_f) / linee;


            PointF p0 = new PointF(x_s, y_s);
            PointF p1 = new PointF(x_s, y_f);
            g.DrawLine(Pens.Magenta, p0, p1);


            Console.WriteLine($"date linee {linee} faro {inter} rettangoli");



            //quanto sono alti? --> quantivalori ci cadono


            for (int j = 0; j < inter; j++)
            {
                int quanti_ne_cadono = 0;

                for (int i = 0; i < val.Count; i++)
                {
                    float min = val.Min() + (spaziatura * j);
                    float m = val.Min() + (spaziatura * (j + 1));
                    if (val[i] > min && val[i] < m)
                    {
                        Console.WriteLine($"intervallo {min} a {m}");
                        quanti_ne_cadono++;     // quanti di successo
                    }
                }


                //grafica
                int l = (int)(quanti_ne_cadono * lunghezza_tratto_r3);
                if (l >= 0)
                {
                    r_insto = new Rectangle(x_s, y_s - (base_rett) * (j + 1), l, base_rett);
                    //Console.WriteLine($"index == {index}");
                    g.DrawRectangle(Pens.Black, r_insto);
                    g.FillRectangle(Brushes.Red, r_insto);



                }




            }













        }
        public void line_chart_num_att()             // qui la divisione in rettangoli è per num attacchi tot
        {
            Point centro_sempre = new Point(r2.r.Left, ((r2.r.Bottom - r2.r.Top) / 2) + r2.r.Top);
            Point fine = new Point(r2.r.Right, ((r2.r.Bottom - r2.r.Top) / 2) + r2.r.Top);

            Pen pen = new Pen(Color.Black, 3);
            //g.DrawLine(Pens.Black, centro_sempre, fine);



            lunghezza_tratto_r2 = (r2.r.Right - r2.r.Left) / sample;
            altezza_tratto_r2 = (r2.r.Bottom - r2.r.Top) / sample;

            Console.WriteLine($"altezza tratto r2 =={altezza_tratto_r2}");

            List<int> somme = new List<int>();
            List<int> somme_med = new List<int>();




            for (int j = 0; j < listOfLists.Count; j++)
            {

                //int j = 0;


                somme.Add(0);
                somme_med.Add(0);


                PointF p0 = new PointF(r2.r.Left, r2.r.Bottom);

                Pen new_p = pens_g[j];
                PointF next;

                for (int i = 0; i < listOflist_of_attacks[j].Count; i++)
                {


                    float p1x = (float)p0.X + (float)(lunghezza_tratto_r2);
                    float p1y = (float)p0.Y - (float)altezza_tratto_r2;
                    float p1y_m = (float)p0.Y;


                    if (listOflist_of_attacks[j][i] > 0)
                    {

                        next = new PointF(p1x, p1y);

                        g.DrawLine(new_p, p0, next);
                        p0 = next;
                        //Console.WriteLine($"valore == {listOflist_of_attacks}");
                        somme[j] = somme[j] + listOflist_of_attacks[j][i];



                    }
                    else
                    {
                        next = new PointF(p1x, p1y_m);
                        g.DrawLine(new_p, p0, next);
                        p0 = next;



                    }
                    if (i < (sample / 2))
                    {
                        somme_med[j] = somme_med[j] + listOflist_of_attacks[j][i];


                    }


                }

                int delta = 5;
                int m = somme.Max();
                int min = somme.Min();
                int q_n = linee / delta;
                int q = (somme.Max() - somme.Min()) / q_n;
                Console.WriteLine($"il massimo è {m} il minimo è {min}");
                Console.WriteLine($"la spaziatura è da {q}");
                int intervalli = somme.Count / delta;
                /*foreach (int number in somme)
                {
                    Console.WriteLine(number);
                }*/



                compute_instogram_attack(somme, intervalli, q, 0);

                int m1 = somme_med.Max();
                int min1 = somme_med.Min();
                int q_n1 = linee / delta;
                int q1 = (somme_med.Max() - somme_med.Min()) / q_n1;
                Console.WriteLine($"il massimo è {m1} il minimo è {min1}");
                Console.WriteLine($"la spaziatura è da {q1}");
                int intervalli1 = somme_med.Count / delta;
                foreach (int number in somme_med)
                {
                    Console.WriteLine("valore in lista ==");
                    Console.WriteLine(number);
                }

                compute_instogram_attack(somme_med, intervalli1, q1, 1);


            }



        }

        public void compute_instogram_attack(List<int> val, int inter, int spaziatura, int f)
        {


            int x_s;

            Rectangle r_insto;
            if (f != 0)
            {
                x_s = (int)(r2.r.Left + (sample / 2) * lunghezza_tratto_r2);     //  la x è questa

            }
            else
            {
                x_s = (int)(r2.r.Left + (sample) * lunghezza_tratto_r2);     //  la x è questa

            }
            int y_s = r2.r.Bottom;
            int y_f = r2.r.Top;
            int base_rett = (y_s - y_f) / linee;


            PointF p0 = new PointF(x_s, y_s);
            PointF p1 = new PointF(x_s, y_f);
            g.DrawLine(Pens.Magenta, p0, p1);


            Console.WriteLine($"date linee {linee} faro {inter} rettangoli");



            //quanto sono alti? --> quantivalori ci cadono


            for (int j = 0; j < inter; j++)
            {
                int quanti_ne_cadono = 0;

                for (int i = 0; i < val.Count; i++)
                {
                    int min = val.Min() + (spaziatura * j);
                    int m = val.Min() + (spaziatura * (j + 1));
                    if (val[i] > min && val[i] < m)
                    {
                        Console.WriteLine($"intervallo {min} a {m}");
                        quanti_ne_cadono++;     // quanti di successo
                    }
                }


                //grafica
                int l = (int)(quanti_ne_cadono * lunghezza_tratto_r2);
                if (l >= 0)
                {
                    r_insto = new Rectangle(x_s, y_s - (base_rett) * (j + 1), l, base_rett);
                    //Console.WriteLine($"index == {index}");
                    g.DrawRectangle(Pens.Black, r_insto);
                    g.FillRectangle(Brushes.Red, r_insto);



                }




            }

        }


        private void linechart()          // la divisione in rettangoli è per num attacchi riusciti
        {
            Console.WriteLine("quante linee quanti sample 2");

            Console.WriteLine(linee);
            Console.WriteLine(sample);

            // 20 top
            //466 bottom
            // 20 left
            // 900 destra
            // linea di mezzo

            Point centro_sempre = new Point(r1.r.Left, ((r1.r.Bottom - r1.r.Top) / 2) + r1.r.Top);
            Point fine = new Point(r1.r.Right, ((r1.r.Bottom - r1.r.Top) / 2) + r1.r.Top);

            Pen pen = new Pen(Color.Black, 3);
            g.DrawLine(Pens.Black, centro_sempre, fine);



            lunghezza_tratto = (r1.r.Right - r1.r.Left) / sample;
            altezza_tratto = (r1.r.Bottom - r1.r.Top) / sample;

            List<int> somme = new List<int>();
            List<int> somme_med = new List<int>();



            for (int j = 0; j < listOfLists.Count; j++)
            {

                //int j = 0;

                somme.Add(0);
                somme_med.Add(0);


                PointF p0 = new PointF(r1.r.Left, ((r1.r.Bottom - r1.r.Top) / 2) + r1.r.Top);   // centro

                Pen new_p = pens[j];
                PointF next;

                for (int i = 0; i < listOfLists[j].Count; i++)                  //for (int i = 0; i < stat.Count; i++)
                {

                    float p1x = (float)p0.X + lunghezza_tratto;
                    float p1y = (float)p0.Y + altezza_tratto;
                    float p1y_m = (float)p0.Y - altezza_tratto;




                    if (listOfLists[j][i] > 0)
                    {

                        next = new PointF(p1x, p1y);

                        g.DrawLine(new_p, p0, next);
                        p0 = next;


                    }
                    else
                    {
                        next = new PointF(p1x, p1y_m);
                        g.DrawLine(new_p, p0, next);
                        p0 = next;



                    }
                    somme[j] = somme[j] + listOfLists[j][i];
                    if (i < (sample / 2))
                    {
                        somme_med[j] = somme_med[j] + listOfLists[j][i];
                    }


                }
                //                        prova qui


            }
            int delta = 5;
            int m = somme.Max();
            int min = somme.Min();
            int q_n = linee / delta;
            int q = (somme.Max() - somme.Min()) / q_n;
            //Console.WriteLine($"il massimo è {m} il minimo è {min}");
            //Console.WriteLine($"la spaziatura è da {q}");
            int intervalli = somme.Count / delta;
            foreach (int number in somme)
            {
                //Console.WriteLine(number);
            }
            compute_instogram(somme, intervalli, q, 0);

            int m1 = somme_med.Max();
            int min1 = somme_med.Min();
            int q_n1 = linee / delta;
            int q1 = (somme_med.Max() - somme_med.Min()) / q_n1;
            Console.WriteLine($"il massimo è {m1} il minimo è {min1}");
            Console.WriteLine($"la spaziatura è da {q1}");
            int intervalli1 = somme_med.Count / delta;
            foreach (int number in somme_med)
            {
                Console.WriteLine("valore in lista ==");
                Console.WriteLine(number);
            }

            compute_instogram(somme_med, intervalli1, q1, 1);




        }


        public void compute_instogram(List<int> val, int inter, int spaziatura, int first)       // l'index mi dice dove sulla inea
        {





            Rectangle r_insto;
            int x_s;

            if (first != 0)
            {
                x_s = (int)(r1.r.Left + (sample / 2) * lunghezza_tratto);
            }
            else
            {

                x_s = (int)(r1.r.Left + (sample) * lunghezza_tratto);     //  la x è questa
            }
            int y_s = r1.r.Bottom;
            int y_f = r1.r.Top;
            int base_rett = (y_s - y_f) / linee;


            PointF p0 = new PointF(x_s, y_s);
            PointF p1 = new PointF(x_s, y_f);
            g.DrawLine(Pens.Magenta, p0, p1);


            Console.WriteLine($"date linee {linee} faro {inter} rettangoli");



            //quanto sono alti? --> quantivalori ci cadono


            for (int j = 0; j < inter; j++)
            {
                int quanti_ne_cadono = 0;

                for (int i = 0; i < val.Count; i++)
                {
                    int min = val.Min() + (spaziatura * j);
                    int m = val.Min() + (spaziatura * (j + 1));
                    if (val[i] > min && val[i] < m)
                    {
                        Console.WriteLine($"intervallo {min} a {m}");
                        quanti_ne_cadono++;     // quanti di successo
                    }
                }


                //grafica
                int l = (int)(quanti_ne_cadono * lunghezza_tratto);
                if (l >= 0)
                {
                    r_insto = new Rectangle(x_s, y_s - (base_rett) * (j + 1), l, base_rett);
                    //Console.WriteLine($"index == {index}");
                    g.DrawRectangle(Pens.Black, r_insto);
                    g.FillRectangle(Brushes.Red, r_insto);



                }




            }








        }







        //qui disegno rettangoli
        private void draw_rectangles()                            // QUI         gli istogrammi sono fatti da due funzioni separate (l'altra posso toglierla)
        {

            g.FillRectangle(Brushes.White, r1.r);
            g.DrawRectangle(Pens.Red, r1.r);
            g.FillRectangle(Brushes.White, r2.r);
            g.DrawRectangle(Pens.Red, r2.r);
            g.FillRectangle(Brushes.White, r3.r);
            g.DrawRectangle(Pens.Red, r3.r);
            g.FillRectangle(Brushes.White, r4.r);
            g.DrawRectangle(Pens.Red, r4.r);


        }

      
    }
}

