IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fnDelimitedVal]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fnDelimitedVal]
GO

	-- Table-valued user-defined function - TVF
CREATE FUNCTION [dbo].[fnDelimitedVal] (@Id INT,@StateId INT)
RETURNS @SplitList TABLE (ListMember varchar(max),ListName varchar(max) )
AS
BEGIN
	DECLARE @listId varchar(max)
	DECLARE @listName varchar(max)
	SELECT @listId=COALESCE(@listId +' ,','')+CONVERT(varchar,CityId),
	@listName=COALESCE(@listName +' ,','')+(CASE WHEN HJ.CityId=-1 THEN 'All Cities' ELSE CONVERT(VARCHAR,Name) END)
	FROM HR_JobLocations HJ
	LEFT JOIN Cities CT ON HJ.CityId = CT.ID
	where HR_JobId = @Id and HJ.StateId=@StateId and IsActive = 1
	INSERT INTO @SplitList VALUES (@listId,@listName)
	
    RETURN
END
