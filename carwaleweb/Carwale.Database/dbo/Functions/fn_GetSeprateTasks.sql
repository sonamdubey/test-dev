IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fn_GetSeprateTasks]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fn_GetSeprateTasks]
GO

	CREATE Function [dbo].[fn_GetSeprateTasks](@Ids varchar(8000))
returns Varchar(4000)
As
Begin

DECLARE @ind int
DECLARE @Start int

SET @Ids=@Ids+','
SET @Start=1

declare @TaskName Varchar(4000)=''

SET @ind =CHARINDEX(',',@Ids)
    WHILE(@ind>0)
    BEGIN
        DECLARE @SubId varchar(100)
        SET @SubId=Substring(@Ids,@Start,(@ind-1))
        IF(@SubId<>'' and @SubId Is Not Null)
        BEGIN
			declare @Tname Varchar(20)
	        
			select @Tname=TaskName from TC_Tasks where id=@SubId  and id in (7,9,10,17,22)      
			set @TaskName=@TaskName+ ',' + isnull(@Tname,'')
		End
        SET @Ids =SUBSTRING(@Ids,@ind+1,LEN(@Ids))
        SET @ind = CHARINDEX(',',@Ids ) 
    END

RETURN @TaskName

END
