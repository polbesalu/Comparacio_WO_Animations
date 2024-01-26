using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Comparacio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Border> elements;
        List<Rectangle> rectangles;
        List<Ellipse> ellipses;
        int[] valorsFigures;
        int numlinies;
        int[] valorsReals;
        int ngruix;
        int nradi;
        double tpausa;
        int[] posintercanvis;


        public MainWindow()
        {
            InitializeComponent(
                );
            numlinies = 100;
            ngruix = 0;
            nradi = 0;
            tpausa = 500;
            elements = new List<Border>();
            valorsFigures = new int[0];
            valorsReals = new int[0];
            posintercanvis = new int[numlinies];
            cb_correcte.ItemsSource = typeof(Colors).GetProperties();
            cb_incorrecte.ItemsSource = typeof(Colors).GetProperties();
            cb_intercanviar.ItemsSource = typeof(Colors).GetProperties();
            cb_fons.ItemsSource = typeof(Colors).GetProperties();
            cb_correcte.SelectedIndex = 51;
            cb_incorrecte.SelectedIndex = 113;
            cb_intercanviar.SelectedIndex = 48;
            cb_fons.SelectedIndex = 136;
        }


        public void GenerarAleatori(int valor)
        {
            Random r = new Random();
            valorsFigures = new int[valor];
            int count = 0;
            valorsReals = new int[valor];

            for (int i = 0; i < valor; i++)
            {
                valorsReals[i] = i + 1;
            }
            while (count < valor)
            {
                int aux = r.Next(1, valor + 1);
                if (!valorsFigures.Contains(aux))
                {
                    valorsFigures[count] = aux;
                    count++;
                }
            }
        }
        
        
        public void CrearRectangles()
        {
            Border border;

            Rectangle rectangle;
            elements = new List<Border>();
            rectangles = new List<Rectangle>();
            for (int i = 0; i < valorsFigures.Length; i++)
            {
                rectangle = new Rectangle();

                border = new Border();
                border.Child = rectangle;
                if (posintercanvis[i] != 0 && chk_intercanvi.IsChecked == true)
                    rectangle.Fill = ObteColor(cb_intercanviar.SelectedItem);
                else if (valorsFigures[i] == valorsReals[i])
                    rectangle.Fill = ObteColor(cb_correcte.SelectedItem);
                else
                    rectangle.Fill = ObteColor(cb_incorrecte.SelectedItem);

                Thickness br = new Thickness(ngruix);
                border.BorderThickness = br;
                border.BorderBrush = Brushes.Black;
                CornerRadius cr = new CornerRadius(nradi);

                border.CornerRadius = cr;
                rectangle.RadiusX = nradi;
                rectangle.RadiusY = nradi;
                rectangle.Width = ((canvas.ActualWidth - ngruix * ngruix) / numlinies);
                rectangle.Height = ((canvas.ActualHeight - ngruix) * valorsFigures[i] / numlinies);
                Canvas.SetLeft(border, canvas.ActualWidth * i / numlinies);
                Canvas.SetBottom(border, 0);
                canvas.Children.Add(border);
                elements.Add(border);
                rectangles.Add(rectangle);
            }
        }
        
        
        public void CrearPunts()
        {
            Border border;
            Ellipse point;
            elements = new List<Border>();
            ellipses = new List<Ellipse>();

            for (int i = 0; i < valorsFigures.Length; i++)
            {
                point = new Ellipse();
                border = new Border();
                border.Child = point;
                CornerRadius cr = new CornerRadius(1000);

                border.CornerRadius = cr;
                if (posintercanvis[i] != 0 && chk_intercanvi.IsChecked == true)
                    point.Fill = ObteColor(cb_intercanviar.SelectedItem);
                else if (valorsFigures[i] == valorsReals[i])
                    point.Fill = ObteColor(cb_correcte.SelectedItem);
                else
                    point.Fill = ObteColor(cb_incorrecte.SelectedItem);

                Thickness br = new Thickness(ngruix);
                border.BorderThickness = br;
                border.BorderBrush = Brushes.Black;

                point.Width = ((canvas.ActualWidth - ngruix * ngruix) / numlinies);
                point.Height = ((canvas.ActualWidth - ngruix * ngruix) / numlinies);
                Canvas.SetLeft(border, canvas.ActualWidth * i / numlinies);
                Canvas.SetBottom(border, ((canvas.ActualHeight - (ngruix + point.Height)) * valorsFigures[i] / numlinies));
                canvas.Children.Add(border);
                elements.Add(border);
                ellipses.Add(point);
            }
        }


        private void mn_genera_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            posintercanvis = new int[numlinies];

            if (chk_aleatori.IsChecked == true)
                GenerarAleatori(numlinies);
            else
                GenerarNormal(numlinies);

            if (chk_invertit.IsChecked == true)
                Invertir(numlinies);

            Crear();
        }


        private void Crear()
        {
            canvas.Children.Clear();
            ComboBoxItem cb = (ComboBoxItem)cb_tfigures.SelectedItem;

            if (cb.Content.ToString() == "Punts")
                CrearPunts();
            else
                CrearRectangles();
        }


        private void GenerarNormal(int valor)
        {
            valorsReals = new int[valor];
            valorsFigures = new int[valor];

            for (int i = 0; i < valor; i++)
            {
                valorsReals[i] = i + 1;
            }

            for (int i = 0; i < valor; i++)
            {
                valorsFigures[i] = i + 1;
            }
        }


        private void Invertir(int valor)
        {
            int[] tempfigures = valorsFigures;
            valorsFigures = new int[valor];
            for (int i = 0; i < valor; i++)
            {
                valorsFigures[i] = tempfigures[valor - 1 - i];
            }
        }


        private void mn_ordena_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)cb_ordenacio.SelectedItem;

            if (valorsFigures.Length != 0)
            {
                while (!CheckOrdenat())
                {
                    if (comboBoxItem.Content.ToString().Contains("Inserció Directa"))
                    {
                        IncercioDirecta();
                    }
                    else if (comboBoxItem.Content.ToString().Contains("Selecció Directa"))
                    {
                        SeleccioDirecta();
                    }
                    else if (comboBoxItem.Content.ToString().Contains("Bombolla Sacsejada"))
                    {
                        Sacsejada();
                    }
                    else
                    {
                        Bombolla();
                    }
                }
            }
        }


        private bool CheckOrdenat()
        {
            int count = 0;
            bool NoEstaOrdentat = false;

            while (count < numlinies && !NoEstaOrdentat)
            {
                if (valorsFigures[count] != valorsReals[count])
                {
                    NoEstaOrdentat = true;
                }
                count++;
            }
            return !NoEstaOrdentat;
        }


        private void SeleccioDirecta()
        {
            for (int i = 0; i < numlinies - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < numlinies; j++)
                {
                    Crear();
                    posintercanvis = new int[numlinies];
                    if (valorsFigures[j] < valorsFigures[min])
                    {
                        int temp = valorsFigures[j];
                        valorsFigures[j] = valorsFigures[min];
                        valorsFigures[min] = temp;
                        posintercanvis[min] = 1;
                        posintercanvis[j] = 1;
                    }
                    Espera(tpausa);
                    DoEvents();
                }
            }
            posintercanvis = new int[numlinies];
            Crear();
        }


        private void Sacsejada()
        {
            bool EsIntercanvi = true;
            int inici = 0;
            int final = numlinies;
            while (EsIntercanvi == true)
            {
                EsIntercanvi = false;

                for (int i = inici; i < final - 1; ++i)
                {
                    Crear();
                    posintercanvis = new int[numlinies];
                    if (valorsFigures[i] > valorsFigures[i + 1])
                    {
                        int temp = valorsFigures[i];
                        valorsFigures[i] = valorsFigures[i + 1];
                        valorsFigures[i + 1] = temp;
                        EsIntercanvi = true;
                    }
                    posintercanvis[i] = 1;
                    posintercanvis[i + 1] = 1;
                    Espera(tpausa);
                    DoEvents();
                }

                final = final - 1;

                if (EsIntercanvi != false)
                {
                    EsIntercanvi = false;
                    for (int i = final - 1; i >= inici; i--)
                    {
                        Crear();
                        posintercanvis = new int[numlinies];
                        if (valorsFigures[i] > valorsFigures[i + 1])
                        {
                            int temp = valorsFigures[i];
                            valorsFigures[i] = valorsFigures[i + 1];
                            valorsFigures[i + 1] = temp;
                            EsIntercanvi = true;
                        }
                        posintercanvis[i] = 1;
                        posintercanvis[i + 1] = 1;
                        Espera(tpausa);
                        DoEvents();
                    }
                }
                inici = inici + 1;
            }
            posintercanvis = new int[numlinies];
            Crear();
        }


        private void IncercioDirecta()
        {
            int temp;
            int j;

            for (int i = 0; i < numlinies; i++)
            {
                temp = valorsFigures[i];
                j = i - 1;
                while (j >= 0 && valorsFigures[j] > temp)
                {
                    Crear();
                    posintercanvis = new int[numlinies];
                    valorsFigures[j + 1] = valorsFigures[j];
                    posintercanvis[j] = 1;
                    posintercanvis[j + 1] = 1;
                    j--;

                    Espera(tpausa);
                    DoEvents();
                }
                Crear();
                posintercanvis = new int[numlinies];
                posintercanvis[i] = 1;
                posintercanvis[j + 1] = 1;
                valorsFigures[j + 1] = temp;
                Espera(tpausa);
                DoEvents();
            }
            posintercanvis = new int[numlinies];
            Crear();
        }


        private void Bombolla()
        {
            int temp;
            for (int i = 1; i < numlinies; i++)
                for (int j = numlinies - 1; j >= i; j--)
                {
                    Crear();
                    posintercanvis = new int[numlinies];
                    if (valorsFigures[j - 1] > valorsFigures[j])
                    {
                        temp = valorsFigures[j - 1];
                        valorsFigures[j - 1] = valorsFigures[j];
                        valorsFigures[j] = temp;
                    }
                    posintercanvis[j] = 1;
                    posintercanvis[j - 1] = 1;

                    Espera(tpausa);
                    DoEvents();
                }
            posintercanvis = new int[numlinies];
            Crear();
        }


        #region Threads

        Thread thread;

        private void Espera(double milliseconds)
        {
            var frame = new DispatcherFrame();
            thread = new Thread((ThreadStart)(() =>
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(milliseconds));
                frame.Continue = false;
            }));
            thread.Start();
            Dispatcher.PushFrame(frame);
        }

        static Action action;

        public static void DoEvents()
        {
            action = new Action(delegate { });
            Application.Current?.Dispatcher?.Invoke(
               System.Windows.Threading.DispatcherPriority.Background,
               action);
        }


        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Dispatcher.InvokeShutdown();
            thread?.Abort();
            base.OnClosed(e);
        }

        #endregion


        private void mn_atura_Click(object sender, RoutedEventArgs e)
        {
            OnClosed(e);
        }


        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Crear();
        }


        private void mn_hamburguer_Click(object sender, RoutedEventArgs e)
        {
            sp_configuracio.Visibility = Visibility.Visible;
            dp_main.Opacity = 0.5;

            mn_atura.IsEnabled = false;
            mn_genera.IsEnabled = false;
            mn_ordena.IsEnabled = false;
        }


        private void dp_main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sp_configuracio.Visibility = Visibility.Collapsed;
            dp_main.Opacity = 1;
            mn_atura.IsEnabled = true;
            mn_genera.IsEnabled = true;
            mn_ordena.IsEnabled = true;
        }


        private void iupd_nelements_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (iupd_nelements.Text != "")
            {

                if (Convert.ToInt32(iupd_nelements.Text) <= 0)
                    numlinies = 1;
                else if (Convert.ToInt32(iupd_nelements.Text) > 250)
                    numlinies = 250;
                else
                    numlinies = Convert.ToInt32(iupd_nelements.Text);
            }
            else
                iupd_nelements.Text = "0";
        }


        private void iupd_gruix_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (iupd_gruix.Text != "")
            {
                if (Convert.ToInt32(iupd_gruix.Text) < 0)
                    ngruix = 0;
                else if (Convert.ToInt32(iupd_gruix.Text) > 10)
                    ngruix = 10;
                else
                    ngruix = Convert.ToInt32(iupd_gruix.Text);
            }
            else
                iupd_gruix.Text = "0";

        }
        
        
        public Brush ObteColor(object colorPropietat)
        {
            if (colorPropietat != null)
            {
                PropertyInfo property = (PropertyInfo)colorPropietat;
                Color color = (Color)property.GetValue(null, null);
                return new SolidColorBrush(color);
            }
            else
                return new SolidColorBrush(Colors.Green);
        }
        
        
        private void iupd_radi_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (iupd_radi.Text != "")
            {
                if (Convert.ToInt32(iupd_radi.Text) < 0)
                    nradi = 0;
                else if (Convert.ToInt32(iupd_radi.Text) > 100)
                    nradi = 100;
                else
                    nradi = Convert.ToInt32(iupd_radi.Text);
            }
            else
                iupd_radi.Text = "0";
        }

        
        private void iupd_tpausa_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (iupd_tpausa.Text != "")
            {
                if (Convert.ToDouble(iupd_tpausa.Text) < 0)
                    tpausa = 0;
                else if (Convert.ToDouble(iupd_tpausa.Text) > 5000)
                    tpausa = 100;
                else
                    tpausa = Convert.ToDouble(iupd_tpausa.Text);
            }
            else
                iupd_tpausa.Text = "0";
        }

        
        private void cb_fons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Brush brush = ObteColor(cb_fons.SelectedItem);
            canvas.Background = brush;
        }
    }
}
