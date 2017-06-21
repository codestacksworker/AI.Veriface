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
public partial class TimeData : TBase
{
  private int _tyear;
  private int _tmonth;
  private int _tday;
  private int _thour;
  private int _tminute;
  private int _tsecond;

  public int Tyear
  {
    get
    {
      return _tyear;
    }
    set
    {
      __isset.tyear = true;
      this._tyear = value;
    }
  }

  public int Tmonth
  {
    get
    {
      return _tmonth;
    }
    set
    {
      __isset.tmonth = true;
      this._tmonth = value;
    }
  }

  public int Tday
  {
    get
    {
      return _tday;
    }
    set
    {
      __isset.tday = true;
      this._tday = value;
    }
  }

  public int Thour
  {
    get
    {
      return _thour;
    }
    set
    {
      __isset.thour = true;
      this._thour = value;
    }
  }

  public int Tminute
  {
    get
    {
      return _tminute;
    }
    set
    {
      __isset.tminute = true;
      this._tminute = value;
    }
  }

  public int Tsecond
  {
    get
    {
      return _tsecond;
    }
    set
    {
      __isset.tsecond = true;
      this._tsecond = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool tyear;
    public bool tmonth;
    public bool tday;
    public bool thour;
    public bool tminute;
    public bool tsecond;
  }

  public TimeData() {
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
            if (field.Type == TType.I32) {
              Tyear = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              Tmonth = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Tday = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              Thour = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              Tminute = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              Tsecond = iprot.ReadI32();
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
      TStruct struc = new TStruct("TimeData");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.tyear) {
        field.Name = "tyear";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Tyear);
        oprot.WriteFieldEnd();
      }
      if (__isset.tmonth) {
        field.Name = "tmonth";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Tmonth);
        oprot.WriteFieldEnd();
      }
      if (__isset.tday) {
        field.Name = "tday";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Tday);
        oprot.WriteFieldEnd();
      }
      if (__isset.thour) {
        field.Name = "thour";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Thour);
        oprot.WriteFieldEnd();
      }
      if (__isset.tminute) {
        field.Name = "tminute";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Tminute);
        oprot.WriteFieldEnd();
      }
      if (__isset.tsecond) {
        field.Name = "tsecond";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Tsecond);
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
    StringBuilder __sb = new StringBuilder("TimeData(");
    bool __first = true;
    if (__isset.tyear) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Tyear: ");
      __sb.Append(Tyear);
    }
    if (__isset.tmonth) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Tmonth: ");
      __sb.Append(Tmonth);
    }
    if (__isset.tday) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Tday: ");
      __sb.Append(Tday);
    }
    if (__isset.thour) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Thour: ");
      __sb.Append(Thour);
    }
    if (__isset.tminute) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Tminute: ");
      __sb.Append(Tminute);
    }
    if (__isset.tsecond) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Tsecond: ");
      __sb.Append(Tsecond);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

