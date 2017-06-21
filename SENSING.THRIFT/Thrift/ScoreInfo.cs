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
public partial class ScoreInfo : TBase
{
  private string _capid;
  private int _score;
  private int _flag;
  private long _time;

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

  public int Score
  {
    get
    {
      return _score;
    }
    set
    {
      __isset.score = true;
      this._score = value;
    }
  }

  public int Flag
  {
    get
    {
      return _flag;
    }
    set
    {
      __isset.flag = true;
      this._flag = value;
    }
  }

  public long Time
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


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool capid;
    public bool score;
    public bool flag;
    public bool time;
  }

  public ScoreInfo() {
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
            if (field.Type == TType.I32) {
              Score = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Flag = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              Time = iprot.ReadI64();
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
      TStruct struc = new TStruct("ScoreInfo");
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
      if (__isset.score) {
        field.Name = "score";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Score);
        oprot.WriteFieldEnd();
      }
      if (__isset.flag) {
        field.Name = "flag";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Flag);
        oprot.WriteFieldEnd();
      }
      if (__isset.time) {
        field.Name = "time";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Time);
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
    StringBuilder __sb = new StringBuilder("ScoreInfo(");
    bool __first = true;
    if (Capid != null && __isset.capid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Capid: ");
      __sb.Append(Capid);
    }
    if (__isset.score) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Score: ");
      __sb.Append(Score);
    }
    if (__isset.flag) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Flag: ");
      __sb.Append(Flag);
    }
    if (__isset.time) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Time: ");
      __sb.Append(Time);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

