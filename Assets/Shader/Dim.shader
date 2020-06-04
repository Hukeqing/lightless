Shader "CameraGrey/Dim"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SampleStrength ("_SampleStrength", float) = 1.0
        _SampleDist ("_SampleDist", float) = 1.0
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

            sampler2D _MainTex;
            uniform float _SampleStrength;
            uniform float _SampleDist;

            fixed4 frag (v2f_img i) : SV_Target
            {
                float aspect = _ScreenParams.x / _ScreenParams.y;
                
                float max_d = sqrt((0.5 * aspect) * (0.5 * aspect) + 0.5 * 0.5);
                
			    float x = (i.uv.x - 0.5f) * aspect;
                float y = i.uv.y - 0.5f;
                float d = sqrt(x * x + y * y);

                d /= max_d;
            
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 sum = col;
                float2 dir = 0.5 - i.uv;
                float dist = length(dir);
                dir /= dist;
                float samples[10] =
                {
                    -0.08,
                    -0.05,
                    -0.03,
                    -0.02,
                    -0.01,
                    0.01,
                    0.02,
                    0.03,
                    0.05,
                    0.08
                };
                for (int it = 0; it < 10; it++) {
                    sum += tex2D(_MainTex, i.uv + dir * samples[it] * _SampleDist);
                }
                sum /= 11;
                float t = saturate(dist * _SampleStrength);
                float4 blur = lerp(col, sum, t);
                return blur;
            }
            ENDCG
        }
    }
}
