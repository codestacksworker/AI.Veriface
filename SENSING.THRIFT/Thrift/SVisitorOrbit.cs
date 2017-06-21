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
public partial class SVisitorOrbit : TBase
{
  private string _time;
  private string _zid;
  private string _zname;

  public string Time
  {
    get
    {
      return _time;
    }
    set
    {
      __isset.time = true;
      this._time = value;
    }
  }

  public string Zid
  {
    get
    {
      return _zid;
    }
    set
    {
      __isset.zid = true;
      this._zid = value;
    }
  }

  public string Zname
  {
    get
    {
      return _zname;
    }
    set
    {
      __isset.zname = true;
      this._zname = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool time;
    public bool zid;
    public bool zname;
  }

  public SVisitorOrbit() {
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
              Time = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Zid = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Zname = iprot.ReadString();
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
      TStruct struc = new TStruct("SVisitorOrbit");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Time != null && __isset.time) {
        field.Name = "time";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Time);
        oprot.WriteFieldEnd();
      }
      if (Zid != null && __isset.zid) {
        field.Name = "zid";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Zid);
        oprot.WriteFieldEnd();
      }
      if (Zname != null && __isset.zname) {
        field.Name = "zname";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Zname);
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
    StringBuilder __sb = new StringBuilder("SVisitorOrbit(");
    bool __first = true;
    if (Time != null && __isset.time) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Time: ");
      __sb.Append(Time);
    }
    if (Zid != null && __isset.zid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Zid: ");
      __sb.Append(Zid);
    }
    if (Zname != null && __isset.zname) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Zname: ");
      __sb.Append(Zname);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

