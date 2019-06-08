using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.BarCodes;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PDFSharpSample
{
    class Program
    {
        #region ■プロパティ

        public static string ExecFolderPath { get; set; } = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
        public static readonly string PDF_FILE_NAME = "test.pdf";

        #endregion

        static void Main(string[] args)
        {
            // PDF作成
            var filePath = Path.Combine(ExecFolderPath, PDF_FILE_NAME);
            PdfSharpCoreManager.Instance.CreatePdf(filePath);

            Console.WriteLine("Hello World!");
        }
    }
}
