Shader "Unlit/HealthbarShader"
{
    Properties // Input Properties
    {
        [NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
        _Health("Health", Range(0.0,1.0)) = 0.5
        _ThresholdLow("Low Health Threshold", Float) = 0.2
        _BorderWidth("Border Width", Float) = 0.1

        _PulseSpeed("Pulse Speed", Float) = 1 // how many pulses per second
    }
    SubShader
    {
        // ShaderLab
        Tags 
        {"RenderType"="Transparent" "Queue"="Transparent" } // Render Tags & Queue

        Blend SrcAlpha OneMinusSrcAlpha // enable traditional transparency for this SubShader

        Pass
        {
            CGPROGRAM // CG/HLSL
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            sampler2D _MainTex;
            float _Health;
            float _ThresholdLow;
            float _PulseSpeed;
            float _BorderWidth;

            struct appdata //Per vertex Mesh Data
            {
                float4 vertex : POSITION; // local space vertex position
                float2 uv0 : TEXCOORD0; // uv0 diffuse/normal map textures
            };

            struct v2f // this data will pass from vertex to fragment shader
            {
                float4 vertex : SV_POSITION; // clip space position
                float2 uv : TEXCOORD1;
            };

            v2f vert (appdata v) // not messing with the vertices here! passthrough
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv0; //passthrough
                return o;
            }

            fixed4 frag(v2f i) : SV_Target // the healthbar color will be handled by the fragment shader
            {
                float2 coords = i.uv;
                coords.x *= 8;

                float2 lineSegPoints = float2(clamp(coords.x, 0.5, 7.5), 0.5);
                float sdf = distance(coords, lineSegPoints) * 2 - 1;
                clip(-sdf); // clip to define rounded edges

                float borderSDF = sdf + _BorderWidth;

                float partD = fwidth(borderSDF); // screen space partial derivative

                float borderMask = 1 - saturate(borderSDF / partD); // Mask to define border

                float healthbarMask = _Health > i.uv.x; // mask to define empty bar
                float3 healthbar = tex2D(_MainTex, float2(_Health, i.uv.y));

                float pulse = abs(cos(TAU *_Time.y * (_PulseSpeed / 2)));
                if (_Health <= _ThresholdLow) {healthbar *= pulse;}

                return float4(healthbar * healthbarMask * borderMask, 1 );
            }
            ENDCG
        }
    }
}
