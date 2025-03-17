# Redis分布式原子自增ID生成器

基于.NET Core和Redis实现的分布式原子自增ID生成器。

## 功能特点

- 基于Redis的INCR命令实现原子自增
- 支持多个不同键名的ID序列
- 提供REST API接口
- 支持获取下一个ID、查询当前ID和重置ID

## 环境要求

- .NET 8.0+
- Redis服务器

## 配置说明

在`appsettings.json`中配置Redis连接字符串：

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

## API接口

### 获取下一个ID

```
GET /api/idgenerator/next/{key}
```

- `key`: ID序列的键名

### 获取当前ID

```
GET /api/idgenerator/current/{key}
```

- `key`: ID序列的键名

### 重置ID

```
POST /api/idgenerator/reset/{key}?startValue={startValue}
```

- `key`: ID序列的键名
- `startValue`: 可选，重置后的起始值，默认为0

## 使用示例

### 获取下一个用户ID

```
GET /api/idgenerator/next/user_id
```

响应：

```json
{
  "key": "user_id",
  "id": 1
}
```

### 获取当前订单ID

```
GET /api/idgenerator/current/order_id
```

响应：

```json
{
  "key": "order_id",
  "id": 100
}
```

### 重置产品ID

```
POST /api/idgenerator/reset/product_id?startValue=1000
```

响应：

```json
{
  "key": "product_id",
  "startValue": 1000,
  "message": "ID已重置"
}
```

## 运行项目

```bash
cd RedisIdGenerator
dotnet run
```

默认情况下，API将在 https://localhost:5001 和 http://localhost:5000 上运行。 