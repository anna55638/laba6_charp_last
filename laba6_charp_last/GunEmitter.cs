using laba6_charp_last;
using System.Drawing.Drawing2D;
using System.Drawing;
using System;

public class GunEmitter : Emitter
{
    private int fireTickCounter = 0;
    private bool forceResetCounter = false;
    private DateTime lastShotTime = DateTime.MinValue;

    public float X;
    public float Y;
    public float Direction = 0;
    public int Spreading = 10;
    public int SpeedMin = 5;
    public int SpeedMax = 10;
    public int RadiusMin = 2;
    public int RadiusMax = 5;
    public Color ColorFrom = Color.Yellow;
    public Color ToColor = Color.FromArgb(0, Color.Orange);
    public static Image GunImage;
    public int FireRate = 20;
    private bool _isUpgraded = false;
    private DateTime upgradeEndTime;
    private int originalFireRate;

    private const float GunBarrelOffset = 25f;

    static GunEmitter()
    {
        try
        {
            var originalImage = Image.FromFile("Resources/cosmo11.png");
            GunImage = new Bitmap(originalImage, new Size(70, 60));
            originalImage.Dispose();
        }
        catch
        {
            GunImage = new Bitmap(70, 60);
            using (var g = Graphics.FromImage(GunImage))
            {
                g.FillRectangle(Brushes.Gray, 0, 0, 40, 40);
            }
        }
    }

    public bool IsUpgraded
    {
        get { return _isUpgraded; }
        set { _isUpgraded = value; }
    }

    public override Particle CreateParticle()
    {
        return new ParticleColorful
        {
            FromColor = ColorFrom,
            ToColor = ToColor,
            Life = 300,
            Radius = 6,  // Стандартный радиус
            StretchFactor = 1.1f
        };
    }

    public override void ResetParticle(Particle particle)
    {
        particle.Life = 300;
        particle.X = X;
        // Поднимаем точку вылета выше (Y - GunBarrelOffset)
        particle.Y = Y - GunBarrelOffset;
        particle.Radius = 6;

        float speed = Particle.rand.Next(8, 12);
        double angle = Direction * Math.PI / 180;
        particle.SpeedX = (float)(Math.Cos(angle) * speed);
        particle.SpeedY = -(float)(Math.Sin(angle) * speed);
    }


    public void UpdateState()
    {
        base.UpdateState();

        // Обновление состояния улучшения
        if (IsUpgraded && DateTime.Now > upgradeEndTime)
        {
            IsUpgraded = false;
            FireRate = originalFireRate;
        }

        // Логика стрельбы на основе FireRate
        fireTickCounter++;
        if (fireTickCounter >= FireRate)
        {
            fireTickCounter = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        // Основной выстрел
        CreateBullet(Direction);

        // Дополнительные выстрелы при улучшении
        if (IsUpgraded)
        {
            CreateBullet(Direction + 25);
            CreateBullet(Direction - 25);
        }
    }

    public void DrawGun(Graphics g)
    {
        GraphicsState state = g.Save();
        try
        {
            g.TranslateTransform(X, Y);
            g.RotateTransform(-Direction + 90);
            g.DrawImage(
                GunImage,
                -GunImage.Width / 2,
                -GunImage.Height / 2,
                GunImage.Width,
                GunImage.Height
            );
        }
        finally
        {
            g.Restore(state);
        }
    }

    public void UpdateUpgradeState()
    {
        if (IsUpgraded && DateTime.Now > upgradeEndTime)
        {
            IsUpgraded = false;
            FireRate = originalFireRate;
        }
    }


    private void CreateBullet(float direction)
    {
        var particle = CreateParticle() as ParticleColorful;
        particle.Life = 300;
        particle.X = X;
        particle.Y = Y - 15;
        particle.Radius = 6;

        float speed = Particle.rand.Next(8, 12);
        double angle = direction * Math.PI / 180;
        particle.SpeedX = (float)(Math.Cos(angle) * speed);
        particle.SpeedY = -(float)(Math.Sin(angle) * speed);

        if (direction != Direction && IsUpgraded)
        {
            particle.FromColor = Color.Cyan;
        }

        particles.Add(particle);
    }

    public void ActivateUpgrade()
    {
        if (!IsUpgraded)
        {
            originalFireRate = FireRate;
            FireRate = (int)(originalFireRate * 0.7f); // Уменьшаем интервал на 30%
        }
        IsUpgraded = true;
        upgradeEndTime = DateTime.Now.AddSeconds(10);
    }
}