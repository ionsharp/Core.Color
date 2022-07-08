using Imagin.Core.Numerics;
using static Imagin.Core.Numerics.M;
using static System.Math;

namespace Imagin.Core.Colors;

public abstract partial class CAM02
{
    /// <summary><see cref="CAM02"/> viewing conditions.</summary>
    public struct ViewingConditions
    {
        #region Properties

        public const double DefaultAbsoluteLuminance = 4;   //4 cd/m^2 (ambient illumination of 64 lux)

        public const double DefaultRelativeLuminance = 20;  //20% gray

        //...

        [Horizontal]
        public Surrounds Surround { get; private set; } = Surrounds.Average;

        [Hidden]
        public readonly double F = 1;          //Average

        [Hidden]
        public readonly double c = 0.690;      //Average

        [Hidden]
        public readonly double Nc = 1;         //Average

        //...

        /// <summary>Achromatic response to white.</summary>
        [Hidden]
        public readonly double Aw = 0;

        /// <summary>Degree of adaptation (discounting)</summary>
        [Hidden]
        public readonly double D = 0;

        /// <summary>Luminance level adaptation factor</summary>
        [Hidden]
        public readonly double FL = 0;

        [Hidden]
        public readonly double n = 0, Nbb = 0, Ncb = 0, z = 0;

        [DisplayName("Absolute luminance of adapting field")]
        public double LA { get; private set; } = DefaultAbsoluteLuminance;

        [DisplayName("Relative luminance of background")]
        [Range(.0, 100.0, 1.0)]
        public double Yb { get; private set; } = DefaultRelativeLuminance;

        #endregion

        #region ViewingConditions

        public ViewingConditions() : this((XYZ)(xyY)(xy)WorkingProfile.DefaultWhite) { }

        public ViewingConditions(Vector3 white) : this(Surrounds.Average, DefaultAbsoluteLuminance, DefaultRelativeLuminance, white) { }

        public ViewingConditions(Surrounds input, double LA, double Yb, Vector3 white)
        {
            Surround = input;
            switch (Surround)
            {
                case Surrounds.Average:
                    F = 1.000; c = 0.690; Nc = 1.000;
                    break;
                case Surrounds.Dim:
                    F = 0.900; c = 0.590; Nc = 0.900;
                    break;
                case Surrounds.Dark:
                    F = 0.800; c = 0.525; Nc = 0.800;
                    break;
            }

            this.LA = LA; this.Yb = Yb;

            n = Compute_n(Yb, white.Y); z = Compute_z(n);
            Nbb = Compute_Nbb(n); Ncb = Nbb;

            D = Compute_D(F, LA); FL = Compute_FL(LA);

            Aw = Compute_Aw(white, D, FL, Nbb);
        }

        #endregion

        #region Methods

        static double Compute_Aw(Vector3 white, double D, double FL, double Nbb)
        {
            double r = 0, g = 0, b = 0;
            double rc, gc, bc;

            double rp = 0, gp = 0, bp = 0;
            double rpa, gpa, bpa;

            var rgb = ChromaticAdaptationTransform.CAT02 * new Vector(white.X * 100, white.Y * 100, white.Z * 100);
            r = rgb[0]; g = rgb[1]; b = rgb[2];

            rc = r * (((white.Y * 100 * D) / r) + (1.0 - D));
            gc = g * (((white.Y * 100 * D) / g) + (1.0 - D));
            bc = b * (((white.Y * 100 * D) / b) + (1.0 - D));

            var rgbp = CAT02_HPE * new Vector(rc, gc, bc);
            rp = rgbp[0]; gp = rgbp[1]; bp = rgbp[2];

            rpa = NonlinearAdaptation(rp, FL);
            gpa = NonlinearAdaptation(gp, FL);
            bpa = NonlinearAdaptation(bp, FL);

            return ((2.0 * rpa) + gpa + ((1.0 / 20.0) * bpa) - 0.305) * Nbb;
        }

        /// <summary>
        /// Theoretically, <b>D</b> ranges from
        /// 
        /// <para>0 = <b>No adaptation to the adopted white point.</b></para>
        /// <para>1 = <b>Complete adaptation to the adopted white point.</b></para>
        /// 
        /// <para>In practice, the minimum <b>D</b> value will not be less than 0.65 for <see cref="Surrounds.Dark"/> and exponentially converges to 1 for <see cref="Surrounds.Average"/> with increasingly large values of L[A].</para>
        /// 
        /// <para>L[A] is the luminance of the adapting field in cd/m^2.</para>
        /// </summary>
        static double Compute_D(double F, double LA)
            => (F * (1.0 - ((1.0 / 3.6) * Exp((-LA - 42.0) / 92.0))));

        static double Compute_FL(double LA)
        {
            double k, fl;
            k = 1.0 / ((5.0 * LA) + 1.0);
            fl = 0.2 * Pow(k, 4.0) * (5.0 * LA) + 0.1 * (Pow((1.0 - Pow(k, 4.0)), 2.0)) * (Pow((5.0 * LA), (1.0 / 3.0)));
            return (fl);
        }

        static double Compute_FL_Inverse(double LA)
        {
            double la5 = LA * 5.0;
            double k = 1.0 / (la5 + 1.0);

            k = Pow4(k);
            return (0.2 * k * la5) + (0.1 * (1.0 - k) * (1.0 - k) * Pow(la5, 1.0 / 3.0));
        }

        static double Compute_n(double Yb, double Yw) => Yb / Yw * 100;

        static double Compute_Nbb(double n) => 0.725 * Pow(1.0 / n, 0.2);

        static double Compute_z(double n) => 1.48 + Pow(n, 0.5);

#endregion
    }
}