# trbank

一个为owoser提供的跨服银行

## 依赖环境

- .Net 7.0 *(运行环境)*
- LitJson.dll
- MySqlConnector
- LiteLoader.NET.dll
- LLMoney.NET.dll
- Ijwhost.dll

## 插件安装

1. 把编译好的 DLL 文件放入服务端`plugins/`目录内，接着放置好上面所列的依赖 DLL 文件到`plugins/lib/`目录内；
2. 把项目文件里的`config.json`复制到`plugins/trbank/`内；*(手动创建trbank目录)*
3. 安装 MySQL 服务端，手动把仓库文件里的 sql 文件导入到 MySQL 后，回到配置文件`config.json`，配置好 db 连接信息即可。
