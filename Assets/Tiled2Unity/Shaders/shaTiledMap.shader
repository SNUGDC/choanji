Shader "Tiled/TextureTintSnap"
{
    Properties
    {
        [PerRendererData] _MainTex ("Tiled Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] Tint ("Use tint", Float) = 1
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="TransparentCutout" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY TINT_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
				#ifdef TINT_ON
                float4 color    : COLOR;
				#endif
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
				#ifdef TINT_ON
                fixed4 color    : COLOR;
				#endif
                half2 texcoord  : TEXCOORD0;
            };

            #ifdef TINT_ON
            fixed4 _Color;
			#endif

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
                OUT.texcoord = IN.texcoord;
				#ifdef TINT_ON
                OUT.color = IN.color * _Color;
				#endif
                OUT.vertex = UnityPixelSnap (OUT.vertex);

                // Supports animations through z-component of tile
                if (IN.vertex.z < 0)
                {
                    // "Hide" frames of a tile animation that are not active
                    OUT.vertex.w = 0;
                }
                else
                {
                    OUT.vertex.z = 0;
                }

                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : COLOR
            {
                half4 texcol = tex2D(_MainTex, IN.texcoord);
				#ifdef TINT_ON
                texcol = texcol * IN.color;
				#endif
                return texcol;
            }
        ENDCG
        }
    }

    Fallback "Sprites/Default"
}