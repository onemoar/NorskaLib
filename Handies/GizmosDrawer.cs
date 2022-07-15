using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GizmosDrawer : MonoBehaviour
{
    public enum Modes { OnSelected, Always }
    public enum Shapes { Cube, Sphere, Custom }
    public enum Styles { Solid, Wired }


    [SerializeField] Shapes shape;
    [ShowIf("@shape == Shapes.Sphere"), LabelText("Radius")]
    [SerializeField] float sphereRadius = 1;
    [ShowIf("@shape == Shapes.Cube"), LabelText("Size")]
    [SerializeField] Vector3 cubeSize = Vector3.one;

    [ShowIf("@shape == Shapes.Custom")]
    [SerializeField] Mesh[] meshes;

    [Space]
    [SerializeField] Styles style;
    [SerializeField] Color color = Color.blue;

    [Space]
    [SerializeField] Modes drawMode;
    [SerializeField] Vector3 offset;

    [Space]
    public bool drawForward;

    private void OnDrawGizmos()
    {
        if (drawMode == Modes.Always)
            Draw();
    }

    private void OnDrawGizmosSelected()
    {
        if (drawMode == Modes.OnSelected)
            Draw();
    }

    void Draw()
    {
        Gizmos.color = color;

        if (drawForward)
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }

        var position = transform.position + offset;
        var rotation = transform.rotation;
        switch (shape)
        {
            case Shapes.Cube:
                switch (style)
                {
                    default:
                    case Styles.Solid:
                        Gizmos.DrawCube(position, cubeSize);
                        break;
                    case Styles.Wired:
                        Gizmos.DrawWireCube(position, cubeSize);
                        break;
                }
                break;
            case Shapes.Sphere:
                switch (style)
                {
                    default:
                    case Styles.Solid:
                        Gizmos.DrawSphere(position, sphereRadius);
                        break;
                    case Styles.Wired:
                        Gizmos.DrawWireSphere(position, sphereRadius);
                        break;
                }
                break;

            case Shapes.Custom:
                switch (style)
                {
                    default:
                    case Styles.Solid:
                        foreach (var mesh in meshes)
                            Gizmos.DrawMesh(mesh, position, rotation);
                        break;
                    case Styles.Wired:
                        foreach (var mesh in meshes)
                            Gizmos.DrawWireMesh(
                                mesh, position, rotation);
                        break;
                }
                break;
        }
    }
}
