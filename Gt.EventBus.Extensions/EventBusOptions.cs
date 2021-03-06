﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gt.Extensions
{
  public  class EventBusOptions
    {

        ///// <summary>
        ///// 固定的程序集
        ///// </summary>
        //public Assembly Assembly { set; get; }


        /// <summary>
        /// 具体类型集合
        /// </summary>
        //public List<Type> Types { set; get; } = new List<Type>();

        /// <summary>
        /// 注入的生命周期
        /// </summary>
        public ServiceLifetime Lifetime { set; get; } = ServiceLifetime.Scoped;

    }
}
