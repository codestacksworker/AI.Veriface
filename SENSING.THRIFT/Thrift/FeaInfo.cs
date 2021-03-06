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
public partial class FeaInfo : TBase
{
  private string _capid;
  private byte[] _fea;

  public string Capid
  {
    get
    {
      return _capid;
    }
    set
    {
      __isset.capid = true;
      this._capid = value;
    }
  }

  public byte[] Fea
  {
    get
    {
      return _fea;
    }
    set
    {
      __isset.fea = true;
      this._fea = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool capid;
    public bool fea;
  }

  public FeaInfo() {
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
            if (field.Type == TType.String) {
              Capid = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Fea = iprot.ReadBinary();
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
      TStruct struc = new TStruct("FeaInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Capid != null && __isset.capid) {
        field.Name = "capid";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Capid);
        oprot.WriteFieldEnd();
      }
      if (Fea != null && __isset.fea) {
        field.Name = "fea";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Fea);
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
    StringBuilder __sb = new StringBuilder("FeaInfo(");
    bool __first = true;
    if (Capid != null && __isset.capid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Capid: ");
      __sb.Append(Capid);
    }
    if (Fea != null && __isset.fea) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Fea: ");
      __sb.Append(Fea);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

