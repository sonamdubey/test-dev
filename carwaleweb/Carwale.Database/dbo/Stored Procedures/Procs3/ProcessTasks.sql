IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ProcessTasks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ProcessTasks]
GO

	create procedure [dbo].[ProcessTasks]
as
declare  @tbl table(id int identity(1,1),RoleId int,Taskset varchar(4000))
	insert into @tbl(RoleId,Taskset)
	select Id,Taskset from TC_Roles
	
declare @rowCount smallint,@loopCount smallint=1
select @rowCount=COUNT(*) from @tbl

declare @RoleId INT, @Taskset varchar(4000)

while @rowCount>=@loopCount 

begin
	
	select @RoleId=RoleId,@Taskset=Taskset  from @tbl	WHERE ID=@loopCount
	insert into TC_RoleTasks(RoleId,TaskId)
	SELECT ID,ListMember 
    FROM  [dbo].[fnCSVToNumber](@RoleId,@Taskset)		
	set @loopCount=@loopCount+1
end
