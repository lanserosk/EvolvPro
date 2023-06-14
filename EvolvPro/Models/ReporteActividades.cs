using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.SS.UserModel;
using System.IO;
using static iTextSharp.text.pdf.AcroFields;

namespace EvolvPro.Models
{
    public class ReporteActividades
    {
        public void GenerarPDF(string ruta, EvolvProContext dbContext, int idCronograma)
        {
            //creamos un documento
            Document documento = new Document(PageSize.A4);
            //creamos un escritor
            PdfWriter escritor = PdfWriter.GetInstance(documento, new FileStream(ruta, FileMode.Create));

            //abrimos el documento
            documento.Open();

            //agregamos contenido
            PdfPTable tabla = new PdfPTable(11); //12= cantidad de columnas a incorporar -- PROYECTOS
            
            //usar datos de la base de datos
            var query = from cron in dbContext.Cronogramas
                        join iss in dbContext.Issues on cron.IdCronograma equals iss.FkCronograma
                        join deti in dbContext.DetalleIssues on iss.IdIssue equals deti.FkIssue
                        join cati in dbContext.CategoriaIssues on deti.FkCategoria equals cati.IdCatissue
                        join rec in dbContext.Recursos on cron.FkRecurso equals rec.IdRecurso
                        join est in dbContext.DetalleEstados on cron.FkEstado equals est.IdDetalleestado
                        join dete in dbContext.DetalleEstados on est.IdDetalleestado equals dete.IdDetalleestado
                        where cron.IdCronograma == idCronograma
                        group new { cron, iss, cati, dete, rec, est } by iss.IdIssue into g
                        select new
                        {
                            idIssue = g.Key,
                            titulo = g.Select(x => x.iss.TituloIssue).FirstOrDefault(),
                            Description = g.Select(x => x.iss.DescripcionIssue).FirstOrDefault(),
                            Fecha = g.Select(x => x.iss.FechaIssue).FirstOrDefault(),
                            FechaCierre = g.Select(x => x.iss.FechaCierre).FirstOrDefault(),
                            Resolucion = g.Select(x => x.iss.ResolucionIssue).FirstOrDefault(),
                            Horas = g.Select(x => x.iss.HorasIssue).FirstOrDefault(),
                            Estado = g.Select(x => x.dete.ValorDestado).FirstOrDefault(),
                            Crono = g.Select(x => x.cron.NombreCrgm).FirstOrDefault(),
                            Recursos = g.Select(x => x.rec.NombreRec).FirstOrDefault(),
                            CategoriaI = g.Select(x => x.cati.Nombre).FirstOrDefault()
                        };

            //agregamos un encabezado
            Paragraph paragraph = new Paragraph();
            //agregar fecha
            DateTime currentDate = DateTime.Now;
            paragraph.Add("Fecha: " + currentDate.ToString("dd/MM/yyyy"));
            paragraph.Alignment = Element.ALIGN_RIGHT;
            documento.Add(paragraph);

            ////agregamos un logo
            string ruta_logo = "./Resource/logo.png";
            Image logo = Image.GetInstance(ruta_logo);
            logo.ScaleToFit(75, 75);
            logo.SetAbsolutePosition(50, 780);
            documento.Add(logo);

            //Nombre del actividad
            Paragraph nomAct = new Paragraph();
            foreach (var item in query)
            {
                Chunk chunkNomActividad = new Chunk(item.Crono.ToString());
                // Agregar los Chunks al Paragraph
                nomAct.Add(chunkNomActividad);
                nomAct.Alignment = Element.ALIGN_CENTER;
                nomAct.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 36);
                // Agregar un salto de línea
                nomAct.Add(Environment.NewLine);
                nomAct.Add(Environment.NewLine);
                nomAct.Add(Environment.NewLine);

            }
            documento.Add(nomAct);

            //agregamos los datos a la tabla proyectos
            //encabezados

            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] { 0.5f, 1.5f, 3f, 1.2f, 1.2f, 3f, 0.7f, 1.2f, 2f, 2f, 1.5f });

            PdfPCell cell1 = new PdfPCell(new Phrase("ID: "));
            cell1.BackgroundColor = new BaseColor(110, 208, 242);           
            tabla.AddCell(cell1);
           
            PdfPCell cell2 = new PdfPCell(new Phrase("Problema: "));
            cell2.BackgroundColor = new BaseColor(110, 208, 242);          
            tabla.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase("Descripcion: "));
            cell3.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase("Inicio: "));
            cell4.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell4);

            PdfPCell cell5 = new PdfPCell(new Phrase("Cierre: "));
            cell5.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell5);
           
            PdfPCell cell6 = new PdfPCell(new Phrase("Resolucion: "));
            cell6.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell2);

            PdfPCell cell7 = new PdfPCell(new Phrase("Horas: "));
            cell7.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell7);

            PdfPCell cell8 = new PdfPCell(new Phrase("Estado: "));
            cell8.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell8);

            PdfPCell cell9 = new PdfPCell(new Phrase("Actividad: "));
            cell9.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell9);
            
            PdfPCell cell10 = new PdfPCell(new Phrase("Recurso: "));
            cell10.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell10);

            PdfPCell cell11 = new PdfPCell(new Phrase("Categoria: "));
            cell11.BackgroundColor = new BaseColor(110, 208, 242);
            tabla.AddCell(cell11);
          

            //cuerpo
            foreach (var item in query)
            {
                PdfPCell idCronoCell = new PdfPCell(new Phrase(item.idIssue.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));               
                tabla.AddCell(idCronoCell);

                PdfPCell tituloIssue = new PdfPCell(new Phrase(item.titulo.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));                
                tabla.AddCell(tituloIssue);

                PdfPCell descripcionCell = new PdfPCell(new Phrase(item.Description.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));                
                tabla.AddCell(descripcionCell);

                PdfPCell fechaCell = new PdfPCell(new Phrase(item.Fecha.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));                    
                tabla.AddCell(fechaCell);

                PdfPCell fechaCierre = new PdfPCell(new Phrase(item.FechaCierre.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla.AddCell(fechaCierre);

                string resolucionValue = item.Resolucion != null ? item.Resolucion.ToString() : string.Empty;
                PdfPCell resolucion = new PdfPCell(new Phrase(resolucionValue, new Font(Font.FontFamily.HELVETICA, 8)));

                tabla.AddCell(resolucion);

                PdfPCell horas = new PdfPCell(new Phrase(item.Horas.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla.AddCell(horas);

                PdfPCell estado = new PdfPCell(new Phrase(item.Estado.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla.AddCell(estado);

                PdfPCell crono = new PdfPCell(new Phrase(item.Crono.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla.AddCell(crono);

                PdfPCell recur = new PdfPCell(new Phrase(item.Recursos.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla.AddCell(recur);

                PdfPCell catI = new PdfPCell(new Phrase(item.CategoriaI.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla.AddCell(catI);

            }
            documento.Add(tabla);          
            documento.NewPage();
            //cerramos el documento
            documento.Close();
        }
    }
}
