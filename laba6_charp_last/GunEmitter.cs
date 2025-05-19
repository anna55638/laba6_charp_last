using laba6_charp_last;
using System.Drawing.Drawing2D;
using System.Drawing;
using System;

public class GunEmitter : Emitter
{
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
    public int FireRate = 3;
    private bool _isUpgraded = false;
    private DateTime upgradeEndTime;
    private int originalFireRate;

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
            GunImage = new Bitmap(40, 40);
            using (var g = Graphics.FromImage(GunImage))
            {
                g.FillRectangle(Brushes.Gray, 0, 0, 40, 40);
            }
        }
    }

    public bool IsUpgraded
    {
        get { return _isUpgraded; }
        set { _isUpgraded = value; } // Теперь set доступен
    }

    public override Particle CreateParticle()
    {
        return new ParticleColorful
        {
            FromColor = ColorFrom,
            ToColor = ToColor,
            Life = 100,
            Radius = 8
        };
    }

    public override void ResetParticle(Particle particle)
    {
        particle.Life = 100;
        particle.X = X;
        particle.Y = Y - 15;
        particle.Radius = 6;

        double angle = (Direction + Particle.rand.Next(Spreading) - Spreading / 2) * Math.PI / 180;
        float speed = Particle.rand.Next(SpeedMin, SpeedMax);

        particle.SpeedX = (float)(Math.Cos(angle) * speed);
        particle.SpeedY = -(float)(Math.Sin(angle) * speed);

        this.GravitationY = 0;
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

    public void ActivateUpgrade()
    {
        if (!IsUpgraded)
        {
            originalFireRate = FireRate;
        }
        IsUpgraded = true;
        FireRate = originalFireRate; // Убираем умножение, чтобы не увеличивать скорострельность
        upgradeEndTime = DateTime.Now.AddSeconds(7);
    }

    public void UpdateUpgradeState()
    {
        if (IsUpgraded && DateTime.Now > upgradeEndTime)
        {
            IsUpgraded = false;
            FireRate = originalFireRate;
        }
    }

    private int fireTickCounter = 0;

    public void UpdateState()
    {
        base.UpdateState();

        fireTickCounter++;
        if (fireTickCounter >= FireRate)
        {
            fireTickCounter = 0;
            CreateBullet();

            if (IsUpgraded)
            {
                // Диагональные выстрелы при улучшении
                CreateBullet(Direction + 30); // Левый
                CreateBullet(Direction - 30); // Правый
            }
        }
    }

    private void CreateBullet(float directionOffset = 0)
    {
        var particle = CreateParticle();
        particle.Life = 100;
        particle.X = X;
        particle.Y = Y - 15;
        particle.Radius = 8; // Нормальный размер

        double angle = (Direction + directionOffset) * Math.PI / 180;
        float speed = Particle.rand.Next(SpeedMin, SpeedMax);

        particle.SpeedX = (float)(Math.Cos(angle) * speed);
        particle.SpeedY = -(float)(Math.Sin(angle) * speed);

        particles.Add(particle);
    }
}