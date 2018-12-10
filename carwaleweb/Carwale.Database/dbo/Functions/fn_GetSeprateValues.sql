IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fn_GetSeprateValues]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fn_GetSeprateValues]
GO

	
Create Function [dbo].[fn_GetSeprateValues](@Ids varchar(8000))
returns @Table Table(Id varchar(100))
As
Begin

DECLARE @ind int
DECLARE @Start int

SET @Ids=@Ids+','
SET @Start=1

SET @ind =CHARINDEX(',',@Ids)
    WHILE(@ind>0)
    BEGIN
        DECLARE @SubId varchar(100)
        SET @SubId=Substring(@Ids,@Start,(@ind-1))
        IF(@SubId<>'' and @SubId Is Not Null)
        BEGIN
            INSERT INTO @Table(Id) VALUES(@SubId)
       End
        SET @Ids =SUBSTRING(@Ids,@ind+1,LEN(@Ids))
        SET @ind = CHARINDEX(',',@Ids ) 
    END

RETURN

END

