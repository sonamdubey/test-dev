IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Content_UpdateLaunches]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Content_UpdateLaunches]
GO

	
CREATE PROCEDURE [dbo].[Content_UpdateLaunches]

	@sIds VARCHAR(8000), -- List of delimited items
	@sSort VARCHAR(8000), -- List of delimited items
	@sDelimiter VARCHAR(8000) = ',', -- delimiter that separates items
 
	@Status	INT OUTPUT
				
 AS
	
BEGIN
	DECLARE @Id VARCHAR(8000)
	DECLARE @Sort VARCHAR(8000)
	
	WHILE CHARINDEX(@sDelimiter,@sIds,0) <> 0
	BEGIN 
		 SELECT
		  @Id=RTRIM(LTRIM(SUBSTRING(@sIds,1,CHARINDEX(@sDelimiter,@sIds,0)-1))),
		  @sIds=RTRIM(LTRIM(SUBSTRING(@sIds,CHARINDEX(@sDelimiter,@sIds,0)+LEN(@sDelimiter),LEN(@sIds)))),
		  
		  @Sort=RTRIM(LTRIM(SUBSTRING(@sSort,1,CHARINDEX(@sDelimiter,@sSort,0)-1))),
		  @sSort=RTRIM(LTRIM(SUBSTRING(@sSort,CHARINDEX(@sDelimiter,@sSort,0)+LEN(@sDelimiter),LEN(@sSort))))
		
		  IF LEN(@Id) > 0
			BEGIN
				UPDATE ExpectedCarLaunches SET Sort = @Sort WHERE ID = @Id
				SET @Status = 1
			END	
	END	
		
END
