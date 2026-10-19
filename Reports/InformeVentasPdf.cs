
using System;
using System.Globalization;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using sistema_gestion_heladeria.Models;

namespace sistema_gestion_heladeria.Reports
{
    public static class InformeVentasPdf
    {
        private static readonly CultureInfo Cultura =
            CultureInfo.GetCultureInfo("es-AR");

        public static void Generar(
            InformeVentasDTO informe,
            string rutaArchivo)
        {
            if (informe == null)
                throw new ArgumentNullException(nameof(informe));

            if (informe.Periodo == null || informe.Resumen == null)
                throw new ArgumentException("El informe está incompleto.");

            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("La ruta no es válida.");

            Document documento = new Document();

            documento.Info.Title = "Informe de ventas - Frazzé";
            documento.Info.Author = "Frazzé";

            ConfigurarEstilos(documento);

            Section seccion = documento.AddSection();

            seccion.PageSetup.PageFormat = PageFormat.A4;
            seccion.PageSetup.TopMargin = Unit.FromCentimeter(2);
            seccion.PageSetup.BottomMargin = Unit.FromCentimeter(2);
            seccion.PageSetup.LeftMargin = Unit.FromCentimeter(2);
            seccion.PageSetup.RightMargin = Unit.FromCentimeter(2);

            AgregarEncabezado(seccion, informe);
            AgregarResumen(seccion, informe);
            AgregarEstados(seccion, informe);
            AgregarMediosPago(seccion, informe);
            AgregarVentasPorDia(seccion, informe);
            AgregarProductos(seccion, informe);
            AgregarSabores(seccion, informe);
            AgregarDetalleVentas(seccion, informe);
            AgregarPiePagina(seccion);

            PdfDocumentRenderer renderer =
                new PdfDocumentRenderer(true);

            renderer.Document = documento;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(rutaArchivo);
        }

        private static void ConfigurarEstilos(Document documento)
        {
            Style normal = documento.Styles["Normal"];
            normal.Font.Name = "Arial";
            normal.Font.Size = 9;
            normal.ParagraphFormat.SpaceAfter =
                Unit.FromPoint(4);
        }

        private static void AgregarEncabezado(
            Section seccion,
            InformeVentasDTO informe)
        {
            Paragraph titulo = seccion.AddParagraph("FRAZZÉ");

            titulo.Format.Font.Size = 20;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceAfter = Unit.FromPoint(5);

            Paragraph subtitulo =
                seccion.AddParagraph("Informe de ventas");

            subtitulo.Format.Font.Size = 14;
            subtitulo.Format.SpaceAfter = Unit.FromPoint(12);

            seccion.AddParagraph(
                "Período: " +
                FormatearFecha(informe.Periodo.Desde) +
                " al " +
                FormatearFecha(informe.Periodo.Hasta));

            seccion.AddParagraph(
                "Fecha de generación: " +
                DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

            seccion.AddParagraph();
        }

        // Resumen en tres columnas.
        private static void AgregarResumen(
            Section seccion,
            InformeVentasDTO informe)
        {
            AgregarTitulo(seccion, "Resumen general");

            Table tabla = seccion.AddTable();

            tabla.Borders.Width = 0.5;

            tabla.AddColumn(Unit.FromCentimeter(5.3));
            tabla.AddColumn(Unit.FromCentimeter(5.3));
            tabla.AddColumn(Unit.FromCentimeter(5.4));

            Row encabezado = tabla.AddRow();
            encabezado.Shading.Color = Colors.LightGray;
            encabezado.Format.Font.Bold = true;

            encabezado.Cells[0].AddParagraph("Cantidad de ventas");
            encabezado.Cells[1].AddParagraph("Total registrado");
            encabezado.Cells[2].AddParagraph("Ticket promedio");

            Row valores = tabla.AddRow();

            valores.Cells[0].AddParagraph(
                informe.Resumen.CantidadVentas.ToString());

            valores.Cells[1].AddParagraph(
                Dinero(informe.Resumen.TotalRegistrado));

            valores.Cells[2].AddParagraph(
                Dinero(informe.Resumen.TicketPromedio));

            valores.Cells[0].Format.Alignment =
                ParagraphAlignment.Center;

            valores.Cells[1].Format.Alignment =
                ParagraphAlignment.Right;

            valores.Cells[2].Format.Alignment =
                ParagraphAlignment.Right;

            seccion.AddParagraph();
        }

        private static void AgregarEstados(
            Section seccion,
            InformeVentasDTO informe)
        {
            AgregarTitulo(seccion, "Ventas por estado");

            Table tabla = CrearTabla(
                seccion,
                new double[] { 6, 3, 7 },
                "Estado", "Cantidad", "Importe");

            var datos = informe.Resumen.VentasPorEstado;

            if (datos != null)
            {
                foreach (var item in datos.OrderBy(x => x.Key))
                {
                    AgregarFila(
                        tabla,
                        item.Key,
                        item.Value.Cantidad.ToString(),
                        Dinero(item.Value.Importe));
                }
            }

            AlinearColumnas(tabla, 1, 2);
            AgregarFilaVaciaSiCorresponde(tabla, 3);
        }

        private static void AgregarMediosPago(
            Section seccion,
            InformeVentasDTO informe)
        {
            AgregarTitulo(seccion, "Ventas por medio de pago");

            Table tabla = CrearTabla(
                seccion,
                new double[] { 6, 3, 7 },
                "Medio de pago", "Cantidad", "Importe");

            var datos = informe.Resumen.VentasPorMedioPago;

            if (datos != null)
            {
                foreach (var item in datos.OrderBy(x => x.Key))
                {
                    AgregarFila(
                        tabla,
                        item.Key,
                        item.Value.Cantidad.ToString(),
                        Dinero(item.Value.Importe));
                }
            }

            AlinearColumnas(tabla, 1, 2);
            AgregarFilaVaciaSiCorresponde(tabla, 3);
        }

        private static void AgregarVentasPorDia(
            Section seccion,
            InformeVentasDTO informe)
        {
            AgregarTitulo(seccion, "Ventas por día");

            Table tabla = CrearTabla(
                seccion,
                new double[] { 6, 3, 7 },
                "Fecha", "Cantidad", "Importe");

            var datos = informe.Resumen.VentasPorDia;

            if (datos != null)
            {
                foreach (var item in datos.OrderBy(x => x.Key))
                {
                    AgregarFila(
                        tabla,
                        FormatearFecha(item.Key),
                        item.Value.Cantidad.ToString(),
                        Dinero(item.Value.Importe));
                }
            }

            AlinearColumnas(tabla, 1, 2);
            AgregarFilaVaciaSiCorresponde(tabla, 3);
        }

        private static void AgregarProductos(
            Section seccion,
            InformeVentasDTO informe)
        {
            AgregarTitulo(seccion, "Productos vendidos");

            Table tabla = CrearTabla(
                seccion,
                new double[] { 7, 3, 6 },
                "Producto", "Unidades", "Importe");

            if (informe.ProductosVendidos != null)
            {
                foreach (var producto in informe.ProductosVendidos
                    .OrderByDescending(x => x.CantidadVendida))
                {
                    AgregarFila(
                        tabla,
                        producto.Producto,
                        producto.CantidadVendida.ToString(),
                        Dinero(producto.ImporteTotal));
                }
            }

            AlinearColumnas(tabla, 1, 2);
            AgregarFilaVaciaSiCorresponde(tabla, 3);
        }

        private static void AgregarSabores(
            Section seccion,
            InformeVentasDTO informe)
        {
            // Mantener esta sección en una página cuando quepa.
            // Si no cabe completa, MigraDoc podrá dividirla.
            Paragraph titulo =
                seccion.AddParagraph("Sabores más elegidos");

            titulo.Format.Font.Size = 12;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceBefore = Unit.FromPoint(14);
            titulo.Format.SpaceAfter = Unit.FromPoint(7);
            titulo.Format.KeepWithNext = true;

            Table tabla = CrearTabla(
                seccion,
                new double[] { 7, 4, 5 },
                "Sabor", "Selecciones", "Porcentaje");

            int total = informe.Resumen.TotalSeleccionesSabores;

            if (informe.SaboresVendidos != null)
            {
                foreach (var sabor in informe.SaboresVendidos
                    .OrderByDescending(x => x.VecesElegido))
                {
                    double porcentaje = total > 0
                        ? (double)sabor.VecesElegido * 100 / total
                        : 0;

                    AgregarFila(
                        tabla,
                        sabor.Sabor,
                        sabor.VecesElegido.ToString(),
                        porcentaje.ToString("N2", Cultura) + " %");
                }
            }

            AlinearColumnas(tabla, 1, 2);
            AgregarFilaVaciaSiCorresponde(tabla, 3);

            // Vincular las filas para evitar cortes innecesarios.
            // Solo se aplica cuando la tabla tiene un tamaño razonable.
            if (tabla.Rows.Count <= 16)
            {
                for (int i = 0; i < tabla.Rows.Count - 1; i++)
                {
                    tabla.Rows[i].KeepWith = 1;
                }
            }

            Paragraph totalSabores = seccion.AddParagraph(
                "Total de selecciones de sabores: " + total);

            totalSabores.Format.SpaceBefore =
                Unit.FromPoint(8);

            seccion.AddParagraph(
                "El porcentaje corresponde a las selecciones " +
                "de sabores registradas, no al volumen vendido.");
        }

        private static void AgregarDetalleVentas(
            Section seccion,
            InformeVentasDTO informe)
        {
            AgregarTitulo(seccion, "Detalle individual de ventas");

            Table tabla = CrearTabla(
                seccion,
                new double[] { 2, 3, 4, 4, 3 },
                "N.º", "Fecha", "Estado", "Medio de pago", "Total");

            if (informe.Ventas != null)
            {
                foreach (var venta in informe.Ventas
                    .OrderBy(x => x.FechaLocal)
                    .ThenBy(x => x.IdVenta))
                {
                    AgregarFila(
                        tabla,
                        venta.IdVenta.ToString(),
                        FormatearFecha(venta.FechaLocal),
                        venta.Estado,
                        venta.MedioPago,
                        Dinero(venta.Total));
                }
            }

            AgregarFilaVaciaSiCorresponde(tabla, 5);

            // Alinear los importes.
            for (int i = 1; i < tabla.Rows.Count; i++)
            {
                tabla.Rows[i].Cells[0].Format.Alignment =
                    ParagraphAlignment.Center;

                tabla.Rows[i].Cells[4].Format.Alignment =
                    ParagraphAlignment.Right;
            }

            // Mantener la última venta junto al total general.
            if (tabla.Rows.Count > 1)
            {
                tabla.Rows[tabla.Rows.Count - 1].KeepWith = 1;
            }

            // Total general al finalizar el detalle.
            Row filaTotal = tabla.AddRow();
            filaTotal.Shading.Color = Colors.LightGray;
            filaTotal.Format.Font.Bold = true;

            filaTotal.Cells[0].MergeRight = 3;
            filaTotal.Cells[0].AddParagraph("TOTAL GENERAL");

            filaTotal.Cells[4].AddParagraph(
                Dinero(informe.Resumen.TotalRegistrado));

            filaTotal.Cells[4].Format.Alignment =
                ParagraphAlignment.Right;
        }


        private static void AgregarPiePagina(Section seccion)
        {
            Paragraph pie =
                seccion.Footers.Primary.AddParagraph();

            pie.Format.Alignment = ParagraphAlignment.Center;
            pie.Format.Font.Size = 8;

            pie.AddText("Frazzé - Informe de ventas | Página ");
            pie.AddPageField();
        }

        private static void AgregarTitulo(
            Section seccion,
            string texto)
        {
            Paragraph titulo = seccion.AddParagraph(texto);

            titulo.Format.Font.Size = 12;
            titulo.Format.Font.Bold = true;
            titulo.Format.SpaceBefore = Unit.FromPoint(14);
            titulo.Format.SpaceAfter = Unit.FromPoint(7);
            titulo.Format.KeepWithNext = true;
        }

        private static Table CrearTabla(
            Section seccion,
            double[] anchos,
            params string[] encabezados)
        {
            Table tabla = seccion.AddTable();

            tabla.Borders.Width = 0.5;
            tabla.Rows.LeftIndent = 0;

            foreach (double ancho in anchos)
                tabla.AddColumn(Unit.FromCentimeter(ancho));

            Row encabezado = tabla.AddRow();
            encabezado.HeadingFormat = true;
            encabezado.Shading.Color = Colors.LightGray;
            encabezado.Format.Font.Bold = true;

            for (int i = 0; i < encabezados.Length; i++)
                encabezado.Cells[i].AddParagraph(encabezados[i]);

            seccion.AddParagraph();

            return tabla;
        }

        private static void AgregarFila(
            Table tabla,
            params string[] valores)
        {
            Row fila = tabla.AddRow();

            for (int i = 0; i < valores.Length; i++)
            {
                fila.Cells[i].AddParagraph(
                    string.IsNullOrWhiteSpace(valores[i])
                        ? "-"
                        : valores[i]);
            }
        }

        private static void AlinearColumnas(
            Table tabla,
            int columnaCantidad,
            int columnaImporte)
        {
            for (int i = 1; i < tabla.Rows.Count; i++)
            {
                tabla.Rows[i].Cells[columnaCantidad]
                    .Format.Alignment = ParagraphAlignment.Center;

                tabla.Rows[i].Cells[columnaImporte]
                    .Format.Alignment = ParagraphAlignment.Right;
            }
        }

        private static void AgregarFilaVaciaSiCorresponde(
            Table tabla,
            int columnas)
        {
            if (tabla.Rows.Count > 1)
                return;

            string[] valores = new string[columnas];
            valores[0] = "Sin datos";

            for (int i = 1; i < columnas; i++)
                valores[i] = "-";

            AgregarFila(tabla, valores);
        }

        private static string Dinero(decimal importe)
        {
            return importe.ToString("C2", Cultura);
        }

        private static string FormatearFecha(string fecha)
        {
            DateTime resultado;

            if (DateTime.TryParseExact(
                fecha,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out resultado))
            {
                return resultado.ToString("dd/MM/yyyy");
            }

            return fecha ?? "-";
        }
    }
}
