Shader "Unlit/MapCameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Probability("Probability", float) = 0
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
			uniform float _Probability;
			
            
            float random(float2 st, float n) {
                st = floor(st * n);
                return frac(sin(dot(st.xy, float2(12.9898,78.233)))*43758.5453123);
            }

            float random1(float s) {
                return frac(sin(s) * 1000.0);
            }

			fixed4 frag(v2f_img i) : COLOR
			{
			    float rand = random(i.uv.xy, 50);
			    if (rand < _Probability) {
			        return random1(rand);
			    }
				fixed4 renderTex = tex2D(_MainTex, i.uv);
				return renderTex;
			}
            ENDCG
        }
    }
}
