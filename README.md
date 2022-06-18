Imagin.Core.Color ![](https://img.shields.io/badge/style-7.1-blue.svg?style=flat&label=Version)
---
Color management for shared projects.

Color models
---
## 3D
Preview	    | Name        | Author      | Year       | Accuracy (%)  |
------------|-------------|-------------|------------|---------------|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/CMY.png?raw=true" width="128" />	| **CMY**	<p>*Cyan, Magenta, Yellow*</p>			| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HCV.png?raw=true" width="128" />	| **HCV**	<p>*Hue, Chroma, Gray*</p>				| -		| -		| 98.379	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HCY.png?raw=true" width="128" />	| **HCY**	<p>*Hue, Chroma, Luminance*</p>			| Kuzma Shapran | -		| 99.3 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HPLuv.png?raw=true" width="128" />	| **HPL<sub>uv</sub>**	<p>*Hue, Saturation, Lightness*</p>		| -		| -		| 69.233	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HSB.png?raw=true" width="128" />	| **HSB**	<p>*Hue, Saturation, Brightness*</p>	| [PARC](https://www.parc.com/)/[NYIT](https://www.nyit.edu/) | mid-1970s | 76.778 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HSBk.png?raw=true" width="128" />	| **HSB<sub>k</sub>** (HSV<sub>ok</sub>)	<p>*Hue, Saturation, Brightness*</p>	| [Björn Ottosson](https://bottosson.github.io/) | -		| 0 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HSL.png?raw=true" width="128" />	| **HSL**	<p>*Hue, Saturation, Lightness*</p>		| [Georges Valensi](https://en.wikipedia.org/wiki/Georges_Valensi) | 1938 | 100 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HSLk.png?raw=true" width="128" />	| **HSL<sub>k</sub>** (HSL<sub>ok</sub>)	<p>*Hue, Saturation, Lightness*</p>		| [Björn Ottosson](https://bottosson.github.io/) | -		| 0 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HSLuv.png?raw=true" width="128" />	| **HSL<sub>uv</sub>**	<p>*Hue, Saturation, Lightness*</p>		| -		| -		| 69.233	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HSM.png?raw=true" width="128" />	| **HSM**	<p>*Hue, Saturation, Mixture*</p>		| Osvaldo Severino Jr/Adilson Gonzaga | 2009 | 62.752 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HSP.png?raw=true" width="128" />	| **HSP**	<p>*Hue, Saturation, Brightness*</p>	| 		| 		| 99.815	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/HWB.png?raw=true" width="128" />	| **HWB**	<p>*Hue, Whitness, Blackness*</p>		| [Alvy Ray Smith](https://en.wikipedia.org/wiki/Alvy_Ray_Smith) | 1996 | 76.778 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/IPT.png?raw=true" width="128" />	| **IPT**	<p>*Intensity, Cyan/red, Blue/yellow*</p>		| Ebner/Fairchild | 1998 | 74.537 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/JPEG.png?raw=true" width="128" />	| **JPEG**	<p>*Luminance, Cb, Cr*</p>				| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/Lab.png?raw=true" width="128" />	| **L\*a\*b\***	<p>*Luminance, Chroma, Chroma*</p>		| [CIE](https://cie.co.at/)	| 1976	| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/Labh.png?raw=true" width="128" />	| **Lab<sub>h</sub>** (Hunter Lab)	<p>*Luminance, Chroma, Chroma*</p>		| [Richard S. Hunter](https://en.wikipedia.org/wiki/Richard_S._Hunter) | 1948 | 100 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/Labi.png?raw=true" width="128" />	| **Lab<sub>i</sub>** (OSA-UCS)	<p>*Luminance, Chroma, Chroma*</p>		| [OSA](https://www.optica.org/en-us/home/)	| 1947	| 50		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/Labj.png?raw=true" width="128" />	| **Lab<sub>j</sub>** (J<sub>z</sub>A<sub>z</sub>b<sub>z</sub>)	<p>*Luminance, Chroma, Chroma*</p>		| Safdar & al. | 2017 | 100 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/Labk.png?raw=true" width="128" />	| **Lab<sub>k</sub>** (OKL<sub>ab</sub>)	<p>*Luminance, Chroma, Chroma*</p>		| [Björn Ottosson](https://bottosson.github.io/) | -		| 0 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/LCHab.png?raw=true" width="128" />	| **LCH<sub>ab</sub>**	<p>*Luminance, Chroma, Hue*</p>			| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/LCHabh.png?raw=true" width="128" />| **LCH<sub>ab*h*</sub>**<p>*Luminance, Chroma, Hue*</p>			| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/LCHabj.png?raw=true" width="128" />| **LCH<sub>ab*j*</sub>** (J<sub>z</sub>C<sub>z</sub>h<sub>z</sub>)<p>*Luminance, Chroma, Hue*</p>			| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/LCHuv.png?raw=true" width="128" />	| **LCH<sub>uv</sub>**	<p>*Luminance, Chroma, Hue*</p>			| -		| -		| 67.628	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/LCHxy.png?raw=true" width="128" />	| **LCH<sub>xy</sub>**	<p>*Luminance, Chroma, Hue*</p>			| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/LMS.png?raw=true" width="128" />	| **LMS**	<p>*Long, Medium, Short*</p>			| Stockman/Sharpe | 2000 | 100 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/Luv.png?raw=true" width="128" />	| **L\*u\*v\***	<p>*Luminance, Chroma, Chroma*</p>		| [CIE](https://cie.co.at/) 	| 1976	| 67.628	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/RGB.png?raw=true" width="128" />	| **RGB**	<p>*Red, Green, Blue*</p>				| [CIE](https://cie.co.at/)	| 1931	| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/rgG.png?raw=true" width="128" />	| **rgG**	<p>*Chroma, Chroma, Luminance*</p>		| -		| -		| -			|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/RYB.png?raw=true" width="128" />	| **RYB**	<p>*Red, Yellow, Blue*</p>				| -		| -		| 59.122	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/TSL.png?raw=true" width="128" />	| **TSL**	<p>*Tint, Saturation, Lightness*</p>	| Jean-Christophe Terrillon/Shigeru Akamatsu | - | 36.884 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/UCS.png?raw=true" width="128" />	| **UCS**	<p>*U, V, W*</p>						| [CIE](https://cie.co.at/)	| 1960	| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/UVW.png?raw=true" width="128" />	| **U\*V\*W\***	<p>*U, V, W*</p>						| [CIE](https://cie.co.at/)	| 1964	| 56.288	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/xvYCC.png?raw=true" width="128" />	| **xvYCC**	<p>*Luminance, Cb, Cr*</p>				| Sony	| 2005	| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/xyY.png?raw=true" width="128" />	| **xyY**	<p>*Chroma, Chroma, Luminance*</p>		| [CIE](https://cie.co.at/)	| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/xyYC.png?raw=true" width="128" />	| **xyYC** (Coloroid)	<p>*Chroma, Chroma, Luminance*</p>						| [Antal Nemcsics](http://www.aicecd.org/index.php?article_id=137&clang=2) | 1962-1980 | 0 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/XYZ.png?raw=true" width="128" />	| **XYZ**	<p>*X, Y, Z*</p>						| [CIE](https://cie.co.at/)	| 1931	| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/YCbCr.png?raw=true" width="128" />	| **YC<sub>b</sub>C<sub>r</sub>**	<p>*Luminance, C<sub>b</sub>, C<sub>r</sub>*</p>				| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/YCoCg.png?raw=true" width="128" />	| **YC<sub>o</sub>C<sub>g</sub>**	<p>*Luminance, C<sub>o</sub>, C<sub>g</sub>*</p>				| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/YDbDr.png?raw=true" width="128" />	| **YD<sub>b</sub>D<sub>r</sub>**	<p>*Luminance, D<sub>b</sub>, D<sub>r</sub>*</p>				| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/YES.png?raw=true" width="128" />	| **YES**	<p>*Luminance, E-factor, S-factor*</p>	| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/YIQ.png?raw=true" width="128" />	| **YIQ**	<p>*Luminance, Intensity, Q*</p>		| -		| -		| 99.765	|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/YPbPr.png?raw=true" width="128" />	| **YP<sub>b</sub>P<sub>r</sub>**	<p>*Luminance, P<sub>b</sub>, P<sub>r</sub>*</p>				| -		| -		| 100		|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/YUV.png?raw=true" width="128" />	| **YUV**	<p>*Luminance, U, V*</p>				| -		| -		| 99.886	|

## 4D
Preview	    | Name        | Accuracy (%)  |
------------|-------------|---------------|
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/CMYK.png?raw=true" width="128" /> | **CMYK**	<p>*Cyan, Magenta, Yellow, Black*</p>	| 100 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/CMYW.png?raw=true" width="128" /> | **CMYW**	<p>*Cyan, Magenta, Yellow, White*</p>	| 100 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/RGBK.png?raw=true" width="128" /> | **RGBK**	<p>*Red, Green, Blue, Black*</p>		| 100 |
<img src="https://github.com/imagin-tech/Core.Color/blob/main/Images/RGBW.png?raw=true" width="128" /> | **RGBW**	<p>*Red, Green, Blue, White*</p>		| 100 |


[Nuget](https://www.nuget.org/packages/Imagin.Core.Color/)
---
### Quick install
##### Imagin.Core.Color
> _`Install-Package Imagin.Core.Color -Version *.0.0`_

[Learn more...](https://github.com/imagin-tech/Core.Color/wiki)

Wiki ![](https://img.shields.io/badge/style-Coming%20soon!-red.svg?style=flat&label=)
---
For help or to find out more, head over to [the wiki](https://github.com/imagin-tech/Core.Color/wiki).

Donate
---
[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AJJG6PWLBYQNG)