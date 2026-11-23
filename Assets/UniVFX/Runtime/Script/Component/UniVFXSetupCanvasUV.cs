using UnityEngine;
using UnityEngine.UI;

namespace UniVFX
{
    [RequireComponent(typeof(Image))]
    public class UniVFXSetupCanvasUV : BaseMeshEffect
    {
        private const float MinAxisSize = 1e-6f;

        [SerializeField] Vector2 _uv2;
        [SerializeField] Vector2 _uv3;

        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            if (!IsActive())
            {
                return;
            }

            var vertexCount = vertexHelper.currentVertCount;
            if (vertexCount == 0)
            {
                return;
            }

            UIVertex vertex = default;
            vertexHelper.PopulateUIVertex(ref vertex, 0);
            var minPosX = vertex.position.x;
            var maxPosX = vertex.position.x;
            var minPosY = vertex.position.y;
            var maxPosY = vertex.position.y;

            for (var i = 1; i < vertexCount; i++)
            {
                vertexHelper.PopulateUIVertex(ref vertex, i);
                var position = vertex.position;
                minPosX = Mathf.Min(minPosX, position.x);
                maxPosX = Mathf.Max(maxPosX, position.x);
                minPosY = Mathf.Min(minPosY, position.y);
                maxPosY = Mathf.Max(maxPosY, position.y);
            }

            var sizeX = maxPosX - minPosX;
            var sizeY = maxPosY - minPosY;
            var scaleX = sizeX > MinAxisSize ? 1f / sizeX : 0f;
            var scaleY = sizeY > MinAxisSize ? 1f / sizeY : 0f;

            for (var i = 0; i < vertexCount; i++)
            {
                vertexHelper.PopulateUIVertex(ref vertex, i);
                var uvX = (vertex.position.x - minPosX) * scaleX;
                var uvY = (vertex.position.y - minPosY) * scaleY;

                vertex.uv1 = new Vector2(uvX, uvY);
                vertex.uv2 = _uv2;
                vertex.uv3 = _uv3;

                vertexHelper.SetUIVertex(vertex, i);
            }
        }
    }
}
