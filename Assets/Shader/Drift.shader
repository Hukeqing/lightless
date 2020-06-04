Shader "CameraGrey/Drift"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _TwistIntensity("_TwistIntensity", float) = 0.1
		_ColorReRange("Grey Range", float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _NoiseTex;
            uniform float _TwistIntensity;
			uniform fixed _ColorReRange;
            
            sampler2D _MainTex;

            float random() {
                return frac(sin(dot(_Time.xy, fixed2(12.9898, 78.233))) * 43758.5453123);
            }

            float3 RGB2HSV(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float3 HSV2RGB(float3 c)
            {
                    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                    return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            fixed4 frag (v2f_img i) : COLOR
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;
                
                float max_d = sqrt((0.5 * aspect) * (0.5 * aspect) + 0.5 * 0.5);
                
			    float x = (i.uv.x - 0.5f) * aspect;
                float y = i.uv.y - 0.5f;
                float d = sqrt(x * x + y * y);

                d /= max_d;
                
                fixed4 basicColor = tex2D(_MainTex, i.uv);
                float twistIntensity = _TwistIntensity;
                float4 noise = tex2D(_NoiseTex, i.uv + random() * _Time.y);

                fixed4 twistedColor = tex2D(_MainTex, i.uv + noise.xy * twistIntensity);
                
                float3 hsvColor = RGB2HSV(twistedColor.rgb);
                hsvColor.x += lerp(0,0.2,sin(UNITY_TWO_PI * frac(_Time.y * 0.5)));
                hsvColor.x = frac(hsvColor.x);
                fixed4 reversedColor = fixed4(0, 0, 0, 0);
                reversedColor.rgb = HSV2RGB(hsvColor.rgb);
                return lerp(basicColor, reversedColor, step(d, _ColorReRange));
            }
            ENDCG
        }
    }
}
