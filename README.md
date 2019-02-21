# Galaxy.Libra.DapperExtensions
一个轻量的Dapper扩展库，基于netcore,支持MySQL,SQLServer等多种常用数据库。

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
### 查询

#### 构建查询条件（基于MySQL）
* 简单查询，包括大于，等于，Like等，生成：(`User`.`Name` LIKE @Name_0)
``` 
Predicates.Field<User>(p => p.Name, Operator.Like, "李%");
``` 
* in查询，生成：(`User`.`Name` IN (@Name_0, @Name_1, @Name_2))
```
List<string> valueList = new List<string>() { "1", "2", "3" };
Predicates.Field<User>(p => p.Name, Operator.Eq, valueList);
```
* between查询，生成：(`User`.`Name` NOT BETWEEN @Name_0 AND @Name_1)
```
Predicates.Between<User>(p => p.Name, new BetweenValues { Value1 = 1, Value2 = 10 }, true);
```
* exist查询，生成：(NOT EXISTS (SELECT 1 FROM `User` WHERE (`User`.`Name` LIKE @Name_0)))
```
IFieldPredicate nameFieldPredicate = Predicates.Field<User>(p => p.Name, Operator.Like, "李%");
Predicates.Exists<User>(nameFieldPredicate, true);
```
* 组合查询，生成：(((`User`.`Name` LIKE @Name_0) AND (`User`.`Name` NOT BETWEEN @Name_1 AND @Name_2)) OR (`User`.`Name` LIKE @Name_3))
```
IList<IPredicate> predList = new List<IPredicate>();
predList.Add(Predicates.Field<User>(p => p.Name, Operator.Like, "李%"));
predList.Add(Predicates.Field<User>(p => p.Name, Operator.Eq, valueList));
IPredicateGroup predGroup1 = Predicates.Group(GroupOperator.And, predList.ToArray());

IList<IPredicate> predList2 = new List<IPredicate>();
predList2.Add(predGroup1);
predList2.Add(Predicates.Field<User>(p => p.Name, Operator.Like, "张%"));
IPredicateGroup predGroup2 = Predicates.Group(GroupOperator.Or, predList2.ToArray());
```
