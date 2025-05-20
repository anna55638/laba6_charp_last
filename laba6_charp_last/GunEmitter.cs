using laba6_charp_last;
using System.Drawing.Drawing2D;
using System.Drawing;
using System;

public class GunEmitter : Emitter
{
    private int fireTickCounter = 0;
    private bool forceResetCounter = false;

    public float X;
    public float Y;
    public float Direction = 0;
    public int Spreading = 10;
    public int SpeedMin = 5;
    public int SpeedMax = 5;
    public int RadiusMin = 2;
    public int RadiusMax = 5;
    public Color ColorFrom = Color.Yellow;
    public Color ToColor = Color.FromArgb(0, Color.Orange);
    public static Image GunImage;
    public int FireRate = 2;
    private bool _isUpgraded = false;
    private DateTime upgradeEndTime;
    private int originalFireRate;

    private int originalSpeedMin;
    private int originalSpeedMax;

    private const int GunLength = 34; 
    private const int GunWidth = 35;

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

    public void ResetParticle(Particle particle, float? customDirection = null)
    {
        particle.Life = 300;

        float direction = customDirection ?? Direction;
        double angle = direction * Math.PI / 180;
        float offsetX = (float)(GunLength * Math.Cos(angle));
        float offsetY = -(float)(GunLength * Math.Sin(angle));

        particle.X = X + offsetX;
        particle.Y = Y + offsetY;
        particle.Radius = 6;

        // Выбираем скорость в зависимости от улучшения
        float speed;
        if (_isUpgraded)
        {
            speed = customDirection.HasValue
                ? Particle.rand.Next(SpeedMin + 2, SpeedMax + 5) // Для боковых пуль
                : Particle.rand.Next(SpeedMin + 5, SpeedMax + 10); // Для центральной пули
        }
        else
        {
            speed = Particle.rand.Next(SpeedMin, SpeedMax);
        }

        particle.SpeedX = (float)(Math.Cos(angle) * speed);
        particle.SpeedY = -(float)(Math.Sin(angle) * speed);
    }


    public void UpdateState()
    {
        base.UpdateState();

        // Обновляем состояние улучшения
        UpdateUpgradeState();

        fireTickCounter++;
        if (fireTickCounter >= FireRate)
        {
            fireTickCounter = 0;

            // Всегда создаем центральную пулю
            var centralParticle = CreateParticle() as ParticleColorful;
            ResetParticle(centralParticle);
            particles.Add(centralParticle);

            if (IsUpgraded)
            {
                // Создаем дополнительные пули под углом
                var leftParticle = CreateParticle() as ParticleColorful;
                ResetParticle(leftParticle, Direction + 25);
                leftParticle.FromColor = Color.Cyan;
                particles.Add(leftParticle);

                var rightParticle = CreateParticle() as ParticleColorful;
                ResetParticle(rightParticle, Direction - 25);
                rightParticle.FromColor = Color.Cyan;
                particles.Add(rightParticle);
            }
        }
    }

    public void DrawGun(Graphics g)
    {
        GraphicsState state = g.Save();
        try
        {
            // Корректируем позицию для правильного вращения
            g.TranslateTransform(X, Y);
            g.RotateTransform(-Direction + 90);

            // Рисуем изображение с учетом центра вращения
            g.DrawImage(
                GunImage,
                -GunWidth,  // Смещаем изображение так, чтобы центр вращения был у основания
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

    private void UpdateUpgradeState()
    {
        if (_isUpgraded && DateTime.Now > upgradeEndTime)
        {
            _isUpgraded = false;
            SpeedMin = originalSpeedMin;
            SpeedMax = originalSpeedMax;
        }
    }


    private void CreateBullet(float direction)
    {
        var particle = CreateParticle() as ParticleColorful;

        // Устанавливаем позицию пули с учетом направления
        double angle = direction * Math.PI / 180;
        float offsetX = (float)(GunLength * Math.Cos(angle));
        float offsetY = -(float)(GunLength * Math.Sin(angle));

        particle.X = X + offsetX;
        particle.Y = Y + offsetY;
        particle.Radius = 6;
        particle.Life = 300;

        // Выбираем скорость в зависимости от типа пули
        float speed;
        if (_isUpgraded && direction != Direction) // Для боковых пуль
        {
            speed = Particle.rand.Next(SpeedMin + 2, SpeedMax + 5); // Можно задать другую скорость
            particle.FromColor = Color.Cyan;
        }
        else // Для центральной пули
        {
            speed = _isUpgraded
                ? Particle.rand.Next(SpeedMin + 5, SpeedMax + 10)
                : Particle.rand.Next(SpeedMin, SpeedMax);
        }

        // Правильно рассчитываем направление скорости
        particle.SpeedX = (float)(Math.Cos(angle) * speed);
        particle.SpeedY = -(float)(Math.Sin(angle) * speed);

        particles.Add(particle);
    }

    public void ActivateUpgrade()
    {
        if (!_isUpgraded)
        {
            originalSpeedMin = SpeedMin;
            originalSpeedMax = SpeedMax;
            _isUpgraded = true;
            upgradeEndTime = DateTime.Now.AddSeconds(10);
        }
    }
}