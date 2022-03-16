Shader "Unlit/EnemyLaser"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}

        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,0,0,1)
        _ColorStart("Color Start", Range(0,1)) = 0
        _ColorEnd("Color Start", Range(0,1)) = 1


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Passthrough UVs
                return o;
            }

            float2 CartToPolar(float2 cartesian)
            {
                float radius = length(cartesian);
                float angle = atan2(cartesian.x, cartesian.y);
                return float2(angle/UNITY_TWO_PI,radius);
            }

            float InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Shift UV Center
                i.uv = (i.uv - 0.5) * 2;

                // Convert Cartesian UV to Polar UV
                float2 polarUV = CartToPolar(i.uv);

                // blend between two colors in polar coordinates

                float t = InverseLerp(_ColorStart, _ColorEnd, polarUV.y);
                float4 outColor = lerp(_ColorA, _ColorB, t);

                return outColor;

                //return float4(polarUV,0,1);
            }
            ENDHLSL
        }
    }
}
