# 项目介绍

考虑到许多公司采用项目制工作方式，因此在不同项目上可能存在多个团队开发独立的代码库，但通用的基础设施却是相同的，这可能导致每个项目都需要编写相同的代码，并重复造轮子。更严重的是，每个项目都有自己独特的用户体系，导致用户在使用不同的服务时需要重新登录，这不仅会破坏用户的体验，也不利于项目的维护和数据的积累。因此 ONEAdmin 正是解决这些问题的一次尝试，它是一个基于 Abp Vnext 框架开发的，包含用户管理、角色管理、身份认证、字典管理、应用菜单管理、数据权限管理等模块。

有任何问题欢迎联系我  **QQ群: 655362692** &#x20;

## DEMO预览

**地址：<http://120.76.117.67>**

*账号\:root*

*密码:123456*

## 在线文档

**地址：**<https://sososu.github.io/ONE.Abp.Doc>



# 快速开始

首先,如果你没有安装[ONEABP CLI](https://docs.abp.io/zh-Hans/abp/latest/CLI),请先安装它:

    dotnet tool install -g ONE.Abp.Cli

在一个空文件夹使用 `abp new` 命令创建新解决方案:

base模板

    oneabp new Acme.BookStore -t base -d ef -dbms postgresql

micro模板

    oneabp new Acme.BookStore -t micro -d ef -dbms postgresql

ONEABP.CLI在ABP.CLI基础上增加了两个模板

1.base模板: 解决方案包含网关，认证服务和基础服务项目

2.micro模板:解决方案包含微服务项目

# 参考文档

## 前端

*   前端使用诺依框架开发

*   文档地址： <https://github.com/yangzongzhuan/RuoYi-Cloud-Vue3>

## 后端

*   后端使用 Abp vnext 框架开发

*   ABP 官方文档地址： <https://docs.abp.io/en/abp/latest/>

**感谢**

*   **Abp Vnext**

<https://www.abp.io/>

*   **RuoYi-Cloud-Vue3**

<http://www.ruoyi.vip/>
