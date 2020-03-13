# consul

![img](D:\Desktop\core-misService\MicroServer\381412-20180602120831901-2002957689.png)

1. 服务注册/发现，

2. 健康监测

3. 集群 之Key/Value存储

   https://www.cnblogs.com/edisonchou/p/9148034.html

启动命令

![img](D:\Desktop\core-misService\MicroServer\381412-20180603134759991-422002399.png)

```shell
 consul agent -dev
 --192.168.80.100会作为leader角色，其余两台192.168.80.101和192.168.80.102会作为follower角色。
 
 当然，实际环境中leader角色不会是一个固定的，会随着环境的变化（比如Leader宕机或失联）由算法选出新的leader。在进行下面的操作会前，请确保三台节点能够相互ping通，并能够和宿主机也ping通。另外，192.168.80.71会作为client角色，并且和其余三台虚拟机互相ping通。
 
 
192.168.80.100>consul agent -server -ui -bootstrap-expect=3 -data-dir=/tmp/consul -node=consul-1 -client=0.0.0.0 -bind=192.168.80.100 -datacenter=dc1

192.168.80.101>consul agent -server -ui -bootstrap-expect=3 -data-dir=/tmp/consul -node=consul-2 -client=0.0.0.0 -bind=192.168.80.101 -datacenter=dc1 -join 192.168.80.100

192.168.80.102>consul agent -server -ui -bootstrap-expect=3 -data-dir=/tmp/consul -node=consul-3 -client=0.0.0.0 -bind=192.168.80.102 -datacenter=dc1 -join 192.168.80.100

consul agent -bind 0.0.0.0 -client 192.168.80.71 -data-dir=C:\Counsul\tempdata -node EDC.DEV.WebServer -join 192.168.80.100
```

注意101和102的启动命令中，有一句 ***-join 192.168.80.100\*** => 有了这一句，就把101和102加入到了100所在的集群中。

　　启动之后，集群就开始了Vote（投票选Leader）的过程，通过下面的命令可以看到集群的情况：

![img](https://images2018.cnblogs.com/blog/381412/201806/381412-20180602151441013-665180870.png)

也可以通过以下命令查看目前的各个Server的角色状态：

```shell
consul operator raft list-peers
```

![img](https://images2018.cnblogs.com/blog/381412/201806/381412-20180603153252227-235069956.png)



# Service

启动命令

```shell
dotnet Abc.ServiceInstance.dll --urls="http://*:5726" --ip="127.0.0.1" --port=5726 --ttt=111

dotnet Abc.ServiceInstance.dll --urls="http://*:5727" --ip="127.0.0.1" --port=5727 --ttt=222

dotnet Abc.ServiceInstance.dll --urls="http://*:5728" --ip="127.0.0.1" --port=5728 --ttt=333
```



# GetWay

## Ocelot：

1. 路由
2. 缓存
3. 限流
4. 熔断

```shell
dotnet Abc.GetWayDemo.dll --urls="http://*:6299" --ip="127.0.0.1" --port=6299
```

## Polly

- 功能1：重试（Retry）
- 功能2：断路器（Circuit-Breaker）
- 功能3：超时检测（Timeout）
- 功能4：缓存（Cache）
- 功能5：降级（Fallback）

# CAP理论

在分区容错的前提下---一致性+可用性不能同时保证

一致性：牺牲可用性，2PC/3PC【预备-提交两个阶段】

​	**微服务中一般不会采用**

可用性：

​	针对数据不一致，提出了**Base理论**【最终一致性】

![image-20200306212646049](C:\Users\Administrator\AppData\Roaming\Typora\typora-user-images\image-20200306212646049.png)