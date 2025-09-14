Shader "Custom/BaseCircleOutline"
{
    Properties
    {
        _FillColor ("Fill Color", Color) = (0.0, 0.6, 1.0, 0.25)
        _OutlineColor ("Outline Color", Color) = (0.0, 0.9, 1.0, 1.0)
        _Radius ("Radius", Float) = 2.0
        _OutlineWidth ("Outline Width", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
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
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            float4 _FillColor;
            float4 _OutlineColor;
            float _Radius;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1; // 把uv范围映射到 -1~1
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 距离中心
                float dist = length(i.uv);

                // 填充部分
                float fillMask = smoothstep(_Radius, _Radius - 0.01, dist);

                // 描边部分
                float outlineMask = smoothstep(_Radius + _OutlineWidth, _Radius, dist) * 
                                    (1 - smoothstep(_Radius, _Radius - _OutlineWidth, dist));

                fixed4 col = 0;

                // 叠加填充
                col = lerp(col, _FillColor, fillMask);

                // 叠加描边（更高优先级，直接覆盖）
                col = lerp(col, _OutlineColor, outlineMask);

                return col;
            }
            ENDCG
        }
    }
}