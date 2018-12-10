// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: modelItem.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace VehicleData.Service.ProtoClass {

  /// <summary>Holder for reflection information generated from modelItem.proto</summary>
  public static partial class ModelItemReflection {

    #region Descriptor
    /// <summary>File descriptor for modelItem.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ModelItemReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg9tb2RlbEl0ZW0ucHJvdG8SC1ZlaGljbGVEYXRhIrQBCglNb2RlbEl0ZW0S",
            "CgoCSWQYASABKAUSDwoHTW9kZWxJZBgCIAEoBRIOCgZJdGVtSWQYAyABKAUS",
            "EAoISXRlbU5hbWUYBCABKAkSEwoLRGVzY3JpcHRpb24YBSABKAkSFgoOT2Vt",
            "RGVzY3JpcHRpb24YBiABKAkSFQoNQXBwbGljYXRpb25JZBgHIAEoBRIRCglV",
            "cGRhdGVkT24YCCABKAkSEQoJVXBkYXRlZEJ5GAkgASgFIjsKEU1vZGVsSXRl",
            "bXNSZXF1ZXN0Eg8KB01vZGVsSWQYASABKAUSFQoNQXBwbGljYXRpb25JZBgC",
            "IAEoBSJPChRNb2RlbEl0ZW1zRGVsZXRlTGlzdBIPCgdNb2RlbElkGAEgASgF",
            "Eg8KB0l0ZW1JZHMYAiADKAUSFQoNQXBwbGljYXRpb25JZBgDIAEoBSI8Cg5N",
            "b2RlbEl0ZW1zTGlzdBIqCgpNb2RlbEl0ZW1zGAEgAygLMhYuVmVoaWNsZURh",
            "dGEuTW9kZWxJdGVtQiGqAh5WZWhpY2xlRGF0YS5TZXJ2aWNlLlByb3RvQ2xh",
            "c3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.ModelItem), global::VehicleData.Service.ProtoClass.ModelItem.Parser, new[]{ "Id", "ModelId", "ItemId", "ItemName", "Description", "OemDescription", "ApplicationId", "UpdatedOn", "UpdatedBy" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.ModelItemsRequest), global::VehicleData.Service.ProtoClass.ModelItemsRequest.Parser, new[]{ "ModelId", "ApplicationId" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.ModelItemsDeleteList), global::VehicleData.Service.ProtoClass.ModelItemsDeleteList.Parser, new[]{ "ModelId", "ItemIds", "ApplicationId" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::VehicleData.Service.ProtoClass.ModelItemsList), global::VehicleData.Service.ProtoClass.ModelItemsList.Parser, new[]{ "ModelItems" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ModelItem : pb::IMessage<ModelItem> {
    private static readonly pb::MessageParser<ModelItem> _parser = new pb::MessageParser<ModelItem>(() => new ModelItem());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ModelItem> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.ModelItemReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItem() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItem(ModelItem other) : this() {
      id_ = other.id_;
      modelId_ = other.modelId_;
      itemId_ = other.itemId_;
      itemName_ = other.itemName_;
      description_ = other.description_;
      oemDescription_ = other.oemDescription_;
      applicationId_ = other.applicationId_;
      updatedOn_ = other.updatedOn_;
      updatedBy_ = other.updatedBy_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItem Clone() {
      return new ModelItem(this);
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

    /// <summary>Field number for the "ModelId" field.</summary>
    public const int ModelIdFieldNumber = 2;
    private int modelId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ModelId {
      get { return modelId_; }
      set {
        modelId_ = value;
      }
    }

    /// <summary>Field number for the "ItemId" field.</summary>
    public const int ItemIdFieldNumber = 3;
    private int itemId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ItemId {
      get { return itemId_; }
      set {
        itemId_ = value;
      }
    }

    /// <summary>Field number for the "ItemName" field.</summary>
    public const int ItemNameFieldNumber = 4;
    private string itemName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ItemName {
      get { return itemName_; }
      set {
        itemName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "Description" field.</summary>
    public const int DescriptionFieldNumber = 5;
    private string description_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Description {
      get { return description_; }
      set {
        description_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "OemDescription" field.</summary>
    public const int OemDescriptionFieldNumber = 6;
    private string oemDescription_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string OemDescription {
      get { return oemDescription_; }
      set {
        oemDescription_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "ApplicationId" field.</summary>
    public const int ApplicationIdFieldNumber = 7;
    private int applicationId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ApplicationId {
      get { return applicationId_; }
      set {
        applicationId_ = value;
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
      return Equals(other as ModelItem);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ModelItem other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (ModelId != other.ModelId) return false;
      if (ItemId != other.ItemId) return false;
      if (ItemName != other.ItemName) return false;
      if (Description != other.Description) return false;
      if (OemDescription != other.OemDescription) return false;
      if (ApplicationId != other.ApplicationId) return false;
      if (UpdatedOn != other.UpdatedOn) return false;
      if (UpdatedBy != other.UpdatedBy) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (ModelId != 0) hash ^= ModelId.GetHashCode();
      if (ItemId != 0) hash ^= ItemId.GetHashCode();
      if (ItemName.Length != 0) hash ^= ItemName.GetHashCode();
      if (Description.Length != 0) hash ^= Description.GetHashCode();
      if (OemDescription.Length != 0) hash ^= OemDescription.GetHashCode();
      if (ApplicationId != 0) hash ^= ApplicationId.GetHashCode();
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
      if (ModelId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(ModelId);
      }
      if (ItemId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(ItemId);
      }
      if (ItemName.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ItemName);
      }
      if (Description.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Description);
      }
      if (OemDescription.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(OemDescription);
      }
      if (ApplicationId != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(ApplicationId);
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
      if (ModelId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ModelId);
      }
      if (ItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ItemId);
      }
      if (ItemName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ItemName);
      }
      if (Description.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Description);
      }
      if (OemDescription.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(OemDescription);
      }
      if (ApplicationId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ApplicationId);
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
    public void MergeFrom(ModelItem other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.ModelId != 0) {
        ModelId = other.ModelId;
      }
      if (other.ItemId != 0) {
        ItemId = other.ItemId;
      }
      if (other.ItemName.Length != 0) {
        ItemName = other.ItemName;
      }
      if (other.Description.Length != 0) {
        Description = other.Description;
      }
      if (other.OemDescription.Length != 0) {
        OemDescription = other.OemDescription;
      }
      if (other.ApplicationId != 0) {
        ApplicationId = other.ApplicationId;
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
            ModelId = input.ReadInt32();
            break;
          }
          case 24: {
            ItemId = input.ReadInt32();
            break;
          }
          case 34: {
            ItemName = input.ReadString();
            break;
          }
          case 42: {
            Description = input.ReadString();
            break;
          }
          case 50: {
            OemDescription = input.ReadString();
            break;
          }
          case 56: {
            ApplicationId = input.ReadInt32();
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

  public sealed partial class ModelItemsRequest : pb::IMessage<ModelItemsRequest> {
    private static readonly pb::MessageParser<ModelItemsRequest> _parser = new pb::MessageParser<ModelItemsRequest>(() => new ModelItemsRequest());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ModelItemsRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.ModelItemReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsRequest(ModelItemsRequest other) : this() {
      modelId_ = other.modelId_;
      applicationId_ = other.applicationId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsRequest Clone() {
      return new ModelItemsRequest(this);
    }

    /// <summary>Field number for the "ModelId" field.</summary>
    public const int ModelIdFieldNumber = 1;
    private int modelId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ModelId {
      get { return modelId_; }
      set {
        modelId_ = value;
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
      return Equals(other as ModelItemsRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ModelItemsRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ModelId != other.ModelId) return false;
      if (ApplicationId != other.ApplicationId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ModelId != 0) hash ^= ModelId.GetHashCode();
      if (ApplicationId != 0) hash ^= ApplicationId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ModelId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ModelId);
      }
      if (ApplicationId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(ApplicationId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ModelId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ModelId);
      }
      if (ApplicationId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ApplicationId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ModelItemsRequest other) {
      if (other == null) {
        return;
      }
      if (other.ModelId != 0) {
        ModelId = other.ModelId;
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
            ModelId = input.ReadInt32();
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

  public sealed partial class ModelItemsDeleteList : pb::IMessage<ModelItemsDeleteList> {
    private static readonly pb::MessageParser<ModelItemsDeleteList> _parser = new pb::MessageParser<ModelItemsDeleteList>(() => new ModelItemsDeleteList());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ModelItemsDeleteList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.ModelItemReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsDeleteList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsDeleteList(ModelItemsDeleteList other) : this() {
      modelId_ = other.modelId_;
      itemIds_ = other.itemIds_.Clone();
      applicationId_ = other.applicationId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsDeleteList Clone() {
      return new ModelItemsDeleteList(this);
    }

    /// <summary>Field number for the "ModelId" field.</summary>
    public const int ModelIdFieldNumber = 1;
    private int modelId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ModelId {
      get { return modelId_; }
      set {
        modelId_ = value;
      }
    }

    /// <summary>Field number for the "ItemIds" field.</summary>
    public const int ItemIdsFieldNumber = 2;
    private static readonly pb::FieldCodec<int> _repeated_itemIds_codec
        = pb::FieldCodec.ForInt32(18);
    private readonly pbc::RepeatedField<int> itemIds_ = new pbc::RepeatedField<int>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> ItemIds {
      get { return itemIds_; }
    }

    /// <summary>Field number for the "ApplicationId" field.</summary>
    public const int ApplicationIdFieldNumber = 3;
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
      return Equals(other as ModelItemsDeleteList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ModelItemsDeleteList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ModelId != other.ModelId) return false;
      if(!itemIds_.Equals(other.itemIds_)) return false;
      if (ApplicationId != other.ApplicationId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ModelId != 0) hash ^= ModelId.GetHashCode();
      hash ^= itemIds_.GetHashCode();
      if (ApplicationId != 0) hash ^= ApplicationId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ModelId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ModelId);
      }
      itemIds_.WriteTo(output, _repeated_itemIds_codec);
      if (ApplicationId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(ApplicationId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ModelId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ModelId);
      }
      size += itemIds_.CalculateSize(_repeated_itemIds_codec);
      if (ApplicationId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ApplicationId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ModelItemsDeleteList other) {
      if (other == null) {
        return;
      }
      if (other.ModelId != 0) {
        ModelId = other.ModelId;
      }
      itemIds_.Add(other.itemIds_);
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
            ModelId = input.ReadInt32();
            break;
          }
          case 18:
          case 16: {
            itemIds_.AddEntriesFrom(input, _repeated_itemIds_codec);
            break;
          }
          case 24: {
            ApplicationId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class ModelItemsList : pb::IMessage<ModelItemsList> {
    private static readonly pb::MessageParser<ModelItemsList> _parser = new pb::MessageParser<ModelItemsList>(() => new ModelItemsList());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ModelItemsList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::VehicleData.Service.ProtoClass.ModelItemReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsList(ModelItemsList other) : this() {
      modelItems_ = other.modelItems_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ModelItemsList Clone() {
      return new ModelItemsList(this);
    }

    /// <summary>Field number for the "ModelItems" field.</summary>
    public const int ModelItemsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::VehicleData.Service.ProtoClass.ModelItem> _repeated_modelItems_codec
        = pb::FieldCodec.ForMessage(10, global::VehicleData.Service.ProtoClass.ModelItem.Parser);
    private readonly pbc::RepeatedField<global::VehicleData.Service.ProtoClass.ModelItem> modelItems_ = new pbc::RepeatedField<global::VehicleData.Service.ProtoClass.ModelItem>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::VehicleData.Service.ProtoClass.ModelItem> ModelItems {
      get { return modelItems_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ModelItemsList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ModelItemsList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!modelItems_.Equals(other.modelItems_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= modelItems_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      modelItems_.WriteTo(output, _repeated_modelItems_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += modelItems_.CalculateSize(_repeated_modelItems_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ModelItemsList other) {
      if (other == null) {
        return;
      }
      modelItems_.Add(other.modelItems_);
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
            modelItems_.AddEntriesFrom(input, _repeated_modelItems_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
