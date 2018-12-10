// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: PredictiveScore.proto
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace Predictive {
  public static partial class PredictiveScore
  {
    static readonly string __ServiceName = "Predictive.PredictiveScore";

    static readonly Marshaller<global::Predictive.CampaignRequest> __Marshaller_CampaignRequest = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Predictive.CampaignRequest.Parser.ParseFrom);
    static readonly Marshaller<global::Predictive.ModelResponse> __Marshaller_ModelResponse = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Predictive.ModelResponse.Parser.ParseFrom);
    static readonly Marshaller<global::Predictive.PremiumMakeRequest> __Marshaller_PremiumMakeRequest = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Predictive.PremiumMakeRequest.Parser.ParseFrom);
    static readonly Marshaller<global::Predictive.PremiumMakeResponse> __Marshaller_PremiumMakeResponse = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Predictive.PremiumMakeResponse.Parser.ParseFrom);
    static readonly Marshaller<global::Predictive.HeartbeatFlag> __Marshaller_HeartbeatFlag = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Predictive.HeartbeatFlag.Parser.ParseFrom);
    static readonly Marshaller<global::Predictive.RefreshFlag> __Marshaller_RefreshFlag = Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Predictive.RefreshFlag.Parser.ParseFrom);

    static readonly Method<global::Predictive.CampaignRequest, global::Predictive.ModelResponse> __Method_GetCampaignScore = new Method<global::Predictive.CampaignRequest, global::Predictive.ModelResponse>(
        MethodType.Unary,
        __ServiceName,
        "GetCampaignScore",
        __Marshaller_CampaignRequest,
        __Marshaller_ModelResponse);

    static readonly Method<global::Predictive.PremiumMakeRequest, global::Predictive.PremiumMakeResponse> __Method_GetPremiumMakeScore = new Method<global::Predictive.PremiumMakeRequest, global::Predictive.PremiumMakeResponse>(
        MethodType.Unary,
        __ServiceName,
        "GetPremiumMakeScore",
        __Marshaller_PremiumMakeRequest,
        __Marshaller_PremiumMakeResponse);

    static readonly Method<global::Predictive.HeartbeatFlag, global::Predictive.HeartbeatFlag> __Method_CheckHeartbeat = new Method<global::Predictive.HeartbeatFlag, global::Predictive.HeartbeatFlag>(
        MethodType.Unary,
        __ServiceName,
        "CheckHeartbeat",
        __Marshaller_HeartbeatFlag,
        __Marshaller_HeartbeatFlag);

    static readonly Method<global::Predictive.RefreshFlag, global::Predictive.RefreshFlag> __Method_RefreshModel = new Method<global::Predictive.RefreshFlag, global::Predictive.RefreshFlag>(
        MethodType.Unary,
        __ServiceName,
        "RefreshModel",
        __Marshaller_RefreshFlag,
        __Marshaller_RefreshFlag);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Predictive.PredictiveScoreReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of PredictiveScore</summary>
    public abstract partial class PredictiveScoreBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Predictive.ModelResponse> GetCampaignScore(global::Predictive.CampaignRequest request, ServerCallContext context)
      {
        throw new RpcException(new Status(StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Predictive.PremiumMakeResponse> GetPremiumMakeScore(global::Predictive.PremiumMakeRequest request, ServerCallContext context)
      {
        throw new RpcException(new Status(StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Predictive.HeartbeatFlag> CheckHeartbeat(global::Predictive.HeartbeatFlag request, ServerCallContext context)
      {
        throw new RpcException(new Status(StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Predictive.RefreshFlag> RefreshModel(global::Predictive.RefreshFlag request, ServerCallContext context)
      {
        throw new RpcException(new Status(StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for PredictiveScore</summary>
    public partial class PredictiveScoreClient : ClientBase<PredictiveScoreClient>
    {
      /// <summary>Creates a new client for PredictiveScore</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public PredictiveScoreClient(Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for PredictiveScore that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public PredictiveScoreClient(CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected PredictiveScoreClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected PredictiveScoreClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Predictive.ModelResponse GetCampaignScore(global::Predictive.CampaignRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetCampaignScore(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Predictive.ModelResponse GetCampaignScore(global::Predictive.CampaignRequest request, CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetCampaignScore, null, options, request);
      }
      public virtual AsyncUnaryCall<global::Predictive.ModelResponse> GetCampaignScoreAsync(global::Predictive.CampaignRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetCampaignScoreAsync(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual AsyncUnaryCall<global::Predictive.ModelResponse> GetCampaignScoreAsync(global::Predictive.CampaignRequest request, CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetCampaignScore, null, options, request);
      }
      public virtual global::Predictive.PremiumMakeResponse GetPremiumMakeScore(global::Predictive.PremiumMakeRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetPremiumMakeScore(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Predictive.PremiumMakeResponse GetPremiumMakeScore(global::Predictive.PremiumMakeRequest request, CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetPremiumMakeScore, null, options, request);
      }
      public virtual AsyncUnaryCall<global::Predictive.PremiumMakeResponse> GetPremiumMakeScoreAsync(global::Predictive.PremiumMakeRequest request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetPremiumMakeScoreAsync(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual AsyncUnaryCall<global::Predictive.PremiumMakeResponse> GetPremiumMakeScoreAsync(global::Predictive.PremiumMakeRequest request, CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetPremiumMakeScore, null, options, request);
      }
      public virtual global::Predictive.HeartbeatFlag CheckHeartbeat(global::Predictive.HeartbeatFlag request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CheckHeartbeat(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Predictive.HeartbeatFlag CheckHeartbeat(global::Predictive.HeartbeatFlag request, CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CheckHeartbeat, null, options, request);
      }
      public virtual AsyncUnaryCall<global::Predictive.HeartbeatFlag> CheckHeartbeatAsync(global::Predictive.HeartbeatFlag request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CheckHeartbeatAsync(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual AsyncUnaryCall<global::Predictive.HeartbeatFlag> CheckHeartbeatAsync(global::Predictive.HeartbeatFlag request, CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CheckHeartbeat, null, options, request);
      }
      public virtual global::Predictive.RefreshFlag RefreshModel(global::Predictive.RefreshFlag request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return RefreshModel(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Predictive.RefreshFlag RefreshModel(global::Predictive.RefreshFlag request, CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_RefreshModel, null, options, request);
      }
      public virtual AsyncUnaryCall<global::Predictive.RefreshFlag> RefreshModelAsync(global::Predictive.RefreshFlag request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return RefreshModelAsync(request, new CallOptions(headers, deadline, cancellationToken));
      }
      public virtual AsyncUnaryCall<global::Predictive.RefreshFlag> RefreshModelAsync(global::Predictive.RefreshFlag request, CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_RefreshModel, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override PredictiveScoreClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new PredictiveScoreClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static ServerServiceDefinition BindService(PredictiveScoreBase serviceImpl)
    {
      return ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetCampaignScore, serviceImpl.GetCampaignScore)
          .AddMethod(__Method_GetPremiumMakeScore, serviceImpl.GetPremiumMakeScore)
          .AddMethod(__Method_CheckHeartbeat, serviceImpl.CheckHeartbeat)
          .AddMethod(__Method_RefreshModel, serviceImpl.RefreshModel).Build();
    }

  }
}
#endregion
