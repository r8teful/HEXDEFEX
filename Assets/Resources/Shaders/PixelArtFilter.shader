Shader "Custom/PixelArtFilter" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
    }

/* From 
https://github.com/GarrettGunnell/Post-Processing/blob/77a2f78900054269874abdb6f736841d737b7464/Assets/Pixel%20Art/PixelArtFilter.shader */
        SubShader{

            Pass {
                CGPROGRAM
                #pragma vertex vp
                #pragma fragment fp

                #include "UnityCG.cginc"

                struct VertexData {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vp(VertexData v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                Texture2D _MainTex;
                SamplerState point_clamp_sampler;

                fixed4 fp(v2f i) : SV_Target {
                    float4 col = _MainTex.Sample(point_clamp_sampler, i.uv);

                    return col;
                }
                ENDCG
            }
    }
}