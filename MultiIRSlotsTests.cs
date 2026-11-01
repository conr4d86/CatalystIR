using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

class MultiIRSlotsTests {
 static int checks;
 static void Check(bool yes,string message){if(!yes)throw new Exception(message);checks++;}
 static int Main(string[] args){try{
  string root=Path.GetFullPath(args[0]),fixture=Path.GetFullPath(args[1]);Directory.CreateDirectory(root);
  string legacy=Path.Combine(root,"legacy"),backup=Path.Combine(legacy,"backup"),v2=Path.Combine(legacy,"v2");Directory.CreateDirectory(backup);
  string wav=Path.Combine(backup,"IR-original.wav");File.Copy(Path.Combine(fixture,"IR1.wav"),wav);
  var profile=new IRLevels.Profile{name="Original del usuario.wav",automatic=false,manualDb=-15,filtersEnabled=true,lowCutHz=82,highCutHz=8000};
  string key=FirmwareCodec.Hash(FirmwareCodec.ReadIR(wav));var json=new JavaScriptSerializer();
  File.WriteAllText(Path.Combine(backup,"IR-settings.json"),json.Serialize(new{profile=profile,original_ir_sha256=key}));
  var session=new OneClickSession{folder=backup,phase="active",templateHash="508602ac8bf2fc95ca64198e825d4ed8ed93219c8295eca3f7df09d166ee6261"};
  string journal=Path.Combine(legacy,"installation.json");session.Save(journal);string oldHash=FirmwareCodec.Hash(File.ReadAllBytes(journal));
  var state=MultiIRSlots.StartFromV1(v2);
  Check(state.Count==1&&state.slots[0].key==key,"Migration lost active IR");
  Check(state.slots[0].profile.manualDb==-15&&state.slots[0].profile.lowCutHz==82,"Migration reset gain/cuts");
  Check(state.slots[0].name==profile.name,"Migration lost original name");
  Check(FirmwareCodec.Hash(File.ReadAllBytes(journal))==oldHash,"Migration changed v1 journal");
  state.Import(1,wav,profile,v2);state.slots[1].profile.manualDb=-9;state.slots[1].profile.highCutHz=6500;
  Check(state.slots[0].profile.manualDb==-15&&state.slots[0].profile.highCutHz==8000,"Same WAV shares profile references");
  state.Import(2,Path.Combine(fixture,"IR3.wav"),null,v2);
  string recipe=state.Recipe();state.Save(Path.Combine(v2,"slots.json"));
  state=MultiIRSlots.Load(Path.Combine(v2,"slots.json"));Check(state.Recipe()==recipe,"Reload lost recipe");
  File.Move(wav,wav+".moved");Check(state.Process().Count(b=>b!=null)==3,"Moving source WAV breaks cached IRs");
  string export=Path.Combine(root,"export");Directory.CreateDirectory(export);state.Backup(export);
  var restored=MultiIRSlots.Load(Path.Combine(export,"IR-slots.json"));Check(restored.Recipe()==recipe,"Portable backup changed IRs/settings");
  string before=state.Recipe();state.slots[2]=null;Check(state.Recipe()!=before&&state.Count==2,"Clearing a slot failed to invalidate recipe");
  Console.WriteLine("PASS "+checks+" migration/cache/profile/backup checks");
  File.WriteAllText(Path.Combine(root,"tests.json"),json.Serialize(new{passed=true,checks=checks,scope="offline fixtures only; no personal files or USB changed"}));return 0;
 }catch(Exception e){Console.Error.WriteLine(e);return 1;}}
}
