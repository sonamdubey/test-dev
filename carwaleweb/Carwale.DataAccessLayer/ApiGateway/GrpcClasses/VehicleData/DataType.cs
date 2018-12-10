// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: dataType.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VehicleData.Service.ProtoClass {

  /// <summary>Holder for reflection information generated from dataType.proto</summary>
  public static partial class DataTypeReflection {

    #region Descriptor
    /// <summary>File descriptor for dataType.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DataTypeReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5kYXRhVHlwZS5wcm90bxILVmVoaWNsZURhdGEicQoIRGF0YVR5cGUSCgoC",
            "SWQYASABKAUSDAoETmFtZRgCIAEoCRITCgtEZXNjcmlwdGlvbhgDIAEoCRIQ",
            "CghJc0FjdGl2ZRgEIAEoCBIRCglVcGRhdGVkT24YBSABKAkSEQoJVXBkYXRl",
            "ZEJ5GAYgASgFIjgKDERhdGFUeXBlTGlzdBIoCglEYXRhVHlwZXMYASADKAsy",
            "FS5WZWhpY2xlRGF0YS5EYXRhVHlwZUIhqgIeVmVoaWNsZURhdGEuU2Vydmlj",
            "ZS5Qcm90b0NsYXNzYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.DataType), global::VehicleData.Service.ProtoClass.DataType.Parser, new[]{ "Id", "Name", "Description", "IsActive", "UpdatedOn", "UpdatedBy" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.DataTypeList), global::VehicleData.Service.ProtoClass.DataTypeList.Parser, new[]{ "DataTypes" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class DataType : pb::IMessage<DataType> {
    private static readonly pb::MessageParser<DataType> _parser = new pb::MessageParser<DataType>(() => new DataType());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DataType> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.DataTypeReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataType() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataType(DataType other) : this() {
      id_ = other.id_;
      name_ = other.name_;
      description_ = other.description_;
      isActive_ = other.isActive_;
      updatedOn_ = other.updatedOn_;
      updatedBy_ = other.updatedBy_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataType Clone() {
      return new DataType(this);
    }

    /// <summary>Field number for the "Id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "Name" field.</summary>
    public const int NameFieldNumber = 2;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Description" field.</summary>
    public const int DescriptionFieldNumber = 3;
    private string description_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Description {
      get { return description_; }
      set {
        description_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "IsActive" field.</summary>
    public const int IsActiveFieldNumber = 4;
    private bool isActive_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool IsActive {
      get { return isActive_; }
      set {
        isActive_ = value;
      }
    }

    /// <summary>Field number for the "UpdatedOn" field.</summary>
    public const int UpdatedOnFieldNumber = 5;
    private string updatedOn_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UpdatedOn {
      get { return updatedOn_; }
      set {
        updatedOn_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "UpdatedBy" field.</summary>
    public const int UpdatedByFieldNumber = 6;
    private int updatedBy_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UpdatedBy {
      get { return updatedBy_; }
      set {
        updatedBy_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DataType);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DataType other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Name != other.Name) return false;
      if (Description != other.Description) return false;
      if (IsActive != other.IsActive) return false;
      if (UpdatedOn != other.UpdatedOn) return false;
      if (UpdatedBy != other.UpdatedBy) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Description.Length != 0) hash ^= Description.GetHashCode();
      if (IsActive != false) hash ^= IsActive.GetHashCode();
      if (UpdatedOn.Length != 0) hash ^= UpdatedOn.GetHashCode();
      if (UpdatedBy != 0) hash ^= UpdatedBy.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Id);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (Description.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Description);
      }
      if (IsActive != false) {
        output.WriteRawTag(32);
        output.WriteBool(IsActive);
      }
      if (UpdatedOn.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(UpdatedOn);
      }
      if (UpdatedBy != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(UpdatedBy);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Description.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Description);
      }
      if (IsActive != false) {
        size += 1 + 1;
      }
      if (UpdatedOn.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UpdatedOn);
      }
      if (UpdatedBy != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(UpdatedBy);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DataType other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Description.Length != 0) {
        Description = other.Description;
      }
      if (other.IsActive != false) {
        IsActive = other.IsActive;
      }
      if (other.UpdatedOn.Length != 0) {
        UpdatedOn = other.UpdatedOn;
      }
      if (other.UpdatedBy != 0) {
        UpdatedBy = other.UpdatedBy;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Id = input.ReadInt32();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 26: {
            Description = input.ReadString();
            break;
          }
          case 32: {
            IsActive = input.ReadBool();
            break;
          }
          case 42: {
            UpdatedOn = input.ReadString();
            break;
          }
          case 48: {
            UpdatedBy = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class DataTypeList : pb::IMessage<DataTypeList> {
    private static readonly pb::MessageParser<DataTypeList> _parser = new pb::MessageParser<DataTypeList>(() => new DataTypeList());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DataTypeList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.DataTypeReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataTypeList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataTypeList(DataTypeList other) : this() {
      dataTypes_ = other.dataTypes_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DataTypeList Clone() {
      return new DataTypeList(this);
    }

    /// <summary>Field number for the "DataTypes" field.</summary>
    public const int DataTypesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::VehicleData.Service.ProtoClass.DataType> _repeated_dataTypes_codec
        = pb::FieldCodec.ForMessage(10, global::VehicleData.Service.ProtoClass.DataType.Parser);
    private readonly pbc::RepeatedField<global::VehicleData.Service.ProtoClass.DataType> dataTypes_ = new pbc::RepeatedField<global::VehicleData.Service.ProtoClass.DataType>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VehicleData.Service.ProtoClass.DataType> DataTypes {
      get { return dataTypes_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DataTypeList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DataTypeList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!dataTypes_.Equals(other.dataTypes_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= dataTypes_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      dataTypes_.WriteTo(output, _repeated_dataTypes_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += dataTypes_.CalculateSize(_repeated_dataTypes_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DataTypeList other) {
      if (other == null) {
        return;
      }
      dataTypes_.Add(other.dataTypes_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            dataTypes_.AddEntriesFrom(input, _repeated_dataTypes_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
