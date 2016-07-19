Shader "Custom/TextureFloatingShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Threshold ("Threshold", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }//"Quene"="Transparent" }
		LOD 200
		//Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		float _Threshold;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);// * _Color;
			o.Alpha = 1;
			if(_Threshold >= c.a)
				o.Albedo = c.rgb;
			else
				o.Albedo = _Color.rgb;
				//o.Alpha = 0;
			// Metallic and smoothness come from slider variables
		}
		ENDCG
	}
	FallBack "Diffuse"
}
