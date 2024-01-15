Shader "Unlit/Water2D"
{
    Properties // Material에 노출하는 속성 - 변수명, 레이블, 타입, 초기값
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Perlin ("Texture", 2D) = "white" {} // 속성 3개 추가
        _Noise ("Noise", Range(0, 0.1)) = 0.02
        _WaveSpeed ("WaveSpeed", Range(0, 10)) = 3
    }

    SubShader // Shader 시작
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" } // Rendering Mode
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        LOD 100

        Pass // Shader의 본체
        {
            CGPROGRAM // 컴파일러 지시자
            #pragma vertex vert // 버텍스 타입의 함수 vert()가 있음
            #pragma fragment frag // 프레그먼트 타입의 함수 farg()가 있음

            #include "UnityCG.cginc" // 컴파일러 지시자

            struct appdata // 구조체
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f // 구조체
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; // 전역변수
            float4 _MainTex_ST;

            sampler2D _Perlin;
            float _Noise;
            float _WaveSpeed;

            v2f vert (appdata v) // 3d 오브젝트의 버텍스 처리 함수
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target // 텍스처의 이미지 처리 함수
            {
                fixed2 offset = i.uv + _Time.x * _WaveSpeed; // 이미지의 좌표 이동
                fixed2 noise = tex2D(_Perlin, offset).rg * _Noise; // Perlin 이미지의 컬러를 읽고 값을 0.2이하로 낮춤

                fixed4 col = tex2D(_MainTex, i.uv + noise); // 원본 이미지의 uv를 noise값 만큼 이동
                col.a *= 0.7; // 컬러에 Alpha 적용
                return col;
            }
            ENDCG
        }
    }
}
