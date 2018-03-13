Shader "TEST/CustomBlending" {
	Properties {
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo ", 2D) = "white" {}
		_FlowSpeedY ("FlowSpeedY ", Range(0,10)) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("SrcBlend Mode", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("SrcBlend Mode", Float) = 10
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
		zwrite off
		blend [_SrcBlend] [_DstBlend]
		cull off

		CGPROGRAM
		#pragma surface surf nolight keepalpha noforwardadd nolightmap noambient novertexlights noshadow

		sampler2D _MainTex;
		sampler2D _MainTex2;
		float4 _TintColor;
		float _FlowSpeedY;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float4 color:COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		fixed4 d = tex2D (_MainTex2,  float2(IN.uv_MainTex2.x , IN.uv_MainTex2.y ));
		fixed4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y ));

		c = c * d.r * 2 * _TintColor * IN.color;
		o.Emission = c.rgb;
		o.Alpha = c.a;
			
		}

		float4 Lightingnolight (SurfaceOutput s, float3 lightDir, float atten){
		return float4(0,0,0, s.Alpha);
		}

		
		ENDCG
	}
	FallBack "legacy shaders/Transparent/VertexLit"
}
