// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: settings.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace HandControl.Model.Protobuf {

  /// <summary>Holder for reflection information generated from settings.proto</summary>
  public static partial class SettingsReflection {

    #region Descriptor
    /// <summary>File descriptor for settings.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SettingsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5zZXR0aW5ncy5wcm90bxIIc2V0dGluZ3MaC2VudW1zLnByb3RvIokBCgtH",
            "ZXRTZXR0aW5ncxIiCgl0eXBlX3dvcmsYASABKA4yDy5lbnVtcy5Nb2RlVHlw",
            "ZRISCgplbmFibGVfZW1nGAIgASgIEhYKDmVuYWJsZV9kaXNwbGF5GAMgASgI",
            "EhMKC2VuYWJsZV9neXJvGAQgASgIEhUKDWVuYWJsZV9kcml2ZXIYBSABKAgi",
            "uQEKC1NldFNldHRpbmdzEiIKCXR5cGVfd29yaxgBIAEoDjIPLmVudW1zLk1v",
            "ZGVUeXBlEhsKE3RlbGVtZXRyeV9mcmVxdWVuY3kYAiABKAUSEgoKZW5hYmxl",
            "X2VtZxgDIAEoCBIWCg5lbmFibGVfZGlzcGxheRgEIAEoCBITCgtlbmFibGVf",
            "Z3lybxgFIAEoCBIVCg1lbmFibGVfZHJpdmVyGAYgASgIEhEKCXBvd2VyX29m",
            "ZhgHIAEoCEIdqgIaSGFuZENvbnRyb2wuTW9kZWwuUHJvdG9idWZiBnByb3Rv",
            "Mw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::HandControl.Model.Protobuf.EnumsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::HandControl.Model.Protobuf.GetSettings), global::HandControl.Model.Protobuf.GetSettings.Parser, new[]{ "TypeWork", "EnableEmg", "EnableDisplay", "EnableGyro", "EnableDriver" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::HandControl.Model.Protobuf.SetSettings), global::HandControl.Model.Protobuf.SetSettings.Parser, new[]{ "TypeWork", "TelemetryFrequency", "EnableEmg", "EnableDisplay", "EnableGyro", "EnableDriver", "PowerOff" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class GetSettings : pb::IMessage<GetSettings>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GetSettings> _parser = new pb::MessageParser<GetSettings>(() => new GetSettings());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GetSettings> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::HandControl.Model.Protobuf.SettingsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetSettings() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetSettings(GetSettings other) : this() {
      typeWork_ = other.typeWork_;
      enableEmg_ = other.enableEmg_;
      enableDisplay_ = other.enableDisplay_;
      enableGyro_ = other.enableGyro_;
      enableDriver_ = other.enableDriver_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetSettings Clone() {
      return new GetSettings(this);
    }

    /// <summary>Field number for the "type_work" field.</summary>
    public const int TypeWorkFieldNumber = 1;
    private global::HandControl.Model.Protobuf.ModeType typeWork_ = global::HandControl.Model.Protobuf.ModeType.ModeMio;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::HandControl.Model.Protobuf.ModeType TypeWork {
      get { return typeWork_; }
      set {
        typeWork_ = value;
      }
    }

    /// <summary>Field number for the "enable_emg" field.</summary>
    public const int EnableEmgFieldNumber = 2;
    private bool enableEmg_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableEmg {
      get { return enableEmg_; }
      set {
        enableEmg_ = value;
      }
    }

    /// <summary>Field number for the "enable_display" field.</summary>
    public const int EnableDisplayFieldNumber = 3;
    private bool enableDisplay_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableDisplay {
      get { return enableDisplay_; }
      set {
        enableDisplay_ = value;
      }
    }

    /// <summary>Field number for the "enable_gyro" field.</summary>
    public const int EnableGyroFieldNumber = 4;
    private bool enableGyro_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableGyro {
      get { return enableGyro_; }
      set {
        enableGyro_ = value;
      }
    }

    /// <summary>Field number for the "enable_driver" field.</summary>
    public const int EnableDriverFieldNumber = 5;
    private bool enableDriver_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableDriver {
      get { return enableDriver_; }
      set {
        enableDriver_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GetSettings);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GetSettings other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TypeWork != other.TypeWork) return false;
      if (EnableEmg != other.EnableEmg) return false;
      if (EnableDisplay != other.EnableDisplay) return false;
      if (EnableGyro != other.EnableGyro) return false;
      if (EnableDriver != other.EnableDriver) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) hash ^= TypeWork.GetHashCode();
      if (EnableEmg != false) hash ^= EnableEmg.GetHashCode();
      if (EnableDisplay != false) hash ^= EnableDisplay.GetHashCode();
      if (EnableGyro != false) hash ^= EnableGyro.GetHashCode();
      if (EnableDriver != false) hash ^= EnableDriver.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        output.WriteRawTag(8);
        output.WriteEnum((int) TypeWork);
      }
      if (EnableEmg != false) {
        output.WriteRawTag(16);
        output.WriteBool(EnableEmg);
      }
      if (EnableDisplay != false) {
        output.WriteRawTag(24);
        output.WriteBool(EnableDisplay);
      }
      if (EnableGyro != false) {
        output.WriteRawTag(32);
        output.WriteBool(EnableGyro);
      }
      if (EnableDriver != false) {
        output.WriteRawTag(40);
        output.WriteBool(EnableDriver);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        output.WriteRawTag(8);
        output.WriteEnum((int) TypeWork);
      }
      if (EnableEmg != false) {
        output.WriteRawTag(16);
        output.WriteBool(EnableEmg);
      }
      if (EnableDisplay != false) {
        output.WriteRawTag(24);
        output.WriteBool(EnableDisplay);
      }
      if (EnableGyro != false) {
        output.WriteRawTag(32);
        output.WriteBool(EnableGyro);
      }
      if (EnableDriver != false) {
        output.WriteRawTag(40);
        output.WriteBool(EnableDriver);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) TypeWork);
      }
      if (EnableEmg != false) {
        size += 1 + 1;
      }
      if (EnableDisplay != false) {
        size += 1 + 1;
      }
      if (EnableGyro != false) {
        size += 1 + 1;
      }
      if (EnableDriver != false) {
        size += 1 + 1;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GetSettings other) {
      if (other == null) {
        return;
      }
      if (other.TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        TypeWork = other.TypeWork;
      }
      if (other.EnableEmg != false) {
        EnableEmg = other.EnableEmg;
      }
      if (other.EnableDisplay != false) {
        EnableDisplay = other.EnableDisplay;
      }
      if (other.EnableGyro != false) {
        EnableGyro = other.EnableGyro;
      }
      if (other.EnableDriver != false) {
        EnableDriver = other.EnableDriver;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            TypeWork = (global::HandControl.Model.Protobuf.ModeType) input.ReadEnum();
            break;
          }
          case 16: {
            EnableEmg = input.ReadBool();
            break;
          }
          case 24: {
            EnableDisplay = input.ReadBool();
            break;
          }
          case 32: {
            EnableGyro = input.ReadBool();
            break;
          }
          case 40: {
            EnableDriver = input.ReadBool();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            TypeWork = (global::HandControl.Model.Protobuf.ModeType) input.ReadEnum();
            break;
          }
          case 16: {
            EnableEmg = input.ReadBool();
            break;
          }
          case 24: {
            EnableDisplay = input.ReadBool();
            break;
          }
          case 32: {
            EnableGyro = input.ReadBool();
            break;
          }
          case 40: {
            EnableDriver = input.ReadBool();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class SetSettings : pb::IMessage<SetSettings>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SetSettings> _parser = new pb::MessageParser<SetSettings>(() => new SetSettings());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SetSettings> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::HandControl.Model.Protobuf.SettingsReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SetSettings() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SetSettings(SetSettings other) : this() {
      typeWork_ = other.typeWork_;
      telemetryFrequency_ = other.telemetryFrequency_;
      enableEmg_ = other.enableEmg_;
      enableDisplay_ = other.enableDisplay_;
      enableGyro_ = other.enableGyro_;
      enableDriver_ = other.enableDriver_;
      powerOff_ = other.powerOff_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SetSettings Clone() {
      return new SetSettings(this);
    }

    /// <summary>Field number for the "type_work" field.</summary>
    public const int TypeWorkFieldNumber = 1;
    private global::HandControl.Model.Protobuf.ModeType typeWork_ = global::HandControl.Model.Protobuf.ModeType.ModeMio;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::HandControl.Model.Protobuf.ModeType TypeWork {
      get { return typeWork_; }
      set {
        typeWork_ = value;
      }
    }

    /// <summary>Field number for the "telemetry_frequency" field.</summary>
    public const int TelemetryFrequencyFieldNumber = 2;
    private int telemetryFrequency_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int TelemetryFrequency {
      get { return telemetryFrequency_; }
      set {
        telemetryFrequency_ = value;
      }
    }

    /// <summary>Field number for the "enable_emg" field.</summary>
    public const int EnableEmgFieldNumber = 3;
    private bool enableEmg_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableEmg {
      get { return enableEmg_; }
      set {
        enableEmg_ = value;
      }
    }

    /// <summary>Field number for the "enable_display" field.</summary>
    public const int EnableDisplayFieldNumber = 4;
    private bool enableDisplay_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableDisplay {
      get { return enableDisplay_; }
      set {
        enableDisplay_ = value;
      }
    }

    /// <summary>Field number for the "enable_gyro" field.</summary>
    public const int EnableGyroFieldNumber = 5;
    private bool enableGyro_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableGyro {
      get { return enableGyro_; }
      set {
        enableGyro_ = value;
      }
    }

    /// <summary>Field number for the "enable_driver" field.</summary>
    public const int EnableDriverFieldNumber = 6;
    private bool enableDriver_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool EnableDriver {
      get { return enableDriver_; }
      set {
        enableDriver_ = value;
      }
    }

    /// <summary>Field number for the "power_off" field.</summary>
    public const int PowerOffFieldNumber = 7;
    private bool powerOff_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool PowerOff {
      get { return powerOff_; }
      set {
        powerOff_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SetSettings);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SetSettings other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TypeWork != other.TypeWork) return false;
      if (TelemetryFrequency != other.TelemetryFrequency) return false;
      if (EnableEmg != other.EnableEmg) return false;
      if (EnableDisplay != other.EnableDisplay) return false;
      if (EnableGyro != other.EnableGyro) return false;
      if (EnableDriver != other.EnableDriver) return false;
      if (PowerOff != other.PowerOff) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) hash ^= TypeWork.GetHashCode();
      if (TelemetryFrequency != 0) hash ^= TelemetryFrequency.GetHashCode();
      if (EnableEmg != false) hash ^= EnableEmg.GetHashCode();
      if (EnableDisplay != false) hash ^= EnableDisplay.GetHashCode();
      if (EnableGyro != false) hash ^= EnableGyro.GetHashCode();
      if (EnableDriver != false) hash ^= EnableDriver.GetHashCode();
      if (PowerOff != false) hash ^= PowerOff.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        output.WriteRawTag(8);
        output.WriteEnum((int) TypeWork);
      }
      if (TelemetryFrequency != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(TelemetryFrequency);
      }
      if (EnableEmg != false) {
        output.WriteRawTag(24);
        output.WriteBool(EnableEmg);
      }
      if (EnableDisplay != false) {
        output.WriteRawTag(32);
        output.WriteBool(EnableDisplay);
      }
      if (EnableGyro != false) {
        output.WriteRawTag(40);
        output.WriteBool(EnableGyro);
      }
      if (EnableDriver != false) {
        output.WriteRawTag(48);
        output.WriteBool(EnableDriver);
      }
      if (PowerOff != false) {
        output.WriteRawTag(56);
        output.WriteBool(PowerOff);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        output.WriteRawTag(8);
        output.WriteEnum((int) TypeWork);
      }
      if (TelemetryFrequency != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(TelemetryFrequency);
      }
      if (EnableEmg != false) {
        output.WriteRawTag(24);
        output.WriteBool(EnableEmg);
      }
      if (EnableDisplay != false) {
        output.WriteRawTag(32);
        output.WriteBool(EnableDisplay);
      }
      if (EnableGyro != false) {
        output.WriteRawTag(40);
        output.WriteBool(EnableGyro);
      }
      if (EnableDriver != false) {
        output.WriteRawTag(48);
        output.WriteBool(EnableDriver);
      }
      if (PowerOff != false) {
        output.WriteRawTag(56);
        output.WriteBool(PowerOff);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) TypeWork);
      }
      if (TelemetryFrequency != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(TelemetryFrequency);
      }
      if (EnableEmg != false) {
        size += 1 + 1;
      }
      if (EnableDisplay != false) {
        size += 1 + 1;
      }
      if (EnableGyro != false) {
        size += 1 + 1;
      }
      if (EnableDriver != false) {
        size += 1 + 1;
      }
      if (PowerOff != false) {
        size += 1 + 1;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SetSettings other) {
      if (other == null) {
        return;
      }
      if (other.TypeWork != global::HandControl.Model.Protobuf.ModeType.ModeMio) {
        TypeWork = other.TypeWork;
      }
      if (other.TelemetryFrequency != 0) {
        TelemetryFrequency = other.TelemetryFrequency;
      }
      if (other.EnableEmg != false) {
        EnableEmg = other.EnableEmg;
      }
      if (other.EnableDisplay != false) {
        EnableDisplay = other.EnableDisplay;
      }
      if (other.EnableGyro != false) {
        EnableGyro = other.EnableGyro;
      }
      if (other.EnableDriver != false) {
        EnableDriver = other.EnableDriver;
      }
      if (other.PowerOff != false) {
        PowerOff = other.PowerOff;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            TypeWork = (global::HandControl.Model.Protobuf.ModeType) input.ReadEnum();
            break;
          }
          case 16: {
            TelemetryFrequency = input.ReadInt32();
            break;
          }
          case 24: {
            EnableEmg = input.ReadBool();
            break;
          }
          case 32: {
            EnableDisplay = input.ReadBool();
            break;
          }
          case 40: {
            EnableGyro = input.ReadBool();
            break;
          }
          case 48: {
            EnableDriver = input.ReadBool();
            break;
          }
          case 56: {
            PowerOff = input.ReadBool();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            TypeWork = (global::HandControl.Model.Protobuf.ModeType) input.ReadEnum();
            break;
          }
          case 16: {
            TelemetryFrequency = input.ReadInt32();
            break;
          }
          case 24: {
            EnableEmg = input.ReadBool();
            break;
          }
          case 32: {
            EnableDisplay = input.ReadBool();
            break;
          }
          case 40: {
            EnableGyro = input.ReadBool();
            break;
          }
          case 48: {
            EnableDriver = input.ReadBool();
            break;
          }
          case 56: {
            PowerOff = input.ReadBool();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
