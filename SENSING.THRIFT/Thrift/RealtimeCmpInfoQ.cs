/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;


#if !SILVERLIGHT
[Serializable]
#endif
public partial class RealtimeCmpInfoQ : TBase
{
  private RealtimeCmpInfo _cfg;
  private string _channelName;

  public RealtimeCmpInfo Cfg
  {
    get
    {
      return _cfg;
    }
    set
    {
      __isset.cfg = true;
      this._cfg = value;
    }
  }

  public string ChannelName
  {
    get
    {
      return _channelName;
    }
    set
    {
      __isset.channelName = true;
      this._channelName = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool cfg;
    public bool channelName;
  }

  public RealtimeCmpInfoQ() {
  }

  public void Read (TProtocol iprot)
  {
    iprot.IncrementRecursionDepth();
    try
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.Struct) {
              Cfg = new RealtimeCmpInfo();
              Cfg.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              ChannelName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }
    finally
    {
      iprot.DecrementRecursionDepth();
    }
  }

  public void Write(TProtocol oprot) {
    oprot.IncrementRecursionDepth();
    try
    {
      TStruct struc = new TStruct("RealtimeCmpInfoQ");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Cfg != null && __isset.cfg) {
        field.Name = "cfg";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        Cfg.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (ChannelName != null && __isset.channelName) {
        field.Name = "channelName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ChannelName);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }
    finally
    {
      oprot.DecrementRecursionDepth();
    }
  }

  public override string ToString() {
    StringBuilder __sb = new StringBuilder("RealtimeCmpInfoQ(");
    bool __first = true;
    if (Cfg != null && __isset.cfg) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Cfg: ");
      __sb.Append(Cfg== null ? "<null>" : Cfg.ToString());
    }
    if (ChannelName != null && __isset.channelName) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ChannelName: ");
      __sb.Append(ChannelName);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

