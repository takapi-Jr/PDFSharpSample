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
        static void Main(string[] args)
        {
            // フォントリゾルバーのグローバル登録
            PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = new JapaneseFontResolver();

            using (var document = new PdfDocument())
            {
                // 新規ドキュメントを作成
                var page = document.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;

                // XGraphicsオブジェクトを取得
                var gfx = XGraphics.FromPdfPage(page);

                // フォント
                var font1 = new XFont("Gen Shin Gothic", 20, XFontStyle.Regular);
                var font2 = new XFont("Gen Shin Gothic", 30, XFontStyle.Bold);
                var font3 = new XFont("Gen Shin Gothic", 40, XFontStyle.Italic);

                Console.WriteLine($"page.Width = {page.Width}");    // Width  595pt
                Console.WriteLine($"page.Height = {page.Height}");  // Height 842pt

                // 文字列の中心位置をXPointで指定可能
                gfx.DrawString("Hello World!!!こんにちは、世界！", font1, XBrushes.Black, new XPoint(0.0, 0.0), XStringFormats.Center);
                gfx.DrawString("Hello World!!!こんにちは、世界！", font2, XBrushes.Red, new XPoint(0.0, 30.0), XStringFormats.Center);
                gfx.DrawString("Hello World!!!こんにちは、世界！", font3, XBrushes.Blue, new XPoint(0.0, 60.0), XStringFormats.Center);

                // 基本はXRectで位置指定
                gfx.DrawString("Hello World!!!こんにちは、世界！", font1, XBrushes.Black, new XRect(0.0, 0.0, page.Width, page.Height), XStringFormats.Center);

                // 線を描画
                var lines = new List<XPoint>()
                {
                    // pt単位
                    new XPoint(0.0, 300.0),
                    new XPoint(100.0, 300.0),
                    new XPoint(100.0, 100.0),
                    new XPoint(200.0, 100.0),
                    new XPoint(0.0, 300.0),
                };
                gfx.DrawLines(XPens.Black, lines.ToArray());

                // PDF保存
                var execFolderPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
                var fileName = "test.pdf";
                var filePath = Path.Combine(execFolderPath, fileName);
                document.Save(filePath);
            }

            Console.WriteLine("Hello World!");
        }
    }

    // 日本語フォントのためのフォントリゾルバー
    public class JapaneseFontResolver : IFontResolver
    {
        // 源真ゴシック（ http://jikasei.me/font/genshin/）
        private static readonly string GEN_SHIN_GOTHIC_BOLD_TTF = "PDFSharpSample.Fonts.GenShinGothic-Monospace-Bold.ttf";
        private static readonly string GEN_SHIN_GOTHIC_LIGHT_TTF = "PDFSharpSample.Fonts.GenShinGothic-Monospace-Light.ttf";
        private static readonly string GEN_SHIN_GOTHIC_MEDIUM_TTF = "PDFSharpSample.Fonts.GenShinGothic-Monospace-Medium.ttf";
        

        public string DefaultFontName => throw new NotImplementedException();

        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case "GenShinGothic#Bold":
                    return LoadFontData(GEN_SHIN_GOTHIC_BOLD_TTF);
                case "GenShinGothic#Light":
                    return LoadFontData(GEN_SHIN_GOTHIC_LIGHT_TTF);
                case "GenShinGothic#Medium":
                    return LoadFontData(GEN_SHIN_GOTHIC_MEDIUM_TTF);
            }
            return null;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            var fontName = familyName.ToLower();

            switch (fontName)
            {
                case "gen shin gothic":
                    if (isBold)
                    {
                        return new FontResolverInfo("GenShinGothic#Bold");
                    }
                    else if (isItalic)
                    {
                        return new FontResolverInfo("GenShinGothic#Light");
                    }
                    else
                    {
                        return new FontResolverInfo("GenShinGothic#Medium");
                    }
            }

            // デフォルトのフォント
            return PlatformFontResolver.ResolveTypeface("Arial", isBold, isItalic);
        }

        // 埋め込みリソースからフォントファイルを読み込む
        private byte[] LoadFontData(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new ArgumentException("No resource with name " + resourceName);
                }

                var count = (int)stream.Length;
                var data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }
}
