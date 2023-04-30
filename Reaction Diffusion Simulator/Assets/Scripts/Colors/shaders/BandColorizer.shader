Shader "Colors/ColorBand/BandColorizer"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			struct ColorBand
			{
				float PositionInLine;
				float4 Color;
				float Bias;
			};

			StructuredBuffer<ColorBand> ColorBands;
			int NumberOfBands;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			float Map(float Value, float MinValue, float MaxValue, float MinRange, float MaxRange)
			{
				return (((Value - MinValue) * (MaxRange - MinRange)) / (MaxValue - MinValue)) + MinRange;
			}
			float Biased(float bias, float x)
			{
				float h = 1 - bias;
				float k = h * h * h;

				return (x * k) / (x * k - x + 1);
			}

            float4 frag (v2f input) : SV_Target
            {
                float abDiff = input.uv.x;

				ColorBand preBand;
				for (int i = 0; i < NumberOfBands; i++)
				{
					ColorBand currentBand = ColorBands[i];

					if (abDiff <= currentBand.PositionInLine)
					{
						if (i == 0)
						{
							return currentBand.Color;
						}
						else
						{
							float k = Map(abDiff, preBand.PositionInLine, currentBand.PositionInLine, 0, 1);
							return lerp(preBand.Color, currentBand.Color, Biased(currentBand.Bias, k));
						}
					}

					preBand = currentBand;
				}

				return preBand.Color;
            }
            ENDCG
        }
    }
}
