using System;
using System.IO;

class OneClickWorkflowTests {
 static int checks;
 static void Check(bool value,string message){if(!value)throw new Exception(message);checks++;}
 static void Reject(Action action,string message){try{action();}catch(InvalidOperationException){checks++;return;}throw new Exception(message);}
 static int Main(string[] args){
  try{
   string sdk="connected=True; name=Catalyst 60; firmware=2.02\nFull device synchronization confirmed\nshutdown complete";
   OneClickSession.CheckSDK(sdk);checks++;
   Reject(()=>OneClickSession.CheckSDK(sdk.Replace("Full device synchronization confirmed","")),"Accepted unsynchronized device");
   Reject(()=>OneClickSession.CheckSDK(sdk.Replace("Catalyst 60","Other device")),"Accepted wrong device");
   Check(OneClickSession.MatchesInstallRequest("install:"+new string('a',64),new string('a',64)),"Matching candidate was rejected");
   Check(!OneClickSession.MatchesInstallRequest("install:"+new string('b',64),new string('a',64)),"Mismatching candidate was accepted");
   Check(!OneClickSession.MatchesInstallRequest("install:",""),"Empty hash was accepted");
   Check(!OneClickSession.MatchesInstallRequest(null,new string('a',64)),"Missing request was accepted");
   var session=new OneClickSession{phase="backed-up",candidateHash=new string('a',64),recipe="test",folder=args[0]};
   Check(!session.ObserveUSB(0)&&!session.ObserveUSB(1),"Activation was allowed before completed transfer");
   string completion="INSTALL_REQUEST_CONFIRMED\nUPDATE_ENGINE_COMPLETED; VERIFY_DEVICE_BOOT_SEPARATELY\nshutdown complete";
   Reject(()=>session.ConfirmTransfer(completion.Replace("INSTALL_REQUEST_CONFIRMED","")),"Accepted transfer without a matching install request");
   Reject(()=>session.ConfirmTransfer(completion.Replace("UPDATE_ENGINE_COMPLETED; VERIFY_DEVICE_BOOT_SEPARATELY","100%")),"Treated 100% as completion");
   Reject(()=>session.ConfirmTransfer(completion.Replace("shutdown complete","")),"Accepted incomplete updater shutdown");
   session.ConfirmTransfer(completion);
   Check(!session.ObserveUSB(1),"Activated before physical disconnect");
   Check(!session.ObserveUSB(0)&&!session.ObserveUSB(2)&&session.ObserveUSB(1),"Disconnect/reconnect gate failed");
   Reject(()=>session.ConfirmActivation("connected=True\nshutdown complete"),"Mere connection was mistaken for IR activation");
   Directory.CreateDirectory(args[0]);string journal=Path.Combine(args[0],"installation.json");session.Save(journal);
   var loaded=OneClickSession.Load(journal);Check(loaded.phase=="awaiting-reboot"&&loaded.recipe=="test"&&loaded.rebootDisconnected,"Pending update journal not preserved");
#if AUTOBOOT
   Reject(()=>loaded.ConfirmActivation("IR virtual cabinet 112 active in edit buffer; no preset stored\nshutdown complete"),"Volatile-only selection mistaken for persistent boot recall");
   loaded.ConfirmActivation("IR_BOOT_RECALL_SAVED index=12\nshutdown complete");Check(loaded.bootRecallSaved,"Boot recall not recorded");loaded.Save(journal);
#else
   loaded.ConfirmActivation("IR virtual cabinet 112 active in edit buffer; no preset stored\nshutdown complete");loaded.Save(journal);
#endif
   Check(OneClickSession.Load(journal).phase=="active","Activation journal not committed");
   Check(OneClickSession.Recipe("abc",-24.1,true,80,8000)!=OneClickSession.Recipe("abc",-24.1,true,80,9000),"Cut change did not invalidate installed IR");
   Check(OneClickSession.Recipe("abc",-24.1,true,80,8000)!=OneClickSession.Recipe("abc",-24.0,true,80,8000),"Level change did not invalidate installed IR");
   Console.WriteLine("PASS: "+checks+" workflow checks; no USB or firmware writes");return 0;
  }catch(Exception e){Console.Error.WriteLine(e);return 1;}
 }
}
