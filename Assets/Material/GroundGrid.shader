Shader "Custom/GroundGrid"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Tint ("Tint Color", Color) = (1,1,1,1)
        _GridColor ("Grid Color", Color) = (0,0,0,1)
        _GridSpacing ("Grid Spacing (meters)", Float) = 1.0
        _LineWidth ("Line Width (meters)", Float) = 0.05
        _LineSoftness ("Line Softness", Range(0.001,0.5)) = 0.02
        _UseWorldSpace ("Use World Space (1=world,0=uv)", Float) = 1
        _FadeDistanceStart ("Fade Start Distance", Float) = 20.0
        _FadeDistanceEnd ("Fade End Distance", Float) = 40.0
        _TopOnly ("Top Only (Offset Depth)", Float) = 0.0
        _GridStrength ("Grid Strength (0-1)", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Tint;
            fixed4 _GridColor;
            float _GridSpacing;
            float _LineWidth;
            float _LineSoftness;
            float _UseWorldSpace;
            float _FadeDistanceStart;
            float _FadeDistanceEnd;
            float _TopOnly;
            float _GridStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float viewDist : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                float3 camPos = _WorldSpaceCameraPos;
                o.viewDist = distance(camPos, o.worldPos);
                return o;
            }

            // Softline function: returns 1.0 at center of line, 0 at far from line
            float SoftLine(float coord, float spacing, float halfWidth, float softness)
            {
                // Map coordinate into repeating cell centered at 0
                float r = fmod(coord + spacing*1000.0, spacing); // shift to positive
                if (r > spacing * 0.5) r -= spacing;
                float d = abs(r) - halfWidth;
                // smoothstep for softness
                return saturate(1.0 - smoothstep(0.0, softness, d));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // base albedo
                fixed4 baseCol = tex2D(_MainTex, i.uv) * _Tint;

                // decide coordinates for grid: world XZ or uv
                float2 coord;
                if (_UseWorldSpace > 0.5)
                {
                    coord = i.worldPos.xz; // world-space grid on X/Z plane
                }
                else
                {
                    coord = i.uv * _GridSpacing; // UV scaled grid (spacing controlled by _GridSpacing)
                }

                // compute soft lines for X and Z (two directions)
                float halfW = _LineWidth * 0.5;
                // If using UV mode, treat spacing as unit, else use spacing in meters
                float spacing = _UseWorldSpace > 0.5 ? _GridSpacing : 1.0;

                float sx = SoftLine(coord.x, spacing, halfW, _LineSoftness);
                float sz = SoftLine(coord.y, spacing, halfW, _LineSoftness);

                // Combine cross lines (additive, clamp)
                float gridMask = saturate(sx + sz);

                // Optional: fade grid by view distance
                float fade = 1.0;
                if (_FadeDistanceEnd > _FadeDistanceStart)
                {
                    fade = 1.0 - saturate((i.viewDist - _FadeDistanceStart) / max(0.0001, (_FadeDistanceEnd - _FadeDistanceStart)));
                }

                // Optional: top-only: small bias so grid only shows if normal faces up (approx)
                float topMask = 1.0;
                if (_TopOnly > 0.5)
                {
                    float upDot = saturate(dot(normalize(i.normal), float3(0,1,0)));
                    topMask = smoothstep(0.6, 0.9, upDot); // only near horizontal top faces
                }

                // Combine everything
                float finalMask = gridMask * fade * topMask * _GridStrength;

                // Blend grid color over base color (alpha blend using finalMask)
                fixed4 result = lerp(baseCol, _GridColor, finalMask);

                return result;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}