using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderToTargetFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {

        private Settings settings;
        private FilteringSettings filteringSettings;
        private ProfilingSampler m_ProfilingSampler;

        private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

        private RenderTargetHandle tempRT;

        public CustomRenderPass(Settings settings, string name)
        {
            this.settings = settings;
            filteringSettings = new FilteringSettings(RenderQueueRange.all, settings.layerMask);
            m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
            m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            m_ProfilingSampler = new ProfilingSampler(name);
            tempRT.Init(settings.tempTargetName);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            //opaqueDesc.colorFormat = RenderTextureFormat.Default;
            opaqueDesc.colorFormat = RenderTextureFormat.DefaultHDR; // if you need to support HDR?
            cmd.GetTemporaryRT(tempRT.id, opaqueDesc, FilterMode.Bilinear);
            ConfigureTarget(tempRT.Identifier());
            ConfigureClear(ClearFlag.All, Color.clear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSampler))
            {
                SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
                DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempRT.id);
        }
    }

    CustomRenderPass m_ScriptablePass;

    [System.Serializable]
    public class Settings
    {
        public string tempTargetName;
        public RenderPassEvent renderPassEvent;
        public LayerMask layerMask;
    }

    public Settings settings;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(settings, name);
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}