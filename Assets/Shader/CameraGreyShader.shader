Shader "CameraGrey/CameraGreyShader"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GreyTex ("Grey (RGB)", 2D) = "white" {}
		_ColorGreyRange("Grey Range", float) = 0.2
		_ColorMixRange("Mix Range", float) = 0.2
	}
	SubShader 
	{
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			// WARNING FOR NEXT LINE
            #pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _GreyTex;
			uniform fixed4 _Center;
			uniform fixed _ColorGreyRange;
			uniform fixed _ColorMixRange;
			
			fixed dist(half2 a) {
			    return sqrt((a.x - 0.5) * (a.x - 0.5) + (a.y - 0.5) * (a.y - 0.5));
			}
			
			fixed4 frag(v2f_img i) : COLOR
			{
				fixed distance = dist(i.uv);
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed4 greyTex = tex2D(_GreyTex, i.uv);
				float luminosity = 0.299 * renderTex.r + 0.587 * renderTex.g + 0.114 * renderTex.b;
				
				if (distance < _ColorGreyRange - _ColorMixRange) {
				    float _LuminosityAmount = distance / (_ColorGreyRange - _ColorMixRange);
				    fixed4 finalColor = lerp(renderTex, luminosity, _LuminosityAmount);
				    return finalColor;
				} else if (distance < _ColorGreyRange) {
				    float _LuminosityAmount = (distance - _ColorGreyRange + _ColorMixRange) / _ColorMixRange;
				    // fixed4 greyColor = _LuminosityAmount;
				    return lerp(luminosity, greyTex, _LuminosityAmount);
				} else {
				    return greyTex;
				}
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
