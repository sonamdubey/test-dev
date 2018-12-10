IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MH59_UpdateMPlan]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MH59_UpdateMPlan]
GO

	
CREATE PROCEDURE [dbo].[MH59_UpdateMPlan]

	@sIds VARCHAR(8000), -- List of delimited items
	@sDistance VARCHAR(8000), -- List of delimited items
	@sTime VARCHAR(8000), -- List of delimited items
	@sDelimiter VARCHAR(8000) = ',', -- delimiter that separates items
 
	@Status	INT OUTPUT
				
 AS
	
BEGIN
	DECLARE @Id VARCHAR(8000)
	DECLARE @Dist VARCHAR(8000)
	DECLARE @Time VARCHAR(8000)
	
	WHILE CHARINDEX(@sDelimiter,@sIds,0) <> 0
	BEGIN 
		 SELECT
		  @Id=RTRIM(LTRIM(SUBSTRING(@sIds,1,CHARINDEX(@sDelimiter,@sIds,0)-1))),
		  @sIds=RTRIM(LTRIM(SUBSTRING(@sIds,CHARINDEX(@sDelimiter,@sIds,0)+LEN(@sDelimiter),LEN(@sIds)))),
		  
		  @Dist=RTRIM(LTRIM(SUBSTRING(@sDistance,1,CHARINDEX(@sDelimiter,@sDistance,0)-1))),
		  @sDistance=RTRIM(LTRIM(SUBSTRING(@sDistance,CHARINDEX(@sDelimiter,@sDistance,0)+LEN(@sDelimiter),LEN(@sDistance)))),
		  
		  @Time=RTRIM(LTRIM(SUBSTRING(@sTime,1,CHARINDEX(@sDelimiter,@sTime,0)-1))),
		  @sTime=RTRIM(LTRIM(SUBSTRING(@sTime,CHARINDEX(@sDelimiter,@sTime,0)+LEN(@sDelimiter),LEN(@sTime))))

		  IF LEN(@Id) > 0
			BEGIN
				UPDATE Con_MaintenancePlan SET IndDistance = @Dist, IndTime = @Time WHERE ID = @Id
				SET @Status = 1
			END	
	END	
		
END
