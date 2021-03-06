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
public partial class RealtimeCapInfoLBS : TBase
{
  private string _id;
  private long _time;
  private string _channel;
  private byte[] _image;
  private long _beginTm;
  private long _endTm;
  private string _channelname;
  private string _address;
  private string _longitude;
  private string _latitude;
  private int _score;

  public string Id
  {
    get
    {
      return _id;
    }
    set
    {
      __isset.id = true;
      this._id = value;
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

  public byte[] Image
  {
    get
    {
      return _image;
    }
    set
    {
      __isset.image = true;
      this._image = value;
    }
  }

  public long BeginTm
  {
    get
    {
      return _beginTm;
    }
    set
    {
      __isset.beginTm = true;
      this._beginTm = value;
    }
  }

  public long EndTm
  {
    get
    {
      return _endTm;
    }
    set
    {
      __isset.endTm = true;
      this._endTm = value;
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


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool id;
    public bool time;
    public bool channel;
    public bool image;
    public bool beginTm;
    public bool endTm;
    public bool channelname;
    public bool address;
    public bool longitude;
    public bool latitude;
    public bool score;
  }

  public RealtimeCapInfoLBS() {
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
              Id = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              Time = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Channel = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              Image = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I64) {
              BeginTm = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I64) {
              EndTm = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.String) {
              Channelname = iprot.ReadString();
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
            if (field.Type == TType.String) {
              Longitude = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 10:
            if (field.Type == TType.String) {
              Latitude = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.I32) {
              Score = iprot.ReadI32();
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
      TStruct struc = new TStruct("RealtimeCapInfoLBS");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Id != null && __isset.id) {
        field.Name = "id";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Id);
        oprot.WriteFieldEnd();
      }
      if (__isset.time) {
        field.Name = "time";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Time);
        oprot.WriteFieldEnd();
      }
      if (Channel != null && __isset.channel) {
        field.Name = "channel";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Channel);
        oprot.WriteFieldEnd();
      }
      if (Image != null && __isset.image) {
        field.Name = "image";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Image);
        oprot.WriteFieldEnd();
      }
      if (__isset.beginTm) {
        field.Name = "beginTm";
        field.Type = TType.I64;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(BeginTm);
        oprot.WriteFieldEnd();
      }
      if (__isset.endTm) {
        field.Name = "endTm";
        field.Type = TType.I64;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(EndTm);
        oprot.WriteFieldEnd();
      }
      if (Channelname != null && __isset.channelname) {
        field.Name = "channelname";
        field.Type = TType.String;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Channelname);
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
      if (Longitude != null && __isset.longitude) {
        field.Name = "longitude";
        field.Type = TType.String;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Longitude);
        oprot.WriteFieldEnd();
      }
      if (Latitude != null && __isset.latitude) {
        field.Name = "latitude";
        field.Type = TType.String;
        field.ID = 10;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Latitude);
        oprot.WriteFieldEnd();
      }
      if (__isset.score) {
        field.Name = "score";
        field.Type = TType.I32;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Score);
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
    StringBuilder __sb = new StringBuilder("RealtimeCapInfoLBS(");
    bool __first = true;
    if (Id != null && __isset.id) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Id: ");
      __sb.Append(Id);
    }
    if (__isset.time) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Time: ");
      __sb.Append(Time);
    }
    if (Channel != null && __isset.channel) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channel: ");
      __sb.Append(Channel);
    }
    if (Image != null && __isset.image) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Image: ");
      __sb.Append(Image);
    }
    if (__isset.beginTm) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("BeginTm: ");
      __sb.Append(BeginTm);
    }
    if (__isset.endTm) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("EndTm: ");
      __sb.Append(EndTm);
    }
    if (Channelname != null && __isset.channelname) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Channelname: ");
      __sb.Append(Channelname);
    }
    if (Address != null && __isset.address) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Address: ");
      __sb.Append(Address);
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
    if (__isset.score) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Score: ");
      __sb.Append(Score);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

