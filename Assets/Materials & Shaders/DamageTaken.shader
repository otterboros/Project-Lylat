// DamageTaken.shader - Vertex jitter and frag color flash to indicate damage taken
//---------------------------------------------------------------------------------

Shader "Unlit/DamageTaken"
{
    Properties
    {
        _ColorBase("Base Color", Color) = (1,1,1,1)
        _ColorDamage("Damage Color", Color) = (1,0,0,1)

        _PulseSpeed("Pulse Speed", Float) = 2 // how many pulses per second
        _Duration("Damage Flash Duration", Float) = 2 // Total time for one cycle of Enemy Damage Flash

        [Toggle(IS_DAMAGED)] _IsDamaged("Is Damaged?", Float) = 0 // Determine whether to trigger damage cycle

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature IS_DAMAGED
            //#pragma multi_compile __ ENABLE_IS_DAMAGED

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

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

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            float4 _ColorBase;
            float4 _ColorDamage;
            float _PulseSpeed;
            float _Duration;
            bool _IsDamaged;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Passthrough UVs
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
            #ifdef IS_DAMAGED
                //float startTime = _Time.y;
                //float pulse = sin(TAU * _Time.y * (_PulseSpeed / 2));
                //while ((_Time.y - startTime) < _Duration)
                //    return pulse * _ColorDamage;
                return _ColorBase;
                _IsDamaged = 0;
            #else
                return _ColorBase;
            #endif
            }
            ENDHLSL
        }
    }
}
