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

### Lua 原方法有哪些，分别什么意思

__index
__newindex
__add
__call
__tostring

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

### Lua table 和 C#的字典的差异在哪

### Struct 在堆还是在栈

C++看是否是用 new 的

### 引用类型在堆还是在栈

堆上

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

### 设计模式相关知识

### 客户端框架有哪些功能个模块

* UI 模块
* BResource 模块
* Assetbundle 模块

### AB 包怎么解决循环依赖问题

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
https://zhuanlan.zhihu.com/p/76562300
SetPassCall 代表渲染状态切换，主要出现在材质不一致的时候，进行渲染状态切换。我们知道一个 batch 包括，提交 vbo，提交 ibo，提交 shader，设置好硬件渲染状态，设置好光源属性等（注意提交纹理严格意义上并不包括在一个 batch 内，纹理可以被缓存并多帧复用）。

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

一个进程由共享空间（包括堆、代码区、数据区、进程空间和打开的文件描述符）和一个或多个线程组成，各个线程之间共享进程的内存空间，而一个标准的线程由线程ID、程序计数器PC、寄存器和栈组成。
进程和线程之间的联系如下图所示：

![](https://img-blog.csdnimg.cn/20201021221100545.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80OTE5OTY0Ng==,size_16,color_FFFFFF,t_70#pic_center)

![](https://img-blog.csdnimg.cn/20201021221114554.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80OTE5OTY0Ng==,size_16,color_FFFFFF,t_70#pic_center)

#### 进程与线程的选择

* 线程的创建或销毁的代价比进程小，需要频繁创建和销毁时应优先选用线程；
* 线程上下文切换的速度比进程快，需要大量计算时优先选用线程；
* 线程在CPU上的使用效率更高，需要多核分布时优先选用线程，需要多机分布时优先选用进程
* 线程的安全性、稳定性没有进程好，需要更稳定安全时优先使用进程。

综上，线程创建和销毁的代价低、上下文切换速度快、对系统资源占用小、对CPU的使用效率高，因此一般情况下优先选择线程进行高并发编程；但线程组的所有线程共用一个进程的内存空间，安全稳定性相对较差，若其中一个线程发生崩溃，可能会使整个进程，因此对安全稳定性要求较高时，需要优先选择进程进行高并发编程。

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

服务器负责转化客户端的操作，每个客户端在固定的逻辑帧执行该帧所有客户端的操作命令，通过在严格一致的时间轴上执行同样的命令序列得到同样的结果。主流的老牌RTS游戏都是帧同步：星际争霸、war3

![](https://pic3.zhimg.com/80/v2-b93344314c698823324b3d9a69b372c2_720w.webp)

##### 优点

* 开发方便，可以无视客户端服务器（但是要考虑逻辑和表现分离）
* 打击感反馈好，例如所有的命中都能在本地马上触发相应的扣血和打击表现，反馈及时准确，无须像状态同步那样等待服务器推送扣血或者向服务器请求扣血。
* 网络流量小，这个会带来很多好处：带宽费用小、用户成本低、降低收发包带来的耗电（据说比较可观）

##### 缺点

* 对网络要求高。这个涉及到具体帧同步实现的方式。

    锁帧问题，服务器会等待所有的客户端的第N帧操作都到齐之后再发送到各客户端第N帧的操作，这样一旦有一个客户端网络波动，所有人都会卡住。war3的做法是加入了超时机制，如果在第N帧超时没有收到某个客户端第N帧的操作，就不再等待这个客户端的这帧操作，并认为该客户端在这帧什么都没做。但是如果这个客户端只是延迟很高，他所有的操作是否都会被服务器判定无效呢？

    逻辑帧平滑问题，一般收到的逻辑帧命令数据会加入客户端正在顺序执行的逻辑帧的队列中。如果队列设置过长，操作延迟就会比较高；如果队列很短，在网络波动时，就会出现队列为空饥饿状态，造成逻辑帧不平滑。这个可以采用逻辑和表现分离、平滑插值等做法。逻辑和表现分离做的比较好的话可以做到无buffer。平滑插值是一些表现的过渡处理，比如卡顿感一个很主要的来源是怪物移动的不平滑，一个比较好的应对方法是以怪物方向为主计算位置，即使没有逻辑帧，表现帧也会继续根据当前方向速度计算位置，这样即使延迟出现波动时也不会出现位移的不平滑。
* 反外挂能力差，容易本地修改，开图，修改属性等。
* 断线重连需要追帧
* 客户端逻辑计算性能压力大，需要每逻辑帧计算所有游戏单位的逻辑状态，即使单位不在屏幕内。
* 对结果一致性控制比较严格，如果使用了第三方的库，需要能够控制结果的一致性。其他因素包括浮点计算在不同平台的误差差异、随机数序列一致性、一些容器的访问顺序问题

#### 状态同步

服务器承载所有计算，客户端只做表现。主流的大型MMO游戏都采用状态同步。

![](https://pic4.zhimg.com/80/v2-d3b538d949651bbbe70bdc71835e510b_720w.webp)

##### 优点

* 容易断线重连
* 容易防外挂
* 简单粗暴

##### 缺点

* 流量大
* 打击感反馈匹配不够精准，因为所有的表现都是服务器推送的，在网络波动、客户端服务器不同计算的误差下，客户端各个表现比较难契合。
* 对网络要求也会比较高，如果2中说的各个表现的契合，以及位移等都会受到网络波动的影响。

无论是状态同步还是帧同步，对于网络延迟的优化都会涉及到的问题是：

* 在网络协议层优化，可靠UDP代替TCP，降低延迟
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
        throw new IllegalArgumentException("数组内的自然数目少于n-1");
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

#### Unity支持的压缩格式的分类

* DXT格式 --- Nvidia Tegra提供
* ETC  --- 安卓原生支持的，OPNEGL2.0都支持,ETC2只有OPENGL3.0支持
* PVRTC --- Imagination PowerVR提供
* ATC --- Qualcomm Snapdragon提供的
* ASTC--- Bridgetek
* IOS只支持PVRTC 和 ASTC 的压缩格式

Unity3D引擎对纹理的处理是智能的：不论你放入的是PNG，PSD还是TGA，它们都会被自动转换成Unity自己的Texture2D格式

要注意一些png图片，在硬盘中占用几KB，怎么在Unity中显示却变大？因为Unity显示的是Texture大小，是实际运行时占用内存的大小，而png却是一种压缩显示格式

### Mipmap 思想，内存变化


### unity 生命周期，实例化后，aos 顺序

Initial->Physics->Input events->GameLogic->Scene rendering->Gizmos->GUI->EndofFrame

### resource 与 streamingAsset 区别

* AssetBundle。很多人应该知道这是unity3d里面用来打包资源的，支持的格式有限，如文理、音频、二进制、文本等。像一些.cs文件、.mp4文件是没法打包的。
* Resources目录下的资源在打包之后，也会生成AssetBundle，只是Resources下的资源会被系统自动处理。
* 那AssetBundle到底是什么呢？其实可以把它简单看成是一个资源集合，必须用WWW类来进行读取。
* 而Resources可以看成是一个特殊的WWW，只能对于Resources目录的资源。而WWW读取的内容也必须是AssetBundle，所以一个单独的ogg文件无法用WWW读取，必须先打包成AssetBundle，才能用WWW读取。
* StreamingAssets又是什么呢？这是个Raw目录，里面的内容不会加密、编码。比如png、ogg

### mipmap1024 一张图能开启几级

9 级 512*512, 256*256, 128*128, 64*64, 32*32, 16*16, 8*8, 4*4, 2*2, 1*1

### upvalue 作用

### lua `self`作用

`self`其实就相当于Java，C++中的`this`对象

```lua
a = {x = 3, y = 4}
 
a.__index = function(table, key)
    print("__index")
    return a[key]
end
--注：
--冒号，在定义的时候省略掉了self
--点，在定义的时候不省略self

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
local b = a:new() --调用可以等价于a.new(a, {})
print(getmetatable(b))
print(b.x)
```

### lua 持续引用 c#如何处理，如果忘记设置 lua 引用变量 = nil 如何处理

### unity 内存分哪些

#### Unity中的内存种类

实际上Unity游戏使用的内存一共有三种：程序代码、托管堆（Managed Heap）以及本机堆（Native Heap）。

程序代码包括了所有的Unity引擎，使用的库，以及你所写的所有的游戏代码。在编译后，得到的运行文件将会被加载到设备中执行，并占用一定内存。这部分内存实际上是没有办法去“管理”的，它们将在内存中从一开始到最后一直存在。一个空的Unity默认场景，什么代码都不放，在iOS设备上占用内存应该在17MB左右，而加上一些自己的代码很容易就飙到20MB左右。想要减少这部分内存的使用，能做的就是减少使用的库，稍后再说。

托管堆是被Mono使用的一部分内存。Mono项目一个开源的.net框架的一种实现，对于Unity开发，其实充当了基本类库的角色。托管堆用来存放类的实例（比如用new生成的列表，实例中的各种声明的变量等）。“托管”的意思是Mono“应该”自动地改变堆的大小来适应你所需要的内存，并且定时地使用垃圾回收（Garbage Collect）来释放已经不需要的内存。关键在于，有时候你会忘记清除对已经不需要再使用的内存的引用，从而导致Mono认为这块内存一直有用，而无法回收。

最后，本机堆是Unity引擎进行申请和操作的地方，比如贴图，音效，关卡数据等。Unity使用了自己的一套内存管理机制来使这块内存具有和托管堆类似的功能。基本理念是，如果在这个关卡里需要某个资源，那么在需要时就加载，之后在没有任何引用时进行卸载。听起来很美好也和托管堆一样，但是由于Unity有一套自动加载和卸载资源的机制，让两者变得差别很大。自动加载资源可以为开发者省不少事儿，但是同时也意味着开发者失去了手动管理所有加载资源的权力，这非常容易导致大量的内存占用

#### 优化程序代码的内存占用

这部分的优化相对简单，因为能做的事情并不多：主要就是减少打包时的引用库，改一改build设置即可。对于一个新项目来说不会有太大问题，但是如果是已经存在的项目，可能改变会导致原来所需要的库的缺失（虽说一般来说这种可能性不大），因此有可能无法做到最优。

当使用Unity开发时，默认的Mono包含库可以说大部分用不上，在Player Setting（Edit->Project Setting->;Player或者Shift+Ctrl(Command)+B里的Player Setting按钮）面板里，将最下方的Optimization栏目中“Api Compatibility Level”选为.NET 2.0 Subset，表示你只会使用到部分的.NET 2.0 Subset，不需要Unity将全部.NET的Api包含进去。接下来的“Stripping Level”表示从build的库中剥离的力度，每一个剥离选项都将从打包好的库中去掉一部分内容。你需要保证你的代码没有用到这部分被剥离的功能，选为“Use micro mscorlib”的话将使用最小的库（一般来说也没啥问题，不行的话可以试试之前的两个）。库剥离可以极大地降低打包后的程序的尺寸以及程序代码的内存占用，唯一的缺点是这个功能只支持Pro版的Unity。

这部分优化的力度需要根据代码所用到的.NET的功能来进行调整，有可能不能使用Subset或者最大的剥离力度。如果超出了限度，很可能会在需要该功能时因为找不到相应的库而crash掉（iOS的话很可能在Xcode编译时就报错了）。比较好地解决方案是仍然用最强的剥离，并辅以较小的第三方的类库来完成所需功能。一个最常见问题是最大剥离时Sysytem.Xml是不被Subset和micro支持的，如果只是为了xml，完全可以导入一个轻量级的xml库来解决依赖（Unity官方推荐这个）。

关于每个设定对应支持的库的详细列表，可以在这里找到。关于每个剥离级别到底做了什么，Unity的文档也有说明。实际上，在游戏开发中绝大多数被剥离的功能使用不上的，因此不管如何，库剥离的优化方法都值得一试

#### 托管堆优化

首先需要明确，托管堆中存储的是你在你的代码中申请的内存（不论是用js，C#还是Boo写的）。一般来说，无非是new或者Instantiate两种生成object的方法（事实上Instantiate中也是调用了new）。在接收到alloc请求后，托管堆在其上为要新生成的对象实例以及其实例变量分配内存，如果可用空间不足，则向系统申请更多空间。

当你使用完一个实例对象之后，通常来说在脚本中就不会再有对该对象的引用了（这包括将变量设置为null或其他引用，超出了变量的作用域，或者对Unity对象发送Destory()）。在每隔一段时间，Mono的垃圾回收机制将检测内存，将没有再被引用的内存释放回收。总的来说，你要做的就是在尽可能早的时间将不需要的引用去除掉，这样回收机制才能正确地把不需要的内存清理出来。但是需要注意在内存清理时有可能造成游戏的短时间卡顿，这将会很影响游戏体验，因此如果有大量的内存回收工作要进行的话，需要尽量选择合适的时间。

如果在你的游戏里，有特别多的类似实例，并需要对它们经常发送Destroy()的话，游戏性能上会相当难看。比如小熊推金币中的金币实例，按理说每枚金币落下台子后都需要对其Destory()，然后新的金币进入台子时又需要Instantiate，这对性能是极大的浪费。一种通常的做法是在不需要时，不摧毁这个GameObject，而只是隐藏它，并将其放入一个重用数组中。之后需要时，再从重用数组中找到可用的实例并显示。这将极大地改善游戏的性能，相应的代价是消耗部分内存，一般来说这是可以接受的。关于对象重用，可以参考Unity关于内存方面的文档中Reusable Object Pools部分，或者Prime31有一个是用Linq来建立重用池的视频教程（Youtube，需要翻墙，上半部分，下半部分）。

如果不是必要，应该在游戏进行的过程中尽量减少对GameObject的Instantiate()和Destroy()调用，因为对计算资源会有很大消耗。在便携设备上短时间大量生成和摧毁物体的话，很容易造成瞬时卡顿。如果内存没有问题的话，尽量选择先将他们收集起来，然后在合适的时候（比如按暂停键或者是关卡切换），将它们批量地销毁并且回收内存。Mono的内存回收会在后台自动进行，系统会选择合适的时间进行垃圾回收。在合适的时候，也可以手动地调用System.GC.Collect()来建议系统进行一次垃圾回收。要注意的是这里的调用真的仅仅只是建议，可能系统会在一段时间后在进行回收，也可能完全不理会这条请求，不过在大部分时间里，这个调用还是靠谱的。

#### 本机堆的优化

当你加载完成一个Unity的scene的时候，scene中的所有用到的asset（包括Hierarchy中所有GameObject上以及脚本中赋值了的的材质，贴图，动画，声音等素材），都会被自动加载（这正是Unity的智能之处）。也就是说，当关卡呈现在用户面前的时候，所有Unity编辑器能认识的本关卡的资源都已经被预先加入内存了，这样在本关卡中，用户将有良好的体验，不论是更换贴图，声音，还是播放动画时，都不会有额外的加载，这样的代价是内存占用将变多。Unity最初的设计目的还是面向台式机，几乎无限的内存和虚拟内存使得这样的占用似乎不是问题，但是这样的内存策略在之后移动平台的兴起和大量移动设备游戏的制作中出现了弊端，因为移动设备能使用的资源始终非常有限。因此在面向移动设备游戏的制作时，尽量减少在Hierarchy对资源的直接引用，而是使用Resource.Load的方法，在需要的时候从硬盘中读取资源，在使用后用Resource.UnloadAsset()和Resources.UnloadUnusedAssets()尽快将其卸载掉。总之，这里是一个处理时间和占用内存空间的trade off，如何达到最好的效果没有标准答案，需要自己权衡。

在关卡结束的时候，这个关卡中所使用的所有资源将会被卸载掉（除非被标记了DontDestroyOnLoad）的资源。注意不仅是DontDestroyOnLoad的资源本身，其相关的所有资源在关卡切换时都不会被卸载。DontDestroyOnLoad一般被用来在关卡之间保存一些玩家的状态，比如分数，级别等偏向文本的信息。如果DontDestroyOnLoad了一个包含很多资源（比如大量贴图或者声音等大内存占用的东西）的话，这部分资源在场景切换时无法卸载，将一直占用内存，这种情况应该尽量避免。

另外一种需要注意的情况是脚本中对资源的引用。大部分脚本将在场景转换时随之失效并被回收，但是，在场景之间被保持的脚本不在此列（通常情况是被附着在DontDestroyOnLoad的GameObject上了）。而这些脚本很可能含有对其他物体的Component或者资源的引用，这样相关的资源就都得不到释放，这绝对是不想要的情况。另外，static的单例（singleton）在场景切换时也不会被摧毁，同样地，如果这种单例含有大量的对资源的引用，也会成为大问题。因此，尽量减少代码的耦合和对其他脚本的依赖是十分有必要的。如果确实无法避免这种情况，那应当手动地对这些不再使用的引用对象调用Destroy()或者将其设置为null。这样在垃圾回收的时候，这些内存将被认为已经无用而被回收。

需要注意的是，Unity在一个场景开始时，根据场景构成和引用关系所自动读取的资源，只有在读取一个新的场景或者reset当前场景时，才会得到清理。因此这部分内存占用是不可避免的。在小内存环境中，这部分初始内存的占用十分重要，因为它决定了你的关卡是否能够被正常加载。因此在计算资源充足或是关卡开始之后还有机会进行加载时，尽量减少Hierarchy中的引用，变为手动用Resource.Load，将大大减少内存占用。在Resource.UnloadAsset()和Resources.UnloadUnusedAssets()时，只有那些真正没有任何引用指向的资源会被回收，因此请确保在资源不再使用时，将所有对该资源的引用设置为null或者Destroy。同样需要注意，这两个Unload方法仅仅对Resource.Load拿到的资源有效，而不能回收任何场景开始时自动加载的资源。与此类似的还有AssetBundle的Load和Unload方法，灵活使用这些手动自愿加载和卸载的方法，是优化Unity内存占用的不二法则

### AB 中出现冗杂原因

循环依赖

### AB 引用计数加载，会出现什么问题

### 引用计数中，循环引用如何解决

### AB 何时执行 unload

### unity 渲染顺序

引擎层面先去分不透明和透明物体单独调用排序算法，排序算法理然后里面按照 order，layer，在按照 RenderQueue，然后是 distance 排序

### 纹理优化
https://zhuanlan.zhihu.com/p/350463940
