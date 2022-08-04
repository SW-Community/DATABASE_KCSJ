--
--这是一个自制的售票系统，目前功能不完善，仅仅是演示使用

--文件系统
create database locomotive_system
on primary
(
	name='locomotive_system',
	filename='C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\locomotive_system.mdf',
	size=1mb,
	maxsize=10mb,
	filegrowth=1mb
)
log on
(
	name='locomotive_system_log',
	filename='C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\locomotive_system.ldf',
	size=1mb,
	maxsize=10mb,
	filegrowth=1mb
)
go

use locomotive_system
go

--机车信息表
create table Locomotive(
	lname char(32) primary key,--机车车号（唯一，出厂即被写死，类似于电脑的MAC地址）
	lmanufacturer char(32),--制造厂
	lbirth datetime,--下线日期
	ljv char(8),--局
	lduan char(8),--段
    lphoto varbinary(MAX)--机车照片
)
go

--车次编组信息表
create table Train
(
	tname char(16) primary key,--车次号
	tcaptain char(8),--列车长姓名，鉴于现实生活中列车长换班也不是经常发生所以放这里了
    tstart char(32),--起点站
    tend char(32),--终点站
)
go

--机车担任车次的值乘表
create table Act
(
	adate datetime,--担任日期
	lname char(32),--机车车号
	tname char(16),--车次号
	ldriver char(8),--大车姓名
    --现实中倒是大车换的勤，所以单独拿出来
	primary key(lname,tname,adate),
	foreign key(lname) references locomotive(lname),
	foreign key(tname) references train(tname)
)
go

--客车车厢信息表
create table Car(
	cid int identity(1,1) primary key,--车厢车号（唯一，出厂即写死）
    --由于车厢数量远大于机车，实地考察数据和手动输入不现实，因此这里用自增列模拟替代
	cmanufacturer char(32),--制造厂
	ctypea char(8) check(ctypea in('YZ','YW','RZ','RW','CA')),--座位类型（硬座软座等等）
	ctypeb char(8) check(ctypeb in('22','22B','24','25A','25B','25G','25K','25T')),--车厢类型，这个很大程度上决定了乘车体验，比如有没有空调
	cjv char(8),--局
	cduan char(8),--段
	ccapacity smallint,--车厢容量，基本上写死，除非想达到某些不可告人的目的（比如超载拉客）
	tname char(16) references train(tname),--车厢属于哪个车次
    cnum char(5)--机后位次，即车厢在本编组的位置（鉴于现实生活中很多人懒，客车车厢编组完成后一直到运用结束都不带拆开的所以这里写进同一个表问题也不大）
    --由于存在某些特殊编号。如+1，+2，因此这里用字符串表示，而座号只可能是正数所以不用这么麻烦
)
go

--座位表（无论是车座还是铺位这里统一称为座位，由其所在的车厢确定具体是什么）
create table Seat(
	seid int identity(1,1) primary key,--座位唯一ID
	cid int,--所在车厢的车厢号
    cno int,--相对车厢的编号，如“8车12座”
	foreign key(cid) references car(cid)
)
go

--用户信息表
create table Users(
	uid int identity(1,1) primary key,--用户ID
	uname nchar(32),--用户昵称
	uemail nchar(32),--邮箱
    upassword nchar(10),--密码
    uphoto varbinary(max)--用户头像
)
go

--订单（一个订单有好几张车票，为了支持所谓的团购）
create table Orders(
	oid int identity(1,1) primary key,--订单编号
	uid int,--用户ID
    odate datetime,--下单日期
	zffs char(6) check(zffs in('现金','刷卡','微信','支付宝','PayPal')),--支付方式
    zje int,--订单总价
    foreign KEY(uid) references users(uid)
)
go

--订单细节（也就是每一张车票）
create table Orderdetail(
	oid int references orders(oid),--外码，对应的订单
	odid int identity(1,1) primary key,
    --这个地方，每张车票有一个流水号（实际也是这样的）
	srid int references seat(seid),--座位ID
	takedate datetime,--乘车日期
	takername char(16)--乘车人姓名（严厉打击黄牛行为）
    --为简化过程，每张车票统一定价2元，类似于城市公交车
)
GO

--车站信息表
create table Station
(
    snname nchar(32) primary key,--站名
    snlevel nchar(5) check(snlevel in('特','一','二','三','四','五')),--车站等级（按现行国铁标准分六种）
)
GO

--运转信息，车次停靠那些站点（后面有索引，所以会排序，按到达时刻顺序排序）
create table Yunzhuan
(
	tname char(16),--车次名
	snname nchar(32),--站点名
    yzarrive time(7),--进站停车时刻（null代表始发站）
    yzleave time(7),--发车时刻（null代表终到站）
	primary key(tname,snname),
	foreign key(tname) references Train(tname),
	foreign key(snname) references Station(snname)
)
GO

--细微修复（添加索引和约束）

--为了在列车时刻表上能看到正确的时间顺序
create index yz_idx 
on Yunzhuan(tname asc,yzarrive asc,yzleave asc)
GO

--订单经常需要按时间查询
create index order_idx1
on Orders(odate desc)
go

--车次的起点站和终到站都必须是已经存在的站点，否则，emm...幽灵列车？？？
alter table Train 
add constraint cxx1 
foreign key(tstart) references station(snname)
GO

alter table Train 
add constraint cxx2 
foreign key(tend) references station(snname)
GO

--对用户填写信息做一些限制
alter table users
add constraint user1 check(uemail like '%@%' )
GO

alter table users
add CONSTRAINT user2 check(len(uname)>6)
GO

--添加视图
--（为了在C#中简化ODBC编程，这里创建一些用到的视图便于生成身实体类）

--每一趟列车当前有哪些座位的一个详情视图
create view Availableseats
as
select Train.tname, --车次名
    Car.cnum,--车厢号
    cytpea= case Car.ctypea--座位类型
	    when 'YZ' then '硬座'
	    when 'YW' then '硬卧'
	    when 'RZ' then '软座'
	    when 'RW' then '软卧'
	    when 'CA' then '无座'--餐车认为买的是无座票或者上车补的票
    end,
    Car.ctypeb,--车厢类型
    Seat.cno,--座号
    Seat.seid
from
    seat inner join car on seat.cid=car.cid
    inner join train on car.tname=train.tname
--补票的话对应的座位一直标记为否就可以卖出任意数量的这种票。。。（济局行为）
GO

--车票详情，用来生成和打印车票
create view Chepiao
as
select users.uid,--用户ID
	users.uname,--用户名
    orderdetail.odid,--车票号
	orders.oid,--订单号
	orders.zffs,--支付方式
	orderdetail.takedate,--乘车日期
    orderdetail.takername,--乘车人姓名
    train.tname,--车次
    car.ctypea,--座位类型
    car.ctypeb,--车厢类型
    car.cnum,--车厢号
    seat.cno, --座号
    orderdetail.takestart,--出发站
    orderdetail.takeend--到达站
from 
	users inner join orders on users.uid=orders.uid 
	inner join orderdetail on orders.oid=orderdetail.oid
	inner join seat on orderdetail.seid=seat.seid 
	inner join car on seat.cid=car.cid 
	inner join train on car.tname=train.tname
go


--每一趟列车具体时刻表（几点几分到什么车站）
create view Liecheshikebiao
as
select 
	train.tname,--车次名
	yunzhuan.yzarrive,--进站时刻
	yunzhuan.yzleave,--出站时刻
	station.snname,--站名
    station.snlevel--车站等级
from 
	train inner join yunzhuan on train.tname=yunzhuan.tname
	inner join station on yunzhuan.snname=station.snname
go


--每一趟列车的概况：车次号，始发站，终到站，本务机车等
create view Liechegaikuang
as
select 
    train.tname,--车次
    train.tstart,--始发站
    train.tend,--终到站
    yz1.yzleave,--发车时间
    yz2.yzarrive,--终到时间
    locomotive.lname,--本务机车
    locomotive.ljv,--局
    locomotive.lduan,--段
    act.ldriver,--大车
    train.tcaptain--列车长
from 
    train inner join act on train.tname=act.tname
    inner join locomotive on locomotive.lname=act.lname
    inner join yunzhuan as yz1 on yz1.tname=train.tname
    inner join yunzhuan as yz2 on yz2.tname=train.tname
where 
    yz1.snname=train.tstart 
    and yz2.snname=train.tend
go

--存储过程，对于几个比较复杂的事务进行封装，保证其一致性，简化客户端程序当中的异常处理过程

--买票，由于mssql没有什么数组的概念，因此只能一张一张买了，批量购买交给客户端实现
create PROCEDURE maipiao @oid int, @ccr char(16),@ccrq DATETIME,@qdz nchar(32),@zdz nchar(32),@seid int
AS
BEGIN
    declare @start TIME
    declare @end TIME
    declare @huoche char(16)

    select @huoche=tname from car where cid in
    (
        select cid from seat
        where seid=@seid
    )

    select @start= yzleave
    from yunzhuan
    where tname=@huoche
    and snname=@qdz
    
    select @end=yzarrive
    from yunzhuan
    where tname=@huoche
    and snname=@zdz

    declare myyoubiao scroll CURSOR 
    for 
        select takestart, takeend
        from orderdetail
        where seid=@seid

    open myyoubiao

    fetch next from myyoubiao
    while(@@FETCH_STATUS=0)
    BEGIN
        DECLARE @qsqj nchar(32)
        declare @zzqj nchar(32)

        fetch next from myyoubiao into @qsqj,@zzqj

        declare @qssk TIME
        declare @zzsk TIME

        select @qssk=yzleave
        from yunzhuan
        where tname=@huoche
        and snname=@qsqj

        select @zzsk=yzarrive
        from yunzhuan
        where tname=@huoche
        and snname=@zzqj

        if(NOT(@start>@zzqk or @end>@qssk))
        BEGIN
            print '矮油，票已经卖出去了'
            RETURN
        END
    end

    Close myyoubiao
    DEALLOCATE myyoubiao

    insert into orderdetail(oid,seid,takedate,takername,takestart,takeend)
    values(@oid,@seid,@ccrq,@ccr,@qdz,@zdz)
end
go

--退票，退掉指定编号的车票
create procedure tuipiao @odid INT
AS
BEGIN
    declare @seid INT
    select @seid=seid from orderdetail
    where odid=@odid

    --基本上是买票的逆过程
    delete from orderdetail
    where odid=@odid
end
GO

--“补 票”————济局行为开始。。。（bushi）
create PROCEDURE bupiao @oid int, @ccr char(16),@ccrq DATETIME,@qdz nchar(32),@zdz nchar(32),@seid int
AS
BEGIN
    --想卖几张卖几张，欸，就是玩，你管我呀。。。

    insert into orderdetail(oid,seid,takedate,takername,takestart,takeend)
    values(@oid,@seid,@ccrq,@ccr,@qdz,@zdz)
end
go--请勿模仿。。。

--查询车票（某座位某天莫区间是否可用）
create PRocedure keyimaima(@seid int, @takedate datetime,@qdz nchar(32),@zdz nchar(32))
AS
BEGIN
    declare @start TIME
    declare @end TIME
    declare @huoche char(16)

    select @huoche=tname from car where cid in
    (
        select cid from seat
        where seid=@seid
    )

    select @start= yzleave
    from yunzhuan
    where tname=@huoche
    and snname=@qdz
    
    select @end=yzarrive
    from yunzhuan
    where tname=@huoche
    and snname=@zdz

    declare myyoubiao scroll CURSOR 
    for 
        select takestart, takeend
        from orderdetail
        where seid=@seid

    open myyoubiao

    fetch next from myyoubiao
    while(@@FETCH_STATUS=0)
    BEGIN
        DECLARE @qsqj nchar(32)
        declare @zzqj nchar(32)

        fetch next from myyoubiao into @qsqj,@zzqj

        declare @qssk TIME
        declare @zzsk TIME

        select @qssk=yzleave
        from yunzhuan
        where tname=@huoche
        and snname=@qsqj

        select @zzsk=yzarrive
        from yunzhuan
        where tname=@huoche
        and snname=@zzqj

        if(NOT(@start>@zzqk or @end>@qssk))
        BEGIN
            print '这个座位已经被购买'
            RETURN
        END
    end
    print '这个座位还没被购买'
    Close myyoubiao
    DEALLOCATE myyoubiao
end
GO

--管理员过程

--添加车辆
create proc addcar(@chexinga char(8), @chexingb char(8) ,@zzc char(16), @jv char(8), @duan char(8))
AS
BEGIN
    declare @capacity int;
    --由于车型各种各样，同一种车型不同制造厂的标准也略有不同，
    --因此下面只选用常见车型的最经典的版本作为本项目的数据
    --（如YZ25T四方自主生产版本和BSP版本定员不一样，统一按BSP版本为准）
    declare @chexing char(20)
    set @chexing = CONCAT(@chexinga,@chexingb)
    set @capacity= case @chexinga
        when 'YZ22' then 116
        when 'YW22' then 77
        when 'RZ22' then 64
        when 'RW22' then 32
        when 'RZ24' then 64
        when 'YZ25B' then 128
        when 'RZ25B' then 80
        when 'YW25B' then 66
        when 'RW25B' then 36
        when 'YZ25G' then 118
        when 'RZ25G' then 80
        when 'YW25G' then 66
        when 'RW25G' then 36
        when 'YZ25K' then 118
        when 'RZ25K' then 72
        when 'YW25K' then 66
        when 'RW25K' then 36
        when 'YZ25T' then 118
        when 'RZ25T' then 78
        when 'YW25T' then 66
        when 'RW25T' then 36
    END
    insert into car(ctypea,ctypeb,ccapacity,cjv,cduan)
    values(@chexinga,@chexingb,@capacity,@ju,@duan);
    --获取刚刚插入的车厢ID
    declare @cid INT
    select @cid = IDENT_CURRENT('car')
    --插入座位
    declare @i INT
    set @i = 0
    WHILE(@i<@capacity)
    BEGIN
        insert INTO seat(cid,cno)
        values(@cid,@i)
        set @i = @i + 1
    END
 end
GO

--修改车次编号
create proc xgcc(@jiucheci char(8),@xincheci char(8))
AS
BEGIN
    declare myyoubiao scroll CURSOR
    for                        
    select tname from Car 
    where tname =@jiucheci

    open myyoubiao
    FETCH NEXT from myyoubiao

    while(@@FETCH_STATUS = 0)
    BEGIN
        update Car
        set tname = @xinchexing
        where CURRENT of myyoubiao
        FETCH NEXT from myyoubiao
    END
    close myyoubiao
    DEALLOCATE myyoubiao
end
GO

--统计乘坐排行榜前N名的车次，乘坐人数和担任该车次的路局
create PROCEDURE countthings(@n INT)
AS
BEGIN
    select DISTINCT act.tname,locomotive.ljv,COUNT(*) 
    from locomotive inner join  act on locomotive.lname=act.lname inner join orderdetial on act.tname=orderdetail.tname
    where count(*) IN
    (
        select DISTINCT top (@n) count(*)
        from orderdetial
        group by tname
    )
end
GO

--查询某站点的累计客流量
create PROCEDURE countpeople(@zhandian nchar(32))
AS
BEGIN
    declare @liuru INT
    select @liuru =COUNT(*)
    from orderdetail
    where takestart=@zhandian1

    declare @liuchu INT
    select @liuchu =COUNT(*)
    from orderdetail
    where takesend=@zhandian1

    PRINT'站点'+@zhandian+'流入量：'+@liuru+'，流出量'+@liuchu
end
GO
--触发器，每天执行常规操作以及维护某些完整性

--打击黄牛党：检查是否有重复购票行为（同一乘车人，同一天，同一车次重复买票即认为是黄牛党）
create trigger dajihuangniudang
on orderdetail
after INSERT
as
BEGIN
    declare @ccr char(16)
    declare @checi char(16)
    declare @ccrq datetime
    select @ccr=takername,@checi=tname,@ccrq=takedate
    from inserted

    DECLARE @cnt INT
    select @cnt=count(*)
    from orderdetail
    WHERE tname=@checi AND takername=@checi AND takedate=@ccrq
    IF(@cnt>=2)--两张及以上车票重复
    BEGIN
        PRINT '黄牛党？见鬼去吧，小心请你去局子里看春晚！'
        ROLLBACK TRANSACTION
    END
end
go

--当一张订单里面的车票全部被退完后，删除这张订单
create trigger qinglifeidingdan
on orderdetail
after DELETE
as
BEGIN
    declare @oid INT
    select @oid = oid from deleted

    declare @cnt INT
    select @cnt=count(*)
    from orders
    where oid=@oid

    if(@cnt=0)--订单里面没票了
    BEGIN
        DELETE from orders
        where oid=@oid
    end
END
GO