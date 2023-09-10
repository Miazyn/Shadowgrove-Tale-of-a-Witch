Shader "Personal/Cutout"
{
    Properties
    {
        [NoScaleOffset] Texture2D_86aca1612be345a98d243ccc88a3a74e("MainTexture", 2D) = "white" {}
        Color_3d35c2accb7e41dda368271fd0578828("Tint", Color) = (0, 0, 0, 0)
        _Position("PlayerPos", Vector) = (0.5, 0.5, 0, 0)
        _Size("Size", Float) = 1
        Vector1_db5c41932ebf4d8cad38a245b6837a77("Smoothness", Range(0, 1)) = 0.5
        Vector1_ec1ad65cceb94d2a9d017e5228892462("Opacity", Range(0, 1)) = 1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
        }
        Pass
        {
            Name "Pass"
            Tags
            {
            // LightMode: <None>
        }

        // Render State
        Cull Back
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite On

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile_fog
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
    #pragma multi_compile _ DIRLIGHTMAP_COMBINED
    #pragma shader_feature _ _SAMPLE_GI
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        float4 texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
        float4 uv0;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyzw = input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.texCoord0 = input.interp1.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 Texture2D_86aca1612be345a98d243ccc88a3a74e_TexelSize;
float4 Color_3d35c2accb7e41dda368271fd0578828;
float2 _Position;
float _Size;
float Vector1_db5c41932ebf4d8cad38a245b6837a77;
float Vector1_ec1ad65cceb94d2a9d017e5228892462;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(Texture2D_86aca1612be345a98d243ccc88a3a74e);
SAMPLER(samplerTexture2D_86aca1612be345a98d243ccc88a3a74e);

// Graph Functions

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
{
    Out = A / B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_Saturate_float(float In, out float Out)
{
    Out = saturate(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_35a7184b12294d1e99a11f407e272760_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_86aca1612be345a98d243ccc88a3a74e);
    float4 _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_35a7184b12294d1e99a11f407e272760_Out_0.tex, _Property_35a7184b12294d1e99a11f407e272760_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_R_4 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.r;
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_G_5 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.g;
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_B_6 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.b;
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_A_7 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.a;
    float4 _Property_3d7ce949cfb841c5894f6afd814d0402_Out_0 = Color_3d35c2accb7e41dda368271fd0578828;
    float4 _Multiply_652e4eb5f8714cd7a55a90ac524bf736_Out_2;
    Unity_Multiply_float(_SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0, _Property_3d7ce949cfb841c5894f6afd814d0402_Out_0, _Multiply_652e4eb5f8714cd7a55a90ac524bf736_Out_2);
    float _Property_e0872af8e10743838aa5b3ae64150423_Out_0 = Vector1_db5c41932ebf4d8cad38a245b6837a77;
    float4 _ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float2 _Property_72888594be844cf08888e417326410d4_Out_0 = _Position;
    float2 _Remap_418a888471ce461bb168a891bcc667e2_Out_3;
    Unity_Remap_float2(_Property_72888594be844cf08888e417326410d4_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_418a888471ce461bb168a891bcc667e2_Out_3);
    float2 _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2;
    Unity_Add_float2((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), _Remap_418a888471ce461bb168a891bcc667e2_Out_3, _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2);
    float2 _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3;
    Unity_TilingAndOffset_float((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), float2 (1, 1), _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2, _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3);
    float2 _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2;
    Unity_Multiply_float(_TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3, float2(2, 2), _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2);
    float2 _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2;
    Unity_Subtract_float2(_Multiply_c739577b0b824d98bad3d059b9810b38_Out_2, float2(1, 1), _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2);
    float _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0 = _Size;
    float _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2;
    Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2);
    float _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2;
    Unity_Multiply_float(_Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2, _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2);
    float2 _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0 = float2(_Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2, _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0);
    float2 _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2;
    Unity_Divide_float2(_Subtract_bb11bc62cc114970ae498eb40883781d_Out_2, _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0, _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2);
    float _Length_33225eafef7b4f01991285d3ea51e481_Out_1;
    Unity_Length_float2(_Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2, _Length_33225eafef7b4f01991285d3ea51e481_Out_1);
    float _OneMinus_c0416c568534499289574d048046bb1d_Out_1;
    Unity_OneMinus_float(_Length_33225eafef7b4f01991285d3ea51e481_Out_1, _OneMinus_c0416c568534499289574d048046bb1d_Out_1);
    float _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1;
    Unity_Saturate_float(_OneMinus_c0416c568534499289574d048046bb1d_Out_1, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1);
    float _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3;
    Unity_Smoothstep_float(0, _Property_e0872af8e10743838aa5b3ae64150423_Out_0, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1, _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3);
    float _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0 = Vector1_ec1ad65cceb94d2a9d017e5228892462;
    float _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2;
    Unity_Multiply_float(_Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3, _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0, _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2);
    float _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    Unity_OneMinus_float(_Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2, _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1);
    surface.BaseColor = (_Multiply_652e4eb5f8714cd7a55a90ac524bf736_Out_2.xyz);
    surface.Alpha = _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    surface.AlphaClipThreshold = 0.5;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
    output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "ShadowCaster"
    Tags
    {
        "LightMode" = "ShadowCaster"
    }

        // Render State
        Cull Back
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite On
    ColorMask 0

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 Texture2D_86aca1612be345a98d243ccc88a3a74e_TexelSize;
float4 Color_3d35c2accb7e41dda368271fd0578828;
float2 _Position;
float _Size;
float Vector1_db5c41932ebf4d8cad38a245b6837a77;
float Vector1_ec1ad65cceb94d2a9d017e5228892462;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(Texture2D_86aca1612be345a98d243ccc88a3a74e);
SAMPLER(samplerTexture2D_86aca1612be345a98d243ccc88a3a74e);

// Graph Functions

void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
{
    Out = A / B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_Saturate_float(float In, out float Out)
{
    Out = saturate(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float _Property_e0872af8e10743838aa5b3ae64150423_Out_0 = Vector1_db5c41932ebf4d8cad38a245b6837a77;
    float4 _ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float2 _Property_72888594be844cf08888e417326410d4_Out_0 = _Position;
    float2 _Remap_418a888471ce461bb168a891bcc667e2_Out_3;
    Unity_Remap_float2(_Property_72888594be844cf08888e417326410d4_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_418a888471ce461bb168a891bcc667e2_Out_3);
    float2 _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2;
    Unity_Add_float2((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), _Remap_418a888471ce461bb168a891bcc667e2_Out_3, _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2);
    float2 _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3;
    Unity_TilingAndOffset_float((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), float2 (1, 1), _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2, _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3);
    float2 _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2;
    Unity_Multiply_float(_TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3, float2(2, 2), _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2);
    float2 _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2;
    Unity_Subtract_float2(_Multiply_c739577b0b824d98bad3d059b9810b38_Out_2, float2(1, 1), _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2);
    float _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0 = _Size;
    float _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2;
    Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2);
    float _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2;
    Unity_Multiply_float(_Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2, _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2);
    float2 _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0 = float2(_Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2, _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0);
    float2 _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2;
    Unity_Divide_float2(_Subtract_bb11bc62cc114970ae498eb40883781d_Out_2, _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0, _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2);
    float _Length_33225eafef7b4f01991285d3ea51e481_Out_1;
    Unity_Length_float2(_Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2, _Length_33225eafef7b4f01991285d3ea51e481_Out_1);
    float _OneMinus_c0416c568534499289574d048046bb1d_Out_1;
    Unity_OneMinus_float(_Length_33225eafef7b4f01991285d3ea51e481_Out_1, _OneMinus_c0416c568534499289574d048046bb1d_Out_1);
    float _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1;
    Unity_Saturate_float(_OneMinus_c0416c568534499289574d048046bb1d_Out_1, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1);
    float _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3;
    Unity_Smoothstep_float(0, _Property_e0872af8e10743838aa5b3ae64150423_Out_0, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1, _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3);
    float _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0 = Vector1_ec1ad65cceb94d2a9d017e5228892462;
    float _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2;
    Unity_Multiply_float(_Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3, _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0, _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2);
    float _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    Unity_OneMinus_float(_Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2, _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1);
    surface.Alpha = _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    surface.AlphaClipThreshold = 0.5;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "DepthOnly"
    Tags
    {
        "LightMode" = "DepthOnly"
    }

        // Render State
        Cull Back
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite On
    ColorMask 0

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 Texture2D_86aca1612be345a98d243ccc88a3a74e_TexelSize;
float4 Color_3d35c2accb7e41dda368271fd0578828;
float2 _Position;
float _Size;
float Vector1_db5c41932ebf4d8cad38a245b6837a77;
float Vector1_ec1ad65cceb94d2a9d017e5228892462;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(Texture2D_86aca1612be345a98d243ccc88a3a74e);
SAMPLER(samplerTexture2D_86aca1612be345a98d243ccc88a3a74e);

// Graph Functions

void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
{
    Out = A / B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_Saturate_float(float In, out float Out)
{
    Out = saturate(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float _Property_e0872af8e10743838aa5b3ae64150423_Out_0 = Vector1_db5c41932ebf4d8cad38a245b6837a77;
    float4 _ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float2 _Property_72888594be844cf08888e417326410d4_Out_0 = _Position;
    float2 _Remap_418a888471ce461bb168a891bcc667e2_Out_3;
    Unity_Remap_float2(_Property_72888594be844cf08888e417326410d4_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_418a888471ce461bb168a891bcc667e2_Out_3);
    float2 _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2;
    Unity_Add_float2((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), _Remap_418a888471ce461bb168a891bcc667e2_Out_3, _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2);
    float2 _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3;
    Unity_TilingAndOffset_float((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), float2 (1, 1), _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2, _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3);
    float2 _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2;
    Unity_Multiply_float(_TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3, float2(2, 2), _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2);
    float2 _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2;
    Unity_Subtract_float2(_Multiply_c739577b0b824d98bad3d059b9810b38_Out_2, float2(1, 1), _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2);
    float _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0 = _Size;
    float _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2;
    Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2);
    float _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2;
    Unity_Multiply_float(_Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2, _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2);
    float2 _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0 = float2(_Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2, _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0);
    float2 _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2;
    Unity_Divide_float2(_Subtract_bb11bc62cc114970ae498eb40883781d_Out_2, _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0, _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2);
    float _Length_33225eafef7b4f01991285d3ea51e481_Out_1;
    Unity_Length_float2(_Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2, _Length_33225eafef7b4f01991285d3ea51e481_Out_1);
    float _OneMinus_c0416c568534499289574d048046bb1d_Out_1;
    Unity_OneMinus_float(_Length_33225eafef7b4f01991285d3ea51e481_Out_1, _OneMinus_c0416c568534499289574d048046bb1d_Out_1);
    float _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1;
    Unity_Saturate_float(_OneMinus_c0416c568534499289574d048046bb1d_Out_1, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1);
    float _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3;
    Unity_Smoothstep_float(0, _Property_e0872af8e10743838aa5b3ae64150423_Out_0, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1, _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3);
    float _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0 = Vector1_ec1ad65cceb94d2a9d017e5228892462;
    float _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2;
    Unity_Multiply_float(_Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3, _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0, _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2);
    float _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    Unity_OneMinus_float(_Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2, _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1);
    surface.Alpha = _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    surface.AlphaClipThreshold = 0.5;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

    ENDHLSL
}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
        }
        Pass
        {
            Name "Pass"
            Tags
            {
            // LightMode: <None>
        }

        // Render State
        Cull Back
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma only_renderers gles gles3 glcore d3d11
    #pragma multi_compile_instancing
    #pragma multi_compile_fog
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
    #pragma multi_compile _ DIRLIGHTMAP_COMBINED
    #pragma shader_feature _ _SAMPLE_GI
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        float4 texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
        float4 uv0;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        float4 interp1 : TEXCOORD1;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyzw = input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.texCoord0 = input.interp1.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 Texture2D_86aca1612be345a98d243ccc88a3a74e_TexelSize;
float4 Color_3d35c2accb7e41dda368271fd0578828;
float2 _Position;
float _Size;
float Vector1_db5c41932ebf4d8cad38a245b6837a77;
float Vector1_ec1ad65cceb94d2a9d017e5228892462;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(Texture2D_86aca1612be345a98d243ccc88a3a74e);
SAMPLER(samplerTexture2D_86aca1612be345a98d243ccc88a3a74e);

// Graph Functions

void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
{
    Out = A / B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_Saturate_float(float In, out float Out)
{
    Out = saturate(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_35a7184b12294d1e99a11f407e272760_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_86aca1612be345a98d243ccc88a3a74e);
    float4 _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_35a7184b12294d1e99a11f407e272760_Out_0.tex, _Property_35a7184b12294d1e99a11f407e272760_Out_0.samplerstate, IN.uv0.xy);
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_R_4 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.r;
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_G_5 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.g;
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_B_6 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.b;
    float _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_A_7 = _SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0.a;
    float4 _Property_3d7ce949cfb841c5894f6afd814d0402_Out_0 = Color_3d35c2accb7e41dda368271fd0578828;
    float4 _Multiply_652e4eb5f8714cd7a55a90ac524bf736_Out_2;
    Unity_Multiply_float(_SampleTexture2D_4d010761b39f4c16b6b9a0cd71a0315f_RGBA_0, _Property_3d7ce949cfb841c5894f6afd814d0402_Out_0, _Multiply_652e4eb5f8714cd7a55a90ac524bf736_Out_2);
    float _Property_e0872af8e10743838aa5b3ae64150423_Out_0 = Vector1_db5c41932ebf4d8cad38a245b6837a77;
    float4 _ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float2 _Property_72888594be844cf08888e417326410d4_Out_0 = _Position;
    float2 _Remap_418a888471ce461bb168a891bcc667e2_Out_3;
    Unity_Remap_float2(_Property_72888594be844cf08888e417326410d4_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_418a888471ce461bb168a891bcc667e2_Out_3);
    float2 _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2;
    Unity_Add_float2((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), _Remap_418a888471ce461bb168a891bcc667e2_Out_3, _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2);
    float2 _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3;
    Unity_TilingAndOffset_float((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), float2 (1, 1), _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2, _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3);
    float2 _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2;
    Unity_Multiply_float(_TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3, float2(2, 2), _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2);
    float2 _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2;
    Unity_Subtract_float2(_Multiply_c739577b0b824d98bad3d059b9810b38_Out_2, float2(1, 1), _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2);
    float _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0 = _Size;
    float _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2;
    Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2);
    float _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2;
    Unity_Multiply_float(_Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2, _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2);
    float2 _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0 = float2(_Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2, _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0);
    float2 _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2;
    Unity_Divide_float2(_Subtract_bb11bc62cc114970ae498eb40883781d_Out_2, _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0, _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2);
    float _Length_33225eafef7b4f01991285d3ea51e481_Out_1;
    Unity_Length_float2(_Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2, _Length_33225eafef7b4f01991285d3ea51e481_Out_1);
    float _OneMinus_c0416c568534499289574d048046bb1d_Out_1;
    Unity_OneMinus_float(_Length_33225eafef7b4f01991285d3ea51e481_Out_1, _OneMinus_c0416c568534499289574d048046bb1d_Out_1);
    float _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1;
    Unity_Saturate_float(_OneMinus_c0416c568534499289574d048046bb1d_Out_1, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1);
    float _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3;
    Unity_Smoothstep_float(0, _Property_e0872af8e10743838aa5b3ae64150423_Out_0, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1, _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3);
    float _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0 = Vector1_ec1ad65cceb94d2a9d017e5228892462;
    float _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2;
    Unity_Multiply_float(_Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3, _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0, _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2);
    float _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    Unity_OneMinus_float(_Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2, _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1);
    surface.BaseColor = (_Multiply_652e4eb5f8714cd7a55a90ac524bf736_Out_2.xyz);
    surface.Alpha = _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    surface.AlphaClipThreshold = 0.5;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
    output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "ShadowCaster"
    Tags
    {
        "LightMode" = "ShadowCaster"
    }

        // Render State
        Cull Back
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite On
    ColorMask 0

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma only_renderers gles gles3 glcore d3d11
    #pragma multi_compile_instancing
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 Texture2D_86aca1612be345a98d243ccc88a3a74e_TexelSize;
float4 Color_3d35c2accb7e41dda368271fd0578828;
float2 _Position;
float _Size;
float Vector1_db5c41932ebf4d8cad38a245b6837a77;
float Vector1_ec1ad65cceb94d2a9d017e5228892462;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(Texture2D_86aca1612be345a98d243ccc88a3a74e);
SAMPLER(samplerTexture2D_86aca1612be345a98d243ccc88a3a74e);

// Graph Functions

void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
{
    Out = A / B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_Saturate_float(float In, out float Out)
{
    Out = saturate(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float _Property_e0872af8e10743838aa5b3ae64150423_Out_0 = Vector1_db5c41932ebf4d8cad38a245b6837a77;
    float4 _ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float2 _Property_72888594be844cf08888e417326410d4_Out_0 = _Position;
    float2 _Remap_418a888471ce461bb168a891bcc667e2_Out_3;
    Unity_Remap_float2(_Property_72888594be844cf08888e417326410d4_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_418a888471ce461bb168a891bcc667e2_Out_3);
    float2 _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2;
    Unity_Add_float2((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), _Remap_418a888471ce461bb168a891bcc667e2_Out_3, _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2);
    float2 _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3;
    Unity_TilingAndOffset_float((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), float2 (1, 1), _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2, _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3);
    float2 _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2;
    Unity_Multiply_float(_TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3, float2(2, 2), _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2);
    float2 _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2;
    Unity_Subtract_float2(_Multiply_c739577b0b824d98bad3d059b9810b38_Out_2, float2(1, 1), _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2);
    float _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0 = _Size;
    float _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2;
    Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2);
    float _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2;
    Unity_Multiply_float(_Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2, _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2);
    float2 _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0 = float2(_Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2, _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0);
    float2 _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2;
    Unity_Divide_float2(_Subtract_bb11bc62cc114970ae498eb40883781d_Out_2, _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0, _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2);
    float _Length_33225eafef7b4f01991285d3ea51e481_Out_1;
    Unity_Length_float2(_Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2, _Length_33225eafef7b4f01991285d3ea51e481_Out_1);
    float _OneMinus_c0416c568534499289574d048046bb1d_Out_1;
    Unity_OneMinus_float(_Length_33225eafef7b4f01991285d3ea51e481_Out_1, _OneMinus_c0416c568534499289574d048046bb1d_Out_1);
    float _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1;
    Unity_Saturate_float(_OneMinus_c0416c568534499289574d048046bb1d_Out_1, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1);
    float _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3;
    Unity_Smoothstep_float(0, _Property_e0872af8e10743838aa5b3ae64150423_Out_0, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1, _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3);
    float _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0 = Vector1_ec1ad65cceb94d2a9d017e5228892462;
    float _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2;
    Unity_Multiply_float(_Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3, _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0, _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2);
    float _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    Unity_OneMinus_float(_Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2, _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1);
    surface.Alpha = _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    surface.AlphaClipThreshold = 0.5;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "DepthOnly"
    Tags
    {
        "LightMode" = "DepthOnly"
    }

        // Render State
        Cull Back
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite On
    ColorMask 0

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma only_renderers gles gles3 glcore d3d11
    #pragma multi_compile_instancing
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpacePosition;
        float4 ScreenPosition;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 Texture2D_86aca1612be345a98d243ccc88a3a74e_TexelSize;
float4 Color_3d35c2accb7e41dda368271fd0578828;
float2 _Position;
float _Size;
float Vector1_db5c41932ebf4d8cad38a245b6837a77;
float Vector1_ec1ad65cceb94d2a9d017e5228892462;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(Texture2D_86aca1612be345a98d243ccc88a3a74e);
SAMPLER(samplerTexture2D_86aca1612be345a98d243ccc88a3a74e);

// Graph Functions

void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
{
    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
}

void Unity_Add_float2(float2 A, float2 B, out float2 Out)
{
    Out = A + B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
{
    Out = A * B;
}

void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
{
    Out = A - B;
}

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Multiply_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
{
    Out = A / B;
}

void Unity_Length_float2(float2 In, out float Out)
{
    Out = length(In);
}

void Unity_OneMinus_float(float In, out float Out)
{
    Out = 1 - In;
}

void Unity_Saturate_float(float In, out float Out)
{
    Out = saturate(In);
}

void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
{
    Out = smoothstep(Edge1, Edge2, In);
}

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

// Graph Pixel
struct SurfaceDescription
{
    float Alpha;
    float AlphaClipThreshold;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    float _Property_e0872af8e10743838aa5b3ae64150423_Out_0 = Vector1_db5c41932ebf4d8cad38a245b6837a77;
    float4 _ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
    float2 _Property_72888594be844cf08888e417326410d4_Out_0 = _Position;
    float2 _Remap_418a888471ce461bb168a891bcc667e2_Out_3;
    Unity_Remap_float2(_Property_72888594be844cf08888e417326410d4_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_418a888471ce461bb168a891bcc667e2_Out_3);
    float2 _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2;
    Unity_Add_float2((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), _Remap_418a888471ce461bb168a891bcc667e2_Out_3, _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2);
    float2 _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3;
    Unity_TilingAndOffset_float((_ScreenPosition_7a0330c2977d43d8a0d8f15b9d001c79_Out_0.xy), float2 (1, 1), _Add_cb85a5a54e144ed2b04181da69befa5b_Out_2, _TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3);
    float2 _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2;
    Unity_Multiply_float(_TilingAndOffset_e23030026255440e90c0ae481492ca3f_Out_3, float2(2, 2), _Multiply_c739577b0b824d98bad3d059b9810b38_Out_2);
    float2 _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2;
    Unity_Subtract_float2(_Multiply_c739577b0b824d98bad3d059b9810b38_Out_2, float2(1, 1), _Subtract_bb11bc62cc114970ae498eb40883781d_Out_2);
    float _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0 = _Size;
    float _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2;
    Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2);
    float _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2;
    Unity_Multiply_float(_Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0, _Divide_97776d2c686c4f7ead4bb219748ae9c5_Out_2, _Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2);
    float2 _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0 = float2(_Multiply_9bfa793598d740d89b9c73cc7e1fb857_Out_2, _Property_d781e03a38684399bb8a8a0ab3e08a15_Out_0);
    float2 _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2;
    Unity_Divide_float2(_Subtract_bb11bc62cc114970ae498eb40883781d_Out_2, _Vector2_204d502062bf4fd0a7378f8d101d13ab_Out_0, _Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2);
    float _Length_33225eafef7b4f01991285d3ea51e481_Out_1;
    Unity_Length_float2(_Divide_a5c80499c08b4808833ae22cf74c4bb8_Out_2, _Length_33225eafef7b4f01991285d3ea51e481_Out_1);
    float _OneMinus_c0416c568534499289574d048046bb1d_Out_1;
    Unity_OneMinus_float(_Length_33225eafef7b4f01991285d3ea51e481_Out_1, _OneMinus_c0416c568534499289574d048046bb1d_Out_1);
    float _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1;
    Unity_Saturate_float(_OneMinus_c0416c568534499289574d048046bb1d_Out_1, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1);
    float _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3;
    Unity_Smoothstep_float(0, _Property_e0872af8e10743838aa5b3ae64150423_Out_0, _Saturate_3a3a6459110c4b33be61cbf477d67255_Out_1, _Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3);
    float _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0 = Vector1_ec1ad65cceb94d2a9d017e5228892462;
    float _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2;
    Unity_Multiply_float(_Smoothstep_0b507c043cda452586aa0ec81bac7d4f_Out_3, _Property_dcb7d831f20d4978b8224d4bd439ea31_Out_0, _Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2);
    float _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    Unity_OneMinus_float(_Multiply_72f1bf88ba6b4895a64364a550b6f2bf_Out_2, _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1);
    surface.Alpha = _OneMinus_e10eec8012b24716beeaefcc2e7103c1_Out_1;
    surface.AlphaClipThreshold = 0.5;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.WorldSpacePosition = input.positionWS;
    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

    ENDHLSL
}
    }
        FallBack "Hidden/Shader Graph/FallbackError"
}