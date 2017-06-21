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
public partial class _POINT : TBase
{
  private int _x;
  private int _y;

  public int X
  {
    get
    {
      return _x;
    }
    set
    {
      __isset.x = true;
      this._x = value;
    }
  }

  public int Y
  {
    get
    {
      return _y;
    }
    set
    {
      __isset.y = true;
      this._y = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool x;
    public bool y;
  }

  public _POINT() {
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
              X = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              Y = iprot.ReadI32();
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
      TStruct struc = new TStruct("_POINT");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.x) {
        field.Name = "x";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(X);
        oprot.WriteFieldEnd();
      }
      if (__isset.y) {
        field.Name = "y";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Y);
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
    StringBuilder __sb = new StringBuilder("_POINT(");
    bool __first = true;
    if (__isset.x) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("X: ");
      __sb.Append(X);
    }
    if (__isset.y) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Y: ");
      __sb.Append(Y);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

