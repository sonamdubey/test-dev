// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: category.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VehicleData.Service.ProtoClass {

  /// <summary>Holder for reflection information generated from category.proto</summary>
  public static partial class CategoryReflection {

    #region Descriptor
    /// <summary>File descriptor for category.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CategoryReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5jYXRlZ29yeS5wcm90bxILVmVoaWNsZURhdGEaCml0ZW0ucHJvdG8i2wEK",
            "CENhdGVnb3J5EgoKAklkGAEgASgFEgwKBE5hbWUYAiABKAkSDQoFTGV2ZWwY",
            "AyABKAUSEAoITm9kZUNvZGUYBCABKAkSEAoISXNBY3RpdmUYBSABKAgSEQoJ",
            "VXBkYXRlZE9uGAYgASgJEhEKCVVwZGF0ZWRCeRgHIAEoBRIVCg1BcHBsaWNh",
            "dGlvbklkGAggASgFEhUKDVByaW9yaXR5T3JkZXIYCSABKAUSIAoFaXRlbXMY",
<<<<<<< HEAD
            "CiADKAsyES5WZWhpY2xlRGF0YS5JdGVtIjQKD0NhdGVnb3J5UmVxdWVzdBIK",
            "CgJJZBgBIAEoBRIVCg1BcHBsaWNhdGlvbklkGAIgASgFIjcKEENhdGVnb3J5",
            "UmVzcG9uc2USCgoCSWQYASABKAUSFwoPSXNEYXRhQXZhaWxhYmxlGAIgASgI",
            "IjkKDENhdGVnb3J5TGlzdBIpCgpDYXRlZ29yaWVzGAEgAygLMhUuVmVoaWNs",
            "ZURhdGEuQ2F0ZWdvcnkiSQoUQ2F0ZWdvcnlSZXNwb25zZUxpc3QSMQoKQ2F0",
            "ZWdvcmllcxgBIAMoCzIdLlZlaGljbGVEYXRhLkNhdGVnb3J5UmVzcG9uc2VC",
            "IaoCHlZlaGljbGVEYXRhLlNlcnZpY2UuUHJvdG9DbGFzc2IGcHJvdG8z"));
=======
            "CiADKAsyES5WZWhpY2xlRGF0YS5JdGVtEgwKBEljb24YCyABKAkiNAoPQ2F0",
            "ZWdvcnlSZXF1ZXN0EgoKAklkGAEgASgFEhUKDUFwcGxpY2F0aW9uSWQYAiAB",
            "KAUiNwoQQ2F0ZWdvcnlSZXNwb25zZRIKCgJJZBgBIAEoBRIXCg9Jc0RhdGFB",
            "dmFpbGFibGUYAiABKAgiOQoMQ2F0ZWdvcnlMaXN0EikKCkNhdGVnb3JpZXMY",
            "ASADKAsyFS5WZWhpY2xlRGF0YS5DYXRlZ29yeSJJChRDYXRlZ29yeVJlc3Bv",
            "bnNlTGlzdBIxCgpDYXRlZ29yaWVzGAEgAygLMh0uVmVoaWNsZURhdGEuQ2F0",
            "ZWdvcnlSZXNwb25zZUIhqgIeVmVoaWNsZURhdGEuU2VydmljZS5Qcm90b0Ns",
            "YXNzYgZwcm90bzM="));
>>>>>>> 0c796b8739268785892b9d7e5f7038f952dc7e97
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::VehicleData.Service.ProtoClass.ItemReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.Category), global::VehicleData.Service.ProtoClass.Category.Parser, new[]{ "Id", "Name", "Level", "NodeCode", "IsActive", "UpdatedOn", "UpdatedBy", "ApplicationId", "PriorityOrder", "Items", "Icon" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.CategoryRequest), global::VehicleData.Service.ProtoClass.CategoryRequest.Parser, new[]{ "Id", "ApplicationId" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.CategoryResponse), global::VehicleData.Service.ProtoClass.CategoryResponse.Parser, new[]{ "Id", "IsDataAvailable" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.CategoryList), global::VehicleData.Service.ProtoClass.CategoryList.Parser, new[]{ "Categories" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.CategoryResponseList), global::VehicleData.Service.ProtoClass.CategoryResponseList.Parser, new[]{ "Categories" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Category : pb::IMessage<Category> {
    private static readonly pb::MessageParser<Category> _parser = new pb::MessageParser<Category>(() => new Category());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Category> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.CategoryReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Category() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Category(Category other) : this() {
      id_ = other.id_;
      name_ = other.name_;
      level_ = other.level_;
      nodeCode_ = other.nodeCode_;
      isActive_ = other.isActive_;
      updatedOn_ = other.updatedOn_;
      updatedBy_ = other.updatedBy_;
      applicationId_ = other.applicationId_;
      priorityOrder_ = other.priorityOrder_;
      items_ = other.items_.Clone();
      icon_ = other.icon_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Category Clone() {
      return new Category(this);
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

    /// <summary>Field number for the "Level" field.</summary>
    public const int LevelFieldNumber = 3;
    private int level_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    /// <summary>Field number for the "NodeCode" field.</summary>
    public const int NodeCodeFieldNumber = 4;
    private string nodeCode_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string NodeCode {
      get { return nodeCode_; }
      set {
        nodeCode_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
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

    /// <summary>Field number for the "UpdatedOn" field.</summary>
    public const int UpdatedOnFieldNumber = 6;
    private string updatedOn_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UpdatedOn {
      get { return updatedOn_; }
      set {
        updatedOn_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "UpdatedBy" field.</summary>
    public const int UpdatedByFieldNumber = 7;
    private int updatedBy_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UpdatedBy {
      get { return updatedBy_; }
      set {
        updatedBy_ = value;
      }
    }

    /// <summary>Field number for the "ApplicationId" field.</summary>
    public const int ApplicationIdFieldNumber = 8;
    private int applicationId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ApplicationId {
      get { return applicationId_; }
      set {
        applicationId_ = value;
      }
    }

    /// <summary>Field number for the "PriorityOrder" field.</summary>
    public const int PriorityOrderFieldNumber = 9;
    private int priorityOrder_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int PriorityOrder {
      get { return priorityOrder_; }
      set {
        priorityOrder_ = value;
      }
    }

    /// <summary>Field number for the "items" field.</summary>
    public const int ItemsFieldNumber = 10;
    private static readonly pb::FieldCodec<global::VehicleData.Service.ProtoClass.Item> _repeated_items_codec
        = pb::FieldCodec.ForMessage(82, global::VehicleData.Service.ProtoClass.Item.Parser);
    private readonly pbc::RepeatedField<global::VehicleData.Service.ProtoClass.Item> items_ = new pbc::RepeatedField<global::VehicleData.Service.ProtoClass.Item>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VehicleData.Service.ProtoClass.Item> Items {
      get { return items_; }
    }

    /// <summary>Field number for the "Icon" field.</summary>
    public const int IconFieldNumber = 11;
    private string icon_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Icon {
      get { return icon_; }
      set {
        icon_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Category);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Category other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Name != other.Name) return false;
      if (Level != other.Level) return false;
      if (NodeCode != other.NodeCode) return false;
      if (IsActive != other.IsActive) return false;
      if (UpdatedOn != other.UpdatedOn) return false;
      if (UpdatedBy != other.UpdatedBy) return false;
      if (ApplicationId != other.ApplicationId) return false;
      if (PriorityOrder != other.PriorityOrder) return false;
      if(!items_.Equals(other.items_)) return false;
      if (Icon != other.Icon) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Level != 0) hash ^= Level.GetHashCode();
      if (NodeCode.Length != 0) hash ^= NodeCode.GetHashCode();
      if (IsActive != false) hash ^= IsActive.GetHashCode();
      if (UpdatedOn.Length != 0) hash ^= UpdatedOn.GetHashCode();
      if (UpdatedBy != 0) hash ^= UpdatedBy.GetHashCode();
      if (ApplicationId != 0) hash ^= ApplicationId.GetHashCode();
      if (PriorityOrder != 0) hash ^= PriorityOrder.GetHashCode();
      hash ^= items_.GetHashCode();
      if (Icon.Length != 0) hash ^= Icon.GetHashCode();
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
      if (Level != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Level);
      }
      if (NodeCode.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(NodeCode);
      }
      if (IsActive != false) {
        output.WriteRawTag(40);
        output.WriteBool(IsActive);
      }
      if (UpdatedOn.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(UpdatedOn);
      }
      if (UpdatedBy != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(UpdatedBy);
      }
      if (ApplicationId != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(ApplicationId);
      }
      if (PriorityOrder != 0) {
        output.WriteRawTag(72);
        output.WriteInt32(PriorityOrder);
      }
      items_.WriteTo(output, _repeated_items_codec);
      if (Icon.Length != 0) {
        output.WriteRawTag(90);
        output.WriteString(Icon);
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
      if (Level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Level);
      }
      if (NodeCode.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(NodeCode);
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
      if (ApplicationId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ApplicationId);
      }
      if (PriorityOrder != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PriorityOrder);
      }
      size += items_.CalculateSize(_repeated_items_codec);
      if (Icon.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Icon);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Category other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Level != 0) {
        Level = other.Level;
      }
      if (other.NodeCode.Length != 0) {
        NodeCode = other.NodeCode;
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
      if (other.ApplicationId != 0) {
        ApplicationId = other.ApplicationId;
      }
      if (other.PriorityOrder != 0) {
        PriorityOrder = other.PriorityOrder;
      }
      items_.Add(other.items_);
      if (other.Icon.Length != 0) {
        Icon = other.Icon;
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
          case 24: {
            Level = input.ReadInt32();
            break;
          }
          case 34: {
            NodeCode = input.ReadString();
            break;
          }
          case 40: {
            IsActive = input.ReadBool();
            break;
          }
          case 50: {
            UpdatedOn = input.ReadString();
            break;
          }
          case 56: {
            UpdatedBy = input.ReadInt32();
            break;
          }
          case 64: {
            ApplicationId = input.ReadInt32();
            break;
          }
          case 72: {
            PriorityOrder = input.ReadInt32();
            break;
          }
          case 82: {
            items_.AddEntriesFrom(input, _repeated_items_codec);
            break;
          }
          case 90: {
            Icon = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class CategoryRequest : pb::IMessage<CategoryRequest> {
    private static readonly pb::MessageParser<CategoryRequest> _parser = new pb::MessageParser<CategoryRequest>(() => new CategoryRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CategoryRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.CategoryReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryRequest(CategoryRequest other) : this() {
      id_ = other.id_;
      applicationId_ = other.applicationId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryRequest Clone() {
      return new CategoryRequest(this);
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

    /// <summary>Field number for the "ApplicationId" field.</summary>
    public const int ApplicationIdFieldNumber = 2;
    private int applicationId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ApplicationId {
      get { return applicationId_; }
      set {
        applicationId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CategoryRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CategoryRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (ApplicationId != other.ApplicationId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (ApplicationId != 0) hash ^= ApplicationId.GetHashCode();
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
      if (ApplicationId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(ApplicationId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (ApplicationId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ApplicationId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CategoryRequest other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.ApplicationId != 0) {
        ApplicationId = other.ApplicationId;
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
            ApplicationId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class CategoryResponse : pb::IMessage<CategoryResponse> {
    private static readonly pb::MessageParser<CategoryResponse> _parser = new pb::MessageParser<CategoryResponse>(() => new CategoryResponse());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CategoryResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.CategoryReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryResponse(CategoryResponse other) : this() {
      id_ = other.id_;
      isDataAvailable_ = other.isDataAvailable_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryResponse Clone() {
      return new CategoryResponse(this);
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

    /// <summary>Field number for the "IsDataAvailable" field.</summary>
    public const int IsDataAvailableFieldNumber = 2;
    private bool isDataAvailable_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool IsDataAvailable {
      get { return isDataAvailable_; }
      set {
        isDataAvailable_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CategoryResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CategoryResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (IsDataAvailable != other.IsDataAvailable) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (IsDataAvailable != false) hash ^= IsDataAvailable.GetHashCode();
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
      if (IsDataAvailable != false) {
        output.WriteRawTag(16);
        output.WriteBool(IsDataAvailable);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (IsDataAvailable != false) {
        size += 1 + 1;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CategoryResponse other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.IsDataAvailable != false) {
        IsDataAvailable = other.IsDataAvailable;
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
            IsDataAvailable = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  public sealed partial class CategoryList : pb::IMessage<CategoryList> {
    private static readonly pb::MessageParser<CategoryList> _parser = new pb::MessageParser<CategoryList>(() => new CategoryList());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CategoryList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.CategoryReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryList(CategoryList other) : this() {
      categories_ = other.categories_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryList Clone() {
      return new CategoryList(this);
    }

    /// <summary>Field number for the "Categories" field.</summary>
    public const int CategoriesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::VehicleData.Service.ProtoClass.Category> _repeated_categories_codec
        = pb::FieldCodec.ForMessage(10, global::VehicleData.Service.ProtoClass.Category.Parser);
    private readonly pbc::RepeatedField<global::VehicleData.Service.ProtoClass.Category> categories_ = new pbc::RepeatedField<global::VehicleData.Service.ProtoClass.Category>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VehicleData.Service.ProtoClass.Category> Categories {
      get { return categories_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CategoryList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CategoryList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!categories_.Equals(other.categories_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= categories_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      categories_.WriteTo(output, _repeated_categories_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += categories_.CalculateSize(_repeated_categories_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CategoryList other) {
      if (other == null) {
        return;
      }
      categories_.Add(other.categories_);
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
            categories_.AddEntriesFrom(input, _repeated_categories_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class CategoryResponseList : pb::IMessage<CategoryResponseList> {
    private static readonly pb::MessageParser<CategoryResponseList> _parser = new pb::MessageParser<CategoryResponseList>(() => new CategoryResponseList());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CategoryResponseList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.CategoryReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryResponseList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryResponseList(CategoryResponseList other) : this() {
      categories_ = other.categories_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CategoryResponseList Clone() {
      return new CategoryResponseList(this);
    }

    /// <summary>Field number for the "Categories" field.</summary>
    public const int CategoriesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::VehicleData.Service.ProtoClass.CategoryResponse> _repeated_categories_codec
        = pb::FieldCodec.ForMessage(10, global::VehicleData.Service.ProtoClass.CategoryResponse.Parser);
    private readonly pbc::RepeatedField<global::VehicleData.Service.ProtoClass.CategoryResponse> categories_ = new pbc::RepeatedField<global::VehicleData.Service.ProtoClass.CategoryResponse>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VehicleData.Service.ProtoClass.CategoryResponse> Categories {
      get { return categories_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CategoryResponseList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CategoryResponseList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!categories_.Equals(other.categories_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= categories_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      categories_.WriteTo(output, _repeated_categories_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += categories_.CalculateSize(_repeated_categories_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CategoryResponseList other) {
      if (other == null) {
        return;
      }
      categories_.Add(other.categories_);
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
            categories_.AddEntriesFrom(input, _repeated_categories_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
