# ASP.NET Core Minimal API Serverless Application

This project shows how to run an ASP.NET Core Web API project as an AWS Lambda exposed through Amazon API Gateway. The NuGet package [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer) contains a Lambda function that is used to translate requests from API Gateway into the ASP.NET Core framework and then the responses from ASP.NET Core back to API Gateway.

For more information about how the Amazon.Lambda.AspNetCoreServer package works and how to extend its behavior view its [README](https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.AspNetCoreServer/README.md) file in GitHub.

## Executable Assembly

.NET Lambda projects that use C# top level statements like this project must be deployed as an executable assembly instead of a class library. To indicate to Lambda that the .NET function is an executable assembly the
Lambda function handler value is set to the .NET Assembly name. This is different then deploying as a class library where the function handler string includes the assembly, type and method name.

To deploy as an executable assembly the Lambda runtime client must be started to listen for incoming events to process. For an ASP.NET Core application the Lambda runtime client is started by included the
`Amazon.Lambda.AspNetCoreServer.Hosting` NuGet package and calling `AddAWSLambdaHosting(LambdaEventSource.HttpApi)` passing in the event source while configuring the services of the application. The
event source can be API Gateway REST API and HTTP API or Application Load Balancer.

### Project Files

- serverless.template - an AWS CloudFormation Serverless Application Model template file for declaring your Serverless functions and other AWS resources
- aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS
- Program.cs - entry point to the application that contains all of the top level statements initializing the ASP.NET Core application.
  The call to `AddAWSLambdaHosting` configures the application to work in Lambda when it detects Lambda is the executing environment.
- Controllers\CalculatorController - example Web API controller

You may also have a test project depending on the options selected.

## YouTube Video reference : https://www.youtube.com/watch?v=rImaNyfKhZk

## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select _Publish to AWS Lambda_.

To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.

## Nuget packages :

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>XitizNetCore.WebAPI</RootNamespace>
    <GenerateRuntimeConfigurationFiles>true</GenerateRuntimeConfigurationFiles>
    <Nullable>enable</Nullable>
    <AWSProjectType>Lambda</AWSProjectType>
    <!-- This property makes the build directory similar to a publish directory and helps the AWS .NET Lambda Mock Test Tool find project dependencies. -->
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
    <!-- Generate ready to run images during publishing to improve cold start time. -->
    <PublishReadyToRun>true</PublishReadyToRun>
  </PropertyGroup>
  <ItemGroup>
      <PackageReference Include="Amazon.Lambda.Core" Version="2.1.0" />
      <PackageReference Include="Amazon.Lambda.Serialization.SystemTextJson" Version="2.3.0" />
      <PackageReference Include="Amazon.Lambda.APIGatewayEvents" Version="2.4.0" />
      <PackageReference Include="Amazon.Lambda.AspNetCoreServer.Hosting" Version="1.1.0" />
  </ItemGroup>

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.

```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.

```
    dotnet tool update -g Amazon.Lambda.Tools
```

IAM Role :
Create IAM role: my-lambda-deployment-role
Add Permission to above role : AWSLambdaBasicExecutionRole
IAM User :
Create IAM user: vs_deploy_agent
Add Permission to above user :
AdministratorAccess
AWSLambdaBasicExecutionRole
AWSCodeDeployRoleForLambda
AWSLambdaExecute

Deploy application

```
    cd "API"
    dotnet lambda deploy-serverless || dotnet lambda deploy-function
    Enter Region Name : us-east-2
    Enter Function Name : XitizNetCore-API
```
