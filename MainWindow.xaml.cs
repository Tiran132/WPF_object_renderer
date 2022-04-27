using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace Mover
{
    public class Cordinate
    {
        public double x { get; set; } = 0;
        public double y { get; set; } = 0;
    }

    public partial class MainWindow : Window
    {
        private DispatcherTimer mainTimer = new DispatcherTimer();
        private TimeSpan frame_duration = TimeSpan.FromMilliseconds(100);
        private int frame_index = 0;

        private List<Cordinate> cords = new List<Cordinate>();
        private Cordinate zero_cordinate = new Cordinate { x = 0, y = 0 };

        private double current_scale = 1;
        private double current_translate_X = 0;
        private double current_translate_Y = 0;

        public MainWindow()
        {
            InitializeComponent();
            zero_cordinate = new Cordinate { x = Canvas.GetLeft(main_obj), y = Canvas.GetTop(main_obj) };
            
            FillArray();

            TimeSlider.Maximum = cords.Count;

            MyCanvas.Focus();
            mainTimer.Tick += OnUpdateEvent;
            mainTimer.Interval = frame_duration;
            mainTimer.Start();
        }

        // Генератор кординат (пока не понятно как именно будем передавать координаты)
        private void FillArray()
        {
            double a = 0.5;
            double speral_l = 1;
            for (double i = 0; i < 100; i += a)
            {
                speral_l += 0.008;
                cords.Add(new Cordinate { x = Math.Cos(i)*speral_l, y = Math.Sin(i)*speral_l });

            }
            cords.Add(new Cordinate { x = 5, y = -3});
            cords.Add(new Cordinate { x = 6, y = -3.25 });
            cords.Add(new Cordinate { x = 6.2, y = -3.5 });

            double _y = -3.5;
            double k = 0;
            for (double i = 6.2; i > -6; i -= (0.8 - k))
            {
                k += 0.001;
                _y += 0.25;
                cords.Add(new Cordinate { x = i, y = _y});

            }
            for (int i = 0; i < 4; i++)
            {
                cords.Add(new Cordinate { x = -6, y = 0});
            }

            for (double i = -6; i < 0; i += 0.08)
            {
                cords.Add(new Cordinate { x = i, y = Math.Sin(i)*2 });
            }
            cords.Add(new Cordinate { x = 0, y = 0 });


        }


        // Создание плавной анимации между двумя кадрами
        private void OnUpdateEvent(object sender, EventArgs e)
        {
            if (frame_index >= cords.Count)
            {
                return;
            }

            float gridLength = 50;
            var x = Canvas.GetLeft(main_obj);
            var y = Canvas.GetTop(main_obj);


            DoubleAnimation obj_left_animation = new DoubleAnimation();
            obj_left_animation.From = x;
            obj_left_animation.To = cords[frame_index].x * gridLength + zero_cordinate.x;
            obj_left_animation.Duration = frame_duration;

            main_obj.BeginAnimation(Canvas.LeftProperty, obj_left_animation);



            DoubleAnimation obj_top_animation = new DoubleAnimation();
            obj_top_animation.From = y;
            obj_top_animation.To = cords[frame_index].y * -gridLength + zero_cordinate.y;
            obj_top_animation.Duration = frame_duration;

            main_obj.BeginAnimation(Canvas.TopProperty, obj_top_animation);


            frame_index += 1;
            TimeSlider.Value = frame_index;
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P:
                    current_scale += 0.1;
                    break;

                case Key.M:
                    current_scale -= 0.1;
                    break;

                case Key.Up:
                    current_translate_Y -= 50;
                    break;

                case Key.Down:
                    current_translate_Y += 50;
                    break;

                case Key.Left:
                    current_translate_X -= 50;
                    break;

                case Key.Right:
                    current_translate_X += 50;
                    break;
            }

            var tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform(current_scale, current_scale));

            main_obj.RenderTransform = new TranslateTransform(current_translate_X, current_translate_Y);
            MyCanvas.LayoutTransform = tg;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            frame_index = (int)e.NewValue;
        }
    }
}
