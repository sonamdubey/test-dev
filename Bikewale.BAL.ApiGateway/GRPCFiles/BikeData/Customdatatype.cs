// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: customDataType.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
namespace VehicleData.Service.ProtoClass
{

    /// <summary>Holder for reflection information generated from customDataType.proto</summary>
    public static partial class CustomDataTypeReflection {

    #region Descriptor
    /// <summary>File descriptor for customDataType.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CustomDataTypeReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRjdXN0b21EYXRhVHlwZS5wcm90bxILVmVoaWNsZURhdGEiswEKDkN1c3Rv",
            "bURhdGFUeXBlEgoKAklkGAEgASgFEg4KBkl0ZW1JZBgCIAEoBRIMCgROYW1l",
            "GAMgASgJEhMKC0Rlc2NyaXB0aW9uGAQgASgJEhAKCElzQWN0aXZlGAUgASgI",
            "EhEKCVNvcnRPcmRlchgGIAEoBRIXCg9WYWx1ZUltcG9ydGFuY2UYByABKAES",
            "EQoJVXBkYXRlZE9uGAggASgJEhEKCVVwZGF0ZWRCeRgJIAEoBSJKChJDdXN0",
            "b21EYXRhVHlwZUxpc3QSNAoPQ3VzdG9tRGF0YVR5cGVzGAEgAygLMhsuVmVo",
            "aWNsZURhdGEuQ3VzdG9tRGF0YVR5cGVCIaoCHlZlaGljbGVEYXRhLlNlcnZp",
            "Y2UuUHJvdG9DbGFzc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.CustomDataType), global::VehicleData.Service.ProtoClass.CustomDataType.Parser, new[]{ "Id", "ItemId", "Name", "Description", "IsActive", "SortOrder", "ValueImportance", "UpdatedOn", "UpdatedBy" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.CustomDataTypeList), global::VehicleData.Service.ProtoClass.CustomDataTypeList.Parser, new[]{ "CustomDataTypes" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CustomDataType : pb::IMessage<CustomDataType> {
    private static readonly pb::MessageParser<CustomDataType> _parser = new pb::MessageParser<CustomDataType>(() => new CustomDataType());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CustomDataType> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.CustomDataTypeReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CustomDataType() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CustomDataType(CustomDataType other) : this() {
      id_ = other.id_;
      itemId_ = other.itemId_;
      name_ = other.name_;
      description_ = other.description_;
      isActive_ = other.isActive_;
      sortOrder_ = other.sortOrder_;
      valueImportance_ = other.valueImportance_;
      updatedOn_ = other.updatedOn_;
      updatedBy_ = other.updatedBy_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CustomDataType Clone() {
      return new CustomDataType(this);
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

    /// <summary>Field number for the "ItemId" field.</summary>
    public const int ItemIdFieldNumber = 2;
    private int itemId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ItemId {
      get { return itemId_; }
      set {
        itemId_ = value;
      }
    }

    /// <summary>Field number for the "Name" field.</summary>
    public const int NameFieldNumber = 3;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Description" field.</summary>
    public const int DescriptionFieldNumber = 4;
    private string description_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Description {
      get { return description_; }
      set {
        description_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "IsActive" field.</summary>
    public const int IsActiveFieldNumber = 5;
    private bool isActive_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool IsActive {
      get { return isActive_; }
      set {
        isActive_ = value;
      }
    }

    /// <summary>Field number for the "SortOrder" field.</summary>
    public const int SortOrderFieldNumber = 6;
    private int sortOrder_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SortOrder {
      get { return sortOrder_; }
      set {
        sortOrder_ = value;
      }
    }

    /// <summary>Field number for the "ValueImportance" field.</summary>
    public const int ValueImportanceFieldNumber = 7;
    private double valueImportance_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double ValueImportance {
      get { return valueImportance_; }
      set {
        valueImportance_ = value;
      }
    }

    /// <summary>Field number for the "UpdatedOn" field.</summary>
    public const int UpdatedOnFieldNumber = 8;
    private string updatedOn_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UpdatedOn {
      get { return updatedOn_; }
      set {
        updatedOn_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "UpdatedBy" field.</summary>
    public const int UpdatedByFieldNumber = 9;
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
      return Equals(other as CustomDataType);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CustomDataType other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (ItemId != other.ItemId) return false;
      if (Name != other.Name) return false;
      if (Description != other.Description) return false;
      if (IsActive != other.IsActive) return false;
      if (SortOrder != other.SortOrder) return false;
      if (ValueImportance != other.ValueImportance) return false;
      if (UpdatedOn != other.UpdatedOn) return false;
      if (UpdatedBy != other.UpdatedBy) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (ItemId != 0) hash ^= ItemId.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Description.Length != 0) hash ^= Description.GetHashCode();
      if (IsActive != false) hash ^= IsActive.GetHashCode();
      if (SortOrder != 0) hash ^= SortOrder.GetHashCode();
      if (ValueImportance != 0D) hash ^= ValueImportance.GetHashCode();
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
      if (ItemId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(ItemId);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Name);
      }
      if (Description.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Description);
      }
      if (IsActive != false) {
        output.WriteRawTag(40);
        output.WriteBool(IsActive);
      }
      if (SortOrder != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(SortOrder);
      }
      if (ValueImportance != 0D) {
        output.WriteRawTag(57);
        output.WriteDouble(ValueImportance);
      }
      if (UpdatedOn.Length != 0) {
        output.WriteRawTag(66);
        output.WriteString(UpdatedOn);
      }
      if (UpdatedBy != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(UpdatedBy);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (ItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ItemId);
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
      if (SortOrder != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SortOrder);
      }
      if (ValueImportance != 0D) {
        size += 1 + 8;
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
    public void MergeFrom(CustomDataType other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.ItemId != 0) {
        ItemId = other.ItemId;
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
      if (other.SortOrder != 0) {
        SortOrder = other.SortOrder;
      }
      if (other.ValueImportance != 0D) {
        ValueImportance = other.ValueImportance;
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
          case 16: {
            ItemId = input.ReadInt32();
            break;
          }
          case 26: {
            Name = input.ReadString();
            break;
          }
          case 34: {
            Description = input.ReadString();
            break;
          }
          case 40: {
            IsActive = input.ReadBool();
            break;
          }
          case 48: {
            SortOrder = input.ReadInt32();
            break;
          }
          case 57: {
            ValueImportance = input.ReadDouble();
            break;
          }
          case 66: {
            UpdatedOn = input.ReadString();
            break;
          }
          case 72: {
            UpdatedBy = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class CustomDataTypeList : pb::IMessage<CustomDataTypeList> {
    private static readonly pb::MessageParser<CustomDataTypeList> _parser = new pb::MessageParser<CustomDataTypeList>(() => new CustomDataTypeList());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CustomDataTypeList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.CustomDataTypeReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CustomDataTypeList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CustomDataTypeList(CustomDataTypeList other) : this() {
      customDataTypes_ = other.customDataTypes_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CustomDataTypeList Clone() {
      return new CustomDataTypeList(this);
    }

    /// <summary>Field number for the "CustomDataTypes" field.</summary>
    public const int CustomDataTypesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::VehicleData.Service.ProtoClass.CustomDataType> _repeated_customDataTypes_codec
        = pb::FieldCodec.ForMessage(10, global::VehicleData.Service.ProtoClass.CustomDataType.Parser);
    private readonly pbc::RepeatedField<global::VehicleData.Service.ProtoClass.CustomDataType> customDataTypes_ = new pbc::RepeatedField<global::VehicleData.Service.ProtoClass.CustomDataType>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VehicleData.Service.ProtoClass.CustomDataType> CustomDataTypes {
      get { return customDataTypes_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CustomDataTypeList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CustomDataTypeList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!customDataTypes_.Equals(other.customDataTypes_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= customDataTypes_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      customDataTypes_.WriteTo(output, _repeated_customDataTypes_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += customDataTypes_.CalculateSize(_repeated_customDataTypes_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CustomDataTypeList other) {
      if (other == null) {
        return;
      }
      customDataTypes_.Add(other.customDataTypes_);
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
            customDataTypes_.AddEntriesFrom(input, _repeated_customDataTypes_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
