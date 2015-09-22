﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NuclearWatchdog.Heartbeat {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://SWEN.755.Heartbeat", ConfigurationName="Heartbeat.IHeartbeat")]
    public interface IHeartbeat {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://SWEN.755.Heartbeat/IHeartbeat/Register", ReplyAction="http://SWEN.755.Heartbeat/IHeartbeat/RegisterResponse")]
        void Register();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://SWEN.755.Heartbeat/IHeartbeat/Register", ReplyAction="http://SWEN.755.Heartbeat/IHeartbeat/RegisterResponse")]
        System.Threading.Tasks.Task RegisterAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://SWEN.755.Heartbeat/IHeartbeat/Unregister", ReplyAction="http://SWEN.755.Heartbeat/IHeartbeat/UnregisterResponse")]
        void Unregister();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://SWEN.755.Heartbeat/IHeartbeat/Unregister", ReplyAction="http://SWEN.755.Heartbeat/IHeartbeat/UnregisterResponse")]
        System.Threading.Tasks.Task UnregisterAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://SWEN.755.Heartbeat/IHeartbeat/Beat", ReplyAction="http://SWEN.755.Heartbeat/IHeartbeat/BeatResponse")]
        void Beat();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://SWEN.755.Heartbeat/IHeartbeat/Beat", ReplyAction="http://SWEN.755.Heartbeat/IHeartbeat/BeatResponse")]
        System.Threading.Tasks.Task BeatAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IHeartbeatChannel : NuclearWatchdog.Heartbeat.IHeartbeat, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class HeartbeatClient : System.ServiceModel.ClientBase<NuclearWatchdog.Heartbeat.IHeartbeat>, NuclearWatchdog.Heartbeat.IHeartbeat {
        
        public HeartbeatClient() {
        }
        
        public HeartbeatClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public HeartbeatClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HeartbeatClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public HeartbeatClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Register() {
            base.Channel.Register();
        }
        
        public System.Threading.Tasks.Task RegisterAsync() {
            return base.Channel.RegisterAsync();
        }
        
        public void Unregister() {
            base.Channel.Unregister();
        }
        
        public System.Threading.Tasks.Task UnregisterAsync() {
            return base.Channel.UnregisterAsync();
        }
        
        public void Beat() {
            base.Channel.Beat();
        }
        
        public System.Threading.Tasks.Task BeatAsync() {
            return base.Channel.BeatAsync();
        }
    }
}