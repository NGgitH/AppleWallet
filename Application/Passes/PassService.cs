using Application.Interfaces;
using Passbook.Generator;
using Passbook.Generator.Fields;

namespace Application.Passes
{
    public class PassService : IPassService
    {
        public PassService() { }

        public async Task GeneratePassAsync()
        {
            PassGenerator generator = new PassGenerator();
            PassGeneratorRequest request = new PassGeneratorRequest();

            request.PassTypeIdentifier = "pass.apple.sportian"; //tipo de identificador : tenemos que poner el id que generamos en apple antes de crear el ceritificado lo primero que hicimos 
                                                               //el de los 3 puntos pass.passbook.advientoexample
            request.TeamIdentifier = "M7J69WP665"; //esto debemos ir a la cuenta de developer en apple en membership en account encontramos el TeamId
            request.Description = "Pase para partido"; //descripcion que no va en la etiqueta
            request.OrganizationName = "Sportian"; //organizacion tampoco se imprime en etiqueta
            request.LogoText = "La Liga"; //titulo encabezado con el logo a la izquierda
            request.BackgroundColor = "#FFFFFF";
            request.LabelColor = "#228581";
            request.ForegroundColor = "#228581";

            request.Style = PassStyle.Generic; //hacemos un pase generico

            //debo agregar los logos que quiero ver en el ticket deben estar dentro de la carpeta resources
            request.Images.Add(PassbookImage.Logo, System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "~/Resources/pc.png")));  //logo izq arriba
            request.Images.Add(PassbookImage.Logo2X, System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "~/Resources/pc@2x.png"))); ////logo izq arriba
           // request.Images.Add(PassbookImage.Thumbnail, System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "~/Resources/code.png")));
          //  request.Images.Add(PassbookImage.Thumbnail2X, System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "~/Resources/code@2x.png")));

            request.AddPrimaryField(new StandardField("title", "", "Barcelona vs Real Madrid")); //Titulo prinicipal debajo de encabezado
            request.AddSecondaryField(new StandardField("primer", "Entrada", "55")); //textos secundarios agregados uno al lado de otro
            request.AddSecondaryField(new StandardField("segundo", "Sección", "Norte"));
            request.AddSecondaryField(new StandardField("tercero", "Fila/Asiento", "D/20"));

            request.SerialNumber = Guid.NewGuid().ToString(); //tiene que ser unico y uso Guid
            request.TransitType = TransitType.PKTransitTypeAir;
            request.SetBarcode(BarcodeType.PKBarcodeFormatQR, "codigo qr", "UTF-8", "Visit barcelona.com");
            request.AddHeaderField(new StandardField("fecha", "Fecha", DateTime.Now.ToString("dd/MM/yyyy")));

            try
            {
                request.Certificate = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "~/Resources/CertificatesPassAdviento.p12")); //archivo p12 con las certificaciones ya aprobadas y generadas
                request.CertificatePassword = ""; //contraseña del certificado que le pusimos al crearlo al p12
                request.AppleWWDRCACertificate = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "~/Resources/AppleWWDRCAG3.cer")); //aca va el certificado generico
                byte[] generatedPass = generator.Generate(request);

                /*
                FileContentResult result = new FileContentResult(generatedPass, "application/vnd.apple.pkpass")
                {
                    FileDownloadName = "UnSimpleDeveloperPase"
                };
                */

                await System.IO.File.WriteAllBytesAsync("Sample.pkpass", generatedPass);
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }


        }
    }
}
