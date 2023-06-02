using UnityEngine;
using UnityEngine.Rendering;
namespace CustomRenderPipeline
{
    public class SpaceRunPipelineRender : RenderPipeline
    {
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            CamerasRender(context, cameras);
        }

        private void CamerasRender(ScriptableRenderContext context, Camera[] cameras)
        {

            foreach (var camera in cameras)
            {
                var _cameraRenderer = new CameraRenderer();
                _cameraRenderer.Render(context, camera);
            }
        }
    }
}
