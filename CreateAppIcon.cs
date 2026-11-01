using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;

// Code-native vector artwork, rendered at every Windows icon size.
class CreateAppIcon {
 static GraphicsPath Round(RectangleF r,float radius){float d=radius*2;var p=new GraphicsPath();p.AddArc(r.X,r.Y,d,d,180,90);p.AddArc(r.Right-d,r.Y,d,d,270,90);p.AddArc(r.Right-d,r.Bottom-d,d,d,0,90);p.AddArc(r.X,r.Bottom-d,d,d,90,90);p.CloseFigure();return p;}
 static Bitmap Draw(int size){
  var image=new Bitmap(size,size,PixelFormat.Format32bppArgb);
  using(var g=Graphics.FromImage(image)){
   g.Clear(Color.Transparent);g.SmoothingMode=SmoothingMode.AntiAlias;g.ScaleTransform(size/256f,size/256f);
   using(var p=Round(new RectangleF(12,12,232,232),48))using(var b=new LinearGradientBrush(new Rectangle(12,12,232,232),Color.FromArgb(38,64,79),Color.FromArgb(10,20,35),70))using(var pen=new Pen(Color.FromArgb(72,126,141),4)){g.FillPath(b,p);g.DrawPath(pen,p);}
   using(var p=Round(new RectangleF(91,35,74,20),9))using(var pen=new Pen(Color.FromArgb(159,182,187),6))g.DrawPath(pen,p);
   using(var p=Round(new RectangleF(41,55,174,166),17))using(var b=new SolidBrush(Color.FromArgb(15,29,44)))using(var pen=new Pen(Color.FromArgb(105,225,166),6)){g.FillPath(b,p);g.DrawPath(pen,p);}
   using(var b=new SolidBrush(Color.FromArgb(105,225,166))){g.FillRectangle(b,54,69,102,8);g.FillEllipse(b,171,66,13,13);g.FillEllipse(b,191,66,8,8);}
   using(var pen=new Pen(Color.FromArgb(55,93,99),2))for(int y=97;y<206;y+=10)g.DrawLine(pen,53,y,203,y);
   using(var font=new Font("Segoe UI",87,FontStyle.Bold,GraphicsUnit.Pixel))using(var b=new SolidBrush(Color.FromArgb(228,247,239)))using(var format=new StringFormat{Alignment=StringAlignment.Center,LineAlignment=StringAlignment.Center})g.DrawString("IR",font,b,new RectangleF(43,89,171,116),format);
  }return image;
 }
 static int Main(string[] args){
  Directory.CreateDirectory(args[0]);var frames=new List<byte[]>();int[] sizes={16,24,32,48,64,128,256};
  foreach(int size in sizes)using(var image=Draw(size))using(var stream=new MemoryStream()){image.Save(stream,ImageFormat.Png);frames.Add(stream.ToArray());if(size==256)image.Save(Path.Combine(args[0],"catalystir-icon.png"),ImageFormat.Png);}
  using(var writer=new BinaryWriter(File.Create(Path.Combine(args[0],"catalystir.ico")))){
   writer.Write((ushort)0);writer.Write((ushort)1);writer.Write((ushort)sizes.Length);int offset=6+16*sizes.Length;
   for(int i=0;i<sizes.Length;i++){writer.Write((byte)(sizes[i]==256?0:sizes[i]));writer.Write((byte)(sizes[i]==256?0:sizes[i]));writer.Write((byte)0);writer.Write((byte)0);writer.Write((ushort)1);writer.Write((ushort)32);writer.Write(frames[i].Length);writer.Write(offset);offset+=frames[i].Length;}
   foreach(var frame in frames)writer.Write(frame);
  }Console.WriteLine("Created seven-size CatalystIR icon");return 0;
 }
}
