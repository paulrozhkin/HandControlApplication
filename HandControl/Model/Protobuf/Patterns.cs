// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: patterns.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace HandControl.Model.Protobuf {

  /// <summary>Holder for reflection information generated from patterns.proto</summary>
  public static partial class PatternsReflection {

    #region Descriptor
    /// <summary>File descriptor for patterns.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PatternsReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5wYXR0ZXJucy5wcm90bxIIcGF0dGVybnMaCnV1aWQucHJvdG8iPQoKTWlv",
            "UGF0dGVybhIPCgdwYXR0ZXJuGAEgASgDEh4KCmdlc3R1cmVfaWQYAiABKAsy",
            "Ci51dWlkLlVVSUQiOAoOR2V0TWlvUGF0dGVybnMSJgoIcGF0dGVybnMYASAD",
            "KAsyFC5wYXR0ZXJucy5NaW9QYXR0ZXJuIjgKDlNldE1pb1BhdHRlcm5zEiYK",
            "CHBhdHRlcm5zGAEgAygLMhQucGF0dGVybnMuTWlvUGF0dGVybkIdqgIaSGFu",
            "ZENvbnRyb2wuTW9kZWwuUHJvdG9idWZiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::HandControl.Model.Protobuf.UuidReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::HandControl.Model.Protobuf.MioPattern), global::HandControl.Model.Protobuf.MioPattern.Parser, new[]{ "Pattern", "GestureId" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::HandControl.Model.Protobuf.GetMioPatterns), global::HandControl.Model.Protobuf.GetMioPatterns.Parser, new[]{ "Patterns" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::HandControl.Model.Protobuf.SetMioPatterns), global::HandControl.Model.Protobuf.SetMioPatterns.Parser, new[]{ "Patterns" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class MioPattern : pb::IMessage<MioPattern>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<MioPattern> _parser = new pb::MessageParser<MioPattern>(() => new MioPattern());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MioPattern> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::HandControl.Model.Protobuf.PatternsReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MioPattern() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MioPattern(MioPattern other) : this() {
      pattern_ = other.pattern_;
      gestureId_ = other.gestureId_ != null ? other.gestureId_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MioPattern Clone() {
      return new MioPattern(this);
    }

    /// <summary>Field number for the "pattern" field.</summary>
    public const int PatternFieldNumber = 1;
    private long pattern_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Pattern {
      get { return pattern_; }
      set {
        pattern_ = value;
      }
    }

    /// <summary>Field number for the "gesture_id" field.</summary>
    public const int GestureIdFieldNumber = 2;
    private global::HandControl.Model.Protobuf.UUID gestureId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::HandControl.Model.Protobuf.UUID GestureId {
      get { return gestureId_; }
      set {
        gestureId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MioPattern);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MioPattern other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Pattern != other.Pattern) return false;
      if (!object.Equals(GestureId, other.GestureId)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Pattern != 0L) hash ^= Pattern.GetHashCode();
      if (gestureId_ != null) hash ^= GestureId.GetHashCode();
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
      if (Pattern != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(Pattern);
      }
      if (gestureId_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(GestureId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Pattern != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(Pattern);
      }
      if (gestureId_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(GestureId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Pattern != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Pattern);
      }
      if (gestureId_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(GestureId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MioPattern other) {
      if (other == null) {
        return;
      }
      if (other.Pattern != 0L) {
        Pattern = other.Pattern;
      }
      if (other.gestureId_ != null) {
        if (gestureId_ == null) {
          GestureId = new global::HandControl.Model.Protobuf.UUID();
        }
        GestureId.MergeFrom(other.GestureId);
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
            Pattern = input.ReadInt64();
            break;
          }
          case 18: {
            if (gestureId_ == null) {
              GestureId = new global::HandControl.Model.Protobuf.UUID();
            }
            input.ReadMessage(GestureId);
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
            Pattern = input.ReadInt64();
            break;
          }
          case 18: {
            if (gestureId_ == null) {
              GestureId = new global::HandControl.Model.Protobuf.UUID();
            }
            input.ReadMessage(GestureId);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class GetMioPatterns : pb::IMessage<GetMioPatterns>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GetMioPatterns> _parser = new pb::MessageParser<GetMioPatterns>(() => new GetMioPatterns());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GetMioPatterns> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::HandControl.Model.Protobuf.PatternsReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetMioPatterns() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetMioPatterns(GetMioPatterns other) : this() {
      patterns_ = other.patterns_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetMioPatterns Clone() {
      return new GetMioPatterns(this);
    }

    /// <summary>Field number for the "patterns" field.</summary>
    public const int PatternsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::HandControl.Model.Protobuf.MioPattern> _repeated_patterns_codec
        = pb::FieldCodec.ForMessage(10, global::HandControl.Model.Protobuf.MioPattern.Parser);
    private readonly pbc::RepeatedField<global::HandControl.Model.Protobuf.MioPattern> patterns_ = new pbc::RepeatedField<global::HandControl.Model.Protobuf.MioPattern>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::HandControl.Model.Protobuf.MioPattern> Patterns {
      get { return patterns_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GetMioPatterns);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GetMioPatterns other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!patterns_.Equals(other.patterns_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= patterns_.GetHashCode();
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
      patterns_.WriteTo(output, _repeated_patterns_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      patterns_.WriteTo(ref output, _repeated_patterns_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += patterns_.CalculateSize(_repeated_patterns_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GetMioPatterns other) {
      if (other == null) {
        return;
      }
      patterns_.Add(other.patterns_);
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
          case 10: {
            patterns_.AddEntriesFrom(input, _repeated_patterns_codec);
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
          case 10: {
            patterns_.AddEntriesFrom(ref input, _repeated_patterns_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class SetMioPatterns : pb::IMessage<SetMioPatterns>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SetMioPatterns> _parser = new pb::MessageParser<SetMioPatterns>(() => new SetMioPatterns());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SetMioPatterns> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::HandControl.Model.Protobuf.PatternsReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SetMioPatterns() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SetMioPatterns(SetMioPatterns other) : this() {
      patterns_ = other.patterns_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SetMioPatterns Clone() {
      return new SetMioPatterns(this);
    }

    /// <summary>Field number for the "patterns" field.</summary>
    public const int PatternsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::HandControl.Model.Protobuf.MioPattern> _repeated_patterns_codec
        = pb::FieldCodec.ForMessage(10, global::HandControl.Model.Protobuf.MioPattern.Parser);
    private readonly pbc::RepeatedField<global::HandControl.Model.Protobuf.MioPattern> patterns_ = new pbc::RepeatedField<global::HandControl.Model.Protobuf.MioPattern>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::HandControl.Model.Protobuf.MioPattern> Patterns {
      get { return patterns_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SetMioPatterns);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SetMioPatterns other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!patterns_.Equals(other.patterns_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= patterns_.GetHashCode();
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
      patterns_.WriteTo(output, _repeated_patterns_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      patterns_.WriteTo(ref output, _repeated_patterns_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += patterns_.CalculateSize(_repeated_patterns_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SetMioPatterns other) {
      if (other == null) {
        return;
      }
      patterns_.Add(other.patterns_);
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
          case 10: {
            patterns_.AddEntriesFrom(input, _repeated_patterns_codec);
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
          case 10: {
            patterns_.AddEntriesFrom(ref input, _repeated_patterns_codec);
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
