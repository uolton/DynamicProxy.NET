/*****************************************
 * author:jinshuai
 * 
 * E-mail:redfox2008@126.com
 * 
 * Date:2016-04-28 
 *  
 * ***************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace DynamicProxy.Core
{
    /// <summary>
    /// 代理工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProxyFactory<T>
    {
        public static T Create(T obj, Dictionary<string, DynamicAction> proxyMethods = null)
        {
            var proxy = new DynamicProxy<T>(obj) { ProxyMethods = proxyMethods };

            return (T)proxy.GetTransparentProxy();
        }
    }


    /// <summary>
    /// 动态代理类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicProxy<T> : RealProxy
    {
        private readonly T _targetInstance = default(T);

        public Dictionary<string, DynamicAction> ProxyMethods { get; set; }

        public DynamicProxy(T targetInstance)
            : base(typeof(T))
        {
            _targetInstance = targetInstance;
        }
        public override IMessage Invoke(IMessage msg)
        {
            var reqMsg = msg as IMethodCallMessage;

            if (reqMsg == null)
            {
                return new ReturnMessage(new Exception("调用失败！"), null);
            }

            var target = _targetInstance as MarshalByRefObject;

            if (target == null)
            {
                return new ReturnMessage(new Exception("调用失败！请把目标对象 继承自 System.MarshalByRefObject"), reqMsg);
            }

            var methodName = reqMsg.MethodName;

            DynamicAction actions = null;

            if (ProxyMethods != null && ProxyMethods.ContainsKey(methodName))
            {
                actions = ProxyMethods[methodName];
            }

            if (actions != null && actions.BeforeAction != null)
            {
                actions.BeforeAction();
            }

            var result = RemotingServices.ExecuteMessage(target, reqMsg);

            if (actions != null && actions.AfterAction != null)
            {
                actions.AfterAction();
            }

            return result;
        }
    }


    /// <summary>
    /// 动态代理要执行的方法
    /// </summary>
    public class DynamicAction
    {
        /// <summary>
        /// 执行目标方法前执行
        /// </summary>
        public Action BeforeAction { get; set; }


        /// <summary>
        /// 执行目标方法后执行
        /// </summary>
        public Action AfterAction { get; set; }


    }




}
