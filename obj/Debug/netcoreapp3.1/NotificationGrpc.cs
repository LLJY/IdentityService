// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/notification.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace NotificationService.Protos {
  /// <summary>
  /// notification will use firebase admin to send a notification, this allows us to send notification from anywhere using userid as reference, without using the deprecated C2DM API
  /// </summary>
  public static partial class Notification
  {
    static readonly string __ServiceName = "services.Notification";

    static readonly grpc::Marshaller<global::NotificationService.Protos.UserIdNotificationRequest> __Marshaller_services_UserIdNotificationRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::NotificationService.Protos.UserIdNotificationRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::NotificationService.Protos.UserIdNotificationResponse> __Marshaller_services_UserIdNotificationResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::NotificationService.Protos.UserIdNotificationResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::NotificationService.Protos.TopicNotificationRequest> __Marshaller_services_TopicNotificationRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::NotificationService.Protos.TopicNotificationRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::NotificationService.Protos.TopicNotificationResponse> __Marshaller_services_TopicNotificationResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::NotificationService.Protos.TopicNotificationResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::NotificationService.Protos.TokenNotificationRequest> __Marshaller_services_TokenNotificationRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::NotificationService.Protos.TokenNotificationRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::NotificationService.Protos.TokenNotificationResponse> __Marshaller_services_TokenNotificationResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::NotificationService.Protos.TokenNotificationResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::NotificationService.Protos.UserIdNotificationRequest, global::NotificationService.Protos.UserIdNotificationResponse> __Method_SendNotificationByUserId = new grpc::Method<global::NotificationService.Protos.UserIdNotificationRequest, global::NotificationService.Protos.UserIdNotificationResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SendNotificationByUserId",
        __Marshaller_services_UserIdNotificationRequest,
        __Marshaller_services_UserIdNotificationResponse);

    static readonly grpc::Method<global::NotificationService.Protos.TopicNotificationRequest, global::NotificationService.Protos.TopicNotificationResponse> __Method_SendNotificationByTopic = new grpc::Method<global::NotificationService.Protos.TopicNotificationRequest, global::NotificationService.Protos.TopicNotificationResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SendNotificationByTopic",
        __Marshaller_services_TopicNotificationRequest,
        __Marshaller_services_TopicNotificationResponse);

    static readonly grpc::Method<global::NotificationService.Protos.TokenNotificationRequest, global::NotificationService.Protos.TokenNotificationResponse> __Method_SendNotificationByToken = new grpc::Method<global::NotificationService.Protos.TokenNotificationRequest, global::NotificationService.Protos.TokenNotificationResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SendNotificationByToken",
        __Marshaller_services_TokenNotificationRequest,
        __Marshaller_services_TokenNotificationResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::NotificationService.Protos.NotificationReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Notification</summary>
    [grpc::BindServiceMethod(typeof(Notification), "BindService")]
    public abstract partial class NotificationBase
    {
      /// <summary>
      /// sends notification by userid
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::NotificationService.Protos.UserIdNotificationResponse> SendNotificationByUserId(global::NotificationService.Protos.UserIdNotificationRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// sends notification by topic
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::NotificationService.Protos.TopicNotificationResponse> SendNotificationByTopic(global::NotificationService.Protos.TopicNotificationRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// sends notification by notification token
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::NotificationService.Protos.TokenNotificationResponse> SendNotificationByToken(global::NotificationService.Protos.TokenNotificationRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(NotificationBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SendNotificationByUserId, serviceImpl.SendNotificationByUserId)
          .AddMethod(__Method_SendNotificationByTopic, serviceImpl.SendNotificationByTopic)
          .AddMethod(__Method_SendNotificationByToken, serviceImpl.SendNotificationByToken).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, NotificationBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SendNotificationByUserId, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::NotificationService.Protos.UserIdNotificationRequest, global::NotificationService.Protos.UserIdNotificationResponse>(serviceImpl.SendNotificationByUserId));
      serviceBinder.AddMethod(__Method_SendNotificationByTopic, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::NotificationService.Protos.TopicNotificationRequest, global::NotificationService.Protos.TopicNotificationResponse>(serviceImpl.SendNotificationByTopic));
      serviceBinder.AddMethod(__Method_SendNotificationByToken, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::NotificationService.Protos.TokenNotificationRequest, global::NotificationService.Protos.TokenNotificationResponse>(serviceImpl.SendNotificationByToken));
    }

  }
}
#endregion
