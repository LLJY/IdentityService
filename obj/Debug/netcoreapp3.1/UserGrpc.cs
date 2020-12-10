// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/user.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace UserService.Protos {
  /// <summary>
  /// User service definition
  /// </summary>
  public static partial class User
  {
    static readonly string __ServiceName = "services.User";

    static readonly grpc::Marshaller<global::UserService.Protos.GetUserInfoRequest> __Marshaller_services_GetUserInfoRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::UserService.Protos.GetUserInfoRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::UserService.Protos.GetUserInfoResponse> __Marshaller_services_GetUserInfoResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::UserService.Protos.GetUserInfoResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::UserService.Protos.UpdateProfileRequest> __Marshaller_services_UpdateProfileRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::UserService.Protos.UpdateProfileRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::UserService.Protos.UpdateProfileResponse> __Marshaller_services_UpdateProfileResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::UserService.Protos.UpdateProfileResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::UserService.Protos.GetUserInfoRequest, global::UserService.Protos.GetUserInfoResponse> __Method_GetUserInfo = new grpc::Method<global::UserService.Protos.GetUserInfoRequest, global::UserService.Protos.GetUserInfoResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetUserInfo",
        __Marshaller_services_GetUserInfoRequest,
        __Marshaller_services_GetUserInfoResponse);

    static readonly grpc::Method<global::UserService.Protos.UpdateProfileRequest, global::UserService.Protos.UpdateProfileResponse> __Method_UpdateProfile = new grpc::Method<global::UserService.Protos.UpdateProfileRequest, global::UserService.Protos.UpdateProfileResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UpdateProfile",
        __Marshaller_services_UpdateProfileRequest,
        __Marshaller_services_UpdateProfileResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::UserService.Protos.UserReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of User</summary>
    [grpc::BindServiceMethod(typeof(User), "BindService")]
    public abstract partial class UserBase
    {
      /// <summary>
      /// get individual user info
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::UserService.Protos.GetUserInfoResponse> GetUserInfo(global::UserService.Protos.GetUserInfoRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// executed right after sign up/OTP, allows the user to setup more details like BIO and profile picture, also users can make use of this when updating profiles
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::UserService.Protos.UpdateProfileResponse> UpdateProfile(global::UserService.Protos.UpdateProfileRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(UserBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetUserInfo, serviceImpl.GetUserInfo)
          .AddMethod(__Method_UpdateProfile, serviceImpl.UpdateProfile).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, UserBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetUserInfo, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::UserService.Protos.GetUserInfoRequest, global::UserService.Protos.GetUserInfoResponse>(serviceImpl.GetUserInfo));
      serviceBinder.AddMethod(__Method_UpdateProfile, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::UserService.Protos.UpdateProfileRequest, global::UserService.Protos.UpdateProfileResponse>(serviceImpl.UpdateProfile));
    }

  }
}
#endregion
