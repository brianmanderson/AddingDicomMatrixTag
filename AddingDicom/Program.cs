using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FellowOakDicom;
namespace AddingDicom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string base_path = @"M:\KuznetsovaS\DICOM Example\REG_test4";
            string dicom_file = Path.Combine(base_path, "test.dcm");
            DicomFile RT_file = DicomFile.Open(dicom_file, FileReadOption.ReadAll);
            DicomSequence DeformableRegistrationSequence = RT_file.Dataset.GetDicomItem<DicomSequence>(DicomTag.DeformableRegistrationSequence);
            DicomDataset DeformableRegistrationDataset = DeformableRegistrationSequence.Items[1];
            DicomSequence PreDeformationMatrixSequence = DeformableRegistrationDataset.GetDicomItem<DicomSequence>(DicomTag.PreDeformationMatrixRegistrationSequence);
            DicomDataset PreDeformationMatrixDataset = PreDeformationMatrixSequence.Items[0];

            DicomDataset new_matrix = new DicomDataset();
            new_matrix.AddOrUpdate(DicomTag.FrameOfReferenceTransformationMatrixType, "RIGID");
            DicomDataset postdataset = new DicomDataset(PreDeformationMatrixDataset);
            //float[] testing = postdataset.GetValues<float>(DicomTag.FrameOfReferenceTransformationMatrix);
            float[] new_values = { 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F };
            postdataset.AddOrUpdate(DicomTag.FrameOfReferenceTransformationMatrix, new_values);
            DeformableRegistrationDataset.AddOrUpdate(DicomTag.PostDeformationMatrixRegistrationSequence, postdataset);
            RT_file.Dataset.AddOrUpdate(DicomTag.SeriesInstanceUID, DicomUIDGenerator.GenerateDerivedFromUUID());
            RT_file.Dataset.AddOrUpdate(DicomTag.ManufacturerModelName, "Brian M Anderson");
            RT_file.Save(Path.Combine(base_path, "New.dcm"));
        }
    }
}
