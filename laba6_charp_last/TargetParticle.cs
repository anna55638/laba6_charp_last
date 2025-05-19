using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba6_charp_last
{
    public class TargetParticle : ParticleColorful
    {
        public int HitCount = 0;
        public int HitsToDestroy = 3;
        public static Image RocketImage; // Загружаем изображение ракеты
        private static Size rocketSize = new Size(40, 40);

        static TargetParticle()
        {
            try
            {
                // Загружаем изображение и изменяем его размер
                var originalImage = Image.FromFile("Resources/raketa.png");
                RocketImage = new Bitmap(originalImage, rocketSize);
                originalImage.Dispose(); // Освобождаем ресурсы
            }
            catch
            {
                // Создаем временное изображение, если файл не найден
                RocketImage = new Bitmap(rocketSize.Width, rocketSize.Height);
                using (var g = Graphics.FromImage(RocketImage))
                {
                    g.FillRectangle(Brushes.Red, 0, 0, rocketSize.Width, rocketSize.Height);
                }
            }
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(RocketImage,
                X - RocketImage.Width / 2,
                Y - RocketImage.Height / 2,
                RocketImage.Width,
                RocketImage.Height);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(
                $"{HitCount}/{HitsToDestroy}",
                new Font("Arial", 8),
                Brushes.White,
                X,
                Y + RocketImage.Height / 2 + 10,
                stringFormat
            );
        }
    }
}
