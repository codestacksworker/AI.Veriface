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
public partial class CmpFaceLogDSLBS : TBase
{
  private string _ID;
  private string _channel;
  private long _time;
  private string _channelname;
  private byte[] _capimg;
  private byte[] _senceimg;
  private List<CmpFaceObjDS> _ft;
  private string _address;
  private int _channel_type;
  private string _typestr;
  private string _longitude;
  private string _latitude;

  public string ID
  {
    get
    {
      return _ID;
    }
    set
    {
      __isset.ID = true;
      this._ID = value;
    }
  }

  public string Channel
  {
    get
    {
      return _channel;
    }
    set
    {
      __isset.channel = true;
      this._channel = value;
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

  public string Channelname
  {
    get
    {
      return _channelname;
    }
    set
    {
      __isset.channelname = true;
      this._channelname = value;
    }
  }

  public byte[] Capimg
  {
    get
    {
      return _capimg;
    }
    set
    {
      __isset.capimg = true;
      this._capimg = value;
    }
  }

  public byte[] Senceimg
  {
    get
    {
      return _senceimg;
    }
    set
    {
      __isset.senceimg = true;
      this._senceimg = value;
    }
  }

  public List<CmpFaceObjDS> Ft
  {
    get
    {
      return _ft;
    }
    set
    {
      __isset.ft = true;
      this._ft = value;
    }
  }

  public string Address
  {
    get
    {
      return _address;
    }
    set
    {
      __isset.address = true;
      this._address = value;
    }
  }

  public int Channel_type
  {
    get
    {
      return _channel_type;
    }
    set
    {
      __isset.channel_type = true;
      this._channel_type = value;
    }
  }

  public string Typestr
  {
    get
    {
      return _typestr;
    }
    set
    {
      __isset.typestr = true;
      this._typestr = value;
    }
  }

  public string Longitude
  {
    get
    {
      return _longitude;
    }
    set
    {
      __isset.longitude = true;
      this._longitude = value;
    }
  }

  public string Latitude
  {
    get
    {
      return _latitude;
    }
    set
    {
      __isset.latitude = true;
      this._latitude = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool ID;
    public bool channel;
    public bool time;
    public bool channelname;
    public bool capimg;
    public bool senceimg;
    public bool ft;
    public bool address;
    public bool channel_type;
    public bool typestr;
    public bool longitude;
    public bool latitude;
  }

  public CmpFaceLogDSLBS() {
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
              ID = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Channel = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              Time = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              Channelname = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              Capimg = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              Senceimg = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.List) {
              {
                Ft = new List<CmpFaceObjDS>();
                TList _list48 = iprot.ReadListBegin();
                for( int _i49 = 0; _i49 < _list48.Count; ++_i49)
                {
                  CmpFaceObjDS _elem50;
                  _elem50 = new CmpFaceObjDS();
                  _elem50.Read(iprot);
                  Ft.Add(_elem50);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.String) {
              Address = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.I32) {
              Channel_type = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.String) {
              Typestr = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.String) {
              Longitude = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.String) {
              Latitude = iprot.ReadString();
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
      TStruct struc = new TStruct("CmpFaceLogDSLBS");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (ID != null && __isset.ID) {
        field.Name = "ID";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ID);
        oprot.WriteFieldEnd();
      }
      if (Channel != null && __isset.channel) {
        field.Name = "channel";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Channel);
        oprot.WriteFieldEnd();
      }
      if (__isset.time) {
        field.Name = "time";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Time);
        oprot.WriteFieldEnd();
      }
      if (Channelname != null && __isset.channelname) {
        field.Name = "channelname";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Channelname);
        oprot.WriteFieldEnd();
      }
      if (Capimg != null && __isset.capimg) {
        field.Name = "capimg";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Capimg);
        oprot.WriteFieldEnd();
      }
      if (Senceimg != null && __isset.senceimg) {
        field.Name = "senceimg";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Senceimg);
        oprot.WriteFieldEnd();
      }
      if (Ft != null && __isset.ft) {
        field.Name = "ft";
        field.Type = TType.List;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, Ft.Count));
          foreach (CmpFaceObjDS _iter51 in Ft)
          {
            _iter51.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (Address != null && __isset.address) {
        field.Name = "address";
        field.Type = TType.String;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Address);
        oprot.WriteFieldEnd();
      }
      if (__isset.channel_type) {
        field.Name = "channel_type";
        field.Type = TType.I32;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Channel_type);
        oprot.WriteFieldEnd();
      }
      if (Typestr != null && __isset.typestr) {
        field.Name = "typestr";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Typestr);
        oprot.WriteFieldEnd();
      }
      if (Longitude != null && __isset.longitude) {
        field.Name = "longitude";
        field.Type = TType.String;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Longitude);
        oprot.WriteFieldEnd();
      }
      if (Latitude != null && __isset.latitude) {
        field.Name = "latitude";
        field.Type = TType.String;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Latitude);
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
    StringBuilder __sb = new StringBuilder("CmpFaceLogDSLBS(");
    bool __first = true;
    if (ID != null && __isset.ID) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ID: ");
      __sb.Append(ID);
    }
    if (Channel != null && __isset.channel) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channel: ");
      __sb.Append(Channel);
    }
    if (__isset.time) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Time: ");
      __sb.Append(Time);
    }
    if (Channelname != null && __isset.channelname) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channelname: ");
      __sb.Append(Channelname);
    }
    if (Capimg != null && __isset.capimg) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Capimg: ");
      __sb.Append(Capimg);
    }
    if (Senceimg != null && __isset.senceimg) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Senceimg: ");
      __sb.Append(Senceimg);
    }
    if (Ft != null && __isset.ft) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Ft: ");
      __sb.Append(Ft);
    }
    if (Address != null && __isset.address) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Address: ");
      __sb.Append(Address);
    }
    if (__isset.channel_type) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channel_type: ");
      __sb.Append(Channel_type);
    }
    if (Typestr != null && __isset.typestr) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Typestr: ");
      __sb.Append(Typestr);
    }
    if (Longitude != null && __isset.longitude) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Longitude: ");
      __sb.Append(Longitude);
    }
    if (Latitude != null && __isset.latitude) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Latitude: ");
      __sb.Append(Latitude);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

