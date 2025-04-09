using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkHU_lung
{
    internal class Trash
    {
    }
    /*private void GenerateVoxelData()
{
    var copy = (ExternalPlanSetup)_context.Course.CopyPlanSetup(_context.PlanSetup);
    List<Beam> beams = copy.Beams.ToList();

    copy.CopyEvaluationDose(copy.Beams.ToList());
    Dose dose = _context.PlanSetup.Dose;
    DoseValue[,,] transformedDoseMatrix = new DoseValue[_context.Image.XSize, _context.Image.YSize, _context.Image.ZSize];

    for (int z = 0; z < _context.Image.ZSize; z++)
    {
            Debug.WriteLine($"Récupération de la dose = {z}");
        for (int y = 0; y < _context.Image.YSize; y++)
        {
            for (int x = 0; x < _context.Image.XSize; x++)
            {
                int index = x + (y * _context.Image.XSize) + (z * _context.Image.XSize * _context.Image.YSize);
                double transformedDose = _V.EQD2(dose.VoxelToDoseValue(index).Dose);
                transformedDoseMatrix[x, y, z] = new DoseValue(transformedDose, DoseValue.DoseUnit.Gy);
            }
        }
    }

    using (StreamWriter writer = new StreamWriter(path_EQD2))
    {
        for (int z = 0; z < transformedDoseMatrix.GetLength(2); z++)
        {
            Debug.WriteLine($"Calcul de la dose EQD2 = {z}");

            for (int y = 0; y < transformedDoseMatrix.GetLength(1); y++)
            {
                for (int x = 0; x < transformedDoseMatrix.GetLength(0); x++)
                {
                    double doseValue = transformedDoseMatrix[x, y, z].Dose;

                    writer.Write(doseValue);

                    if (x < transformedDoseMatrix.GetLength(0) - 1)
                        writer.Write(", ");
                }

                writer.WriteLine();
            }

            // Après modification, mettre à jour l'objet Dose dans le plan
            //_context.Course.AddExternalPlanSetup(_context.StructureSet).Name = "EQD2";
            //_context.Course.ExternalPlanSetups.First(x => x.Name.Equals("EQD2")).BaseDosePlanningItem = new Dose(doseMatrix);
        }
    }
}

/*private void GenerateVoxelData(Structure st, int cpt)
{
    List<double> result = new List<double>();
    VVector p = new VVector();
    Rect3D st_bounds = st.MeshGeometry.Bounds;
    VVector centre = st.CenterPoint;
    Image dose = _context.Image;

    List<Tuple<double, double>> voxels = new List<Tuple<double, double>>();

    int x = (int)((centre.x - dose.Origin.x) / dose.XRes);
    int y = (int)((centre.y - dose.Origin.y) / dose.YRes);
    int z = (int)((centre.z - dose.Origin.z) / dose.ZRes);

    int zinit = z - (int)st_bounds.SizeZ / 2;
    int zfin = z + (int)st_bounds.SizeZ / 2;

    int xinit = x - (int)st_bounds.SizeX / 2;
    int xfin = x + (int)st_bounds.SizeX / 2;

    int yinit = y - (int)st_bounds.SizeY / 2;
    int yfin = y + (int)st_bounds.SizeY / 2;

    for (z = zinit; z < zfin; ++z)
    {
        for (y = yinit; y < yfin; ++y)
        {
            for (x = xinit; x < xfin; ++x)
            {
                p.x = x * dose.XRes;
                p.y = y * dose.YRes;
                p.z = z * dose.ZRes;

                p += dose.Origin;

                if (st_bounds.Contains(p.x, p.y, p.z) // trimming
                && st.IsPointInsideSegment(p)) // this is an expensive call
                {

                    int[,] voxel = new int[dose.XSize, dose.YSize];
                    _VD[cpt].AddVoxelToList(st, x, y, z, _context.Image.VoxelToDisplayValue(voxel[x, y]), (float)_context.PlanSetup.Dose.GetDoseToPoint(new VVector(x, y, z)).Dose);
                }
            }
        }
    }

}

/* private void DisplayVoxels()
{
    Model3DGroup group = new Model3DGroup();

    for (int x = 0; x < _context.Image.XSize; x++)
    {
        for (int y = 0; y < _context.Image.YSize; y++)
        {
            for (int z = 0; z < _context.Image.ZSize; z++)
            {

                var color = GetColorFromDose(_VD.Any(v => v.GetX().Any(i => i.Equals(x)) && v.GetY().Any(i => i.Equals(y)) && v.GetZ().Any(i => i.Equals(z)))
? _VD.First(u => u.GetX().Any(i => i.Equals(x)) && u.GetY().Any(j => j.Equals(y)) && u.GetZ().Any(k => k.Equals(z))).GetDose(x, y, z) : 0, _VD.Max(min => min.GetMinDose), _VD.Max(max => max.GetMaxDose));
                var voxel = CreateVoxel(x, y, z, color);
                group.Children.Add(voxel);
            }
        }
    }

    var modelVisual = new ModelVisual3D { Content = group };
    var helixViewport = new HelixViewport3D();
    helixViewport.Children.Add(modelVisual);
}

private GeometryModel3D CreateVoxel(int x, int y, int z, Color color)
{
    var meshBuilder = new MeshBuilder();
    meshBuilder.AddBox(new Point3D(x, y, z), 1, 1, 1);
    var geometry = meshBuilder.ToMesh();

    var material = new DiffuseMaterial(new SolidColorBrush(color));
    return new GeometryModel3D(geometry, material);
}

private Color GetColorFromDose(float dose, float min, float max)
{
    float normalizedDose = (dose - min) / (max - min);
    normalizedDose = Math.Max(0, Math.Min(1, normalizedDose));

    //var jetColor = ColorMap.Jet(normalizedDose);
    var jetColor = Color.FromRgb((byte)(normalizedDose * 255), (byte)((1 - normalizedDose) * 255), (byte)((normalizedDose > 0.5 ? (1 - normalizedDose) : normalizedDose) * 255));

    //return Color.FromArgb((int)(jetColor.Item1 * 255), (int)(jetColor.Item2 * 255), (int)(jetColor.Item3 * 255));
    return jetColor;
}*/


}
