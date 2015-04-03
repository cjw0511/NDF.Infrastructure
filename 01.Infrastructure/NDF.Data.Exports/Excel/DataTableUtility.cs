using Aspose.Cells;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Exports.Excel
{
    /// <summary>
    /// 提供一组用于将 <see cref="DataTable"/> 数据表格转换成 Excel 表格对象 <see cref="Workbook"/> 的扩展方法。
    /// </summary>
    public static class DataTableUtility
    {

        /// <summary>
        /// 将 <see cref="System.Data.DataTable"/> 数据表格对象中的所有数据导出到 Aspose.Cells 工作簿中的 sheet 表格对象 <see cref="Aspose.Cells.Worksheet"/> 中。
        /// </summary>
        /// <param name="_this">要导出数据的 <see cref="System.Data.DataTable"/> 数据表格对象。</param>
        /// <param name="sheet">要进行数据导出的目标 Aspose.Cells 表格工作簿对象 <see cref="Aspose.Cells.Workbook"/>。</param>
        public static void ExportToWorkbook(this DataTable _this, Worksheet sheet)
        {
            Check.NotNull(_this);
            Check.NotNull(sheet);

            Workbook book = sheet.Workbook;
            Cells cells = sheet.Cells;

            DataColumnCollection columns = _this.Columns;
            DataRowCollection rows = _this.Rows;
            
            if (!string.IsNullOrEmpty(_this.TableName))
            {
                Style titleStyle = book.Styles[book.Styles.Add()];
                titleStyle.HorizontalAlignment = TextAlignmentType.Center;
                titleStyle.Font.Size = 14;
                titleStyle.Font.IsBold = true;

                cells.Merge(0, 0, 1, columns.Count);
                cells[0, 0].Value = _this.TableName;
                cells[0, 0].SetStyle(titleStyle);
            }

            Style bodyStyle = book.Styles[book.Styles.Add()];
            bodyStyle.Font.Size = 11;
            bodyStyle.Borders.SetColor(Color.Black);
            bodyStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            bodyStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            bodyStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            bodyStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

            for (int i = 0; i < rows.Count; i++)
            {
                int index = i + 2;
                for (int j = 0; j < columns.Count; j++)
                {
                    Cell cell = cells[index, j];
                    cell.Value = Convert.ToString(rows[i][j]);
                    cell.SetStyle(bodyStyle);
                }
            }

            Style headerStyle = book.Styles[book.Styles.Add()];
            headerStyle.HorizontalAlignment = TextAlignmentType.Center;
            headerStyle.Font.Size = 11;
            headerStyle.Font.IsBold = true;
            headerStyle.Borders.SetColor(Color.Black);
            headerStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            headerStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

            for (int i = 0; i < columns.Count; i++)
            {
                Cell cell = cells[1, i];
                cell.Value = columns[i].ColumnName;
                cell.SetStyle(headerStyle);

                sheet.AutoFitColumn(i);
                double width = cells.GetColumnWidth(i);
                if (width > 40)
                    cells.SetColumnWidth(i, 40);
                if (width < 6)
                    cells.SetColumnWidth(i, 6);
            }
        }


        /// <summary>
        /// 将 <see cref="System.Data.DataTable"/> 数据表格对象转换为 Aspose.Cells 表格工作簿对象 <see cref="Aspose.Cells.Workbook"/>。
        /// </summary>
        /// <param name="_this">要被转换的 <see cref="System.Data.DataTable"/> 数据表格对象。</param>
        /// <returns>返回一个 Aspose.Cells 表格工作簿对象 <see cref="Aspose.Cells.Workbook"/>，其中的列和行数据对应于被转换的源表格 <paramref name="_this"/> 中的数据。</returns>
        public static Workbook ToExcelWorkbook(this DataTable _this)
        {
            Check.NotNull(_this);

            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            ExportToWorkbook(_this, sheet);

            return book;
        }


    }
}
