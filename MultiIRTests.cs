using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Web.Script.Serialization;

class MultiIRTests {
 static int checks;
 static void Check(bool yes,string message){if(!yes)throw new Exception(message);checks++;}
 static void Reject(Action action,string message){try{action();}catch(InvalidDataException){checks++;return;}throw new Exception(message);}
 static uint BE(byte[] b,int p){return ((uint)b[p]<<24)|((uint)b[p+1]<<16)|((uint)b[p+2]<<8)|b[p+3];}
 static byte[] Mutate(byte[] candidate,uint target,byte value){
  var c=FirmwareCodec.Parse(candidate);var image=c.Chunks.Single(x=>x.Tag=="dinf"&&BE(x.Data,0)==0);int off=(int)BE(image.Data,8),size=(int)BE(image.Data,12);
  uint high=0;bool touched=false;var output=new StringBuilder(size);
  foreach(string line in Encoding.ASCII.GetString(c.Raw,off,size).Split(new[]{"\r\n"},StringSplitOptions.RemoveEmptyEntries)){
   byte[] r=Enumerable.Range(0,(line.Length-1)/2).Select(i=>Convert.ToByte(line.Substring(1+i*2,2),16)).ToArray();
   if(r[3]==4)high=(uint)((r[4]<<8)|r[5])<<16;
   uint a=high+(uint)((r[1]<<8)|r[2]);
   if(r[3]==0&&a<=target&&target<a+r[0]){r[4+target-a]=value;r[r.Length-1]=0;r[r.Length-1]=(byte)(-r.Sum(x=>(int)x)&255);touched=true;}
   output.Append(":"+BitConverter.ToString(r).Replace("-","")+"\r\n");
  }
  if(!touched)throw new Exception("Test address absent");byte[] raw=(byte[])c.Raw.Clone();Buffer.BlockCopy(Encoding.ASCII.GetBytes(output.ToString()),0,raw,off,size);return FirmwareCodec.Repack(c,raw);
 }
 static int Main(string[] args){try{
  string root=Path.GetFullPath(args[1]);Directory.CreateDirectory(root);byte[] template=TemplateSource.Load(args[0]);
  var irs=new byte[3][];for(int s=0;s<3;s++){irs[s]=new byte[8192];Buffer.BlockCopy(BitConverter.GetBytes(.25f*(s+1)),0,irs[s],(2047-s*17)*4,4);Buffer.BlockCopy(BitConverter.GetBytes(-.125f),0,irs[s],0,4);}
  for(int mask=1;mask<8;mask++){
   byte[][] subset=irs.Select((b,i)=>(mask&(1<<i))==0?null:b).ToArray();byte[] candidate=MultiIRCodec.WithIRs(template,subset);FirmwareCodec.TestGuards(template,candidate);
   byte[][] read=MultiIRCodec.ReadIRs(candidate);
   for(int i=0;i<3;i++)Check(subset[i]==null?read[i]==null:subset[i].SequenceEqual(read[i]),"Wrong slot or coefficients");
   File.WriteAllBytes(Path.Combine(root,"mask-"+mask+".ctf"),candidate);
  }
  byte[] all=MultiIRCodec.WithIRs(template,irs);
  Reject(()=>FirmwareCodec.ValidateCandidate(template,Mutate(all,0x601ac000,0)),"Executable patch accepted");
  for(int s=0;s<3;s++){
   int slot=s;Reject(()=>FirmwareCodec.ValidateCandidate(template,Mutate(all,MultiIRCodec.Coefficients[slot]+0x2000,0x7f)),"Wrong FFT bank accepted");
  }
  byte[] only2=MultiIRCodec.WithIRs(template,new[]{null,irs[1],null});
  Reject(()=>FirmwareCodec.ValidateCandidate(template,Mutate(only2,MultiIRCodec.Coefficients[0],1)),"Disabled slot carried hidden coefficients");
  Reject(()=>FirmwareCodec.ValidateCandidate(template,Mutate(all,MultiIRCodec.MaskAddress,0)),"Empty mask accepted");
  Reject(()=>FirmwareCodec.ValidateCandidate(template,Mutate(all,MultiIRCodec.MaskAddress,8)),"Out-of-range mask accepted");
  var metadata=FirmwareCodec.Parse(all);metadata.Chunks.Single(c=>c.Tag=="HEAD").Data[0]^=1;
  Reject(()=>FirmwareCodec.ValidateCandidate(template,FirmwareCodec.Repack(metadata,metadata.Raw)),"Altered metadata accepted");
  var broken=(byte[])all.Clone();broken[broken.Length/2]^=1;Reject(()=>FirmwareCodec.ValidateCandidate(template,broken),"Damaged container accepted");
  File.WriteAllText(Path.Combine(root,"tests.json"),new JavaScriptSerializer().Serialize(new{passed=true,checks=checks,masks=7,guards="code, FFT, disabled slots, mask, metadata, corrupt CTF",scope="offline; no USB"}));
  Console.WriteLine("PASS "+checks+" v2 slot/container checks");return 0;
 }catch(Exception e){Console.Error.WriteLine(e);return 1;}}
}
