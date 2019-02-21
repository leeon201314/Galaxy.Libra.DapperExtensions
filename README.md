# Galaxy.Libra.DapperExtensions
一个轻量的Dapper扩展库，基于netcore。

## 例子

### 定义一个实体类
``` 
public class User
{
     public int Id { get; set; }

     public string Name { get; set; }

     public string Psw { get; set; }
}
```
### 构建查询条件
* 简单查询，包括大于，等于，Like等
``` 
Predicates.Field<User>(p => p.Name, Operator.Like, "李%");
``` 
