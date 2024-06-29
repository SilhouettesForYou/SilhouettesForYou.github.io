### 状态机和行为树的区别

对游戏 NPC 的行为控制一般有 2 种，一种是状态机，一种是行为树。

状态机：
Unity 对人物动画的控制是基于状态机的，如下图：

可以看到，每个状态除了包含自身的状态行为外，还需要和其他的状态打交道，需要输入状态机之间切换的条件。因为这一点，导致增加状态会使得状态机越来越复杂。

状态机是一种网状结构，耦合性很大。

行为树：

1、行为树是将游戏 NPC 的行为（走动，攻击，跳跃）搭建为一颗树，父节点是行为分支，叶节点是行为的具体表现。

2、行为树将 NPC 的行为包装为一个对象，符合面向对象的设计理念。

3、行为树的好处是将行为逻辑和状态数据剥离，降低耦合，方便策划配置，而无需对每个人物的行为进行代码控制，提高了可视性，简化问题排查，提高效率。

4、行为树把行为和状态数据剥离，可以这么理解，比如 NPC 要跳跃，那么前提条件可以是：已完成上一个动作，HP 大于 0，下一个行为状态设置为跳跃。把这些条件看作状态数据，把跳跃的 Animation 看做行为，那么当要切换行为的时候，就不用去管上一个状态是什么行为、和上一个状态是什么关系。构建出来的行为树，只需要判断当前的状态数据是否满足跳跃的条件，满足就执行行为，不满足就返回 false。

5、行为树是树状结构，相对于状态机的网状结构来说耦合性更低。

6、行为树的子节点执行时需要返回，返回类型有：False，True，Running。

7、行为树的父节点存在多个情况，1 个节点和多个节点，当父节点有多个字节点时，有几种组合顺序：

①并行执行：并行执行多个子节点的行为。

②随机执行：从多个字节点中随机选择一个执行。

③选择执行：从多个字节点中按照顺序选择一个能执行的行为。

④顺序执行：按照顺序依次执行多个子节点。

### Lua 表是什么结构

Table 是 Lua 中唯一的一个数据结构，（自定义数据类型）通过 table, 我们能扩展出其他的数据结构，比如：数组，类。

构造器是创建和初始化表的表达式。表是 Lua 特有的功能强大的东西。最简单的构造函数是{}，用来创建一个空表。可以直接初始化数组：

```lua
-- 初始化表
mytable = {}

-- 指定值
mytable[1]= "Lua"

-- 移除引用
mytable = nil
-- lua 垃圾回收会释放内存

```

当我们为 table a 并设置元素，然后将 a 赋值给 b，则 a 与 b 都指向同一个内存。如果 a 设置为 nil ，则 b 同样能访问 table 的元素。如果没有指定的变量指向 a，Lua 的垃圾回收机制会清理相对应的内存。

以下实例演示了以上的描述情况：

```lua
-- 简单的 table
mytable = {}
print("mytable 的类型是 ",type(mytable))

mytable[1]= "Lua"
mytable["wow"] = "修改前"
print("mytable 索引为 1 的元素是 ", mytable[1])
print("mytable 索引为 wow 的元素是 ", mytable["wow"])

-- alternatetable 和 mytable 的是指同一个 table
alternatetable = mytable

print("alternatetable 索引为 1 的元素是 ", alternatetable[1])
print("mytable 索引为 wow 的元素是 ", alternatetable["wow"])

alternatetable["wow"] = "修改后"

print("mytable 索引为 wow 的元素是 ", mytable["wow"])

-- 释放变量
alternatetable = nil
print("alternatetable 是 ", alternatetable)

-- mytable 仍然可以访问
print("mytable 索引为 wow 的元素是 ", mytable["wow"])

mytable = nil
print("mytable 是 ", mytable)
```

### Lua 元表是什么，有什么作用

lua 中的每个值都可以拥有一个元表，只是 table 和 userdata 可以有各自独立的元表，而其他类型的值则共享其类型所属的单一元表。lua 侧代码中只能设置 table 的元表，至于其他类型值的元表只能通过 C 代码设置。多个 table 可以共享一个通用的元表，但是每个 table 只能拥有一个元表

元表通过一个 string key 关联元方法 (Metamethods)，key 用加有`__`前缀的字符串来表示。
元表决定了一个对象在数学运算、位运算、比较、连接、 取长度、调用、索引时的行为

### Lua 原方法有哪些，分别什么意思

|元方法|作用|
|:---|:---|
|`__add`|`+`操作。 如果任何不是数字的值（包括不能转换为数字的字符串）做加法， Lua 就会尝试调用元方法。 首先、Lua 检查第一个操作数（即使它是合法的）， 如果这个操作数没有为`__add`事件定义元方法， Lua 就会接着检查第二个操作数。 一旦 Lua 找到了元方法， 它将把两个操作数作为参数传入元方法， 元方法的结果（调整为单个值）作为这个操作的结果。 如果找不到元方法，将抛出一个错误。|
|`__sub`|运算符`-`操作|
|`__mul`|运算符`*`|
|`__div`|运算符`/`|
|`__mod`|运算符`%`|
|`__unm`|运算符`^`（取反）|
|`__concat`|运算符`..`|
|`__eq`|运算符`==`|
|`__lt`|运算符`<`|
|`__le`|运算符`<=`|
|`__tostring`|转化为字符串|
|`__call`|函数调用操作`func(args)`。 当 Lua 尝试调用一个非函数的值的时候会触发这个事件 （即`func`不是一个函数）。 查找`func`的元方法， 如果找得到，就调用这个元方法，`func`作为第一个参数传入，原来调用的参数`args`后依次排在后面|
|`__index`|调索引`table[key]`。 当`table`不是表或是表`table`中不存在`key`这个键时，这个事件被触发。此时，会读出`table`相应的元方法。管名字取成这样， 这个事件的元方法其实可以是一个函数也可以是一张表。 如果它是一个函数，则以`table`和`key`作为参数调用它。 如果它是一张表，最终的结果就是以`key`取索引这张表的结果。 （这个索引过程是走常规的流程，而不是直接索引， 所以这次索引有可能引发另一次元方法。）|
|`__newindex`|索引赋值`table[key] = value`。 和索引事件类似，它发生在`table`不是表或是表`table`中不存在`key`这个键的时候。 此时，会读出`table`相应的元方法。同索引过程那样， 这个事件的元方法即可以是函数，也可以是一张表。 如果是一个函数， 则以`table`、`key`、以及`value`为参数传入。 如果是一张表， Lua 对这张表做索引赋值操作。 （这个索引过程是走常规的流程，而不是直接索引赋值， 所以这次索引赋值有可能引发另一次元方法。|

### Lua 的异常处理方法是什么，怎么用

Lua 应用在一般情况下很少使用到异常错误处理，但有时为了防止模块调用异常、函数调用异常、文件读写异常等一些非关键路径（有重试/容错手段）直接抛出异常，中断执行，会封装这些函数的调用，进行异常捕获。

Lua 的异常捕获主要基于 pcall 及 xpcall 函数

#### `pcall`函数

```lua
Summary
Calls a function in protected mode
Prototype
ok, result [ , result2 ...] = pcall (f, arg1, arg2, ...)

Description
Calls function f with the supplied arguments in protected mode. Catches errors and returns:

On success:
true
function result(s) - may be more than one

On failure:
false
error message
```

```lua
--- 求和
function sum(a,b,c)
    d = a + b + c
    return d
end

local e = sum(10, 20, 30)
print ("e:", e)
local h = sum("ten", "forty", "nine")
print ("h:", h)

--output:
e:      60
lua: src/pcall_test.lua:12: attempt to perform arithmetic on local 'a' (a string value)
stack traceback:
        src/pcall_test.lua:12: in function 'sum'
        src/pcall_test.lua:26: in main chunk
        [C]: in ?
```

如上述代码所示，当 sum 函数碰到无法处理的字符串输出时，抛出了一个异常，中止了程序运行。

如果我们期望捕获这种异常，做处理，并继续运行程序，可以如下这样调用：

```lua
local f, vrf = pcall(sum, "ten", "twenty", "thirty")
if f then
    print(vrf)
else
    print("failed to call sum function:" .. vrf)
end

--output:
failed to call sum function:src/pcall_test.lua:12: attempt to perform arithmetic on local 'a' (a string value)

```

#### `xpcall`函数

```lua
Summary
Calls a function with a custom error handler

Prototype
ok, result = xpcall (f, err)

If an error occurs in f it is caught and the error-handler 'err' is called. Then xpcall returns false, and whatever the error handler returned.

If there is no error in f, then xpcall returns true, followed by the function results from f.

```

```lua
local function err_handle(x)
    print("err_handle info:" .. x)
end

local f, res = xpcall(function ()
    return sum(10, 20, "a")
end , err_handle)
print(f, res)

--output:
err_handle info:src/pcall_test.lua:12: attempt to perform arithmetic on local 'c' (a string value)
false   nil
```

上面的 err_handle 就是定义的一个错误处理函数，当然也可以直接改成 debug 自带的相关函数，如下`debug.traceback`：

```lua
local f, res = xpcall(function ()
    return sum(10, 20, "a")
end , debug.traceback)
print(f, res)

-- output:
false   src/pcall_test.lua:12: attempt to perform arithmetic on local 'c' (a string value)
stack traceback:
        src/pcall_test.lua:12: in function <src/pcall_test.lua:11>
        (...tail calls...)
        [C]: in function 'xpcall'
        src/pcall_test.lua:41: in main chunk
        [C]: in ?
```

### Lua 与 C#如何交互

#### C#与 Lua 的交互过程

##### C#调用 Lua

由 C#文件调用 Lua 解析器底层 dll 库（由 C 语言编写），再由 dll 文件执行相应的 Lua 文件。

##### Lua 调用 C#

* Wrap 方式：首先生成 C#源文件对应的 Wrap 文件， 由 Lua 文件调用 Wrap 文件，再由 Wrap 文件调用 C#文件。

* 反射方式：当索引系统 API、dll 或者第三方库时，如果无法将代码的具体实现进行代码生成，可采用此方式实现交互。缺点是执行效率低

#### C#与 Lua 的交互原理

原理：Lua 用一个抽象的栈在 Lua 和 C#之间交换值。栈中的每一条记录都可以保存任何 Lua 值无论你何时想从 Lua 请求一个值（比如一个全局变量的值），调用 Lua，被请求的值将会被压入栈。无论你何时想要传递一个值给 Lua，首先将这个值压入栈，然后调用 Lua，这个值将会被弹出

##### C#调用 Lua

由 C#先将数据放入栈顶，由 Lua 从栈顶取出该数据，并且再 Lua 中做出相应的处理，然后返回对应的值到栈顶，最后再从 C#从栈顶取出 Lua 处理完的数据，完成交互

##### Lua 调用 C#

先生成 C#源文件所对应的 Wrap 文件（使用反射）或者编写 C#源文件所对应的 C 模块，然后将源文件内容通过 Wrap 文件或者 C 模块注册到 Lua 解释器中，然后由 Lua 去调用这个模块的函数

### Lua table 和 C#的字典的差异在哪

### 引用类型在堆还是在栈

* 值类型：值类型只需要一段单独的内存，用于存储实际的数据
* 引用类型：引用类型需要两段内存，第一段存储实际的数据，它总是位于堆中。第二段是一个引用，指向数据在堆中的存放位置

![](https://img-blog.csdnimg.cn/20201012152329285.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2FuYW5sZWxlXw==,size_16,color_FFFFFF,t_70)

### 什么是装箱和拆箱

值类型与引用类型的互相转换

### 什么会引起 GC, 如何降低 GC

当用`new`创建一个对象时，当可分配的内存不足 GC 就会去回收未使用的对象，但是 GC 的操作是非常复杂的，会占用很多 CPU 时间，对于移动设备来说频繁的垃圾回收会严重影响性能

下面的建议可以避免 GC 频繁操作

* 减少`new`产生对象的次数
* 使用公用的对象，即静态成员`Static`，但要谨慎使用
* 将`String`换成`StringBuilder`拼接字符串，`String`容易导致内存泄露
* 使用对象池 GamObject Pool
* 在字符串暂存池中的是不会被 GC 的
* 避免使用`foreach`，尽量使用`for`循环

### 实例化为什么会卡

* 当在执行多次 Instantiate 实例化物体时，会卡顿严重甚至在移动端会导致程序崩溃，因为 Instantiate 会产生大量的 GC，使 CPU 过高，导致崩溃
* 卡顿或程序崩溃的原因就是在某一帧中产生了大量的 GC，所以可以把一帧的操作分帧进行

### List 和 LinkList 的区别

#### `Array`

##### 特点

* 数组是一种线性结构，需要声明长度
* 通过下标查找时间复杂度为$O(1)$
* 插入删除比较复杂

##### 常用属性

* `Length`：获取出数组所有维度的长度
* `Rank`：获取数组的维度

##### 方法

* `Clear(Array, Int32, Int32)`：在指定数组的某一范围将数组恢复为默认值（比如整数数组归零）
* `Sort(Array)`：对于数组中的元素进行排序
* `Clone()`：创建 Array 副本
* `GetType()`：获取当前数组实例的 Type
* `Initialize()`：通过调用值类型的无参数构造函数，初始化值类型 Array 的每一个元素
* `Reverse(Array)`：反转数组中元素的顺序

##### 使用方式

```c#
int[] nums = new int[] { 2, 7, 11, 15 };
//leangth 来获取数组的长度
Console.WriteLine(nums.Length);

//对数组中元素进行排序
Array.Sort(nums);

//反转数组中元素的顺序
Array.Reverse(nums);

//复制 nums 数组
int[] nums_Copy = (int[])nums.Clone();

//清除数组元素，将 nums 中所有元素变为 0
Array.Clear(nums, 0, nums.Length);

//同样将数组中元素恢复默认
nums.Initialize();
```

#### `ArrayList`

`ArrayList`核心是数组，但是是在数组的基础上进行了扩展，首先就是其动态扩容的特点，然后再一定程度上同日出生了其查询速度

`ArrayList`的使用需要引入命名方法：`using System.Collections;`

##### 特点

* 可以动态扩容：通过创建一个更大的新数组，来将原来的数组转移到新数组
* 插入删除比数组方便
* 类似于数组，同样通过下标索引

##### 存储的是对象

* 需要装箱、拆箱操作
* 是不安全类型

> 注意
>
> * 装箱：将值类型转换为引用类型（隐式转换）
> * 拆箱：将引用类型转换为值类型（显式转换）

##### 属性

* `Capacity`：获取或者设置`ArrayList`中可以包含的元素个数
* `Count`：获取`ArrayList`中实际包含的元素个数
* `ArrayList[]`：通过下标索引

##### 方法

* `Add`：添加元素在`ArrayList`的尾部
* `Clear`：清除所有元素
* `Contains`：确定某个元素是否在`ArrayList`内
* `Sort`：排序

##### 使用方式

```c#
//创建
ArrayList m_arrayList = new ArrayList();
ArrayList al = new ArrayList { 1, 2, 3, 4 };

//元素插入
m_arrayList.Add("001");
m_arrayList.Add(1);

//第一中遍历方式
for(int i=0;i<m_arrayList.Count;i++)
{
    Console.WriteLine(m_arrayList[i]);
}

//第二种遍历方式
foreach(Object j in m_arrayList)
{
    Console.WriteLine(j);
}
```

#### `List`

`List`就是通过将泛型数据存储在一个泛型数组中，从而实现一个数据安全类型的列表，添加元素时若超过中当前泛型数组的容量，则进行二倍扩容，进而实现`List`大小动态变化

##### 特点

核心是数组

* 可以动态扩容：同样是创建新的更大数组来迁移数据
* 使用泛型来实现对装箱拆箱操作的避免

##### 内存优化

* 对于 List 可以规定长度，来避免动态扩容操作，这样就可以避免新的内存空间的消耗
* 相对于`ArrayList`，`List`避免了装箱拆箱操作，性能表现更好

##### 属性

* `Capacity`：获取或设置该内部数据结构在不调整大小的情况下能够容纳的元素总数。
* `Count`：获取 `List` 中包含的元素数。
* `Item[Int32]`：获取或设置指定索引处的元素。

##### 方法

* `Add`：添加元素在`List`的尾部
* `Clear`：清除所有元素
* `Contains`：确定某个元素是否在`List`内
* `Sort`：排序

##### 使用方式

```c#
List<int> nums = new List<int>(12);

//向列表插入数据
nums.Add(1);
nums.Add(2);

//遍历列表元素
for(int i=0;i<nums.Count;i++)
{
    Console.WriteLine(nums[i]);
}
foreach(int num in nums)
{
    Console.WriteLine(num);
}

//判断某一个元素是否在列表中
if(nums.Contains(1))
{
    Console.WriteLine("1 在列表 nums 中");
}

//排序
nums.Sort();
```

#### `HashTable`

##### 特点

* `HashTable`类似于字典，也是键值对的形式
* 查询速度快，插入速度慢
* 容量固定，根据 数组索引获得值

```c#
//哈希表结构体
private struct bucket {
   	public Object key;//键
    public Object values;//值
    public int hashCode;//哈希码
}
```

##### 原理

* 不定长的二进制数据通过哈希函数映射到一个较短的二进制数据集，即`Key`通过`HashFunction`函数获得`HashCode`
* 但是这样的`HashCode`依旧很长， 不方便索引，于是又将这些 HashCode 通过哈希桶算法进行分段（一般都是取余数），这样就会减小每一段的索引距离

#### `LinkedList`

##### 特点

* 链表插入删除方便，查找相对于数组来说比较慢
* 为了解决线性表的删除问题，对于数组，列表，数组列表插入删除是很复杂的一件事，而链表则不需要对于其进行遍历，即可实现其遍历过程
* 链表通过递归实现的数据结构

##### 属性

* `Count`：包含的节点数
* `First`：获取链表的第一个节点
* `Last`：获取链表的最后一个节点

##### 方法

* `AddFirst(T)`：开头处添加指定值的节点
* `AddLast(T)`：结尾处添加指定的节点
* `Clear()`：移除所有的节点
* `Contains(T)`：判断是否包含某一个值
* `FInd(T)`：查找包含值的第一个节点
* `FIndLast(T)`：查找包含值的最后一个节点
* `Remove()`：移除指定值的第一个匹配项
* `RemoveFirst()`：移除位于开头的节点
* `RemoveLast()`：移除位于结尾处的节点

#### `Dictionary`

`Dictionary<TKey,TValue>`泛型类提供一组键到一组值的映射。 每次对字典的添加都包含一个值和与其关联的键。 使用其键检索值的速度非常快，接近$O(1)$，因为`Dictionary<TKey,TValue>`该类是作为哈希表实现的

字典在 C#中是一种很常用的数据容器，在 Unity 中有很多的应用场景，尤其是框架阶段，如下：

* 事件管理器
* UI 界面管理器
* 有限状态机
* 资源加载器
* 对象池

##### 特点

* 索引方便，时间复杂度接近$O(1)$
* 是安全类型
* 键唯一

##### 属性

* `Count`：获取字典中包含的键值对的个数
* `Keys`：获取字典中包含键的集合
* `Values`：获取字典中包含的值的集合

##### 方法

* `Add(Key,Value)`：添加键值对
* `Clear()`：移除字典中所有键值对
* `ContainsKey(Key)`：判断字典中是否有该键
* `ContainsValue(Value)`：判断字典中是否有该值
* `Remove（Key)`：从字典中移除指定键的值
* `Remove（Key，Value)`：从元素中移除指定键的值，并复制给 Value
* `TryAdd(Key,Value)`：尝试将键值对插入到字典中，如果成功则返回 True

##### 使用方式

```c#
//创建一个字典
Dictionary<string, string> peoples = new Dictionary<string, string>();

//插入键值对
if(!peoples.ContainsKey("小明"))
{
    peoples.Add("小明", "男");
    peoples.Add("小红", "女");
}

//遍历字典中所有的键，同样可以遍历所有的值
Dictionary<string,string>.KeyCollection keys = peoples.Keys;
foreach(string s in keys)
{
    Console.WriteLine(s);
}

//遍历字典并输出键和值
foreach (KeyValuePair<string, string> kv in peoples)
{
    Console.WriteLine(kv.Key);
    Console.WriteLine(kv.Value);
}

//修改键值对的值
if (peoples.ContainsKey("小明"))
{
    peoples["小明"] = "女";
}

//删除键值对
if (peoples.ContainsKey("小明"))
{
    peoples.Remove("小明");
}
```

#### `Queue`

队列的特点是先进先出，主要是应用在一些特殊的场景，需要实现数据的一个先进先出的效果

##### 属性

* `Count`：获取`Queue<T>`中包含的元素数

##### 方法

* `Clear()`：从`Queue<T>`中移除所有对象。
* `Contains(T)`：确定某元素是否在`Queue<T>`中。
* `CopyTo(T[], Int32)`：从指定数组索引开始将`Queue<T>`元素复制到现有一维`Array`中。
* `Dequeue()`：移除并返回位于`Queue<T>`开始处的对象。
* `Enqueue(T)`：将对象添加到`Queue<T>`的结尾处。

#### `Stack`

##### 特点

* 先进后出
* 底层数组，两倍动态扩容

##### 属性

* `Count`：获取`Stack`中包含的元素数。

##### 方法

* `Clear()`: 从`Stack<T>`中移除所有对象。
* `Contains(T)`：确定某元素是否在`Stack<T>`中
* `Peek()`：返回位于`Stack<T>`顶部的对象但不将其移除。
* `Pop()`：删除并返回`Stack<T>`顶部的对象。
* `Push(T)`：在`Stack<T>`的顶部插入一个对象。

### Array 和 List 的复杂度

### 二分法的最大比较次数

长度为$n$的有序数组，根据二分查找的定义，每次比较之后问题规模都会减小一半，所以查找次数$k$满足$2^k=n$，又因为最后只剩一个元素是，也要进行比较，所以最大比较次数为$\lfloor log_2^{n} + 1 \rfloor$

### 如何分析内存泄漏

内存泄漏的简单定义，就是申请了内存，却没有在该释放的时候释放。游戏程序由代码和资源两部分组成，Unity 下的内存泄漏也主要分为代码侧的泄漏和资源侧的泄漏，当然，资源侧的泄漏也是因为在代码中对资源的不合理引用引起的

#### 代码中的泄漏 – Mono 内存泄漏

Unity 是使用基于 Mono 的 C#（当然还有其他脚本语言，不过使用的人似乎很少，在此不做讨论）作为脚本语言，它是基于 Garbage Collection（以下简称 GC）机制的内存托管语言。那么既然是内存托管了，为什么还会存在内存泄漏呢？因为 GC 本身并不是万能的，GC 能做的是通过一定的算法找到“垃圾”，并且自动将“垃圾”占用的内存回收

* 实际代码中，并非只有显示调用 new 才会分配内存，很多隐式的分配是不容易被发现的，例如产生一个 List 来存储数据，缓存了服务器下发的一份配置，产生一个字符串等等，这些操作都会产生内存的分配
* 在 Unity 环境下，Mono 堆内存的占用，是只会增加不会减少的。具体来说，可以将 Mono 堆，理解为一个内存池，每次 Mono 内存的申请，都会在池内进行分配；释放的时候，也是归还给池，而不会归还给操作系统。如果某次分配，发现池内内存不够了，则会对池进行扩建——向操作系统申请更多的内存扩大池以满足该次的内存分配。需要注意的是，每次对池的扩建，都是一次较大的内存分配，每次扩建，都会将池扩大 6-10M 左右

#### 资源中的泄漏 – Native 内存泄漏

资源泄漏，顾名思义，是指将资源加载之后占有了内存，但是在资源不用之后，没有将资源卸载导致内存的无谓占用

上文中说的代码分配的内存，是通过 Mono 虚拟机，分配在 Mono 堆内存上的，其内存占用量一般较小，主要目的是程序员在处理程序逻辑时使用；而 Unity 的资源，是通过 Unity 的 C++层，分配在 Native 堆内存上的那部分内存。举个简单的例子，通过 UnityEngine 命名空间中的接口分配的内存，将会通过 Unity 分配在 Native 堆；通过 System 命名空间中的接口分配的内存，将会通过 Mono Runtime 分配在 Mono 堆

![](https://ask.qcloudimg.com/http-save/467825/38w9a4zoze.png)

Mono 内存是通过 GC 来回收的，而 Unity 也提供了一种类似的方式来回收内存。不同的是，Unity 的内存回收是需要主动触发的。主动调用的接口是`Resources.UnloadUnusedAssets()`。其实 GC 也提供了同样的接口`GC.Collect()`。用来主动触发垃圾回收，这两个接口都需要很大的计算量，我们不建议在游戏运行时时不时主动调用一番，一般来说，为了避免游戏卡顿，建议在加载环节来处理垃圾回收的操作

`Resources.UnloadUnusedAssets()`内部本身就会调用`GC.Collect()`。Unity 还提供了另外一个更加暴力的方式——`Resources.UnloadAsset()`来卸载资源，但是这个接口无论资源是不是“垃圾”，都会直接删除，是一个很危险的接口，建议确定资源不使用的情况下，再调用该接口

* 首先和代码侧的泄漏一样，由于“存在该释放却没有释放的错误引用”，导致回收机制认为目标对象不是“垃圾”，以至于不能被回收，这也是最常见的一种情况
* 针对资源，还有一种典型的泄漏情况。由于资源卸载是主动触发的，那么清除对资源引用的时机就显得尤为重要。现在游戏的逻辑趋于复杂化，同时如果有新成员加入项目组，也未必能够清楚地了解所有资源管理的细节，如果“在触发了资源卸载之后，才清除对资源引用”，同样也会出现内存泄漏了
* 还有一种资源上的泄漏，是因为 Unity 的一些接口在调用时会产生一份拷贝（例如 [Renderer.Material](https://docs.unity3d.com/ScriptReference/Renderer-material.html)），如果在使用上不注意的话，运行时会产生较多的资源拷贝，造成内存的无端浪费

#### 修复内存泄漏

* New Memory Profiler For Unity5
* Mono 内存的放大镜——Cube
* 顺藤摸瓜——从 Mono 中寻找资源引用

### 设计模式相关知识

### 客户端框架有哪些功能模块

![](https://img-blog.csdnimg.cn/20191114114031714.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2UyOTUxNjYzMTk=,size_16,color_FFFFFF,t_70)

* UI 框架（UGUI + MVC）
* 消息管理（Message Manager）
* 网络层框架（Socket + Protobuf）
* 表格数据（Protobuf）
* 资源管理（Unity5.x 的 AssetBundle 方案）
* 热更框架（tolua）

#### UI 框架

![](https://img-blog.csdnimg.cn/20191114114336900.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2UyOTUxNjYzMTk=,size_16,color_FFFFFF,t_70)

基于 Unity3D 和 UGUI 实现的简单的 UI 框架，实现内容：

* 加载、显示、隐藏、关闭页面，根据标示获得相应界面实例；
* 提供界面显示隐藏动画接口；
* 单独界面层级，Collider，背景管理；
* 根据存储的导航信息完成界面导航；
* 界面通用对话框管理；
* 便于进行需求和功能扩展；

#### 消息管理（Message Manager）

* 一个消息系统的核心功能：
* 一个通用的事件监听器；
* 管理各个业务监听的事件类型（注册和解绑事件监听器）；
* 全局广播事件；
* 广播事件所传参数数量和数据类型都是可变的

消息管理设计思路：在消息系统初始化时将每个模块绑定的消息列表，根据消息类型分类（用一个`string`类型的数据类标识），即建立一个字典`Dictionary<string, List<Model>>`：每条消息触发时需要通知的模块列表：某条消息触发，遍历字典中绑定的模块列表

#### 网络层框架（NetworkManager）

* 除了单机游戏，限制绝大多数的网游都是以强联网的方式实现的，选用 Socket 通信可以实时地更新玩家状态。
* 选定了联网方式后，还需要考虑网络协议定制的问题，Protobuf 无疑是个比较好的选择，一方面是跨平台特性好，另一方面是数据量小可以节省通信成本。
* Socket 通信：联网方式、联网步骤，数据收发以及协议数据格式。加入线程池管理已经用一个队列来管理同时发起的请求，让 Socket 请求和接收异步执行，基本的思路就是引入多线程和异步等技术。
* Protobuf 网络框架主要用途是：数据存储（序列化和反序列化），功能类似 xml 和 json 等；制作网络通信协议等。* Protobuf 不仅可以进行 excel 表格数据的导出，还能直接用于网络通信协议的定制。
* Protobuf 是由 Google 公司发布的一个开源的项目，是一款方便而又通用的数据传输协议。在 Unity 中可借助 Protobuf 来进行数据存储和网络协议两方面的开发。

#### 表格数据

![](https://img-blog.csdnimg.cn/20191114115353713.jpg?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2UyOTUxNjYzMTk=,size_16,color_FFFFFF,t_70)

* 在游戏开发中，有很多数据是不需要通过网络层从服务器拉取下来的，而是通过表格配置的格式存储在本地。
* 游戏中的一个道具，通常服务器只下发该道具的 ID（唯一标识）和 LV（等级），然后客户端从本地数据中检索到该道具的具体属性值。通常使用 Excel 表格来配置数据，可以使用 Protobuf、JSON、XML 等序列化和反序列化特性对表格数据转化。

#### 资源管理（AssetBundle）

* AssetBundle 是 Unity 引擎提供的一种资源压缩文件，文件扩展名通常为 unity3d 或 assetbundle。
* 对于资源的管理，其实是为热更新提供可能，Unity 制作游戏的资源管理方式就通过 AssetBundle 工具将资源打成多个 ab 包，通过网络下载新的 ab 包来替换本地旧的包，从而实现热更的目的。
* AssetBundle 是 Unity 编辑器在编辑环境创建的一系列的文件，这些文件可以被用在项目的运行环境中。包括的资源文件有：模型文件（models）、材质（materials）、纹理（textures）和场景（scenes）等。

#### 热更新框架（tolua）

* 使用 C#编写底层框架，使用 lua 编写业务逻辑，这是业内最常见的设计方式，还有一个非常成熟的热更新框架 tolua。
* 通常可热更新的有：图片资源、UI 预制和 lua 脚本，而处于跨平台的考虑，C#脚本是不允许进行热更的。

### AB 包怎么解决循环依赖问题

#### AB的基础知识

##### Unity中资源相关目录介绍

* Resources：全部资源都会被压缩，转化成二进制。打包后该路径不存在，不可写也不可读。只能使用`Resources.Load`加载资源。
* StreamingAssets：全部资源原封不动打包。在移动平台下，是只读的，不能写入数据，其他平台下可以使用`System.File`类进行读写。

##### AB是什么

AB即AssetBundle，是一种Unity提供的用于存放资源的包。
一个AssetBundle可以当做一个文件集合，它包含了Unity可以在运行时加载的特定于平台的非代码资产(例如模型、纹理、预制组件、音频，甚至整个场景)。AssetBundle可以表示彼此之间的依赖关系;例如，一个AssetBundle中的material可以引用另一个AssetBundle中的texture。为了在减轻网络传输压力，您可以根据需求选择内置算法(LZMA和LZ4)来压缩AssetBundle。
通过将资源分布在不同的AB包中可以最大程度地减少运行时的内存压力，并且可以有选择地加载内容

##### AB的内部结构

总的来说，AssetBundle就像传统的压缩包一样，由两个部分组成：包头和数据段。
包头包含有关AssetBundle的信息，比如标识符、压缩类型和内容清单。清单是一个以Objects name为键的查找表。每个条目都提供一个字节索引，用来指示该Objects在AssetBundle数据段的位置。在大多数平台上，这个查找表是用平衡搜索树实现的。具体来说，Windows和OSX派生平台（包括iOS）都采用了红黑树。因此，构建清单所需的时间会随着AssetBundle中Assets的数量增加而线性增加
数据段包含通过序列化AssetBundle中的Assets而生成的原始数据。如果指定LZMA为压缩方案，则对所有序列化Assets后的完整字节数组进行压缩。如果指定了LZ4，则单独压缩单独Assets的字节。如果不使用压缩，数据段将保持为原始字节流。

普通资源的AB结构和场景资源的AB结构不一样

* 普通资源的AB

    ![](https://img-blog.csdnimg.cn/20190113184924459.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzE5MjY5NTI3,size_16,color_FFFFFF,t_70)
    AssetBundleFileHeader：记录了版本号、压缩等主要描述信息。
    AssetFileHeader：包含一个文件列表，记录了每个资源的name、offset、length等信息。
    Asset1：
    AssetHeader：记录了TypeTree大小、文件大小、format等信息。
    TypeTree（可选，有不要TypeTree的构建方式）：记录了Asset对象的class ID。Unity可以用class ID来序列化和反序列化一个类。（每个class对应了一个ID，如0是Object类，1是GameObject类等。具体可在Unity官网上查询。）
    ObjectPath：记录了path ID（资源唯一索引ID）等。
    AssetRef：记录了AB包对外部资源对引用情况。
    Asset2…

* 场景资源的AB

    场景AssetBundle更改自标准的AssetBundles，因为它针对场景及其内容的流加载进行了优化，其内部结构如下：
    ![](https://img-blog.csdnimg.cn/20190113185856951.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzE5MjY5NTI3,size_16,color_FFFFFF,t_70)

阅读链接：

[云风的 BLOG: Unity3D asset bundle 格式简析](https://blog.codingnow.com/2014/08/unity3d_asset_bundle.html)

[Unity AssetBundle文件格式解析](https://chenanbao.github.io/2020/01/08/AssetBundle/)

#### AB和Resources的优缺点

##### AB

* 无法直观地看到包内的资源情况。
* 异步加载，需要写比较繁琐的回调处理。
* 调试的时候，无法通过Hierarchy直接定位到资源。
* 使用之前需要花费时间进行打包，尤其是在开发的时候，调整资源频繁，如果忘记打包可能导致Bug
  
##### Resources

* 对内存管理造成一定的负担。
* 在打开应用时加载时间很长。
* Resources文件夹下的所有资源统一合并到一个序列化文件中（可以看成统一打一个大包，巨型AB包有什么问题它就有什么问题），对资源优化有一定的限制

#### 怎么打AB

```c#
// 1. 让 Unity 通过在 Editor 中标记的 AssetBundleName 来打包
//    正经的项目基本不用
public static AssetBundleManifest BuildAssetBundles(
    string outputPath,
    BuildAssetBundleOptions assetBundleOptions,
    BuildTarget targetPlatform)

// 2. 指定 AssetBundleBuild 数组来构建
//    一般都选这个
public static AssetBundleManifest BuildAssetBundles(
    string outputPath,                            //打出的ab的路径
    AssetBundleBuild[] builds,                    //指定的资源数组  
    BuildAssetBundleOptions assetBundleOptions,   //打包选项
    BuildTarget targetPlatform                    //目标平台
)   
```

##### AssetBundleBuild

```c#
public struct AssetBundleBuild
{
    public string assetBundleName;        //ab的名字
    public string assetBundleVariant;     //ab中包含的文件相对于asset目录的路径
    public string[] assetNames;           //想防御是assetNames的别名，必须和assetName长度一致
    public string[] addressableNames;     //设置ab的变体，就是后缀名
}
```

一般情况下，只要设置AssetBundleBuild结构体中的assetBundleName属性和assetNames属性就可以了，如果你愿意给打在AssetBundle包中的文件重命名的话，就设置一下addressableNames属性，不想重命名的文件就给一个空字符串，这样默认就会使用assetNames了。要是还想设置自定义的后缀名，可以试试AssetBundleVariant

##### BuildAssetBundleOptions

```c#
public enum BuildAssetBundleOptions
{
    //lzma
    None = 0,
    //ab不压缩。ab会很大，但是加载会很快
    UncompressedAssetBundle = 1,
    
    //包含所有依赖，5.0后默认开启
    CollectDependencies = 2,
    //强制包含整个资源
    CompleteAssets = 4,
    //不包含类型信息，发布web平台时，不能使用此选项
    //在Unity 5.x版本后，AssetBundle在制作时会默认写入TypeTree信息，这样做的好处是可以证            //AssetBundle文件的向下兼容性，即高版本可以支持以前低版本制作的AssetBundle件。
    //如果开启DisableWriteTypeTree选项，则可能造成AssetBundle对Unity版本的兼容问题，
    DisableWriteTypeTree = 8,
    
    //使每个object具有唯一不变的hashID,可用于增量发布AssetBundle.热更必须开
    DeterministicAssetBundle = 16,
    //强制构建所有ab
    ForceRebuildAssetBundle = 32,
    //忽略typetree的变化，不能与DisableWriteTypeTree同时使用
    IgnoreTypeTreeChanges = 64,
    //附加hash到ab名字中
    AppendHashToAssetBundleName = 128,
    //使用LZ4格式压缩ab，ab会在加载资源时解压
    unity= 256,
    //使用严格模式build ab,有任何致命的error都不会build成功
    StrictMode = 512,
    //     Do a dry run build.
    DryRunBuild = 1024,
    //不使用fileName来加载ab
    DisableLoadAssetByFileName = 4096,
    //不使用带后缀的文件名来加载ab
    DisableLoadAssetByFileNameWithExtension = 8192,
    //构建时从压缩文件和序列化文件的header中移除Unity版本号
    AssetBundleStripUnityVersion = 0x800
}
```

##### 压缩格式

* 不压缩
  * ab会很大，但是加载会很快
  * 开启方法：BuildAssetBundleOptions.UncompressedAssetBundle
* LZMA
  * LZMA是流压缩方式（stream-based）。流压缩在处理整个数据块时使用同一个字典，它提供了最大可能的压缩率，但是只支持顺序读取。所以加载AB包时，需要将整个包解压，会造成卡顿和额外内存占用
  * 开启方法：默认
  * 使用LZMA压缩算法，该算法压缩后包体很小，但是加载的时候需要花费很长的时间解压。第一次解压之后，该包又会使用LZ4压缩算法再次压缩。这就是为什么第一次加载时间长，之后加载时间就没那么长了。
* LZ4
  * LZ4是块压缩方式（chunk-based）。块压缩的数据被分为大小相同的块，并被分别压缩。如果需要实时解压随机读取，块压缩是比较好的选择。LoadFromFile()和LoadFromStream()都只会加载AB包的Header，相对LoadFromMemory()来说大大节省了内存
  * 开启方法：BuildAssetBundleOptions.ChunkBasedCompression

![](https://img-blog.csdnimg.cn/20190113190116678.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzE5MjY5NTI3,size_16,color_FFFFFF,t_70)

#### 打AB的策略

依赖问题，通俗的话来说就是A包中某资源用了B包中的某资源。然而如果A包加载了，B包没有加载，这就会导致A包中的资源出现丢资源的现象。
在Unity5.0后，BuildAssetBundleOptions.CollectDependencies永久开启，即Unity会自动检测物体引用的资源并且一并打包，防止资源丢失遗漏的问题出现。
因为这个特性，有些情况下，如果没指定某公共资源的存放在哪个AB包中，这个公共资源就会被自动打进引用它的AB包中，所以出现多个不同的AB包中有重复的资源存在的现象。这就是资源冗余。
这种情况下，哪怕资源是一模一样，也无法进行合并优化。
要防止资源冗余，就需要明确指出资源存放在哪个AB包中，形成依赖关系。所以对于一些公共资源，建议单独存放在一个AB包中。
在加载的时候，如果AB包之间相互依赖，那么加载一个AB包中的资源时，先需要加载出另一个AB包的资源。这样就会导致不必要的消耗。所以说尽可能地减少AB包之间的依赖，并且公共资源尽量提前加载完成

```c#
//
// Summary:
//     Returns an array of the paths of assets that are dependencies of all the assets
//     in the list of pathNames that you provide. Note: GetDependencies() gets thAssets
//     that are referenced by other Assets. For example, a Scene could contain many
//     GameObjects with a Material attached to them. In this case, GetDependencies()
//     will return the path to the Material Assets, but not the GameObjects as those
//     are not Assets on your disk.
//
// Parameters:
//   pathNames:
//     The path to the assets for which dependencies are required.
//
//   recursive:
//     Controls whether this method recursively checks and returns all dependencies
//     including indirect dependencies (when set to true), or whether it only returns
//     direct dependencies (when set to false).
//
// Returns:
//     The paths of all assets that the input depends on.
public static extern string[] (AssetDatabase.)GetDependencies(string[] pathNames, boorecursive)
```

|AB包数量较多，包内资源较少|AB包数量较少，包内资源较多|
|:---|:---|
|加载一个AB包到内存的时间短，玩家不会有卡顿感，每个资源实际上加载时间变长|加载一个AB包到内存的时间较长，顽疾会有卡顿感，但之后包内的每个资源加载快|
|热更新灵活，要更新下载的包体较小|热更新不灵活，要更新下载的包体较大|
|IO次数过多，增大了硬件设备耗能和发热压力|IO次数不多，硬件压力小|
|||

##### 颗粒度问题

细粒度问题即每个AB包分别放入多少资源的问题，一个好的策略至关重要。
加载资源时，先要加载AB包，再加载资源。如果AB包使用了LZMA或LZ4压缩算法，还需要先给AB包解压

### AB 包内单个资源修改如何进行热更

根据文件的 md5 码比对

### Socket 有几种模式

### TCP 粘包如何处理

粘包分包，都是加长度，进行拆分

### 网络数据的增量更新如何解决

### C#如何调用 Jar 包

AndroidJavaClass
类似反射的操作

### 面向对象的基本原则

* 封装
* 继承
* 多态

### 向量的运算

### 矩阵运算

### 渲染管线相关

应用 几何 光珊化

### Batch 和 SetPass 的区别

#### GPU 遇到了瓶颈

首先要考虑的点是纹理填充率和显存带宽

* 纹理填充率：纹理填充率主要是 GPU 渲染像素的速度，实际工作当中需要从 shader 复杂度，overdraw 开销，屏幕分辨率，后处理等着手改善
* 显存带宽：显存带宽则主要是 GPU 和显存进行数据交换的速度，实际工作当中需要从顶点数，顶点复杂度，纹理大小，纹理采样数量，后处理等着手改善

#### CPU 遇到了瓶颈

考虑的点主要是程序逻辑执行的复杂度，drawcall 等

##### Set Pass Call

Set Pass Call 代表渲染状态切换，主要出现在材质不一致的时候，进行渲染状态切换。一个 Batch 包括，提交 VBO（顶点信息），提交 IBO（ 顶点的索引信息），提交 shader，设置好硬件渲染状态，设置好光源属性等（注意提交纹理严格意义上并不包括在一个 Batch 内，纹理可以被缓存并多帧复用）。如果一个 Batch 和另一个 Batch 使用的不是同种材质或者同一个材质的不同 pass，那么就要触发一次 Set Pass Call 来重新设定渲染状态

> 例如，Unity 要渲染 20 个物体，这 20 个物体使用同种材质（但不一定 mesh 等价），假设两次 Dynamic Batch 各自合批了 10 个物体，则对于这次渲染，sSet Pass Call 为 1（只需要渲染一个材质），Batch 为 2（向 GPU 提交了两次 VBO，IBO 等数据）

Draw Call 严格意义上，CPU 每次调用图形 API 的渲染函数（使用 OpenGL 举例，是`glDrawElements`或者`DrawIndexedPrimitive`）都算作一次 Draw Call，但是对于 Unity 而言，它可以多个 Draw Call 合并成一个 Batch 去渲染。真正造成开销较大的地方，第一个在于在于切换渲染状态，第二在于整理和提交数据。在真正的实践过程当中，可以不用过于介意 Draw Call 这个数字（因为没有提交数据或者切换渲染状态的话，其实多来几个 Draw Call 没什么所谓），但是 Set Pass Call 和 Batch 两个数字都要想办法降低。由于二者存在强相关性，那么通常降低一个，就一并可以降低第二个

#### Unity 的三种批次合并方法

* Static Batching，将静态物体集合成一个大号 VBO 提交，但是只对要渲染的物体提交其 IBO。这么做不是没有代价。比如说，四个物体要静态批次合并前三个物体每个顶点只需要位置，第一套 uv 坐标信息，法线信息，而第四个物体除了以上信息，还多出来切线信息，则这个 VBO 会在每个顶点都包括所有的四套信息，毫无疑问组合这个 VBO 是要对 CPU 和显存有额外开销的。要求每一次 Static Batching 使用同样的 material，但是对 mesh 不要求相同
* Dynamic Batching，将物体动态组装成一个个稍大的 VBO + IBO 提交。这个过程不要求使用同样的 mesh，但是也一样要求同样的材质。但是，由于每一帧 CPU 都要将每个物体的顶点从模型坐标空间变换到组装后的模型的坐标空间，这样做会带来一定的计算压力。所以对于 Unity 引擎，一个批次的动态物体顶点数是有限制的
* GPU Instancing 是只提交一个物体的 mesh，但是将多个使用同种 mesh 和 material 的物体的差异化信息（包括位置，缩放，旋转，shader 上面的参数等。shader 参数不包括纹理）组合成一个 PIA（Per Instanced Attribute）提交。在 GPU 侧，通过读取每个物体的 PIA 数据，对同一个 mesh 进行各种变换后绘制。这种方式相比 Static Batching 和 Dynamic Batching 节约显存，又相比 Dynamic Batching 节约 CPU 开销。但是相比这两种批次合并方案，会略微给 GPU 带来一定的计算压力。但这种压力通常可以忽略不计。限制是必须相同材质相同物体，但是不同物体的材质上的参数可以不同

Unity 默认策略是优先 Static Batching，其次 GPU Instancing，最后 Dynamic Batching。当然如果顶点数过于巨大（比如渲染它几千颗使用同种 mesh 的树），那么 GPU Instancing 或许比 Static Batching 是一个更加合适的方案

### 如何降低 Draw Call

### 虚函数的实现原理，父类和子类的内存布局

### 协程和进程的区别，怎么实现，协程程序怎么写，有什么问题

#### 概念

* 进程：进程是一个具有一定独立功能的程序关于某个数据集合上的一次运行活动，是系统资源分配和独立运行的最小单位；
* 线程：线程是进程的一个执行单元，是任务调度和系统执行的最小单位；
* 协程：协程是一种用户态的轻量级线程，协程的调度完全由用户控制。

#### 进程与线程的区别

* 根本区别：进程是操作系统资源分配和独立运行的最小单位；线程是任务调度和系统执行的最小单位。
* 地址空间区别：每个进程都有独立的地址空间，一个进程崩溃不影响其它进程；一个进程中的多个线程共享该 进程的地址空间，一个线程的非法操作会使整个进程崩溃。
* 上下文切换开销区别：每个进程有独立的代码和数据空间，进程之间上下文切换开销较大；线程组共享代码和数据空间，线程之间切换的开销较小。

#### 进程与线程的联系

一个进程由共享空间（包括堆、代码区、数据区、进程空间和打开的文件描述符）和一个或多个线程组成，各个线程之间共享进程的内存空间，而一个标准的线程由线程 ID、程序计数器 PC、寄存器和栈组成。
进程和线程之间的联系如下图所示：

![](https://img-blog.csdnimg.cn/20201021221100545.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80OTE5OTY0Ng==,size_16,color_FFFFFF,t_70#pic_center)

![](https://img-blog.csdnimg.cn/20201021221114554.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80OTE5OTY0Ng==,size_16,color_FFFFFF,t_70#pic_center)

#### 进程与线程的选择

* 线程的创建或销毁的代价比进程小，需要频繁创建和销毁时应优先选用线程；
* 线程上下文切换的速度比进程快，需要大量计算时优先选用线程；
* 线程在 CPU 上的使用效率更高，需要多核分布时优先选用线程，需要多机分布时优先选用进程
* 线程的安全性、稳定性没有进程好，需要更稳定安全时优先使用进程。

综上，线程创建和销毁的代价低、上下文切换速度快、对系统资源占用小、对 CPU 的使用效率高，因此一般情况下优先选择线程进行高并发编程；但线程组的所有线程共用一个进程的内存空间，安全稳定性相对较差，若其中一个线程发生崩溃，可能会使整个进程，因此对安全稳定性要求较高时，需要优先选择进程进行高并发编程。

#### 协程

协程拥有自己的寄存器上下文和栈。协程调度切换时，将寄存器上下文和栈保存到其他地方，在切回来的时候，恢复先前保存的寄存器上下文和栈。因此，协程能保留上一次调用时的状态（即所有局部状态的一个特定组合），每次过程重入时，就相当于进入上一次调用的状态。这个过程完全由程序控制，不需要内核进行调度。
协程与线程的关系如下图所示：

![](https://img-blog.csdnimg.cn/a550c0974c584398a71c29f783d9b4d9.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80OTE5OTY0Ng==,size_16,color_FFFFFF,t_70)

#### 协程与线程的区别

* 根本区别：协程是用户态的轻量级线程，不受内核调度；线程是任务调度和系统执行的最小单位，需要内核调度。
* 运行机制区别：线程和进程是同步机制，而协程是异步机制。
* 上下文切换开销区别：线程运行状态切换及上下文切换需要内核调度，会消耗系统资源；而协程完全由程序控制，状态切换及上下文切换不需要内核参与

### Struct 和 class 的区别，值类型和引用类型区别

### C#装箱和拆箱

### Inline 函数有啥作用，和宏定义有啥区别

### 闭包

### Action 和 function 区别，以及内部实现，注册函数如何防止重复，如何删除

### Map 怎么实现的，Dictionary 如何实现

### 红黑树和 avl 树还有堆的区别，内存&效率

### 快排的时间空间复杂度以及实现

### 堆排的实现，时间空间复杂度

### Enum 作 Key 的问题

GC, 涉及 ICompare

### CLR 是什么

### il2cpp 和 mono

### 如何实现一个扇形进度条

Unity Image Filled

### 渲染管线流程，mvp 变换等，各种 test

### Shadowmap 实现

[Shadowmap](https://blog.csdn.net/the_shy33/article/details/120043177) 的原理非常简单，首先它在光源处放置一台相机，通过该相机生成一张深度图，然后在渲染需要接受阴影的物体时把当前渲染的点的深度与深度图里的深度比较。当然，当前点的深度需要转换到光源视角的坐标下，不然就变成了游戏主相机的深度了

![](https://img-blog.csdnimg.cn/660862be3d2a48dda71110bc551c5065.png?x-oss-process=image,shadow_50,text_Q1NETiBAdGhlX3NoeTMz,size_20,color_FFFFFF,t_70,g_se,x_16#pic_center)

### 如何高效实现阴影

### CPU 和 GPU 区别，如何设计

### 几种反走样算法实现、问题、效率

### 前向渲染和延迟渲染的区别，什么时候用

### 延迟渲染需要几个 buffer，需要记录什么信息

渲染路径，切换渲染路径，

### 数组和链表区别

### C# GC 算法

### 如何实现战争迷雾

https://blog.csdn.net/august5291/article/details/120262524

### Unity 优化手段，dc cpu gpu

https://www.cnblogs.com/murongxiaopifu/p/4284988.html

### GPUInstance

https://blog.csdn.net/mango9126/article/details/120561912

### Pbr 最重要的参数，几个方程

### 如何搭建一个 pbr 工作流

https://zhuanlan.zhihu.com/p/60972473

### Topk 问题以及变种，各种解法

### 100 万个数据快速插入删除和随机

### Unity 资源相关问题，内存分布，是否 copy 等

### 帧同步和状态同步区别等一系列问题

#### 帧同步

服务器负责转化客户端的操作，每个客户端在固定的逻辑帧执行该帧所有客户端的操作命令，通过在严格一致的时间轴上执行同样的命令序列得到同样的结果。主流的老牌 RTS 游戏都是帧同步：星际争霸、war3

![](https://pic3.zhimg.com/80/v2-b93344314c698823324b3d9a69b372c2_720w.webp)

##### 优点

* 开发方便，可以无视客户端服务器（但是要考虑逻辑和表现分离）
* 打击感反馈好，例如所有的命中都能在本地马上触发相应的扣血和打击表现，反馈及时准确，无须像状态同步那样等待服务器推送扣血或者向服务器请求扣血。
* 网络流量小，这个会带来很多好处：带宽费用小、用户成本低、降低收发包带来的耗电（据说比较可观）

##### 缺点

* 对网络要求高。这个涉及到具体帧同步实现的方式。

    锁帧问题，服务器会等待所有的客户端的第 N 帧操作都到齐之后再发送到各客户端第 N 帧的操作，这样一旦有一个客户端网络波动，所有人都会卡住。war3 的做法是加入了超时机制，如果在第 N 帧超时没有收到某个客户端第 N 帧的操作，就不再等待这个客户端的这帧操作，并认为该客户端在这帧什么都没做。但是如果这个客户端只是延迟很高，他所有的操作是否都会被服务器判定无效呢？

    逻辑帧平滑问题，一般收到的逻辑帧命令数据会加入客户端正在顺序执行的逻辑帧的队列中。如果队列设置过长，操作延迟就会比较高；如果队列很短，在网络波动时，就会出现队列为空饥饿状态，造成逻辑帧不平滑。这个可以采用逻辑和表现分离、平滑插值等做法。逻辑和表现分离做的比较好的话可以做到无 buffer。平滑插值是一些表现的过渡处理，比如卡顿感一个很主要的来源是怪物移动的不平滑，一个比较好的应对方法是以怪物方向为主计算位置，即使没有逻辑帧，表现帧也会继续根据当前方向速度计算位置，这样即使延迟出现波动时也不会出现位移的不平滑。
* 反外挂能力差，容易本地修改，开图，修改属性等。
* 断线重连需要追帧
* 客户端逻辑计算性能压力大，需要每逻辑帧计算所有游戏单位的逻辑状态，即使单位不在屏幕内。
* 对结果一致性控制比较严格，如果使用了第三方的库，需要能够控制结果的一致性。其他因素包括浮点计算在不同平台的误差差异、随机数序列一致性、一些容器的访问顺序问题

#### 状态同步

服务器承载所有计算，客户端只做表现。主流的大型 MMO 游戏都采用状态同步。

![](https://pic4.zhimg.com/80/v2-d3b538d949651bbbe70bdc71835e510b_720w.webp)

##### 优点

* 容易断线重连
* 容易防外挂
* 简单粗暴

##### 缺点

* 流量大
* 打击感反馈匹配不够精准，因为所有的表现都是服务器推送的，在网络波动、客户端服务器不同计算的误差下，客户端各个表现比较难契合。
* 对网络要求也会比较高，如果 2 中说的各个表现的契合，以及位移等都会受到网络波动的影响。

无论是状态同步还是帧同步，对于网络延迟的优化都会涉及到的问题是：

* 在网络协议层优化，可靠 UDP 代替 TCP，降低延迟
* 在表现上优化，客户端先行、平滑插值等在表现上降低对延迟的感受

### 帧同步要注意的问题

* 结果一致性
* 网络延迟
* 作弊
* 随机种子
* 断线重连

### 随机数如何保证同步

### 如何设计一个技能系统以及 buff 系统

### 数组第 k 大的数

### 1~n 有一个数字没有，找到这个数

```java
public int FindMissNumberBySum(int array[], int n) {
    int sum = 0;
    if (array.length != n - 1)
        throw new IllegalArgumentException("数组内的自然数目少于 n-1");
    for (int i : array) {
        sum += i;
    }
    return n * (n + 1) / 2 - sum;
}
```

### 如何分析程序运行效率瓶颈，log 分析

### UI 背包如何优化

### Unity UI 如何自适应

Canvas

### A *寻路实现

A *实现
A *优化
A *平滑路径
https://www.cnblogs.com/zhaoqingqing/p/3960799.html
https://zhuanlan.zhihu.com/p/80707067

### Unity Navimesh

### 体素的思想和实现

### 碰撞检测算法，优化，物理引擎检测优化

### 连续碰撞检测 POI

### 设计个 UIManager，ui 层级关系，ui 优化

最简单的栈 UI 思想

### MVC 思想

### 设计模式准则

* 单一职责原则
  * 类的职责要单一，不能将太多得职责放在一个类中
  * 高内聚，低耦合
* 里氏替换原则 在软件系统中，一个可以接受基类对象得地方必然可以接受
* 迪米特法则
* 依赖倒置原则
* 接口隔离原则
* 开闭原则

### UML 图

https://baijiahao.baidu.com/s?id=1700899956563526148&wfr=spider&for=pc

### 消息管理器实现

EventManager, ET 的 EventSystem.
SignalSet. Action + Dictionary
Dictionary<string, List<Action>> + 消息排序

### lua `dofile`和`require`区别

在加载一个。lua 文件时，`require`会现在`package.loaded`中查找此模块是否存在，如果存在则直接返回模块，如果不存在，则加载此模块。

* `dofile`会对读入的模块编译执行，每调用`dofile`一次，都会重新编译执行一次
* `require`它的参数只是文件名，而`dofile`要求参数必须带上文件名的后缀

### Lua 面向对象实现以及与区别

https://www.runoob.com/lua/lua-object-oriented.html

### 如何防止大量 GC

对象池

### 如何实现热更

ILRuntime, xlua

### 游戏 AI 如何实现，行为树要点

* Action
* Composite
* Conditional
* Decorate

### 如何实现一个延时运行功能

Timer, Coroutine

### Unity 纹理压缩格式

#### Unity 支持的压缩格式的分类

* DXT 格式 --- Nvidia Tegra 提供
* ETC  --- 安卓原生支持的，OPNEGL2.0 都支持，ETC2 只有 OPENGL3.0 支持
* PVRTC --- Imagination PowerVR 提供
* ATC --- Qualcomm Snapdragon 提供的
* ASTC--- Bridgetek
* IOS 只支持 PVRTC 和 ASTC 的压缩格式

Unity3D 引擎对纹理的处理是智能的：不论你放入的是 PNG，PSD 还是 TGA，它们都会被自动转换成 Unity 自己的 Texture2D 格式

要注意一些 png 图片，在硬盘中占用几 KB，怎么在 Unity 中显示却变大？因为 Unity 显示的是 Texture 大小，是实际运行时占用内存的大小，而 png 却是一种压缩显示格式

### Mipmap 思想，内存变化

### unity 生命周期，实例化后，aos 顺序

Initial->Physics->Input events->GameLogic->Scene rendering->Gizmos->GUI->EndofFrame

### resource 与 streamingAsset 区别

* AssetBundle。很多人应该知道这是 unity3d 里面用来打包资源的，支持的格式有限，如文理、音频、二进制、文本等。像一些。cs 文件、.mp4 文件是没法打包的。
* Resources 目录下的资源在打包之后，也会生成 AssetBundle，只是 Resources 下的资源会被系统自动处理。
* 那 AssetBundle 到底是什么呢？其实可以把它简单看成是一个资源集合，必须用 WWW 类来进行读取。
* 而 Resources 可以看成是一个特殊的 WWW，只能对于 Resources 目录的资源。而 WWW 读取的内容也必须是 AssetBundle，所以一个单独的 ogg 文件无法用 WWW 读取，必须先打包成 AssetBundle，才能用 WWW 读取。
* StreamingAssets 又是什么呢？这是个 Raw 目录，里面的内容不会加密、编码。比如 png、ogg

### mipmap1024 一张图能开启几级

9 级 512*512, 256*256, 128*128, 64*64, 32*32, 16*16, 8*8, 4*4, 2*2, 1*1

### upvalue 作用

### lua `self`作用

`self`其实就相当于 Java，C++中的`this`对象

```lua
a = {x = 3, y = 4}
 
a.__index = function(table, key)
    print("__index")
    return a[key]
end
--注：
--冒号，在定义的时候省略掉了 self
--点，在定义的时候不省略 self

function a.new(self, o)
    o = o or {}
    print("new")
    setmetatable(o, self)
    return o
end
 
function a.new(o)
    o = o or {}
    print("new")
    setmetatable(o, a)
    return o
end
 
--上下两种写法是等价的
local b = a:new() --调用可以等价于 a.new(a, {})
print(getmetatable(b))
print(b.x)
```

### lua 持续引用 c#如何处理，如果忘记设置 lua 引用变量 = nil 如何处理

### unity 内存分哪些

#### Unity 中的内存种类

实际上 Unity 游戏使用的内存一共有三种：程序代码、托管堆（Managed Heap）以及本机堆（Native Heap）。

程序代码包括了所有的 Unity 引擎，使用的库，以及你所写的所有的游戏代码。在编译后，得到的运行文件将会被加载到设备中执行，并占用一定内存。这部分内存实际上是没有办法去“管理”的，它们将在内存中从一开始到最后一直存在。一个空的 Unity 默认场景，什么代码都不放，在 iOS 设备上占用内存应该在 17MB 左右，而加上一些自己的代码很容易就飙到 20MB 左右。想要减少这部分内存的使用，能做的就是减少使用的库，稍后再说。

托管堆是被 Mono 使用的一部分内存。Mono 项目一个开源的.Net 框架的一种实现，对于 Unity 开发，其实充当了基本类库的角色。托管堆用来存放类的实例（比如用 new 生成的列表，实例中的各种声明的变量等）。“托管”的意思是 Mono“应该”自动地改变堆的大小来适应你所需要的内存，并且定时地使用垃圾回收（Garbage Collect）来释放已经不需要的内存。关键在于，有时候你会忘记清除对已经不需要再使用的内存的引用，从而导致 Mono 认为这块内存一直有用，而无法回收。

最后，本机堆是 Unity 引擎进行申请和操作的地方，比如贴图，音效，关卡数据等。Unity 使用了自己的一套内存管理机制来使这块内存具有和托管堆类似的功能。基本理念是，如果在这个关卡里需要某个资源，那么在需要时就加载，之后在没有任何引用时进行卸载。听起来很美好也和托管堆一样，但是由于 Unity 有一套自动加载和卸载资源的机制，让两者变得差别很大。自动加载资源可以为开发者省不少事儿，但是同时也意味着开发者失去了手动管理所有加载资源的权力，这非常容易导致大量的内存占用

#### 优化程序代码的内存占用

这部分的优化相对简单，因为能做的事情并不多：主要就是减少打包时的引用库，改一改 build 设置即可。对于一个新项目来说不会有太大问题，但是如果是已经存在的项目，可能改变会导致原来所需要的库的缺失（虽说一般来说这种可能性不大），因此有可能无法做到最优。

当使用 Unity 开发时，默认的 Mono 包含库可以说大部分用不上，在 Player Setting（Edit->Project Setting->;Player 或者 Shift+Ctrl(Command)+B 里的 Player Setting 按钮）面板里，将最下方的 Optimization 栏目中“Api Compatibility Level”选为.Net 2.0 Subset，表示你只会使用到部分的.Net 2.0 Subset，不需要 Unity 将全部.Net 的 Api 包含进去。接下来的“Stripping Level”表示从 build 的库中剥离的力度，每一个剥离选项都将从打包好的库中去掉一部分内容。你需要保证你的代码没有用到这部分被剥离的功能，选为“Use micro mscorlib”的话将使用最小的库（一般来说也没啥问题，不行的话可以试试之前的两个）。库剥离可以极大地降低打包后的程序的尺寸以及程序代码的内存占用，唯一的缺点是这个功能只支持 Pro 版的 Unity。

这部分优化的力度需要根据代码所用到的.Net 的功能来进行调整，有可能不能使用 Subset 或者最大的剥离力度。如果超出了限度，很可能会在需要该功能时因为找不到相应的库而 crash 掉（iOS 的话很可能在 Xcode 编译时就报错了）。比较好地解决方案是仍然用最强的剥离，并辅以较小的第三方的类库来完成所需功能。一个最常见问题是最大剥离时 Sysytem.Xml 是不被 Subset 和 micro 支持的，如果只是为了 xml，完全可以导入一个轻量级的 xml 库来解决依赖（Unity 官方推荐这个）。

关于每个设定对应支持的库的详细列表，可以在这里找到。关于每个剥离级别到底做了什么，Unity 的文档也有说明。实际上，在游戏开发中绝大多数被剥离的功能使用不上的，因此不管如何，库剥离的优化方法都值得一试

#### 托管堆优化

首先需要明确，托管堆中存储的是你在你的代码中申请的内存（不论是用 js，C#还是 Boo 写的）。一般来说，无非是 new 或者 Instantiate 两种生成 object 的方法（事实上 Instantiate 中也是调用了 new）。在接收到 alloc 请求后，托管堆在其上为要新生成的对象实例以及其实例变量分配内存，如果可用空间不足，则向系统申请更多空间。

当你使用完一个实例对象之后，通常来说在脚本中就不会再有对该对象的引用了（这包括将变量设置为 null 或其他引用，超出了变量的作用域，或者对 Unity 对象发送 Destory()）。在每隔一段时间，Mono 的垃圾回收机制将检测内存，将没有再被引用的内存释放回收。总的来说，你要做的就是在尽可能早的时间将不需要的引用去除掉，这样回收机制才能正确地把不需要的内存清理出来。但是需要注意在内存清理时有可能造成游戏的短时间卡顿，这将会很影响游戏体验，因此如果有大量的内存回收工作要进行的话，需要尽量选择合适的时间。

如果在你的游戏里，有特别多的类似实例，并需要对它们经常发送 Destroy() 的话，游戏性能上会相当难看。比如小熊推金币中的金币实例，按理说每枚金币落下台子后都需要对其 Destory()，然后新的金币进入台子时又需要 Instantiate，这对性能是极大的浪费。一种通常的做法是在不需要时，不摧毁这个 GameObject，而只是隐藏它，并将其放入一个重用数组中。之后需要时，再从重用数组中找到可用的实例并显示。这将极大地改善游戏的性能，相应的代价是消耗部分内存，一般来说这是可以接受的。关于对象重用，可以参考 Unity 关于内存方面的文档中 Reusable Object Pools 部分，或者 Prime31 有一个是用 Linq 来建立重用池的视频教程（Youtube，需要翻墙，上半部分，下半部分）。

如果不是必要，应该在游戏进行的过程中尽量减少对 GameObject 的 Instantiate() 和 Destroy() 调用，因为对计算资源会有很大消耗。在便携设备上短时间大量生成和摧毁物体的话，很容易造成瞬时卡顿。如果内存没有问题的话，尽量选择先将他们收集起来，然后在合适的时候（比如按暂停键或者是关卡切换），将它们批量地销毁并且回收内存。Mono 的内存回收会在后台自动进行，系统会选择合适的时间进行垃圾回收。在合适的时候，也可以手动地调用 System.GC.Collect() 来建议系统进行一次垃圾回收。要注意的是这里的调用真的仅仅只是建议，可能系统会在一段时间后在进行回收，也可能完全不理会这条请求，不过在大部分时间里，这个调用还是靠谱的。

#### 本机堆的优化

当你加载完成一个 Unity 的 scene 的时候，scene 中的所有用到的 asset（包括 Hierarchy 中所有 GameObject 上以及脚本中赋值了的的材质，贴图，动画，声音等素材），都会被自动加载（这正是 Unity 的智能之处）。也就是说，当关卡呈现在用户面前的时候，所有 Unity 编辑器能认识的本关卡的资源都已经被预先加入内存了，这样在本关卡中，用户将有良好的体验，不论是更换贴图，声音，还是播放动画时，都不会有额外的加载，这样的代价是内存占用将变多。Unity 最初的设计目的还是面向台式机，几乎无限的内存和虚拟内存使得这样的占用似乎不是问题，但是这样的内存策略在之后移动平台的兴起和大量移动设备游戏的制作中出现了弊端，因为移动设备能使用的资源始终非常有限。因此在面向移动设备游戏的制作时，尽量减少在 Hierarchy 对资源的直接引用，而是使用 Resource.Load 的方法，在需要的时候从硬盘中读取资源，在使用后用 Resource.UnloadAsset() 和 Resources.UnloadUnusedAssets() 尽快将其卸载掉。总之，这里是一个处理时间和占用内存空间的 trade off，如何达到最好的效果没有标准答案，需要自己权衡。

在关卡结束的时候，这个关卡中所使用的所有资源将会被卸载掉（除非被标记了 DontDestroyOnLoad）的资源。注意不仅是 DontDestroyOnLoad 的资源本身，其相关的所有资源在关卡切换时都不会被卸载。DontDestroyOnLoad 一般被用来在关卡之间保存一些玩家的状态，比如分数，级别等偏向文本的信息。如果 DontDestroyOnLoad 了一个包含很多资源（比如大量贴图或者声音等大内存占用的东西）的话，这部分资源在场景切换时无法卸载，将一直占用内存，这种情况应该尽量避免。

另外一种需要注意的情况是脚本中对资源的引用。大部分脚本将在场景转换时随之失效并被回收，但是，在场景之间被保持的脚本不在此列（通常情况是被附着在 DontDestroyOnLoad 的 GameObject 上了）。而这些脚本很可能含有对其他物体的 Component 或者资源的引用，这样相关的资源就都得不到释放，这绝对是不想要的情况。另外，static 的单例（singleton）在场景切换时也不会被摧毁，同样地，如果这种单例含有大量的对资源的引用，也会成为大问题。因此，尽量减少代码的耦合和对其他脚本的依赖是十分有必要的。如果确实无法避免这种情况，那应当手动地对这些不再使用的引用对象调用 Destroy() 或者将其设置为 null。这样在垃圾回收的时候，这些内存将被认为已经无用而被回收。

需要注意的是，Unity 在一个场景开始时，根据场景构成和引用关系所自动读取的资源，只有在读取一个新的场景或者 reset 当前场景时，才会得到清理。因此这部分内存占用是不可避免的。在小内存环境中，这部分初始内存的占用十分重要，因为它决定了你的关卡是否能够被正常加载。因此在计算资源充足或是关卡开始之后还有机会进行加载时，尽量减少 Hierarchy 中的引用，变为手动用 Resource.Load，将大大减少内存占用。在 Resource.UnloadAsset() 和 Resources.UnloadUnusedAssets() 时，只有那些真正没有任何引用指向的资源会被回收，因此请确保在资源不再使用时，将所有对该资源的引用设置为 null 或者 Destroy。同样需要注意，这两个 Unload 方法仅仅对 Resource.Load 拿到的资源有效，而不能回收任何场景开始时自动加载的资源。与此类似的还有 AssetBundle 的 Load 和 Unload 方法，灵活使用这些手动自愿加载和卸载的方法，是优化 Unity 内存占用的不二法则

### AB 中出现冗杂原因

循环依赖

### AB 引用计数加载，会出现什么问题

### 引用计数中，循环引用如何解决

### AB 何时执行 unload

### unity 渲染顺序

引擎层面先去分不透明和透明物体单独调用排序算法，排序算法理然后里面按照 order，layer，在按照 RenderQueue，然后是 distance 排序

### 纹理优化
https://zhuanlan.zhihu.com/p/350463940
