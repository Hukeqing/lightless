Shader "CameraGrey/CameraGreyShader"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Grey ("Object (RGBA)", Color) = (0.3, 0.3, 0.3, 1)
		_ColorGreyRange("Grey Range", float) = 0.2
	}
	SubShader 
	{
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform fixed4 _Grey;
			uniform fixed4 _Center;
			uniform fixed _ColorGreyRange;
			
			fixed dist(half2 a) {
			    return sqrt((a.x - 0.5) * (a.x - 0.5) + (a.y - 0.5) * (a.y - 0.5));
			}
			
			fixed4 frag(v2f_img i) : COLOR
			{
				fixed distance = dist(i.uv);
				if (distance > _ColorGreyRange) {
				    return _Grey;
				} else  {
				    fixed4 renderTex = tex2D(_MainTex, i.uv);
				    float luminosity = 0.299 * renderTex.r + 0.587 * renderTex.g + 0.114 * renderTex.b;
				    float _LuminosityAmount = distance / _ColorGreyRange;
				    fixed4 finalColor = lerp(renderTex, luminosity, _LuminosityAmount);
				    return finalColor;
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
