# 父镜像
FROM microsoft/aspnetcore:latest

# 设置工作目录
WORKDIR /app

# 复制发布文件到/app下
COPY . /app

# 设置端口
EXPOSE 8810

# 使用dotnet Manulife.DNC.MSAD.NB.AgentService.dll来运行ASP.NET Core项目，注意大小写
ENTRYPOINT ["dotnet", "webhello.dll", "--server.urls", "http://*:8810"]