FROM yjsjade/62hd-image AS runtime

WORKDIR /app

COPY publish/ .

EXPOSE 80

ENTRYPOINT ["dotnet", "54HD.dll"]
