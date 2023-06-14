using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using static iTextSharp.text.pdf.AcroFields;

namespace EvolvPro.Models
{
    public class ReporteProyecto
    {
        public void GenerarPDF(string ruta, EvolvProContext dbContext, int idProyecto)
        {
            //creamos un documento
            Document documento = new Document(PageSize.A4);
            //creamos un escritor
            PdfWriter escritor = PdfWriter.GetInstance(documento, new FileStream(ruta, FileMode.Create));

            //abrimos el documento
            documento.Open();

            //agregamos contenido
            PdfPTable tabla = new PdfPTable(4); //12= cantidad de columnas a incorporar -- PROYECTOS
             //8= cantidad de columnas a incorporar -- REUNIONES
            PdfPTable tabla3 = new PdfPTable(9); //9= cantidad de columnas a incorporar -- CRONOGRAMA

            //usar datos de la base de datos
            //var data = dbContext.Proyectos.Where(x => x.IdProyecto == idProyecto).ToList();
            var data = from proy in dbContext.Proyectos
                       join cli in dbContext.Clientes on proy.FkCliente equals cli.IdCliente
                       join usu in dbContext.Usuarios on proy.FkUsuario equals usu.IdUsuario
                       join tusu in dbContext.TipoUsuarios on usu.FkTipousu equals tusu.IdTipousu
                       join est in dbContext.Estados on proy.FkEstado equals est.IdEstado
                       where proy.IdProyecto == idProyecto
                       select new
                       {
                           proy.IdProyecto,
                           proy.NombrePry,
                           proy.CasoNegocio,
                           proy.HorasTotales,
                           proy.HorasTotalesreal,
                           proy.Interesados,
                           proy.FechaInicio,
                           proy.FechaFinalProp,
                           proy.FechaFinalReal,
                           NombreCliente = cli.NombreCliente,
                           PMUsu = usu.NombreUsu,
                           Estados = est.Nombre
                          
                       };
            var pmUs = from proy in dbContext.Proyectos
                     join usu in dbContext.Usuarios on proy.FkUsuario equals usu.IdUsuario
                     join tusu in dbContext.TipoUsuarios on usu.FkTipousu equals tusu.IdTipousu
                     where proy.IdProyecto == idProyecto && tusu.IdTipousu ==  2                 
                     select new
                     {
                         PMUser = usu.NombreUsu                      
                     };



            var pm = from proy in dbContext.Proyectos
                     join est in dbContext.Estados on proy.FkEstado equals est.IdEstado
                     join dete in dbContext.DetalleEstados on proy.FkEstado equals dete.IdDetalleestado                     
                     where proy.IdProyecto == idProyecto
                     select new
                     {
                         EstadoProy = dete.ValorDestado,
                         Interesados = proy.Interesados
                     };


            var data2 = from reu in dbContext.Reuniones
                        join prj in dbContext.Proyectos on reu.FkProyecto equals prj.IdProyecto
                        where prj.IdProyecto == idProyecto
                        select new
                        {
                            reu.IdReunion,
                            reu.TituloReu,
                            reu.PuntosTratar,
                            reu.DesarrolloPunto,
                            reu.Asistentes,
                            reu.TiempoReu,
                            reu.FechaReunion,
                            NombreProy = prj.NombrePry                            
                        };
            var data3 = from cron in dbContext.Cronogramas
                        join prj in dbContext.Proyectos on cron.FkProyecto equals prj.IdProyecto
                        join est in dbContext.Estados on prj.FkEstado equals est.IdEstado
                        join dete in dbContext.DetalleEstados on est.IdEstado equals dete.FkEstado
                        join rolh in dbContext.RolHoras on prj.IdProyecto equals rolh.FkProyecto
                        join rec in dbContext.Recursos on cron.FkRecurso equals rec.IdRecurso
                        where prj.IdProyecto == idProyecto
                        select new
                        {
                            cron.IdCronograma,
                            cron.NombreCrgm,
                            cron.DescripcionCrgm,
                            cron.HorasCrgm,
                            cron.Jerarquia,
                            NomProy = prj.NombrePry,                           
                            EstadoAct = dete.ValorDestado,
                            ValRolh = rolh.ValorHora,                           
                            NomRec = rec.NombreRec
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

            //Nombre del proyecto
            Paragraph nomPry = new Paragraph();
            foreach (var item in data)
            {                
                Chunk chunkNombrePry = new Chunk(item.NombrePry);    
                // Agregar los Chunks al Paragraph
                nomPry.Add(chunkNombrePry);
                nomPry.Alignment = Element.ALIGN_CENTER;
                nomPry.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 36);
                // Agregar un salto de línea
                nomPry.Add(Environment.NewLine);
                
            }
            documento.Add(nomPry);

            //Horas
            Paragraph horaPry = new Paragraph();
            foreach (var item in data)
            {
                Chunk chunkHoraActPry = new Chunk(item.HorasTotalesreal.ToString());
                // Agregar los Chunks al Paragraph
                horaPry.Add(chunkHoraActPry);
                Chunk HoraTPry = new Chunk(" horas actuales de: ");
                horaPry.Add(HoraTPry);
                Chunk chunkHoraPry = new Chunk(item.HorasTotales.ToString());//
                // Agregar los Chunks al Paragraph
                horaPry.Add(chunkHoraPry);
                Chunk TPry = new Chunk(" horas propuestas");
                horaPry.Add(TPry);
                horaPry.Alignment = Element.ALIGN_CENTER;
                horaPry.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 36);
                // Agregar un salto de línea
                horaPry.Add(Environment.NewLine);
                horaPry.Add(Environment.NewLine);
                horaPry.Add(Environment.NewLine);               
            }
            documento.Add(horaPry);

            //agregamos los datos a la tabla proyectos
            //encabezados

            PdfPCell cell1 = new PdfPCell(new Phrase("Fecha Inicio: "));
            //cell1.BackgroundColor = new BaseColor(110, 208, 242);
            cell1.Border = PdfPCell.NO_BORDER;
            tabla.AddCell(cell1);

            DateTime currentDate1 = DateTime.Now;
            PdfPCell cell2 = new PdfPCell(new Phrase("Fecha Actual: "));
            //cell2.BackgroundColor = new BaseColor(110, 208, 242);
            cell2.Border = PdfPCell.NO_BORDER;
            tabla.AddCell(cell2);

            PdfPCell cell3 = new PdfPCell(new Phrase("Fecha Final Presupuestada: "));
            //cell3.BackgroundColor = new BaseColor(110, 208, 242);
            cell3.Border = PdfPCell.NO_BORDER;
            tabla.AddCell(cell3);

            PdfPCell cell4 = new PdfPCell(new Phrase("Fecha Final: "));
            //cell4.BackgroundColor = new BaseColor(110, 208, 242);
            cell4.Border = PdfPCell.NO_BORDER;
            tabla.AddCell(cell4);

            //cuerpo
            foreach (var item in data)
            {
                PdfPCell fechaInicioCell = new PdfPCell(new Phrase(item.FechaInicio.ToString()));
                fechaInicioCell.Border = PdfPCell.NO_BORDER;
                tabla.AddCell(fechaInicioCell);

                PdfPCell currentDateCell = new PdfPCell(new Phrase(currentDate1.ToString()));
                currentDateCell.Border = PdfPCell.NO_BORDER;
                tabla.AddCell(currentDateCell);

                PdfPCell fechaFinalPropCell = new PdfPCell(new Phrase(item.FechaFinalProp.ToString()));
                fechaFinalPropCell.Border = PdfPCell.NO_BORDER;
                tabla.AddCell(fechaFinalPropCell);

                PdfPCell fechaFinalRealCell = new PdfPCell(new Phrase(item.FechaFinalReal.ToString()));
                fechaFinalRealCell.Border = PdfPCell.NO_BORDER;
                tabla.AddCell(fechaFinalRealCell);  
                
            }          
            documento.Add(tabla);

            //Tabla projectM
            Paragraph pryMan = new Paragraph();
            foreach (var item in pmUs)
            {
                pryMan.Add(Environment.NewLine);
                Chunk PmPry = new Chunk("Project Manager:");
                pryMan.Add(PmPry);
                Chunk chunkPm = new Chunk(item.PMUser.ToString());
                // Agregar los Chunks al Paragraph
                pryMan.Add(chunkPm);
                pryMan.Alignment = Element.ALIGN_LEFT;
                //tablaiz.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                // Agregar un salto de línea
                pryMan.Add(Environment.NewLine);
            }
            documento.Add(pryMan);
            pryMan.Alignment = Element.ALIGN_LEFT;

            //tabla izquierda
            Paragraph tablaiz = new Paragraph();
            foreach (var item in data)
            {
                Chunk ClientePry = new Chunk("Cliente:");
                tablaiz.Add(ClientePry);
                Chunk chunkClientePry = new Chunk(item.NombreCliente.ToString());
                // Agregar los Chunks al Paragraph
                tablaiz.Add(chunkClientePry);
                tablaiz.Alignment = Element.ALIGN_LEFT;
                //tablaiz.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                // Agregar un salto de línea
                tablaiz.Add(Environment.NewLine);

                Chunk CasoNegPry = new Chunk("Caso de Negocio:");
                tablaiz.Add(CasoNegPry);
                Chunk chunkCasoNeg = new Chunk(item.CasoNegocio.ToString());
                // Agregar los Chunks al Paragraph
                tablaiz.Add(chunkCasoNeg);
                tablaiz.Alignment = Element.ALIGN_LEFT;
                // tablaiz.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                tablaiz.Add(Environment.NewLine);
            }            
            documento.Add(tablaiz);
            tablaiz.Alignment = Element.ALIGN_LEFT;       

            //Tabla derecha
            Paragraph tablader = new Paragraph();
            foreach (var item in pm)
            {
                Chunk EstadoPry = new Chunk("Estado del proyecto:");
                tablader.Add(EstadoPry);
                Chunk chunkEstadoPry = new Chunk(item.EstadoProy.ToString());
                // Agregar los Chunks al Paragraph
                tablader.Add(chunkEstadoPry);
                tablader.Alignment = Element.ALIGN_RIGHT;
                //tablaiz.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                // Agregar un salto de línea
                tablader.Add(Environment.NewLine);

                Chunk InteresadoPry = new Chunk("Interesados:");
                tablader.Add(InteresadoPry);
                Chunk chunkInt = new Chunk(item.Interesados.ToString());
                // Agregar los Chunks al Paragraph
                tablader.Add(chunkInt);
                tablader.Alignment = Element.ALIGN_RIGHT;
                //tablaiz.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                // Agregar un salto de línea
                
            }
            documento.Add(tablader);
            tablader.Alignment = Element.ALIGN_RIGHT;            
            //documento.NewPage();

            //agregamos un encabezado
            Paragraph paragraph2 = new Paragraph();
            //agregar fecha
            DateTime currentDate2 = DateTime.Now;
            paragraph2.Add("Fecha: " + currentDate2.ToString("dd/MM/yyyy"));
            paragraph2.Alignment = Element.ALIGN_RIGHT;
            documento.Add(paragraph2);

            //titulo
            Paragraph titulo2 = new Paragraph("Reuniones");
            titulo2.Add(Environment.NewLine); //salto de linea
            titulo2.Add(Environment.NewLine);
            titulo2.Alignment = Element.ALIGN_CENTER;
            titulo2.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
            documento.Add(titulo2);

            ////agregamos un logo
            string ruta_logo2 = "./Resource/logo.png";
            Image logo2 = Image.GetInstance(ruta_logo2);
            logo2.ScaleToFit(75, 75);
            logo2.SetAbsolutePosition(50, 780);
            documento.Add(logo2);

            PdfPTable tabla2 = new PdfPTable(8);
            tabla2.WidthPercentage = 100;
            tabla2.SetWidths(new float[] { 0.7f, 1.5f, 2f, 2f, 2f, 1.5f, 1.5f, 2f });
            //agregamos los datos a la tabla reuniones
            //encabezados
            PdfPCell celda1 = new PdfPCell(new Phrase("ID"));
            celda1.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda1);

            PdfPCell celda2 = new PdfPCell(new Phrase("Reunion"));
            celda2.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda2);

            PdfPCell celda3 = new PdfPCell(new Phrase("Puntos a tratar"));
            celda3.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda3);

            PdfPCell celda4 = new PdfPCell(new Phrase("Desarrollo punto"));
            celda4.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda4);

            PdfPCell celda5 = new PdfPCell(new Phrase("Asistentes"));
            celda5.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda5);

            PdfPCell celda6 = new PdfPCell(new Phrase("Tiempo"));
            celda6.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda6);

            PdfPCell celda7 = new PdfPCell(new Phrase("Fecha Reunion"));
            celda7.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda7);

            PdfPCell celda8 = new PdfPCell(new Phrase("Proyecto"));
            celda8.BackgroundColor = new BaseColor(110, 208, 242);
            tabla2.AddCell(celda8);

            //cuerpo
            foreach (var item in data2)
            {
                tabla2.AddCell(new Phrase(item.IdReunion.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla2.AddCell(new Phrase(item.TituloReu.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla2.AddCell(new Phrase(item.PuntosTratar.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla2.AddCell(new Phrase(item.DesarrolloPunto.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla2.AddCell(new Phrase(item.Asistentes.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla2.AddCell(new Phrase(item.TiempoReu.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla2.AddCell(new Phrase(item.FechaReunion.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla2.AddCell(new Phrase(item.NombreProy.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));

            }
            documento.Add(tabla2);
            //documento.NewPage();

            //agregamos un encabezado
            Paragraph paragraph3 = new Paragraph();
            //agregar fecha
            DateTime currentDate3 = DateTime.Now;
            paragraph3.Add("Fecha: " + currentDate3.ToString("dd/MM/yyyy"));
            paragraph3.Alignment = Element.ALIGN_RIGHT;
            documento.Add(paragraph3);

            //titulo
            Paragraph titulo3 = new Paragraph("Actividades");
            titulo3.Add(Environment.NewLine); //salto de linea
            titulo3.Add(Environment.NewLine);
            titulo3.Alignment = Element.ALIGN_CENTER;
            titulo3.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
            documento.Add(titulo3);

            ////agregamos un logo
            string ruta_logo3 = "./Resource/logo.png";
            Image logo3 = Image.GetInstance(ruta_logo3);
            logo3.ScaleToFit(75, 75);
            logo3.SetAbsolutePosition(50, 780);
            documento.Add(logo3);


            //agregamos los datos a la tabla cronogramas
            //encabezados
            tabla3.WidthPercentage = 100;
            tabla3.SetWidths(new float[] { 0.5f, 1.5f, 3f, 1f, 1f, 2f, 1.1f, 1f, 1f });
            PdfPCell cel1 = new PdfPCell(new Phrase("ID"));
            cel1.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel1);

            PdfPCell cel2 = new PdfPCell(new Phrase("Nombre"));
            cel2.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel2);

            PdfPCell cel3 = new PdfPCell(new Phrase("Descripcion"));
            cel3.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel3);

            PdfPCell cel4 = new PdfPCell(new Phrase("Horas"));
            cel4.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel4);

            PdfPCell cel5 = new PdfPCell(new Phrase("Jerarquia"));
            cel5.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel5);

            PdfPCell cel6 = new PdfPCell(new Phrase("Proyecto"));
            cel6.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel6);

            PdfPCell cel7 = new PdfPCell(new Phrase("Estado"));
            cel7.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel7);

            PdfPCell cel8 = new PdfPCell(new Phrase("Rol_hora"));
            cel8.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel8);

            PdfPCell cel9 = new PdfPCell(new Phrase("Recurso"));
            cel9.BackgroundColor = new BaseColor(110, 208, 242);
            tabla3.AddCell(cel9);

            //cuerpo
            foreach (var item in data3)
            {
                tabla3.AddCell(new Phrase(item.IdCronograma.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.NombreCrgm.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.DescripcionCrgm.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.HorasCrgm.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.Jerarquia.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.NomProy.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.EstadoAct.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.ValRolh.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
                tabla3.AddCell(new Phrase(item.NomRec.ToString(), new Font(Font.FontFamily.HELVETICA, 8)));
            }
            documento.Add(tabla3);
            documento.NewPage();
      
            //cerramos el documento
            documento.Close();
        }
    }
}
