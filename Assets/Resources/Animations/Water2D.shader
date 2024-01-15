Shader "Unlit/Water2D"
{
    Properties // Material�� �����ϴ� �Ӽ� - ������, ���̺�, Ÿ��, �ʱⰪ
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Perlin ("Texture", 2D) = "white" {} // �Ӽ� 3�� �߰�
        _Noise ("Noise", Range(0, 0.1)) = 0.02
        _WaveSpeed ("WaveSpeed", Range(0, 10)) = 3
    }

    SubShader // Shader ����
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" } // Rendering Mode
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        LOD 100

        Pass // Shader�� ��ü
        {
            CGPROGRAM // �����Ϸ� ������
            #pragma vertex vert // ���ؽ� Ÿ���� �Լ� vert()�� ����
            #pragma fragment frag // �����׸�Ʈ Ÿ���� �Լ� farg()�� ����

            #include "UnityCG.cginc" // �����Ϸ� ������

            struct appdata // ����ü
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f // ����ü
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; // ��������
            float4 _MainTex_ST;

            sampler2D _Perlin;
            float _Noise;
            float _WaveSpeed;

            v2f vert (appdata v) // 3d ������Ʈ�� ���ؽ� ó�� �Լ�
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target // �ؽ�ó�� �̹��� ó�� �Լ�
            {
                fixed2 offset = i.uv + _Time.x * _WaveSpeed; // �̹����� ��ǥ �̵�
                fixed2 noise = tex2D(_Perlin, offset).rg * _Noise; // Perlin �̹����� �÷��� �а� ���� 0.2���Ϸ� ����

                fixed4 col = tex2D(_MainTex, i.uv + noise); // ���� �̹����� uv�� noise�� ��ŭ �̵�
                col.a *= 0.7; // �÷��� Alpha ����
                return col;
            }
            ENDCG
        }
    }
}
