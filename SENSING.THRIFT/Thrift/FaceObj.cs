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
public partial class FaceObj : TBase
{
  private string _tcUuid;
  private string _tcName;
  private int _nMain_ftID;
  private int _nType;
  private int _nSST;
  private int _nExten;
  private int _nSex;
  private int _nAge;
  private long _dTm;
  private string _tcRemarks;
  private List<FaceObjTemplate> _tmplate;

  public string TcUuid
  {
    get
    {
      return _tcUuid;
    }
    set
    {
      __isset.tcUuid = true;
      this._tcUuid = value;
    }
  }

  public string TcName
  {
    get
    {
      return _tcName;
    }
    set
    {
      __isset.tcName = true;
      this._tcName = value;
    }
  }

  public int NMain_ftID
  {
    get
    {
      return _nMain_ftID;
    }
    set
    {
      __isset.nMain_ftID = true;
      this._nMain_ftID = value;
    }
  }

  public int NType
  {
    get
    {
      return _nType;
    }
    set
    {
      __isset.nType = true;
      this._nType = value;
    }
  }

  public int NSST
  {
    get
    {
      return _nSST;
    }
    set
    {
      __isset.nSST = true;
      this._nSST = value;
    }
  }

  public int NExten
  {
    get
    {
      return _nExten;
    }
    set
    {
      __isset.nExten = true;
      this._nExten = value;
    }
  }

  public int NSex
  {
    get
    {
      return _nSex;
    }
    set
    {
      __isset.nSex = true;
      this._nSex = value;
    }
  }

  public int NAge
  {
    get
    {
      return _nAge;
    }
    set
    {
      __isset.nAge = true;
      this._nAge = value;
    }
  }

  public long DTm
  {
    get
    {
      return _dTm;
    }
    set
    {
      __isset.dTm = true;
      this._dTm = value;
    }
  }

  public string TcRemarks
  {
    get
    {
      return _tcRemarks;
    }
    set
    {
      __isset.tcRemarks = true;
      this._tcRemarks = value;
    }
  }

  public List<FaceObjTemplate> Tmplate
  {
    get
    {
      return _tmplate;
    }
    set
    {
      __isset.tmplate = true;
      this._tmplate = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool tcUuid;
    public bool tcName;
    public bool nMain_ftID;
    public bool nType;
    public bool nSST;
    public bool nExten;
    public bool nSex;
    public bool nAge;
    public bool dTm;
    public bool tcRemarks;
    public bool tmplate;
  }

  public FaceObj() {
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
              TcUuid = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              TcName = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              NMain_ftID = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              NType = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              NSST = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              NExten = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.I32) {
              NSex = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.I32) {
              NAge = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I64) {
              DTm = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.String) {
              TcRemarks = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.List) {
              {
                Tmplate = new List<FaceObjTemplate>();
                TList _list0 = iprot.ReadListBegin();
                for( int _i1 = 0; _i1 < _list0.Count; ++_i1)
                {
                  FaceObjTemplate _elem2;
                  _elem2 = new FaceObjTemplate();
                  _elem2.Read(iprot);
                  Tmplate.Add(_elem2);
                }
                iprot.ReadListEnd();
              }
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
      TStruct struc = new TStruct("FaceObj");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (TcUuid != null && __isset.tcUuid) {
        field.Name = "tcUuid";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TcUuid);
        oprot.WriteFieldEnd();
      }
      if (TcName != null && __isset.tcName) {
        field.Name = "tcName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TcName);
        oprot.WriteFieldEnd();
      }
      if (__isset.nMain_ftID) {
        field.Name = "nMain_ftID";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(NMain_ftID);
        oprot.WriteFieldEnd();
      }
      if (__isset.nType) {
        field.Name = "nType";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(NType);
        oprot.WriteFieldEnd();
      }
      if (__isset.nSST) {
        field.Name = "nSST";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(NSST);
        oprot.WriteFieldEnd();
      }
      if (__isset.nExten) {
        field.Name = "nExten";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(NExten);
        oprot.WriteFieldEnd();
      }
      if (__isset.nSex) {
        field.Name = "nSex";
        field.Type = TType.I32;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(NSex);
        oprot.WriteFieldEnd();
      }
      if (__isset.nAge) {
        field.Name = "nAge";
        field.Type = TType.I32;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(NAge);
        oprot.WriteFieldEnd();
      }
      if (__isset.dTm) {
        field.Name = "dTm";
        field.Type = TType.I64;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(DTm);
        oprot.WriteFieldEnd();
      }
      if (TcRemarks != null && __isset.tcRemarks) {
        field.Name = "tcRemarks";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(TcRemarks);
        oprot.WriteFieldEnd();
      }
      if (Tmplate != null && __isset.tmplate) {
        field.Name = "tmplate";
        field.Type = TType.List;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, Tmplate.Count));
          foreach (FaceObjTemplate _iter3 in Tmplate)
          {
            _iter3.Write(oprot);
          }
          oprot.WriteListEnd();
        }
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
    StringBuilder __sb = new StringBuilder("FaceObj(");
    bool __first = true;
    if (TcUuid != null && __isset.tcUuid) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("TcUuid: ");
      __sb.Append(TcUuid);
    }
    if (TcName != null && __isset.tcName) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("TcName: ");
      __sb.Append(TcName);
    }
    if (__isset.nMain_ftID) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("NMain_ftID: ");
      __sb.Append(NMain_ftID);
    }
    if (__isset.nType) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("NType: ");
      __sb.Append(NType);
    }
    if (__isset.nSST) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("NSST: ");
      __sb.Append(NSST);
    }
    if (__isset.nExten) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("NExten: ");
      __sb.Append(NExten);
    }
    if (__isset.nSex) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("NSex: ");
      __sb.Append(NSex);
    }
    if (__isset.nAge) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("NAge: ");
      __sb.Append(NAge);
    }
    if (__isset.dTm) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("DTm: ");
      __sb.Append(DTm);
    }
    if (TcRemarks != null && __isset.tcRemarks) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("TcRemarks: ");
      __sb.Append(TcRemarks);
    }
    if (Tmplate != null && __isset.tmplate) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Tmplate: ");
      __sb.Append(Tmplate);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}
