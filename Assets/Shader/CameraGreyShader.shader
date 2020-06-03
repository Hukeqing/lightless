Shader "CameraGrey/CameraGreyShader"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_GreyTex ("Grey (RGB)", 2D) = "white" {}
		_ColorGreyRange("Grey Range", float) = 0.2
		_ColorMixRange("Mix Range", float) = 0.2
		_ColorReRange("ReColor Range", float) = 0
		_ColorStop("Stop", float) = 1
	}
	SubShader 
	{
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
            #pragma multi_compile_fog

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _GreyTex;
			uniform fixed4 _Center;
			uniform fixed _ColorGreyRange;
			uniform fixed _ColorMixRange;
			uniform fixed _ColorReRange;
			uniform fixed _ColorStop;

			fixed4 frag(v2f_img i) : COLOR
			{
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				fixed4 iceTex = tex2D(_GreyTex, i.uv);

			    float aspect = _ScreenParams.x / _ScreenParams.y;
                float max_d = sqrt((0.5 * aspect) * (0.5 * aspect) + 0.5 * 0.5);
                
			    float x = (i.uv.x - 0.5f) * aspect;
                float y = i.uv.y - 0.5f;
                float d = sqrt(x * x + y * y);

                float half_theta = asin(clamp(y / d, -1, 1)) * (step(0, x) - 0.5);

                float deformFactor = (1 + 0.1 * sin(half_theta * 24) * lerp(0, 0.5, sin(UNITY_TWO_PI * _Time.y * 0.5))
                                   + 0.25 * x * sin(1 + half_theta * 6.5) * lerp(0.25, 0.75, sin(UNITY_TWO_PI * _Time.y * 0.2))
                                   + 0.1 * x * x * sin(2 + half_theta * 9.5) * lerp(0.25, 0.75, sin(UNITY_TWO_PI * _Time.y * 0.1))
                                   );
                _ColorGreyRange *= deformFactor;

                half power = 20;
                fixed4 resColor = 1;
                d /= max_d;

				if (d < _ColorGreyRange) {
				// 内核
				    float r = _ColorGreyRange;
                    float t = saturate(d / r);
                    fixed4 rim = lerp(0, r, pow(t, power));
                    resColor = renderTex + rim * rim.a;
				} else if (d < _ColorGreyRange + _ColorMixRange) {
				// 外壳
				    float r = _ColorGreyRange + _ColorMixRange;
				    float _LuminosityAmount = d / r;
				    resColor = lerp(renderTex, iceTex, _LuminosityAmount);
				} else {
				// 外层
				    resColor = iceTex;
				}

				float luminosity = 0.299 * resColor.r + 0.587 * resColor.g + 0.114 * resColor.b;
				return lerp(lerp(resColor, 1 - resColor, step(d, _ColorReRange)), resColor, step(0, _ColorStop));
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
