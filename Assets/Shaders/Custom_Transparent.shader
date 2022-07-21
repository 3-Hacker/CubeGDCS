// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Custom_Transparent"
{
    Properties
    {
        [Header(SHADING)]_ShadingOffset("Shading Offset", Range( -1 , 1)) = 0
        _ShadingSmoothness("Shading Smoothness", Range( 0 , 1)) = 0
        _ShadowColor("Shadow Color", Color) = (0.8,0.8,0.8,0)
        _InnerShadows("Inner Shadows", Range( 0 , 1)) = 0.25
        [Header(COLOR)]_Color3("Albedo Color", Color) = (1,1,1,0)
        _MainTex("Albedo Texture", 2D) = "white" {}
        [Toggle(_USEGRADIENT_ON)] _UseGradient("Use Gradient?", Float) = 0
        [KeywordEnum(X,Y,Z)] _GradientOrientation("Gradient Orientation", Float) = 0
        _GradientColor1("Gradient Color 1", Color) = (1,1,1,0)
        _GradientColor2("Gradient Color 2", Color) = (1,1,1,0)
        _GradientPosition("Gradient Position", Float) = 0
        _GradientSharpness("Gradient Sharpness", Range( 0 , 50)) = 1
        [Header(HIGHLIGHT)]_HighlightContribution("Highlight Contribution", Range( 0 , 1)) = 0.25
        _HightlightSmoothness("Hightlight Smoothness", Range( 0 , 1)) = 0.5
        [Header(OPACITY)]_Opacity("Opacity", Range( 0 , 1)) = 1
        _FresnelContribution("Fresnel Contribution", Range( 0 , 1)) = 0
        [HDR]_FresnelColor("Fresnel Color", Color) = (1,1,1,0)
        _Fresnel_Offset("Fresnel_Offset", Float) = 1
        _FresnelSmoothness("Fresnel Smoothness", Range( 0 , 1)) = 0.5
        [Header(SHINE)]_ShineColor("Shine Color", Color) = (1,1,1,0)
        _ShineSize("Shine Size", Range( 0 , 1)) = 0
        _ShineSpeed("Shine Speed", Range( 0 , 1)) = 1
        [Header(COLOR CORRECTION)]_Brightness("Brightness", Range( -1 , 1)) = 0
        _Saturation("Saturation", Range( -1 , 1)) = 0.2
        _Contrast("Contrast", Range( -1 , 1)) = 0.1
        [HideInInspector] _texcoord( "", 2D ) = "white" {}
        [HideInInspector] __dirty( "", Int ) = 1
    }

    SubShader
    {
        Pass
        {
            ColorMask 0
            ZWrite On
        }

        Tags
        {
            "RenderType" = "Transparent" "Queue" = "Transparent+0" "IgnoreProjector" = "True"
        }
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add
        CGINCLUDE
        #include "UnityPBSLighting.cginc"
        #include "UnityShaderVariables.cginc"
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #pragma target 3.0
        #pragma shader_feature_local _USEGRADIENT_ON
        #pragma shader_feature_local _GRADIENTORIENTATION_X _GRADIENTORIENTATION_Y _GRADIENTORIENTATION_Z
        struct Input
        {
            float3 worldPos;
            half3 worldNormal;
            float4 screenPos;
            float2 uv_texcoord;
        };

        struct SurfaceOutputCustomLightingCustom
        {
            half3 Albedo;
            half3 Normal;
            half3 Emission;
            half Metallic;
            half Smoothness;
            half Occlusion;
            half Alpha;
            Input SurfInput;
            UnityGIInput GIData;
        };

        uniform half _Fresnel_Offset;
        uniform half _FresnelSmoothness;
        uniform half _FresnelContribution;
        uniform half _ShineSpeed;
        uniform half _ShineSize;
        uniform half _Opacity;
        uniform half _Brightness;
        uniform half4 _ShadowColor;
        uniform half _InnerShadows;
        uniform half _ShadingSmoothness;
        uniform half _ShadingOffset;
        uniform half4 _Color3;
        uniform sampler2D _MainTex;
        uniform half4 _MainTex_ST;
        uniform half4 _GradientColor1;
        uniform half4 _GradientColor2;
        uniform half _GradientPosition;
        uniform half _GradientSharpness;
        uniform float _HightlightSmoothness;
        uniform float _HighlightContribution;
        uniform half4 _ShineColor;
        uniform half4 _FresnelColor;
        uniform half _Contrast;
        uniform half _Saturation;


        inline float4 ASE_ComputeGrabScreenPos(float4 pos)
        {
            #if UNITY_UV_STARTS_AT_TOP
            float scale = -1.0;
            #else
			float scale = 1.0;
            #endif
            float4 o = pos;
            o.y = pos.w * 0.5f;
            o.y = (pos.y - o.y) * _ProjectionParams.x * scale + o.y;
            return o;
        }


        float4 CalculateContrast(float contrastValue, float4 colorTarget)
        {
            float t = 0.5 * (1.0 - contrastValue);
            return mul(float4x4(contrastValue, 0, 0, t, 0, contrastValue, 0, t, 0, 0, contrastValue, t, 0, 0, 0, 1),
                       colorTarget);
        }

        inline half4 LightingStandardCustomLighting(inout SurfaceOutputCustomLightingCustom s, half3 viewDir,
                                                    UnityGI gi)
        {
            UnityGIInput data = s.GIData;
            Input i = s.SurfInput;
            half4 c = 0;
            #ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
            #else
            float3 ase_lightAttenRGB = gi.light.color / ((_LightColor0.rgb) + 0.000001);
            float ase_lightAtten = max(max(ase_lightAttenRGB.r, ase_lightAttenRGB.g), ase_lightAttenRGB.b);
            #endif
            #if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
            #endif
            float3 ase_worldPos = i.worldPos;
            half3 ase_worldViewDir = normalize(UnityWorldSpaceViewDir(ase_worldPos));
            half3 ase_worldNormal = i.worldNormal;
            half fresnelNdotV811 = dot(ase_worldNormal, ase_worldViewDir);
            half fresnelNode811 = (0.0 + _Fresnel_Offset * pow(max(1.0 - fresnelNdotV811, 0.0001),
                                                               (10.0 + (_FresnelSmoothness - 0.0) * (1.0 - 10.0) / (1.0
                                                                   - 0.0))));
            half Fresnel818 = (saturate(fresnelNode811) * _FresnelContribution);
            float4 ase_screenPos = float4(i.screenPos.xyz, i.screenPos.w + 0.00000000001);
            float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos(ase_screenPos);
            half4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
            float cos787 = cos(-0.7);
            float sin787 = sin(-0.7);
            half2 rotator787 = mul(((ase_grabScreenPosNorm).xy * 2.0 + -1.0) - float2(0, 0),
                                   float2x2(cos787, -sin787, sin787, cos787)) + float2(0, 0);
            half mulTime760 = _Time.y * _ShineSpeed;
            half temp_output_758_0 = (-2.0 + (sin((mulTime760 % (0.5 * UNITY_PI))) - 0.0) * (2.0 - -2.0) / (1.0 - 0.0));
            half Shine765 = saturate((step(rotator787.x, temp_output_758_0) - step(
                rotator787.x, (temp_output_758_0 + (0.0 + (_ShineSize - 0.0) * (-0.25 - 0.0) / (1.0 - 0.0))))));
            half lerpResult822 = lerp(saturate((Fresnel818 + Shine765)), 1.0, _Opacity);
            #if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc

			half4 ase_lightColor = 0;
            #else //aselc
            half4 ase_lightColor = _LightColor0;
            #endif //aselc
            half4 ShadowColor699 = _ShadowColor;
            half4 lerpResult734 = lerp(ShadowColor699, float4(1, 1, 1, 0), ase_lightAtten);
            half4 lerpResult719 = lerp(float4(1, 1, 1, 0), lerpResult734, _InnerShadows);
            half3 ase_normWorldNormal = normalize(ase_worldNormal);
            #if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			half3 ase_worldlightDir = 0;
            #else //aseld
            half3 ase_worldlightDir = normalize(UnityWorldSpaceLightDir(ase_worldPos));
            #endif //aseld
            half dotResult3 = dot(ase_normWorldNormal, ase_worldlightDir);
            half NxL553 = dotResult3;
            half smoothstepResult559 = smoothstep(0.0, _ShadingSmoothness, (NxL553 + _ShadingOffset));
            half Shading554 = smoothstepResult559;
            half IsPointLight709 = _WorldSpaceLightPos0.w;
            half4 temp_output_713_0 = (max(ShadowColor699, (lerpResult719 * Shading554)) * (1.0 - IsPointLight709));
            half4 Lighting557 = (ase_lightColor * temp_output_713_0);
            float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
            half4 temp_output_487_0 = (_Color3 * tex2D(_MainTex, uv_MainTex));
            half3 temp_cast_0 = ((_GradientPosition + 0.5)).xxx;
            float3 ase_vertex3Pos = mul(unity_WorldToObject, float4(i.worldPos, 1));
            half3 break492 = saturate(
                (((temp_cast_0 - (ase_vertex3Pos * 0.5 + 0.5)) * _GradientSharpness) + _GradientPosition));
            #if defined(_GRADIENTORIENTATION_X)
				half staticSwitch488 = break492.x;
            #elif defined(_GRADIENTORIENTATION_Y)
				half staticSwitch488 = break492.y;
            #elif defined(_GRADIENTORIENTATION_Z)
				half staticSwitch488 = break492.z;
            #else
            half staticSwitch488 = break492.x;
            #endif
            half4 lerpResult486 = lerp(_GradientColor1, _GradientColor2, staticSwitch488);
            #ifdef _USEGRADIENT_ON
				half4 staticSwitch484 = ( temp_output_487_0 * lerpResult486 );
            #else
            half4 staticSwitch484 = temp_output_487_0;
            #endif
            half4 Color561 = staticSwitch484;
            half smoothstepResult617 = smoothstep(0.0, _HightlightSmoothness,
                                                  saturate((NxL553 - (1.0 - _HighlightContribution))));
            half4 Highlight570 = (smoothstepResult617 * Color561);
            half4 blendOpSrc618 = Color561;
            half4 blendOpDest618 = Highlight570;
            half4 lerpResult805 = lerp(
                (Lighting557 * (saturate((1.0 - (1.0 - blendOpSrc618) * (1.0 - blendOpDest618))))), _ShineColor,
                Shine765);
            half4 lerpResult826 = lerp(lerpResult805, _FresnelColor, Fresnel818);
            half4 FinalColor670 = lerpResult826;
            half4 temp_cast_1 = (0.5).xxxx;
            half3 desaturateInitialColor668 = (((FinalColor670 - temp_cast_1) * (_Contrast + 1.0)) + 0.5).rgb;
            half desaturateDot668 = dot(desaturateInitialColor668, float3(0.299, 0.587, 0.114));
            half3 desaturateVar668 = lerp(desaturateInitialColor668, desaturateDot668.xxx, (_Saturation * -1.0));
            half4 ColorCorrected623 = CalculateContrast((_Brightness + 1.0), half4(desaturateVar668, 0.0));
            c.rgb = ColorCorrected623.rgb;
            c.a = lerpResult822;
            return c;
        }

        inline void LightingStandardCustomLighting_GI(inout SurfaceOutputCustomLightingCustom s, UnityGIInput data,
                                                      inout UnityGI gi)
        {
            s.GIData = data;
        }

        void surf(Input i, inout SurfaceOutputCustomLightingCustom o)
        {
            o.SurfInput = i;
        }
        ENDCG
        CGPROGRAM
        #pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred
        ENDCG
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
            ZWrite On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_shadowcaster
            #pragma multi_compile UNITY_PASS_SHADOWCASTER
            #pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
            #include "HLSLSupport.cginc"
            #if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
            #define CAN_SKIP_VPOS
            #endif
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            sampler3D _DitherMaskLOD;

            struct v2f
            {
                V2F_SHADOW_CASTER;
                float2 customPack1 : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float3 worldNormal : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                Input customInputData;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldNormal = worldNormal;
                o.customPack1.xy = customInputData.uv_texcoord;
                o.customPack1.xy = v.texcoord;
                o.worldPos = worldPos;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            half4 frag(v2f IN
                #if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
                #endif
            ) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                Input surfIN;
                UNITY_INITIALIZE_OUTPUT(Input, surfIN);
                surfIN.uv_texcoord = IN.customPack1.xy;
                float3 worldPos = IN.worldPos;
                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                surfIN.worldPos = worldPos;
                surfIN.worldNormal = IN.worldNormal;
                surfIN.screenPos = IN.screenPos;
                SurfaceOutputCustomLightingCustom o;
                UNITY_INITIALIZE_OUTPUT(SurfaceOutputCustomLightingCustom, o)
                surf(surfIN, o);
                UnityGI gi;
                UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
                o.Alpha = LightingStandardCustomLighting(o, worldViewDir, gi).a;
                #if defined( CAN_SKIP_VPOS )
                float2 vpos = IN.pos;
                #endif
                half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy * 0.25, o.Alpha * 0.9375)).a;
                clip(alphaRef - 0.01);
                SHADOW_CASTER_FRAGMENT(IN)
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
    CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18910
837;73;1090;1296;2624.735;5392.793;9.031203;True;False
Node;AmplifyShaderEditor.CommentaryNode;481;794.6359,-1095.414;Inherit;False;2379.094;749.3013;;17;561;484;485;486;501;488;500;492;493;494;495;496;497;499;498;483;491;GRADIENT;0.8,1,0.8412889,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;491;846.2569,-1032.562;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;483;821.7079,-783.8678;Inherit;False;Property;_GradientPosition;Gradient Position;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;498;1025.503,-1030.359;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;499;1090.458,-873.5948;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;497;866.54,-632.8397;Inherit;False;Property;_GradientSharpness;Gradient Sharpness;12;0;Create;True;0;0;0;False;0;False;1;1;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;496;1269.312,-1025.55;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;48;-1280,-1792;Inherit;False;1054.052;319.1289;;4;553;3;324;77;N x L;0.7971698,0.9217122,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;77;-1248,-1744;Inherit;True;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;324;-1008,-1648;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;495;1450.999,-891.2899;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;3;-752,-1744;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;494;1604.458,-825.8989;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;553;-592,-1744;Inherit;False;NxL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;51;-1280,-1408;Inherit;False;1790.165;511.6156;;8;554;566;559;700;580;560;82;552;SHADING;0.9169499,0.8,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;493;1734.057,-822.832;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;705;-128,-1792;Inherit;False;612.3698;320.0001;;2;699;694;SHADOW COLOR;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;552;-1264,-1328;Inherit;False;553;NxL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;694;-80,-1744;Inherit;False;Property;_ShadowColor;Shadow Color;3;0;Create;True;0;0;0;False;0;False;0.8,0.8,0.8,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;802;-1297.746,422.7404;Inherit;False;2004.861;888.8467;;21;765;800;755;753;748;756;786;758;757;787;754;759;792;785;784;763;762;783;760;761;806;SHINE;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;482;1755.206,-1668.229;Inherit;False;606;514;;3;490;489;487;MAIN COLOR;0,0,0,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;492;1885.733,-824.7729;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;82;-1264,-1216;Inherit;False;Property;_ShadingOffset;Shading Offset;1;1;[Header];Create;True;1;SHADING;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;580;-960,-1312;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;489;1878.033,-1592.342;Inherit;False;Property;_Color3;Albedo Color;5;1;[Header];Create;False;1;COLOR;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;230;-1280,-512;Inherit;False;1845.873;785.5002;;18;224;225;557;695;702;696;711;713;714;716;723;712;735;734;719;732;698;697;LIGHTING;1,0.8950585,0.8,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;452;781.9012,-256.6172;Inherit;False;1966.261;871.5305;;9;473;469;464;570;590;462;617;619;622;HIGHLIGHT;1,0.8,0.8460335,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;761;-1217.766,814.1282;Inherit;False;Property;_ShineSpeed;Shine Speed;22;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;560;-1104,-1040;Inherit;False;Property;_ShadingSmoothness;Shading Smoothness;2;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;500;2136.281,-1039.349;Inherit;False;Property;_GradientColor1;Gradient Color 1;9;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;488;2020.237,-826.05;Inherit;False;Property;_GradientOrientation;Gradient Orientation;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;3;X;Y;Z;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;699;256,-1744;Inherit;False;ShadowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;501;2133.604,-661.2267;Inherit;False;Property;_GradientColor2;Gradient Color 2;10;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;490;1845.354,-1394.404;Inherit;True;Property;_MainTex;Albedo Texture;6;0;Create;False;1;Main;0;0;False;0;False;-1;None;ab11998d4befcc84d91b17971bbd84ac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;559;-656,-1184;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;487;2167.77,-1490.257;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PiNode;762;-986.6125,939.166;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;732;-1290.765,-100.6832;Inherit;False;699;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GrabScreenPosition;783;-1247.746,472.7404;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;486;2369.786,-852.976;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;708;-1280,-768;Inherit;False;528.8752;183;;2;710;709;Is Point Light?;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;462;828.0024,23.71048;Float;False;Property;_HighlightContribution;Highlight Contribution;13;1;[Header];Create;True;1;HIGHLIGHT;0;0;False;0;False;0.25;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;760;-978.7665,818.1282;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;698;-1342.111,72.02254;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;622;891.0377,-194.0674;Inherit;False;553;NxL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;784;-1037.859,478.9286;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;720;-1269.988,-445.0324;Inherit;False;Property;_InnerShadows;Inner Shadows;4;0;Create;True;0;0;0;False;0;False;0.25;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;590;1117.888,-33.79831;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleRemainderNode;763;-788.7665,826.1282;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;734;-1117.674,-32.05016;Inherit;False;3;0;COLOR;1,1,1,0;False;1;COLOR;1,1,1,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;554;48,-1264;Inherit;False;Shading;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;485;2556.34,-882.3187;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;710;-1232,-720;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ScaleAndOffsetNode;785;-896.3056,486.9359;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;473;1312.106,-124.0732;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;792;-871.2364,650.7416;Inherit;False;Constant;_Angle;Angle;21;0;Create;True;0;0;0;False;0;False;-0.7;-0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;696;-947.0576,-96.29541;Inherit;False;554;Shading;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;719;-953.6686,-308.0218;Inherit;False;3;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;709;-992,-704;Inherit;False;IsPointLight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;484;2720.924,-903.6026;Inherit;False;Property;_UseGradient;Use Gradient?;7;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;754;-704.0201,1122.951;Inherit;False;Property;_ShineSize;Shine Size;21;0;Create;True;1;;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;759;-622.7665,820.928;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;757;-325.5845,1128.6;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;758;-416.2772,829.6172;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-2;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;807;3229.654,686.8884;Inherit;False;1588.91;696.7859;;8;818;815;813;812;811;810;809;808;OPACITY;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;702;-800.6348,-427.1397;Inherit;False;699;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;711;-694.2122,-127.7276;Inherit;False;709;IsPointLight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;787;-643.4847,474.7887;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;-0.75;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;469;1629.979,38.39489;Float;False;Property;_HightlightSmoothness;Hightlight Smoothness;14;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;464;1640.675,-112.3835;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;561;2974.976,-902.7508;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;697;-721.5426,-229.5978;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;619;2114.773,147.2656;Inherit;False;561;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;712;-449.1128,-187.5278;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;695;-528.5134,-313.4041;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;786;-415.2756,474.9796;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;756;-131.5942,954.9906;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;808;3256.693,951.1978;Inherit;False;Property;_FresnelSmoothness;Fresnel Smoothness;19;0;Create;True;0;0;0;False;0;False;0.5;5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;617;1968.583,-110.7174;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;748;53.00883,817.825;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;810;3541.391,953.6057;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;10;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;225;-75.20564,-459.9185;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;809;3390.077,772.0303;Inherit;False;Property;_Fresnel_Offset;Fresnel_Offset;18;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;713;-275.7126,-277.0274;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;620;2298.87,-24.73646;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;753;44.4423,952.5162;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;811;3730.397,765.2796;Inherit;True;Standard;WorldNormal;ViewDir;False;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;10.37;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;755;204.7982,870.0795;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;141.4902,-413.0002;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;692;3413.083,-955.9605;Inherit;False;1408.411;684.9578;;12;814;826;820;670;805;704;767;803;558;618;571;563;FINAL IMAGE;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;570;2486.74,-116.1967;Inherit;False;Highlight;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;557;373.7963,-416.5674;Inherit;False;Lighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;806;370.6629,850.0094;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;812;3902.087,999.8087;Inherit;False;Property;_FresnelContribution;Fresnel Contribution;16;1;[Header];Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;813;4045.702,812.5632;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;571;3446.728,-619.673;Inherit;False;570;Highlight;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;563;3445.745,-747.355;Inherit;False;561;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;815;4210.22,770.0214;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;558;3740.25,-850.0411;Inherit;False;557;Lighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;765;525.4407,843.9053;Inherit;False;Shine;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;618;3702.19,-702.0701;Inherit;False;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;767;4015.358,-474.5027;Inherit;False;765;Shine;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;818;4517.267,767.8103;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;803;3821.417,-565.8497;Inherit;False;Property;_ShineColor;Shine Color;20;1;[Header];Create;True;1;SHINE;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;704;3930.741,-721.5403;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;820;4399.518,-476.1378;Inherit;False;818;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;814;4206.688,-570.0106;Inherit;False;Property;_FresnelColor;Fresnel Color;17;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;805;4197.153,-721.04;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;826;4471.471,-723.2133;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;693;3125.342,-140.0098;Inherit;False;1837.471;588.9877;;14;685;633;688;684;628;686;690;687;672;668;691;689;623;671;COLOR CORRECTION;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;670;4640.217,-729.0615;Inherit;False;FinalColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;671;3160.87,-98.09679;Inherit;False;670;FinalColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;633;3175.342,145.8766;Inherit;False;Property;_Contrast;Contrast;25;0;Create;True;0;0;0;False;0;False;0.1;0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;685;3243.225,51.4033;Inherit;False;Constant;_zeropointfive;zeropointfive;19;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;684;3498.75,-84.5859;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;688;3539.55,108.8242;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;628;3699.052,332.9779;Inherit;False;Property;_Saturation;Saturation;24;0;Create;True;1;COLOR CORRECTION;0;0;False;0;False;0.2;0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;686;3747.812,-86.88168;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;687;3941.306,-90.00985;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;690;4148.123,307.9953;Inherit;False;Property;_Brightness;Brightness;23;1;[Header];Create;True;1;COLOR CORRECTION;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;672;4016.258,222.9875;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;691;4431.256,132.0235;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;668;4196.688,-61.59822;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;827;4938.209,-561.9479;Inherit;False;765;Shine;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;824;4931.736,-663.0802;Inherit;False;818;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;689;4488.113,-71.23624;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;828;5113.209,-605.9479;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;623;4733.544,-83.35471;Inherit;False;ColorCorrected;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;823;4963.033,-343.9781;Inherit;False;Property;_Opacity;Opacity;15;1;[Header];Create;True;1;OPACITY;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;829;5242.209,-611.9479;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;723;-593.7228,62.5435;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;822;5318.393,-482.0757;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;716;-261.8129,-35.62782;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;725;5030.432,793.8096;Inherit;False;debug;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;566;-275.9196,-1163.422;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;735;-11.85205,-157.0026;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;714;-141.8428,-162.5076;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;800;-656.3974,982.9962;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;700;-592,-1360;Inherit;False;699;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;726;5244.177,94.98627;Inherit;False;725;debug;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;624;5203.337,-189.3788;Inherit;False;623;ColorCorrected;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5569.342,-430.8359;Half;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Guru/Garlic_Toon_Transparent_v4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;True;0;Custom;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0.02;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;498;0;491;0
WireConnection;499;0;483;0
WireConnection;496;0;499;0
WireConnection;496;1;498;0
WireConnection;495;0;496;0
WireConnection;495;1;497;0
WireConnection;3;0;77;0
WireConnection;3;1;324;0
WireConnection;494;0;495;0
WireConnection;494;1;483;0
WireConnection;553;0;3;0
WireConnection;493;0;494;0
WireConnection;492;0;493;0
WireConnection;580;0;552;0
WireConnection;580;1;82;0
WireConnection;488;1;492;0
WireConnection;488;0;492;1
WireConnection;488;2;492;2
WireConnection;699;0;694;0
WireConnection;559;0;580;0
WireConnection;559;2;560;0
WireConnection;487;0;489;0
WireConnection;487;1;490;0
WireConnection;486;0;500;0
WireConnection;486;1;501;0
WireConnection;486;2;488;0
WireConnection;760;0;761;0
WireConnection;784;0;783;0
WireConnection;590;0;462;0
WireConnection;763;0;760;0
WireConnection;763;1;762;0
WireConnection;734;0;732;0
WireConnection;734;2;698;0
WireConnection;554;0;559;0
WireConnection;485;0;487;0
WireConnection;485;1;486;0
WireConnection;785;0;784;0
WireConnection;473;0;622;0
WireConnection;473;1;590;0
WireConnection;719;1;734;0
WireConnection;719;2;720;0
WireConnection;709;0;710;2
WireConnection;484;1;487;0
WireConnection;484;0;485;0
WireConnection;759;0;763;0
WireConnection;757;0;754;0
WireConnection;758;0;759;0
WireConnection;787;0;785;0
WireConnection;787;2;792;0
WireConnection;464;0;473;0
WireConnection;561;0;484;0
WireConnection;697;0;719;0
WireConnection;697;1;696;0
WireConnection;712;0;711;0
WireConnection;695;0;702;0
WireConnection;695;1;697;0
WireConnection;786;0;787;0
WireConnection;756;0;758;0
WireConnection;756;1;757;0
WireConnection;617;0;464;0
WireConnection;617;2;469;0
WireConnection;748;0;786;0
WireConnection;748;1;758;0
WireConnection;810;0;808;0
WireConnection;713;0;695;0
WireConnection;713;1;712;0
WireConnection;620;0;617;0
WireConnection;620;1;619;0
WireConnection;753;0;786;0
WireConnection;753;1;756;0
WireConnection;811;2;809;0
WireConnection;811;3;810;0
WireConnection;755;0;748;0
WireConnection;755;1;753;0
WireConnection;224;0;225;0
WireConnection;224;1;713;0
WireConnection;570;0;620;0
WireConnection;557;0;224;0
WireConnection;806;0;755;0
WireConnection;813;0;811;0
WireConnection;815;0;813;0
WireConnection;815;1;812;0
WireConnection;765;0;806;0
WireConnection;618;0;563;0
WireConnection;618;1;571;0
WireConnection;818;0;815;0
WireConnection;704;0;558;0
WireConnection;704;1;618;0
WireConnection;805;0;704;0
WireConnection;805;1;803;0
WireConnection;805;2;767;0
WireConnection;826;0;805;0
WireConnection;826;1;814;0
WireConnection;826;2;820;0
WireConnection;670;0;826;0
WireConnection;684;0;671;0
WireConnection;684;1;685;0
WireConnection;688;0;633;0
WireConnection;686;0;684;0
WireConnection;686;1;688;0
WireConnection;687;0;686;0
WireConnection;687;1;685;0
WireConnection;672;0;628;0
WireConnection;691;0;690;0
WireConnection;668;0;687;0
WireConnection;668;1;672;0
WireConnection;689;1;668;0
WireConnection;689;0;691;0
WireConnection;828;0;824;0
WireConnection;828;1;827;0
WireConnection;623;0;689;0
WireConnection;829;0;828;0
WireConnection;723;0;696;0
WireConnection;723;1;734;0
WireConnection;822;0;829;0
WireConnection;822;2;823;0
WireConnection;716;0;711;0
WireConnection;716;1;723;0
WireConnection;566;0;700;0
WireConnection;566;2;559;0
WireConnection;735;0;714;0
WireConnection;714;0;713;0
WireConnection;714;1;716;0
WireConnection;0;9;822;0
WireConnection;0;13;624;0
ASEEND*/
//CHKSM=949820FB2BC1C133E90DF09759F6DA39CD6D77D8