using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UniVFX
{
    [RequireComponent(typeof(Image))]
    public class UniVFXSetupCanvasUV : BaseMeshEffect
    {
        [SerializeField] Vector2 _uv2;
        [SerializeField] Vector2 _uv3;
        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            var baseVertices = new List<UIVertex>();
            vertexHelper.GetUIVertexStream(baseVertices);

            var minPosX = baseVertices.Min(x => x.position.x);
            var maxPosX = baseVertices.Max(x => x.position.x);
            var minPosY = baseVertices.Min(x => x.position.y);
            var maxPosY = baseVertices.Max(x => x.position.y);

            var scaleX = 1 / (maxPosX - minPosX);
            var scaleY = 1 / (maxPosY - minPosY);

            for (var i = 0; i < baseVertices.Count; i++)
            {
                var posToUVX = (baseVertices[i].position.x - minPosX) * scaleX;
                var posToUVY = (baseVertices[i].position.y - minPosY) * scaleY;

                var vertex = new UIVertex();
                vertex.position = baseVertices[i].position;
                vertex.uv0 = baseVertices[i].uv0;
                vertex.uv1 = new Vector2(posToUVX, posToUVY);
                vertex.uv2 = _uv2;
                vertex.uv3 = _uv3;
                vertex.color = baseVertices[i].color;

                baseVertices[i] = vertex;
            }

            vertexHelper.Clear();
            vertexHelper.AddUIVertexTriangleStream(baseVertices);

        }

    }
}