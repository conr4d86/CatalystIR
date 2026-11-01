using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class IRLevelsTests {
 static void Assert(bool value,string message){if(!value)throw new Exception(message);}
 static byte[] IR(params float[] samples){var data=new byte[8192];for(int i=0;i<samples.Length;i++)Buffer.BlockCopy(BitConverter.GetBytes(samples[i]),0,data,(2047-i)*4,4);return data;}
 static double Gain(double db){return Math.Pow(10,db/20);}
 static double Response(byte[] taps,double hz){double re=0,im=0;for(int n=0;n<2048;n++){double x=BitConverter.ToSingle(taps,(2047-n)*4),angle=-2*Math.PI*hz*n/48000;re+=x*Math.Cos(angle);im+=x*Math.Sin(angle);}return Math.Sqrt(re*re+im*im);}
 static string Wave(string folder,int bits){
  string path=Path.Combine(folder,"pcm-"+bits+".wav");int width=bits/8;
  using(var stream=File.Create(path))using(var writer=new BinaryWriter(stream)){
   writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));writer.Write(36+width*2);writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));writer.Write(16);writer.Write((ushort)1);writer.Write((ushort)1);writer.Write(48000);writer.Write(48000*width);writer.Write((ushort)width);writer.Write((ushort)bits);writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));writer.Write(width*2);
   long half=1L<<(bits-2);for(int sample=0;sample<2;sample++){long value=sample==0?half:-half;for(int j=0;j<width;j++)writer.Write((byte)(value>>(8*j)));}
  }return path;
 }
 static int Main(string[] args){try{
  string folder=args.Length==1?args[0]:Path.Combine(Path.GetTempPath(),"CatalystIR-level-test-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(folder);
  var a=IRLevels.Analyze(IR(.125f));Assert(Math.Abs(a.automaticDb-(IRLevels.TargetPeakDb-20*Math.Log10(.125)))<1e-8,"Impulse normalization");
  var b=IRLevels.Analyze(IR(1,1));Assert(Math.Abs(b.frequencyPeak-2)<1e-8,"Two-tap peak gain");
  var c=IRLevels.Analyze(IR(1,-1));Assert(Math.Abs(c.frequencyPeak-2)<1e-8,"Nyquist peak gain");
  Assert(Math.Abs(b.frequencyPeak*Gain(b.automaticDb)-Gain(-3))<1e-9,"Automatic headroom");
  byte[] scaled=IRLevels.Apply(IR(.125f),a.automaticDb);
  Assert(Math.Abs(BitConverter.ToSingle(scaled,2047*4)*IRLevels.ExistingFirmwareGain-Gain(-3))<1e-6,"Fixed firmware trim compensation");
  float normal=BitConverter.ToSingle(IRLevels.Apply(IR(.125f),-6),2047*4);
  float lower=BitConverter.ToSingle(IRLevels.Apply(IR(.125f),-12),2047*4);
  Assert(Math.Abs(lower/normal-Gain(-6))<1e-6,"Manual gain ratio");
  Assert(IRLevels.Apply(IR(.125f),20*Math.Log10(IRLevels.ExistingFirmwareGain)).SequenceEqual(IR(.125f)),"Existing firmware level preserved");
  byte[] filtered=IRFilters.Apply(IR(1),true,80,8000);
  Assert(Math.Abs(20*Math.Log10(Response(filtered,80))+3.0103)<.05,"80 Hz high-pass cutoff");
  Assert(Math.Abs(20*Math.Log10(Response(filtered,8000))+3.0103)<.05,"8 kHz low-pass cutoff");
  Assert(Response(filtered,20)<.07&&Response(filtered,16000)<.15,"Subbass and high-frequency attenuation");
  Assert(Response(filtered,1000)>.98&&Response(filtered,0)<1e-5&&Response(filtered,24000)<1e-5,"Passband, DC and Nyquist");
  Assert(IRFilters.Apply(IR(.125f),false).SequenceEqual(IR(.125f)),"Filter bypass must preserve original taps");
  var filteredAnalysis=IRLevels.Analyze(filtered);Assert(Math.Abs(filteredAnalysis.frequencyPeak*Gain(filteredAnalysis.automaticDb)-Gain(-3))<1e-9,"Automatic level after filtering");
  foreach(int bits in new[]{16,24,32})Assert(FirmwareCodec.ReadIR(Wave(folder,bits)).SequenceEqual(IR(.5f,-.5f)),"PCM decoding and zero padding "+bits);
  int rejects=0;foreach(byte[] invalid in new[]{IR(),IR(float.NaN),IR(float.PositiveInfinity)})try{IRLevels.Analyze(invalid);}catch(InvalidDataException){rejects++;}
  Assert(rejects==3,"Silent and nonfinite IRs must be rejected");
  var profiles=new Dictionary<string,IRLevels.Profile>();profiles[a.key]=new IRLevels.Profile{name="renamable.wav",automatic=false,manualDb=-8.5};profiles[b.key]=new IRLevels.Profile{name="other.wav",automatic=true,manualDb=-20};
  string path=Path.Combine(folder,"ir-levels.json");IRLevels.Save(path,profiles);
  var loaded=IRLevels.Load(path);Assert(loaded.Count==2&&loaded[a.key].manualDb==-8.5&&loaded[b.key].automatic,"Profiles remain independent");
  Assert(loaded.ContainsKey(IRLevels.Analyze(IR(.125f)).key),"Profiles identified by content, not filename");
  loaded[a.key].manualDb=-9;IRLevels.Save(path,loaded);Assert(IRLevels.Load(path)[a.key].manualDb==-9&&File.Exists(path+".previous"),"Atomic profile replacement");
  Console.WriteLine("PASS: 80 Hz/8 kHz Butterworth cuts, bypass, post-filter normalization, manual dB, fixed-trim compensation, PCM and independent persistent profiles.");return 0;
 }catch(Exception e){Console.WriteLine(e);return 1;}}
}
