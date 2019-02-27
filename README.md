# Galaxy.Libra.DapperExtensions
一个轻量的Dapper扩展库，基于netcore,支持MySQL,SQLServer等多种常用数据库。
基于[Dapper-Extensions](https://github.com/tmsmith/Dapper-Extensions)优化。

## 例子

### 定义一个实体类
``` c#
public class User
{
     public int Id { get; set; }

     public string Name { get; set; }

     public string Psw { get; set; }
}
```
### 基本使用

#### 插入
```c#
using (SqlConnection cn = new SqlConnection(_connectionString))
{
    cn.Open();
    User user = new User { Name = "波多野结衣" , Psw = "123"};
    int id = cn.Insert(user);
    cn.Close();
}
```
#### 更新
```
using (SqlConnection cn = new SqlConnection(_connectionString))
{
    cn.Open();
    int id = 1;
    User user = cn.Get<User>(id);
    user.Name = "林志玲";
    cn.Update(user);
    cn.Close();
}
```
#### 删除
```c#
using (SqlConnection cn = new SqlConnection(_connectionString))
{
    cn.Open();
    int id = 1;
    User user = cn.Get<User>(id);
    cn.Delete(user);
    cn.Close();
}
```
#### 基于ID查询
```c#
using (SqlConnection cn = new SqlConnection(_connectionString))
{
    cn.Open();
    int Id = 1;
    Person person = cn.Get<User>(Id);	
    cn.Close();
}
```
#### 基于动态对象查询
```c#
using (SqlConnection cn = new SqlConnection(_connectionString))
{
    cn.Open();
    Person person = cn.GetList<User>(new { Name = "波多野结衣" , Psw = "123"});	
    cn.Close();
}
```
#### 基于表达式查询
```c#
using (SqlConnection cn = new SqlConnection(_connectionString))
{
    cn.Open();
    Person person = cn.Get<User>(u => u.Id != 1);
    person = cn.GetList<User>(u => u.Name.Contains("1"));	//Like查询
    person = cn.GetList<User>(u => u.Name.StartsWith("1")); //Like查询
    person = cn.GetList<User>(u => u.Name.EndsWith("1"));	//Like查询
    
    List<string> valueList = new List<string>() { "1", "2", "3" };
    person = cn.GetList<User>(u => valueList.Contains(u.Name));	//in查询
    person = cn.GetList<User>(u => !valueList.Contains(u.Name)); //not in查询
    
    person = cn.GetList<User>(u => u.Id == 1 || (u.Id == 2 && u.Name.Contains("1"))); //复合查询
    cn.Close();
}
```
#### 基于Predicate查询
```c#
using (SqlConnection cn = new SqlConnection(_connectionString))
{
    cn.Open();
    var predicate = Predicates.Field<User>(p => p.Name, Operator.Like, "李%");
    IEnumerable<User> list = cn.GetList<User>(predicate);	
    cn.Close();
}
```   
##### 构建查询条件（基于MySQL）
* 简单查询，包括大于，等于，Like等，生成：(`User`.`Name` LIKE @Name_0)
```c#
Predicates.Field<User>(p => p.Name, Operator.Like, "李%");
```
* in查询，生成：(`User`.`Name` IN (@Name_0, @Name_1, @Name_2))
```c#
List<string> valueList = new List<string>() { "1", "2", "3" };
Predicates.Field<User>(p => p.Name, Operator.Eq, valueList);
```
* between查询，生成：(`User`.`Name` NOT BETWEEN @Name_0 AND @Name_1)
```c#
Predicates.Between<User>(p => p.Name, new BetweenValues { Value1 = 1, Value2 = 10 }, true);
```
* exist查询，生成：(NOT EXISTS (SELECT 1 FROM `User` WHERE (`User`.`Name` LIKE @Name_0)))
```c#
IFieldPredicate nameFieldPredicate = Predicates.Field<User>(p => p.Name, Operator.Like, "李%");
Predicates.Exists<User>(nameFieldPredicate, true);
```
* 组合查询，生成：(((`User`.`Name` LIKE @Name_0) AND (`User`.`Name` NOT BETWEEN @Name_1 AND @Name_2)) OR (`User`.`Name` LIKE @Name_3))
```c#
IList<IPredicate> predList = new List<IPredicate>();
predList.Add(Predicates.Field<User>(p => p.Name, Operator.Like, "李%"));
predList.Add(Predicates.Field<User>(p => p.Name, Operator.Eq, valueList));
IPredicateGroup predGroup1 = Predicates.Group(GroupOperator.And, predList.ToArray());

IList<IPredicate> predList2 = new List<IPredicate>();
predList2.Add(predGroup1);
predList2.Add(Predicates.Field<User>(p => p.Name, Operator.Like, "张%"));
IPredicateGroup predGroup2 = Predicates.Group(GroupOperator.Or, predList2.ToArray());
```
